using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C70 RID: 3184
	public class LordJob_HateChant : LordJob
	{
		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x06005A4F RID: 23119 RVA: 0x002137E0 File Offset: 0x002119E0
		public override bool ShouldExistWithoutPawns
		{
			get
			{
				return this.spawningFinished = false;
			}
		}

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x06005A50 RID: 23120 RVA: 0x002137F8 File Offset: 0x002119F8
		private int CallInterval
		{
			get
			{
				if (!this.smallGroup)
				{
					return LordJob_HateChant.CallIntervalSeconds.RandomInRange.SecondsToTicks();
				}
				return LordJob_HateChant.CallIntervalSecondsSmallGroup.RandomInRange.SecondsToTicks();
			}
		}

		// Token: 0x06005A52 RID: 23122 RVA: 0x00213874 File Offset: 0x00211A74
		public override StateGraph CreateGraph()
		{
			List<Pawn> list = new List<Pawn>();
			if (!this.spawningStarted)
			{
				list.AddRange(this.lord.ownedPawns);
				for (int i = 0; i < list.Count; i++)
				{
					this.lord.RemovePawn(list[i]);
				}
				this.spawningStarted = true;
			}
			this.assaultLossThreshold = Mathf.Min(Mathf.CeilToInt(LordJob_HateChant.AssaultLossPercent.RandomInRange * (float)(this.lord.ownedPawns.Count + list.Count)), 10);
			StateGraph stateGraph = new StateGraph();
			LordToil_SpawnHateChantersDelayed lordToil_SpawnHateChantersDelayed = new LordToil_SpawnHateChantersDelayed(list, "PositionsReady", 5, 0.8f);
			stateGraph.AddToil(lordToil_SpawnHateChantersDelayed);
			LordToil_PsychicRitualParticipantGoto traversalToil = new LordToil_PsychicRitualParticipantGoto(10800);
			stateGraph.AddToil(traversalToil);
			Transition transition = new Transition(lordToil_SpawnHateChantersDelayed, traversalToil, false, true);
			transition.AddTrigger(new Trigger_Signal("PositionsReady"));
			transition.AddPreAction(new TransitionAction_Custom(delegate()
			{
				this.participants = this.GenerateParticipants().ToList<PsychicRitualParticipant>();
				traversalToil.SetParticipants(this.participants);
				this.spawningFinished = true;
			}));
			stateGraph.AddTransition(transition, false);
			LordToil_HateChant chantToil = new LordToil_HateChant();
			stateGraph.AddToil(chantToil);
			Transition transition2 = new Transition(traversalToil, chantToil, false, true);
			transition2.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && (traversalToil.AllPawnsArrived || traversalToil.TimedOut)));
			transition2.AddPreAction(new TransitionAction_Custom(delegate()
			{
				chantToil.SetParticipants(this.participants);
			}));
			stateGraph.AddTransition(transition2, false);
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(false, false);
			stateGraph.AddToil(lordToil_AssaultColony);
			Transition transition3 = new Transition(chantToil, lordToil_AssaultColony, false, true);
			transition3.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.lostPawns >= this.assaultLossThreshold));
			transition3.AddPostAction(new TransitionAction_Custom(delegate()
			{
				foreach (Sustainer sustainer in this.sustainers)
				{
					sustainer.End();
				}
				this.sustainers.Clear();
				Find.LetterStack.ReceiveLetter("HateChantersAttackingLabel".Translate(), "HateChantersAttackingText".Translate(), LetterDefOf.ThreatBig, null, 0, true);
				this.EnableSelfResurrect();
				this.MakePawnsWait();
			}));
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x06005A53 RID: 23123 RVA: 0x00213A50 File Offset: 0x00211C50
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			base.Notify_PawnLost(p, condition);
			if (condition == PawnLostCondition.Killed)
			{
				this.lostPawns++;
				p.addCorpseToLord = true;
			}
			else if (condition == PawnLostCondition.Incapped)
			{
				this.lostPawns++;
			}
			this.motes.RemoveWhere((Mote m) => m.link1.Target == p);
			if (this.lostPawns == 1 && this.assaultLossThreshold > 1)
			{
				Messages.Message("MessageHateChantersAbsorbed".Translate(), MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x06005A54 RID: 23124 RVA: 0x00213AEC File Offset: 0x00211CEC
		public override void Notify_CorpseLost(Corpse c)
		{
			base.Notify_CorpseLost(c);
			this.lostPawns++;
		}

		// Token: 0x06005A55 RID: 23125 RVA: 0x00213B03 File Offset: 0x00211D03
		public override void LordJobTick()
		{
			base.LordJobTick();
			if (this.lord.CurLordToil is LordToil_HateChant)
			{
				this.TickSustainersAndMotes();
			}
		}

		// Token: 0x06005A56 RID: 23126 RVA: 0x00213B24 File Offset: 0x00211D24
		private void TickSustainersAndMotes()
		{
			if (this.sustainers.Count == 0)
			{
				this.InitializeSfx();
				this.nextCallTick = Find.TickManager.TicksGame + this.CallInterval;
				foreach (Pawn thing in this.lord.ownedPawns)
				{
					this.motes.Add(MoteMaker.MakeAttachedOverlay(thing, ThingDefOf.Mote_HateChantShadow, new Vector3(0f, 0f, 0f), 1f, -1f));
					this.motes.Add(MoteMaker.MakeAttachedOverlay(thing, ThingDefOf.Mote_PsychicRitualInvocation, new Vector3(0f, 0f, 0f), 1f, -1f));
				}
			}
			if (this.lord.ownedPawns.Count > 0)
			{
				if (base.Map.IsHashIntervalTick(300))
				{
					this.callMotes.RemoveAll((KeyValuePair<IntVec3, Mote> m) => m.Value.Destroyed);
				}
				if (Find.TickManager.TicksGame == this.nextCallTick)
				{
					Pawn pawn = this.lord.ownedPawns.RandomElement<Pawn>();
					if (!this.callMotes.ContainsKey(pawn.Position))
					{
						SoundDefOf.Pawn_HateChanter_Call.PlayOneShot(SoundInfo.InMap(new TargetInfo(pawn.PositionHeld, pawn.MapHeld, false), MaintenanceType.None));
						Mote value = MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_HateChantCall, Vector3.zero, 1f, -1f);
						this.callMotes.Add(pawn.Position, value);
					}
					this.nextCallTick = Find.TickManager.TicksGame + this.CallInterval;
				}
				foreach (Mote mote in this.motes)
				{
					if (mote != null)
					{
						mote.Maintain();
					}
				}
			}
			foreach (Sustainer sustainer in this.sustainers)
			{
				if (sustainer != null && !sustainer.Ended)
				{
					sustainer.Maintain();
				}
			}
		}

		// Token: 0x06005A57 RID: 23127 RVA: 0x00213D94 File Offset: 0x00211F94
		private IEnumerable<PsychicRitualParticipant> GenerateParticipants()
		{
			LordJob_HateChant.<GenerateParticipants>d__34 <GenerateParticipants>d__ = new LordJob_HateChant.<GenerateParticipants>d__34(-2);
			<GenerateParticipants>d__.<>4__this = this;
			return <GenerateParticipants>d__;
		}

		// Token: 0x06005A58 RID: 23128 RVA: 0x00213DA4 File Offset: 0x00211FA4
		private IntVec3 GenerateChantingPosition(Pawn pawn, int minDistanceBetween)
		{
			LordJob_HateChant.<>c__DisplayClass35_0 CS$<>8__locals1 = new LordJob_HateChant.<>c__DisplayClass35_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.minDistanceBetween = minDistanceBetween;
			CS$<>8__locals1.minDistFromEdge = LordJob_HateChant.MinDistFromEdgeRange.RandomInRange;
			IntVec3 intVec = CellFinder.RandomClosewalkCellNear(pawn.Position, base.Map, 30, new Predicate<IntVec3>(CS$<>8__locals1.<GenerateChantingPosition>g__Validator|0));
			if (intVec.CloseToEdge(base.Map, CS$<>8__locals1.minDistFromEdge))
			{
				intVec = CellFinder.RandomClosewalkCellNear(pawn.Position, base.Map, 30, new Predicate<IntVec3>(CS$<>8__locals1.<GenerateChantingPosition>g__FallbackValidator|1));
			}
			this.chantingPositions.Add(intVec);
			return intVec;
		}

		// Token: 0x06005A59 RID: 23129 RVA: 0x00213E3C File Offset: 0x0021203C
		private void InitializeSfx()
		{
			this.nextCallTick = Find.TickManager.TicksGame + LordJob_HateChant.CallIntervalSeconds.RandomInRange.SecondsToTicks();
			if (this.smallGroup)
			{
				List<IntVec3> list = new List<IntVec3>();
				foreach (IntVec3 item in this.chantingPositions)
				{
					list.Add(item);
				}
				IntVec3 cell = Gen.AveragePosition(list).ToIntVec3().ClampInsideMap(base.Map);
				this.sustainers.Add(SoundDefOf.HateChant_Sighing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(cell, base.Map, false), MaintenanceType.PerTick)));
				return;
			}
			ValueTuple<int, float> valueTuple = default(ValueTuple<int, float>);
			ValueTuple<int, float> valueTuple2 = default(ValueTuple<int, float>);
			ValueTuple<int, float> valueTuple3 = default(ValueTuple<int, float>);
			ValueTuple<int, float> valueTuple4 = default(ValueTuple<int, float>);
			foreach (IntVec3 intVec in this.chantingPositions)
			{
				if (intVec.x < 15)
				{
					valueTuple3.Item1++;
					valueTuple3.Item2 += (float)intVec.z;
				}
				else if (intVec.x > base.Map.Size.x - 15)
				{
					valueTuple4.Item1++;
					valueTuple4.Item2 += (float)intVec.z;
				}
				else if (intVec.z < 15)
				{
					valueTuple.Item1++;
					valueTuple.Item2 += (float)intVec.x;
				}
				else if (intVec.z > base.Map.Size.z - 15)
				{
					valueTuple2.Item1++;
					valueTuple2.Item2 += (float)intVec.x;
				}
			}
			if (valueTuple.Item1 > 0)
			{
				valueTuple.Item2 /= (float)valueTuple.Item1;
				IntVec3 cell2 = new IntVec3(Mathf.RoundToInt(valueTuple.Item2), 0, 15).ClampInsideMap(base.Map);
				this.sustainers.Add(SoundDefOf.HateChant_Sighing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(cell2, base.Map, false), MaintenanceType.PerTick)));
			}
			if (valueTuple2.Item1 > 0)
			{
				valueTuple2.Item2 /= (float)valueTuple2.Item1;
				IntVec3 cell3 = new IntVec3(Mathf.RoundToInt(valueTuple2.Item2), 0, base.Map.Size.z - 15).ClampInsideMap(base.Map);
				this.sustainers.Add(SoundDefOf.HateChant_Sighing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(cell3, base.Map, false), MaintenanceType.PerTick)));
			}
			if (valueTuple3.Item1 > 0)
			{
				valueTuple3.Item2 /= (float)valueTuple3.Item1;
				IntVec3 cell4 = new IntVec3(15, 0, Mathf.RoundToInt(valueTuple3.Item2)).ClampInsideMap(base.Map);
				this.sustainers.Add(SoundDefOf.HateChant_Sighing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(cell4, base.Map, false), MaintenanceType.PerTick)));
			}
			if (valueTuple4.Item1 > 0)
			{
				valueTuple4.Item2 /= (float)valueTuple4.Item1;
				IntVec3 cell5 = new IntVec3(base.Map.Size.x - 15, 0, Mathf.RoundToInt(valueTuple4.Item2)).ClampInsideMap(base.Map);
				this.sustainers.Add(SoundDefOf.HateChant_Sighing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(cell5, base.Map, false), MaintenanceType.PerTick)));
			}
		}

		// Token: 0x06005A5A RID: 23130 RVA: 0x0021420C File Offset: 0x0021240C
		private void EnableSelfResurrect()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Hediff_DeathRefusal firstHediff = ownedPawns[i].health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
				if (firstHediff != null)
				{
					firstHediff.AIEnabled = true;
				}
			}
			List<Corpse> ownedCorpses = this.lord.ownedCorpses;
			for (int j = 0; j < ownedCorpses.Count; j++)
			{
				Hediff_DeathRefusal firstHediff2 = ownedCorpses[j].InnerPawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
				if (firstHediff2 != null)
				{
					firstHediff2.AIEnabled = true;
					firstHediff2.Notify_PawnDied(null, null);
				}
			}
		}

		// Token: 0x06005A5B RID: 23131 RVA: 0x002142B4 File Offset: 0x002124B4
		private void MakePawnsWait()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				PawnUtility.ForceWait(ownedPawns[i], LordJob_HateChant.PawnWaitBeforeAssaultSeconds.RandomInRange.SecondsToTicks(), null, false, false);
			}
		}

		// Token: 0x06005A5C RID: 23132 RVA: 0x00214300 File Offset: 0x00212500
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lostPawns, "lostPawns", 0, false);
			Scribe_Values.Look<int>(ref this.assaultLossThreshold, "assaultLossThreshold", 0, false);
			Scribe_Values.Look<bool>(ref this.spawningFinished, "spawningFinished", false, false);
			Scribe_Values.Look<bool>(ref this.spawningStarted, "spawningStarted", false, false);
			Scribe_Values.Look<bool>(ref this.smallGroup, "smallGroup", false, false);
			Scribe_Collections.Look<IntVec3>(ref this.chantingPositions, "chantingPositions", LookMode.Value);
		}

		// Token: 0x040033C4 RID: 13252
		private static readonly SimpleCurve MinDistBetweenChantersCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 6f),
				true
			},
			{
				new CurvePoint(30f, 5f),
				true
			},
			{
				new CurvePoint(50f, 4f),
				true
			},
			{
				new CurvePoint(70f, 3f),
				true
			}
		};

		// Token: 0x040033C5 RID: 13253
		public bool spawningFinished;

		// Token: 0x040033C6 RID: 13254
		public bool spawningStarted;

		// Token: 0x040033C7 RID: 13255
		public bool smallGroup;

		// Token: 0x040033C8 RID: 13256
		private int lostPawns;

		// Token: 0x040033C9 RID: 13257
		private int assaultLossThreshold;

		// Token: 0x040033CA RID: 13258
		private HashSet<IntVec3> chantingPositions = new HashSet<IntVec3>();

		// Token: 0x040033CB RID: 13259
		private List<PsychicRitualParticipant> participants = new List<PsychicRitualParticipant>();

		// Token: 0x040033CC RID: 13260
		private List<Sustainer> sustainers = new List<Sustainer>();

		// Token: 0x040033CD RID: 13261
		private readonly List<Mote> motes = new List<Mote>();

		// Token: 0x040033CE RID: 13262
		private readonly Dictionary<IntVec3, Mote> callMotes = new Dictionary<IntVec3, Mote>();

		// Token: 0x040033CF RID: 13263
		private int nextCallTick;

		// Token: 0x040033D0 RID: 13264
		private const string SpawnFinishedSignal = "PositionsReady";

		// Token: 0x040033D1 RID: 13265
		private const int SpawningBatchSize = 5;

		// Token: 0x040033D2 RID: 13266
		private const float SpawningIntervalSeconds = 0.8f;

		// Token: 0x040033D3 RID: 13267
		private const int MaxAssaultLossThreshold = 10;

		// Token: 0x040033D4 RID: 13268
		private const int SustainerDistFromEdge = 15;

		// Token: 0x040033D5 RID: 13269
		private const int GotoToilTimeout = 10800;

		// Token: 0x040033D6 RID: 13270
		private const int CallMoteCleanupInterval = 300;

		// Token: 0x040033D7 RID: 13271
		private static readonly IntRange MinDistFromEdgeRange = new IntRange(5, 25);

		// Token: 0x040033D8 RID: 13272
		private static readonly FloatRange AssaultLossPercent = new FloatRange(0.1f, 0.3f);

		// Token: 0x040033D9 RID: 13273
		private static readonly FloatRange PawnWaitBeforeAssaultSeconds = new FloatRange(0.1f, 6f);

		// Token: 0x040033DA RID: 13274
		private static readonly FloatRange CallIntervalSeconds = new FloatRange(3f, 5f);

		// Token: 0x040033DB RID: 13275
		private static readonly FloatRange CallIntervalSecondsSmallGroup = new FloatRange(7f, 12f);
	}
}
