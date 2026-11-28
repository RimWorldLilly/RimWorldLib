using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C5F RID: 3167
	public class LordJob_BestowingCeremony : LordJob_Ritual
	{
		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x060059D3 RID: 22995 RVA: 0x00210B78 File Offset: 0x0020ED78
		public override bool AlwaysShowWeapon
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x060059D4 RID: 22996 RVA: 0x00210B7B File Offset: 0x0020ED7B
		public override IntVec3 Spot
		{
			get
			{
				return this.targetSpot.Cell;
			}
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x060059D5 RID: 22997 RVA: 0x00210B88 File Offset: 0x0020ED88
		public override string RitualLabel
		{
			get
			{
				return "BestowingCeremonyLabel".Translate().CapitalizeFirst();
			}
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x060059D6 RID: 22998 RVA: 0x00210BAC File Offset: 0x0020EDAC
		public override bool AllowStartNewGatherings
		{
			get
			{
				return this.lord.CurLordToil != this.ceremonyToil;
			}
		}

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x060059D7 RID: 22999 RVA: 0x00210BC4 File Offset: 0x0020EDC4
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/BestowCeremony", true);
				}
				return this.icon;
			}
		}

		// Token: 0x060059D8 RID: 23000 RVA: 0x00210BEB File Offset: 0x0020EDEB
		public LordJob_BestowingCeremony()
		{
		}

		// Token: 0x060059D9 RID: 23001 RVA: 0x00210C0C File Offset: 0x0020EE0C
		public LordJob_BestowingCeremony(Pawn bestower, Pawn target, LocalTargetInfo targetSpot, IntVec3 spotCell, Thing shuttle = null, string questEndedSignal = null)
		{
			this.bestower = bestower;
			this.target = target;
			this.targetSpot = targetSpot;
			this.spotCell = spotCell;
			this.shuttle = shuttle;
			this.questEndedSignal = questEndedSignal;
		}

		// Token: 0x060059DA RID: 23002 RVA: 0x00210C64 File Offset: 0x0020EE64
		public override AcceptanceReport AllowsDrafting(Pawn pawn)
		{
			if (this.lord.CurLordToil == this.ceremonyToil)
			{
				return new AcceptanceReport("ParticipatingInRitual".Translate(pawn, this.RitualLabel));
			}
			return true;
		}

		// Token: 0x060059DB RID: 23003 RVA: 0x00210CB0 File Offset: 0x0020EEB0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
			if (!ModLister.CheckRoyalty("Bestowing ceremony"))
			{
				return stateGraph;
			}
			this.outcome = (RitualOutcomeEffectWorker_Bestowing)RitualOutcomeEffectDefOf.BestowingCeremony.GetInstance();
			LordToil_Wait lordToil_Wait = new LordToil_Wait(true);
			stateGraph.AddToil(lordToil_Wait);
			LordToil_Wait lordToil_Wait2 = new LordToil_Wait(true);
			stateGraph.AddToil(lordToil_Wait2);
			LordToil_Wait lordToil_Wait3 = new LordToil_Wait(true);
			stateGraph.AddToil(lordToil_Wait3);
			LordToil_Wait lordToil_Wait4 = new LordToil_Wait(true);
			stateGraph.AddToil(lordToil_Wait4);
			LordToil_BestowingCeremony_MoveInPlace lordToil_BestowingCeremony_MoveInPlace = new LordToil_BestowingCeremony_MoveInPlace(this.spotCell, this.target);
			stateGraph.AddToil(lordToil_BestowingCeremony_MoveInPlace);
			LordToil_BestowingCeremony_Wait lordToil_BestowingCeremony_Wait = new LordToil_BestowingCeremony_Wait(this.target, this.bestower);
			stateGraph.AddToil(lordToil_BestowingCeremony_Wait);
			this.ceremonyToil = new LordToil_BestowingCeremony_Perform(this.target, this.bestower);
			stateGraph.AddToil(this.ceremonyToil);
			this.exitToil = ((this.shuttle == null) ? new LordToil_ExitMap(LocomotionUrgency.Jog, false, false) : new LordToil_EnterShuttleOrLeave(this.shuttle, LocomotionUrgency.Jog, true, true));
			stateGraph.AddToil(this.exitToil);
			TransitionAction_Custom action = new TransitionAction_Custom(delegate()
			{
				this.lord.RemovePawns(this.colonistParticipants);
			});
			Transition transition = new Transition(lordToil_Wait, lordToil_Wait2, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.bestower.Spawned));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Wait2, lordToil_BestowingCeremony_MoveInPlace, false, true);
			transition2.AddTrigger(new Trigger_TicksPassed(600));
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_BestowingCeremony_MoveInPlace, lordToil_BestowingCeremony_Wait, false, true);
			transition3.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.bestower.Position == this.spotCell));
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_BestowingCeremony_Wait, this.ceremonyToil, false, true);
			transition4.AddTrigger(new Trigger_Memo(LordJob_BestowingCeremony.MemoCeremonyStarted));
			transition4.postActions.Add(new TransitionAction_Custom(delegate()
			{
				this.ceremonyStarted = true;
			}));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(this.ceremonyToil, lordToil_Wait4, false, true);
			transition5.AddTrigger(new Trigger_Custom((TriggerSignal s) => s.type == TriggerSignalType.Tick && this.bestower.InMentalState));
			transition5.AddPreAction(action);
			transition5.postActions.Add(new TransitionAction_Custom(delegate()
			{
				this.ceremonyStarted = false;
				this.lord.RemovePawn(this.target);
			}));
			transition5.AddPreAction(new TransitionAction_Custom(delegate()
			{
				Messages.Message("MessageBestowingInterrupted".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
			}));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_Wait4, lordToil_BestowingCeremony_Wait, false, true);
			transition6.AddTrigger(new Trigger_Custom((TriggerSignal s) => s.type == TriggerSignalType.Tick && !this.bestower.InMentalState));
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = new Transition(lordToil_BestowingCeremony_Wait, this.exitToil, false, true);
			transition7.AddTrigger(new Trigger_TicksPassed(30000));
			transition7.AddPreAction(action);
			transition7.AddPostAction(new TransitionAction_Custom(delegate()
			{
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyExpired", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition7, false);
			Transition transition8 = new Transition(this.ceremonyToil, this.exitToil, false, true);
			transition8.AddTrigger(new Trigger_Signal(this.questEndedSignal));
			transition8.AddPreAction(action);
			transition8.AddPreAction(new TransitionAction_Custom(delegate()
			{
				Messages.Message("MessageBestowingInterrupted".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
			}));
			stateGraph.AddTransition(transition8, false);
			Transition transition9 = new Transition(this.ceremonyToil, lordToil_Wait3, false, true);
			transition9.AddTrigger(new Trigger_Memo("CeremonyFinished"));
			transition9.AddPostAction(new TransitionAction_Custom(delegate()
			{
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyDone", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition9, false);
			Transition transition10 = new Transition(lordToil_Wait3, this.exitToil, false, true);
			transition10.AddPreAction(action);
			transition10.AddTrigger(new Trigger_TicksPassed(600));
			stateGraph.AddTransition(transition10, false);
			Transition transition11 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
			transition11.AddSource(lordToil_BestowingCeremony_Wait);
			transition11.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition11.AddPreAction(action);
			stateGraph.AddTransition(transition11, false);
			Transition transition12 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
			transition12.AddSource(lordToil_BestowingCeremony_Wait);
			transition12.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.bestower.Spawned && !this.bestower.CanReach(this.spotCell, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn)));
			transition12.AddPreAction(action);
			transition12.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Messages.Message("MessageBestowingSpotUnreachable".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyFailed", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition12, false);
			if (!this.questEndedSignal.NullOrEmpty())
			{
				Transition transition13 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
				transition13.AddSource(lordToil_BestowingCeremony_Wait);
				transition13.AddSource(lordToil_Wait);
				transition13.AddSource(lordToil_Wait2);
				transition13.AddTrigger(new Trigger_Signal(this.questEndedSignal));
				transition13.AddPreAction(action);
				stateGraph.AddTransition(transition13, false);
			}
			Transition transition14 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
			transition14.AddSource(lordToil_BestowingCeremony_Wait);
			transition14.AddTrigger(new Trigger_Custom(delegate(TriggerSignal signal)
			{
				if (signal.type == TriggerSignalType.Tick && !this.bestower.Dead)
				{
					Hediff firstHediffOfDef = this.bestower.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
					if (firstHediffOfDef != null && firstHediffOfDef.Severity >= 0.35f)
					{
						return true;
					}
					Hediff firstHediffOfDef2 = this.bestower.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke, false);
					if (firstHediffOfDef2 != null && firstHediffOfDef2.Severity >= 0.35f)
					{
						return true;
					}
				}
				return false;
			}));
			transition14.AddPreAction(action);
			Func<Pawn, bool> <>9__15;
			transition14.AddPostAction(new TransitionAction_Custom(delegate()
			{
				CompShuttle compShuttle = compShuttle;
				if (compShuttle != null)
				{
					Func<Pawn, bool> pawnToLeaveBehind;
					if ((pawnToLeaveBehind = <>9__15) == null)
					{
						pawnToLeaveBehind = (<>9__15 = ((Pawn p) => p != this.bestower));
					}
					compShuttle.SetPawnToLeaveBehind(pawnToLeaveBehind);
				}
				this.bestower.Faction.TryAffectGoodwillWith(Faction.OfPlayer, -50, true, true, null, null);
				Messages.Message("MessageBestowingDangerTemperature".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "BeingAttacked", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition14, false);
			Transition transition15 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
			transition15.AddSource(lordToil_BestowingCeremony_Wait);
			transition15.AddTrigger(new Trigger_Custom(delegate(TriggerSignal signal)
			{
				if (signal.type == TriggerSignalType.Tick && !this.bestower.Dead)
				{
					if (ModsConfig.BiotechActive)
					{
						Hediff firstHediffOfDef = this.bestower.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxGasExposure, false);
						if (firstHediffOfDef != null && firstHediffOfDef.Severity >= 0.9f)
						{
							return true;
						}
					}
					Hediff firstHediffOfDef2 = this.bestower.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.LungRotExposure, false);
					if (firstHediffOfDef2 != null && firstHediffOfDef2.Severity >= 0.9f)
					{
						return true;
					}
				}
				return false;
			}));
			transition15.AddPreAction(action);
			Func<Pawn, bool> <>9__18;
			transition15.AddPostAction(new TransitionAction_Custom(delegate()
			{
				CompShuttle compShuttle = compShuttle;
				if (compShuttle != null)
				{
					Func<Pawn, bool> pawnToLeaveBehind;
					if ((pawnToLeaveBehind = <>9__18) == null)
					{
						pawnToLeaveBehind = (<>9__18 = ((Pawn p) => p != this.bestower));
					}
					compShuttle.SetPawnToLeaveBehind(pawnToLeaveBehind);
				}
				this.bestower.Faction.TryAffectGoodwillWith(Faction.OfPlayer, -50, true, true, null, null);
				Messages.Message("MessageBestowingDangerGas".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "BeingAttacked", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition15, false);
			Transition transition16 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
			transition16.AddSource(lordToil_BestowingCeremony_Wait);
			transition16.AddTrigger(new Trigger_PawnHarmed(1f, true, base.Map.ParentFaction, null, null));
			transition16.AddPreAction(action);
			Func<Pawn, bool> <>9__20;
			transition16.AddPostAction(new TransitionAction_Custom(delegate()
			{
				CompShuttle compShuttle = compShuttle;
				if (compShuttle != null)
				{
					Func<Pawn, bool> pawnToLeaveBehind;
					if ((pawnToLeaveBehind = <>9__20) == null)
					{
						pawnToLeaveBehind = (<>9__20 = ((Pawn p) => p != this.bestower));
					}
					compShuttle.SetPawnToLeaveBehind(pawnToLeaveBehind);
				}
				Messages.Message("MessageBestowingDanger".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "BeingAttacked", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition16, false);
			return stateGraph;
		}

		// Token: 0x060059DC RID: 23004 RVA: 0x0021126C File Offset: 0x0020F46C
		public override void Notify_InMentalState(Pawn pawn, MentalStateDef stateDef)
		{
			if (stateDef != MentalStateDefOf.SocialFighting)
			{
				this.lord.Notify_PawnLost(pawn, PawnLostCondition.InMentalState, null);
			}
		}

		// Token: 0x060059DD RID: 23005 RVA: 0x00211298 File Offset: 0x0020F498
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			if (p == this.bestower || p == this.target)
			{
				this.MakeCeremonyFail();
			}
		}

		// Token: 0x060059DE RID: 23006 RVA: 0x002112B2 File Offset: 0x0020F4B2
		public void MakeCeremonyFail()
		{
			QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyFailed", this.lord.Named("SUBJECT"));
			this.outcome.ResetCompDatas();
		}

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x060059DF RID: 23007 RVA: 0x002112E4 File Offset: 0x0020F4E4
		public override IEnumerable<Pawn> PawnsToCountTowardsPresence
		{
			get
			{
				return from p in this.lord.ownedPawns
				where p != this.bestower && p != this.target && p.IsColonist
				select p;
			}
		}

		// Token: 0x060059E0 RID: 23008 RVA: 0x00211304 File Offset: 0x0020F504
		public override void LordJobTick()
		{
			if (this.ritual != null && this.ritual.behavior != null)
			{
				this.ritual.behavior.Tick(this);
			}
			if (this.ceremonyStarted)
			{
				RitualOutcomeEffectWorker_Bestowing ritualOutcomeEffectWorker_Bestowing = this.outcome;
				if (ritualOutcomeEffectWorker_Bestowing != null)
				{
					ritualOutcomeEffectWorker_Bestowing.Tick(this, 1f);
				}
			}
			if (this.lord.ownedPawns.Count == 0)
			{
				base.Map.lordManager.RemoveLord(this.lord);
			}
		}

		// Token: 0x060059E1 RID: 23009 RVA: 0x00211380 File Offset: 0x0020F580
		private static bool CanUseSpot(Pawn bestower, IntVec3 spot)
		{
			return spot.InBounds(bestower.Map) && spot.Standable(bestower.Map) && GenSight.LineOfSight(spot, bestower.Position, bestower.Map) && bestower.CanReach(spot, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x060059E2 RID: 23010 RVA: 0x002113DC File Offset: 0x0020F5DC
		public static IntVec3 TryGetUsableSpotAdjacentToBestower(Pawn bestower)
		{
			foreach (int num in Enumerable.Range(1, 4).InRandomOrder(null))
			{
				IntVec3 intVec = bestower.Position + GenRadial.ManualRadialPattern[num];
				if (LordJob_BestowingCeremony.CanUseSpot(bestower, intVec))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x060059E3 RID: 23011 RVA: 0x00211454 File Offset: 0x0020F654
		public IntVec3 GetSpot()
		{
			IntVec3 result = IntVec3.Invalid;
			if (this.targetSpot.Thing != null)
			{
				IntVec3 interactionCell = this.targetSpot.Thing.InteractionCell;
				IntVec3 intVec = this.spotCell;
				foreach (IntVec3 intVec2 in GenSight.PointsOnLineOfSight(interactionCell, intVec))
				{
					if (!(intVec2 == interactionCell) && !(intVec2 == intVec) && LordJob_BestowingCeremony.CanUseSpot(this.bestower, intVec2))
					{
						result = intVec2;
						break;
					}
				}
			}
			if (result.IsValid)
			{
				return result;
			}
			return LordJob_BestowingCeremony.TryGetUsableSpotAdjacentToBestower(this.bestower);
		}

		// Token: 0x060059E4 RID: 23012 RVA: 0x00211508 File Offset: 0x0020F708
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttending".Translate("BestowingCeremonyLabel".Translate());
		}

		// Token: 0x060059E5 RID: 23013 RVA: 0x00211528 File Offset: 0x0020F728
		public void FinishCeremony(Pawn pawn)
		{
			this.lord.ReceiveMemo("CeremonyFinished");
			this.totalPresenceTmp.Clear();
			foreach (KeyValuePair<Pawn, int> keyValuePair in this.ceremonyToil.Data.presentForTicks)
			{
				if (keyValuePair.Key != null && !keyValuePair.Key.Dead)
				{
					if (!this.totalPresenceTmp.ContainsKey(keyValuePair.Key))
					{
						this.totalPresenceTmp.Add(keyValuePair.Key, keyValuePair.Value);
					}
					else
					{
						Dictionary<Pawn, int> dictionary = this.totalPresenceTmp;
						Pawn key = keyValuePair.Key;
						dictionary[key] += keyValuePair.Value;
					}
				}
			}
			this.totalPresenceTmp.RemoveAll((KeyValuePair<Pawn, int> tp) => tp.Value < 2500);
			this.outcome.Apply(1f, this.totalPresenceTmp, this);
			this.outcome.ResetCompDatas();
		}

		// Token: 0x060059E6 RID: 23014 RVA: 0x00211654 File Offset: 0x0020F854
		public override IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
		{
			LordJob_BestowingCeremony.<GetPawnGizmos>d__45 <GetPawnGizmos>d__ = new LordJob_BestowingCeremony.<GetPawnGizmos>d__45(-2);
			<GetPawnGizmos>d__.<>4__this = this;
			<GetPawnGizmos>d__.<>3__p = p;
			return <GetPawnGizmos>d__;
		}

		// Token: 0x060059E7 RID: 23015 RVA: 0x0021166C File Offset: 0x0020F86C
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.bestower, "bestower", false);
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_TargetInfo.Look(ref this.targetSpot, "targetSpot");
			Scribe_Values.Look<string>(ref this.questEndedSignal, "questEndedSignal", null, false);
			Scribe_Values.Look<IntVec3>(ref this.spotCell, "spotCell", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.ceremonyStarted, "ceremonyStarted", false, false);
			Scribe_Collections.Look<Pawn>(ref this.colonistParticipants, "colonistParticipants", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x0400337A RID: 13178
		public const int ExpirationTicks = 30000;

		// Token: 0x0400337B RID: 13179
		public static readonly string MemoCeremonyStarted = "CeremonyStarted";

		// Token: 0x0400337C RID: 13180
		private const string MemoCeremonyFinished = "CeremonyFinished";

		// Token: 0x0400337D RID: 13181
		public const int WaitTimeTicks = 600;

		// Token: 0x0400337E RID: 13182
		public Pawn bestower;

		// Token: 0x0400337F RID: 13183
		public Pawn target;

		// Token: 0x04003380 RID: 13184
		public LocalTargetInfo targetSpot;

		// Token: 0x04003381 RID: 13185
		public IntVec3 spotCell;

		// Token: 0x04003382 RID: 13186
		public Thing shuttle;

		// Token: 0x04003383 RID: 13187
		public string questEndedSignal;

		// Token: 0x04003384 RID: 13188
		public List<Pawn> colonistParticipants = new List<Pawn>();

		// Token: 0x04003385 RID: 13189
		public bool ceremonyStarted;

		// Token: 0x04003386 RID: 13190
		private LordToil_BestowingCeremony_Perform ceremonyToil;

		// Token: 0x04003387 RID: 13191
		private LordToil exitToil;

		// Token: 0x04003388 RID: 13192
		private RitualOutcomeEffectWorker_Bestowing outcome;

		// Token: 0x04003389 RID: 13193
		private const float HeatstrokeHypothermiaMinSeverityForLeaving = 0.35f;

		// Token: 0x0400338A RID: 13194
		private const int HeatstrokeHypothermiaGoodwillOffset = -50;

		// Token: 0x0400338B RID: 13195
		private const float GasExposireSeverityForLeaving = 0.9f;

		// Token: 0x0400338C RID: 13196
		private Texture2D icon;

		// Token: 0x0400338D RID: 13197
		private Dictionary<Pawn, int> totalPresenceTmp = new Dictionary<Pawn, int>();
	}
}
