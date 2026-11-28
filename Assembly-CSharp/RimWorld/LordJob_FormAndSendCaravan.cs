using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C6E RID: 3182
	public class LordJob_FormAndSendCaravan : LordJob
	{
		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x06005A37 RID: 23095 RVA: 0x00212C0B File Offset: 0x00210E0B
		public bool GatheringItemsNow
		{
			get
			{
				return this.lord.CurLordToil == this.gatherItems;
			}
		}

		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x06005A38 RID: 23096 RVA: 0x00212C20 File Offset: 0x00210E20
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x06005A39 RID: 23097 RVA: 0x00212C23 File Offset: 0x00210E23
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x06005A3A RID: 23098 RVA: 0x00212C26 File Offset: 0x00210E26
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x06005A3B RID: 23099 RVA: 0x00212C29 File Offset: 0x00210E29
		public override bool ManagesRopableAnimals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x06005A3C RID: 23100 RVA: 0x00212C2C File Offset: 0x00210E2C
		public IntVec3 ExitSpot
		{
			get
			{
				return this.exitSpot;
			}
		}

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x06005A3D RID: 23101 RVA: 0x00212C34 File Offset: 0x00210E34
		public string Status
		{
			get
			{
				LordToil curLordToil = this.lord.CurLordToil;
				if (curLordToil == this.gatherItems)
				{
					return "FormingCaravanStatus_GatheringItems".Translate();
				}
				if (curLordToil == this.gatherItems_pause)
				{
					return "FormingCaravanStatus_GatheringItems_Pause".Translate();
				}
				if (curLordToil == this.gatherDownedPawns)
				{
					return "FormingCaravanStatus_GatheringDownedPawns".Translate();
				}
				if (curLordToil == this.gatherDownedPawns_pause)
				{
					return "FormingCaravanStatus_GatheringDownedPawns_Pause".Translate();
				}
				if (curLordToil == this.gatherAnimals)
				{
					return "FormingCaravanStatus_GatheringAnimals".Translate();
				}
				if (curLordToil == this.gatherAnimals_pause)
				{
					return "FormingCaravanStatus_GatheringAnimals_Pause".Translate();
				}
				if (curLordToil == this.collectAnimals)
				{
					return "FormingCaravanStatus_GatheringAnimals".Translate();
				}
				if (curLordToil == this.collectAnimals_pause)
				{
					return "FormingCaravanStatus_GatheringAnimals_Pause".Translate();
				}
				if (curLordToil == this.leave)
				{
					return "FormingCaravanStatus_Leaving".Translate();
				}
				if (curLordToil == this.leave_pause)
				{
					return "FormingCaravanStatus_Leaving_Pause".Translate();
				}
				return "FormingCaravanStatus_Waiting".Translate();
			}
		}

		// Token: 0x06005A3E RID: 23102 RVA: 0x00212D56 File Offset: 0x00210F56
		public LordJob_FormAndSendCaravan()
		{
		}

		// Token: 0x06005A3F RID: 23103 RVA: 0x00212D5E File Offset: 0x00210F5E
		public LordJob_FormAndSendCaravan(List<TransferableOneWay> transferables, List<Pawn> downedPawns, IntVec3 meetingPoint, IntVec3 exitSpot, PlanetTile startingTile, PlanetTile destinationTile)
		{
			this.transferables = transferables;
			this.downedPawns = downedPawns;
			this.meetingPoint = meetingPoint;
			this.exitSpot = exitSpot;
			this.startingTile = startingTile;
			this.destinationTile = destinationTile;
		}

		// Token: 0x06005A40 RID: 23104 RVA: 0x00212D94 File Offset: 0x00210F94
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			this.gatherAnimals = new LordToil_PrepareCaravan_GatherAnimals(this.meetingPoint);
			stateGraph.AddToil(this.gatherAnimals);
			this.gatherAnimals_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherAnimals_pause);
			this.gatherItems = new LordToil_PrepareCaravan_GatherItems(this.meetingPoint);
			stateGraph.AddToil(this.gatherItems);
			this.gatherItems_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherItems_pause);
			this.gatherDownedPawns = new LordToil_PrepareCaravan_GatherDownedPawns(this.meetingPoint, this.exitSpot);
			stateGraph.AddToil(this.gatherDownedPawns);
			this.gatherDownedPawns_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.gatherDownedPawns_pause);
			LordToil_PrepareCaravan_Wait lordToil_PrepareCaravan_Wait = new LordToil_PrepareCaravan_Wait(this.meetingPoint);
			stateGraph.AddToil(lordToil_PrepareCaravan_Wait);
			LordToil_PrepareCaravan_Pause lordToil_PrepareCaravan_Pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(lordToil_PrepareCaravan_Pause);
			this.collectAnimals = new LordToil_PrepareCaravan_CollectAnimals(this.exitSpot);
			stateGraph.AddToil(this.collectAnimals);
			this.collectAnimals_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.collectAnimals_pause);
			this.leave = new LordToil_PrepareCaravan_Leave(this.exitSpot);
			stateGraph.AddToil(this.leave);
			this.leave_pause = new LordToil_PrepareCaravan_Pause();
			stateGraph.AddToil(this.leave_pause);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(this.gatherAnimals, this.gatherItems, false, true);
			transition.AddTrigger(new Trigger_Memo("AllAnimalsGathered"));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(this.gatherItems, this.gatherDownedPawns, false, true);
			transition2.AddTrigger(new Trigger_Memo("AllItemsGathered"));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(this.gatherDownedPawns, lordToil_PrepareCaravan_Wait, false, true);
			transition3.AddTrigger(new Trigger_Memo("AllDownedPawnsGathered"));
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_PrepareCaravan_Wait, this.collectAnimals, false, true);
			transition4.AddTrigger(new Trigger_NoPawnsVeryTiredAndSleeping(0f));
			transition4.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(this.collectAnimals, this.leave, false, true);
			transition5.AddTrigger(new Trigger_Memo("AllAnimalsCollected"));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(this.leave, this.collectAnimals, false, true);
			transition6.AddTrigger(new Trigger_Memo("AddedRopeAnimal"));
			transition6.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = new Transition(this.leave, this.collectAnimals, false, true);
			transition7.AddTrigger(new Trigger_Memo("RemovedPawn"));
			transition7.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition7, false);
			Transition transition8 = new Transition(this.leave, lordToil_End, false, true);
			transition8.AddTrigger(new Trigger_Memo("ReadyToExitMap"));
			transition8.AddPreAction(new TransitionAction_Custom(new Action(this.SendCaravan)));
			stateGraph.AddTransition(transition8, false);
			Transition transition9 = this.PauseTransition(this.gatherAnimals, this.gatherAnimals_pause);
			stateGraph.AddTransition(transition9, false);
			Transition transition10 = this.UnpauseTransition(this.gatherAnimals_pause, this.gatherAnimals);
			stateGraph.AddTransition(transition10, false);
			Transition transition11 = this.PauseTransition(this.gatherItems, this.gatherItems_pause);
			stateGraph.AddTransition(transition11, false);
			Transition transition12 = this.UnpauseTransition(this.gatherItems_pause, this.gatherItems);
			stateGraph.AddTransition(transition12, false);
			Transition transition13 = this.PauseTransition(this.gatherDownedPawns, this.gatherDownedPawns_pause);
			stateGraph.AddTransition(transition13, false);
			Transition transition14 = this.UnpauseTransition(this.gatherDownedPawns_pause, this.gatherDownedPawns);
			stateGraph.AddTransition(transition14, false);
			Transition transition15 = this.PauseTransition(this.collectAnimals, this.collectAnimals_pause);
			stateGraph.AddTransition(transition15, false);
			Transition transition16 = this.UnpauseTransition(this.collectAnimals_pause, this.collectAnimals);
			stateGraph.AddTransition(transition16, false);
			Transition transition17 = this.PauseTransition(this.leave, this.leave_pause);
			stateGraph.AddTransition(transition17, false);
			Transition transition18 = this.UnpauseTransition(this.leave_pause, this.collectAnimals);
			stateGraph.AddTransition(transition18, false);
			Transition transition19 = this.PauseTransition(lordToil_PrepareCaravan_Wait, lordToil_PrepareCaravan_Pause);
			stateGraph.AddTransition(transition19, false);
			Transition transition20 = this.UnpauseTransition(lordToil_PrepareCaravan_Pause, lordToil_PrepareCaravan_Wait);
			stateGraph.AddTransition(transition20, false);
			Transition transition21 = new Transition(this.gatherAnimals, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(this.gatherItems, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(this.gatherDownedPawns, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(lordToil_PrepareCaravan_Wait, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(this.collectAnimals, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(this.gatherAnimals_pause, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(this.gatherItems_pause, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(this.gatherDownedPawns_pause, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(lordToil_PrepareCaravan_Pause, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			transition21 = new Transition(this.collectAnimals_pause, this.leave, false, true);
			transition21.AddTrigger(new Trigger_Memo("ForceDepartNow"));
			transition21.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition21, false);
			return stateGraph;
		}

		// Token: 0x06005A41 RID: 23105 RVA: 0x00213424 File Offset: 0x00211624
		public override void LordJobTick()
		{
			base.LordJobTick();
			for (int i = this.downedPawns.Count - 1; i >= 0; i--)
			{
				if (this.downedPawns[i].Destroyed)
				{
					this.downedPawns.RemoveAt(i);
				}
				else if (!this.downedPawns[i].Downed)
				{
					this.lord.AddPawn(this.downedPawns[i]);
					this.downedPawns.RemoveAt(i);
				}
			}
		}

		// Token: 0x06005A42 RID: 23106 RVA: 0x002134A6 File Offset: 0x002116A6
		public override string GetReport(Pawn pawn)
		{
			return "LordReportFormingCaravan".Translate();
		}

		// Token: 0x06005A43 RID: 23107 RVA: 0x002134B8 File Offset: 0x002116B8
		private Transition PauseTransition(LordToil from, LordToil to)
		{
			Transition transition = new Transition(from, to, false, true);
			transition.AddPreAction(new TransitionAction_Message("MessageCaravanFormationPaused".Translate(), MessageTypeDefOf.NegativeEvent, () => this.lord.ownedPawns.FirstOrDefault((Pawn x) => x.InMentalState), null, 1f));
			transition.AddTrigger(new Trigger_MentalState());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			return transition;
		}

		// Token: 0x06005A44 RID: 23108 RVA: 0x00213518 File Offset: 0x00211718
		private Transition UnpauseTransition(LordToil from, LordToil to)
		{
			Transition transition = new Transition(from, to, false, true);
			transition.AddPreAction(new TransitionAction_Message("MessageCaravanFormationUnpaused".Translate(), MessageTypeDefOf.SilentInput, null, 1f, null));
			transition.AddTrigger(new Trigger_NoMentalState());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			return transition;
		}

		// Token: 0x06005A45 RID: 23109 RVA: 0x0021356C File Offset: 0x0021176C
		public override void ExposeData()
		{
			Scribe_Collections.Look<TransferableOneWay>(ref this.transferables, "transferables", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.downedPawns, "downedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<IntVec3>(ref this.meetingPoint, "meetingPoint", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.exitSpot, "exitSpot", default(IntVec3), false);
			Scribe_Values.Look<PlanetTile>(ref this.startingTile, "startingTile", default(PlanetTile), false);
			Scribe_Values.Look<PlanetTile>(ref this.destinationTile, "destinationTile", default(PlanetTile), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.downedPawns.RemoveAll((Pawn x) => x.DestroyedOrNull());
			}
		}

		// Token: 0x06005A46 RID: 23110 RVA: 0x00213640 File Offset: 0x00211840
		private void SendCaravan()
		{
			this.caravanSent = true;
			CaravanFormingUtility.FormAndCreateCaravan(this.lord.ownedPawns.Concat(from x in this.downedPawns
			where JobGiver_PrepareCaravan_GatherDownedPawns.IsDownedPawnNearExitPoint(x, this.exitSpot)
			select x), this.lord.faction, base.Map.Tile, this.startingTile, this.destinationTile);
		}

		// Token: 0x06005A47 RID: 23111 RVA: 0x002136A4 File Offset: 0x002118A4
		public override void Notify_PawnAdded(Pawn p)
		{
			base.Notify_PawnAdded(p);
			Pawn_JobTracker jobs = p.jobs;
			if (jobs != null)
			{
				jobs.SetFormingCaravanTick(false);
			}
			ReachabilityUtility.ClearCacheFor(p);
			if (AnimalPenUtility.NeedsToBeManagedByRope(p))
			{
				if (p.roping.IsRopedByPawn)
				{
					p.roping.BreakAllRopes();
				}
				this.lord.ReceiveMemo("AddedRopeAnimal");
			}
		}

		// Token: 0x06005A48 RID: 23112 RVA: 0x00213700 File Offset: 0x00211900
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			base.Notify_PawnLost(p, condition);
			ReachabilityUtility.ClearCacheFor(p);
			if (!this.caravanSent)
			{
				if ((condition == PawnLostCondition.Incapped || condition == PawnLostCondition.Killed) && p.Downed)
				{
					this.downedPawns.Add(p);
				}
				CaravanFormingUtility.RemovePawnFromCaravan(p, this.lord, false);
				this.lord.ReceiveMemo("RemovedPawn");
			}
		}

		// Token: 0x06005A49 RID: 23113 RVA: 0x0021375C File Offset: 0x0021195C
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return !p.FenceBlocked;
		}

		// Token: 0x040033B2 RID: 13234
		public List<TransferableOneWay> transferables;

		// Token: 0x040033B3 RID: 13235
		public List<Pawn> downedPawns;

		// Token: 0x040033B4 RID: 13236
		private IntVec3 meetingPoint;

		// Token: 0x040033B5 RID: 13237
		private IntVec3 exitSpot;

		// Token: 0x040033B6 RID: 13238
		private PlanetTile startingTile;

		// Token: 0x040033B7 RID: 13239
		private PlanetTile destinationTile;

		// Token: 0x040033B8 RID: 13240
		private bool caravanSent;

		// Token: 0x040033B9 RID: 13241
		private LordToil gatherAnimals;

		// Token: 0x040033BA RID: 13242
		private LordToil gatherAnimals_pause;

		// Token: 0x040033BB RID: 13243
		private LordToil gatherItems;

		// Token: 0x040033BC RID: 13244
		private LordToil gatherItems_pause;

		// Token: 0x040033BD RID: 13245
		private LordToil gatherDownedPawns;

		// Token: 0x040033BE RID: 13246
		private LordToil gatherDownedPawns_pause;

		// Token: 0x040033BF RID: 13247
		private LordToil collectAnimals;

		// Token: 0x040033C0 RID: 13248
		private LordToil collectAnimals_pause;

		// Token: 0x040033C1 RID: 13249
		private LordToil leave;

		// Token: 0x040033C2 RID: 13250
		private LordToil leave_pause;

		// Token: 0x040033C3 RID: 13251
		public const float CustomWakeThreshold = 0.5f;
	}
}
