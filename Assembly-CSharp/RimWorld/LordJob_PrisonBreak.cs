using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C78 RID: 3192
	public class LordJob_PrisonBreak : LordJob
	{
		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x06005A88 RID: 23176 RVA: 0x00214E71 File Offset: 0x00213071
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x06005A89 RID: 23177 RVA: 0x00214E74 File Offset: 0x00213074
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005A8A RID: 23178 RVA: 0x00214E77 File Offset: 0x00213077
		public LordJob_PrisonBreak()
		{
		}

		// Token: 0x06005A8B RID: 23179 RVA: 0x00214E86 File Offset: 0x00213086
		public LordJob_PrisonBreak(IntVec3 groupUpLoc, IntVec3 exitPoint, int sapperThingID)
		{
			this.groupUpLoc = groupUpLoc;
			this.exitPoint = exitPoint;
			this.sapperThingID = sapperThingID;
		}

		// Token: 0x06005A8C RID: 23180 RVA: 0x00214EAC File Offset: 0x002130AC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.groupUpLoc);
			lordToil_Travel.maxDanger = Danger.Deadly;
			lordToil_Travel.useAvoidGrid = true;
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_PrisonerEscape lordToil_PrisonerEscape = new LordToil_PrisonerEscape(this.exitPoint, this.sapperThingID);
			lordToil_PrisonerEscape.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_PrisonerEscape);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, false, false);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMap lordToil_ExitMap2 = new LordToil_ExitMap(LocomotionUrgency.Jog, true, false);
			stateGraph.AddToil(lordToil_ExitMap2);
			Transition transition = new Transition(lordToil_Travel, lordToil_ExitMap2, false, true);
			transition.AddSources(new LordToil[]
			{
				lordToil_PrisonerEscape,
				lordToil_ExitMap
			});
			transition.AddTrigger(new Trigger_PawnCannotReachMapEdge());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_ExitMap2, lordToil_ExitMap, false, true);
			transition2.AddTrigger(new Trigger_PawnCanReachMapEdge());
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_Travel, lordToil_PrisonerEscape, false, true);
			transition3.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_Travel, lordToil_PrisonerEscape, false, true);
			transition4.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_PrisonerEscape, lordToil_PrisonerEscape, true, true);
			transition5.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
			transition5.AddTrigger(new Trigger_PawnHarmed(1f, false, null, null, null));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_PrisonerEscape, lordToil_ExitMap, false, true);
			transition6.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition6, false);
			return stateGraph;
		}

		// Token: 0x06005A8D RID: 23181 RVA: 0x0021503C File Offset: 0x0021323C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.groupUpLoc, "groupUpLoc", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.exitPoint, "exitPoint", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.sapperThingID, "sapperThingID", -1, false);
		}

		// Token: 0x06005A8E RID: 23182 RVA: 0x00215095 File Offset: 0x00213295
		public override void Notify_PawnAdded(Pawn p)
		{
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x06005A8F RID: 23183 RVA: 0x0021509D File Offset: 0x0021329D
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x06005A90 RID: 23184 RVA: 0x002150A5 File Offset: 0x002132A5
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return true;
		}

		// Token: 0x06005A91 RID: 23185 RVA: 0x002150A8 File Offset: 0x002132A8
		public override bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			Pawn pawn = target as Pawn;
			if (pawn == null)
			{
				return true;
			}
			MentalStateDef mentalStateDef = pawn.MentalStateDef;
			return mentalStateDef == null || !mentalStateDef.escapingPrisonersIgnore;
		}

		// Token: 0x040033EC RID: 13292
		private IntVec3 groupUpLoc;

		// Token: 0x040033ED RID: 13293
		private IntVec3 exitPoint;

		// Token: 0x040033EE RID: 13294
		private int sapperThingID = -1;
	}
}
