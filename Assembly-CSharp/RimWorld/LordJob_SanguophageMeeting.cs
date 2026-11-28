using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C7C RID: 3196
	public class LordJob_SanguophageMeeting : LordJob
	{
		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x06005A9F RID: 23199 RVA: 0x002155D7 File Offset: 0x002137D7
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005AA0 RID: 23200 RVA: 0x002155DA File Offset: 0x002137DA
		public LordJob_SanguophageMeeting()
		{
		}

		// Token: 0x06005AA1 RID: 23201 RVA: 0x002155E4 File Offset: 0x002137E4
		public LordJob_SanguophageMeeting(IntVec3 targetCell, List<Thing> enemyTargets, int meetingDurationTicks, string outSignalMeetingStarted, string outSignalMeetingEnded, string outSignalAllSanguophagesGone, string inSignalDefend, string inSignalQuestSuccess, string inSignalQuestFail)
		{
			this.targetCell = targetCell;
			this.enemyTargets = enemyTargets;
			this.meetingDurationTicks = meetingDurationTicks;
			this.outSignalMeetingStarted = outSignalMeetingStarted;
			this.outSignalMeetingEnded = outSignalMeetingEnded;
			this.outSignalAllSanguophagesGone = outSignalAllSanguophagesGone;
			this.inSignalDefend = inSignalDefend;
			this.inSignalQuestSuccess = inSignalQuestSuccess;
			this.inSignalQuestFail = inSignalQuestFail;
		}

		// Token: 0x06005AA2 RID: 23202 RVA: 0x0021563C File Offset: 0x0021383C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckBiotech("Sanguophage meeting"))
			{
				return stateGraph;
			}
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.targetCell);
			stateGraph.AddToil(lordToil_Travel);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_SanguophageMeeting lordToil_SanguophageMeeting = new LordToil_SanguophageMeeting(this.targetCell, this.meetingDurationTicks);
			stateGraph.AddToil(lordToil_SanguophageMeeting);
			LordToil_ExitMapRandom lordToil_ExitMapRandom = new LordToil_ExitMapRandom();
			stateGraph.AddToil(lordToil_ExitMapRandom);
			LordToil_ExitMapAndDefendSelf lordToil_ExitMapAndDefendSelf = new LordToil_ExitMapAndDefendSelf();
			stateGraph.AddToil(lordToil_ExitMapAndDefendSelf);
			LordToil_AssaultThings lordToil_AssaultThings = new LordToil_AssaultThings(this.enemyTargets);
			stateGraph.AddToil(lordToil_AssaultThings);
			Transition transition = new Transition(lordToil_Travel, lordToil_SanguophageMeeting, false, true);
			transition.AddTrigger(new Trigger_Memo("TravelArrived"));
			transition.AddPostAction(new TransitionAction_Custom(delegate()
			{
				if (!this.outSignalMeetingStarted.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalMeetingStarted, false));
				}
				Building building = (Building)ThingMaker.MakeThing(ThingDefOf.SanguphageMeetingTorch, null);
				building.SetFaction(this.lord.faction, null);
				if (GenPlace.TryPlaceThing(building, this.targetCell, base.Map, ThingPlaceMode.Near, null, null, null, 1))
				{
					this.lord.AddBuilding(building);
				}
			}));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_SanguophageMeeting, lordToil_ExitMapRandom, false, true);
			transition2.AddTrigger(new Trigger_TicksPassed(this.meetingDurationTicks));
			transition2.AddTrigger(new Trigger_Signal(this.inSignalQuestSuccess));
			transition2.AddTrigger(new Trigger_Signal(this.inSignalQuestFail));
			transition2.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignalMeetingEnded, false));
			}));
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_SanguophageMeeting, lordToil_ExitMapAndDefendSelf, false, true);
			transition3.AddSource(lordToil_Travel);
			transition3.AddSource(lordToil_ExitMapRandom);
			transition3.AddSource(lordToil_AssaultThings);
			transition3.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition3.AddTrigger(new Trigger_PawnKilled());
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_Travel, lordToil_ExitMapAndDefendSelf, false, true);
			transition4.AddSource(lordToil_SanguophageMeeting);
			transition4.AddTrigger(new Trigger_PawnHarmed(1f, true, Faction.OfPlayer, null, null));
			transition4.AddPostAction(new TransitionAction_Custom(delegate()
			{
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "BeingAttacked", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_SanguophageMeeting, lordToil_AssaultThings, false, true);
			transition5.AddTrigger(new Trigger_Signal(this.inSignalDefend));
			transition5.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_AssaultThings, lordToil_ExitMapRandom, false, true);
			transition6.AddTrigger(new Trigger_Signal(this.inSignalQuestSuccess));
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = new Transition(lordToil_SanguophageMeeting, lordToil_ExitMapAndDefendSelf, false, true);
			transition7.AddTrigger(new Trigger_Custom(delegate(TriggerSignal signal)
			{
				if (signal.type == TriggerSignalType.Tick)
				{
					for (int i = 0; i < this.lord.ownedPawns.Count; i++)
					{
						Pawn pawn = this.lord.ownedPawns[i];
						if (!pawn.Dead)
						{
							Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
							if (firstHediffOfDef != null && firstHediffOfDef.Severity >= 0.35f)
							{
								return true;
							}
							Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke, false);
							if (firstHediffOfDef2 != null && firstHediffOfDef2.Severity >= 0.35f)
							{
								return true;
							}
						}
					}
				}
				return false;
			}));
			transition7.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Messages.Message("SanguophagesLeavingTemperature".Translate(), this.lord.ownedPawns, MessageTypeDefOf.NegativeEvent, true);
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "BeingAttacked", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition7, false);
			return stateGraph;
		}

		// Token: 0x06005AA3 RID: 23203 RVA: 0x002158AC File Offset: 0x00213AAC
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			base.Notify_PawnLost(p, condition);
			if ((condition == PawnLostCondition.ExitedMap || condition == PawnLostCondition.Incapped || condition == PawnLostCondition.Killed || condition == PawnLostCondition.ChangedFaction || condition == PawnLostCondition.Vanished) && this.lord.ownedPawns.Count == 0)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignalAllSanguophagesGone, false));
			}
		}

		// Token: 0x06005AA4 RID: 23204 RVA: 0x00215900 File Offset: 0x00213B00
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.targetCell, "targetCell", default(IntVec3), false);
			Scribe_Collections.Look<Thing>(ref this.enemyTargets, "enemyTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.meetingDurationTicks, "meetingDurationTicks", 0, false);
			Scribe_Values.Look<string>(ref this.outSignalMeetingStarted, "outSignalMeetingStarted", null, false);
			Scribe_Values.Look<string>(ref this.outSignalMeetingEnded, "outSignalMeetingEnded", null, false);
			Scribe_Values.Look<string>(ref this.outSignalAllSanguophagesGone, "outSignalAllSanguophagesGone", null, false);
			Scribe_Values.Look<string>(ref this.inSignalDefend, "inSignalDefend", null, false);
			Scribe_Values.Look<string>(ref this.inSignalQuestSuccess, "inSignalQuestSuccess", null, false);
			Scribe_Values.Look<string>(ref this.inSignalQuestFail, "inSignalQuestFail", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.enemyTargets.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x040033F8 RID: 13304
		public IntVec3 targetCell;

		// Token: 0x040033F9 RID: 13305
		public List<Thing> enemyTargets;

		// Token: 0x040033FA RID: 13306
		public int meetingDurationTicks;

		// Token: 0x040033FB RID: 13307
		public string outSignalMeetingStarted;

		// Token: 0x040033FC RID: 13308
		public string outSignalMeetingEnded;

		// Token: 0x040033FD RID: 13309
		public string outSignalAllSanguophagesGone;

		// Token: 0x040033FE RID: 13310
		public string inSignalDefend;

		// Token: 0x040033FF RID: 13311
		public string inSignalQuestSuccess;

		// Token: 0x04003400 RID: 13312
		public string inSignalQuestFail;

		// Token: 0x04003401 RID: 13313
		private const float HeatstrokeHypothermiaMinSeverityForLeaving = 0.35f;
	}
}
