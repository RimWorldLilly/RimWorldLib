using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C63 RID: 3171
	public class LordJob_CreepJoiner : LordJob
	{
		// Token: 0x06005A03 RID: 23043 RVA: 0x00212009 File Offset: 0x00210209
		public LordJob_CreepJoiner()
		{
		}

		// Token: 0x06005A04 RID: 23044 RVA: 0x00212011 File Offset: 0x00210211
		public LordJob_CreepJoiner(IntVec3 idleSpot, Pawn target)
		{
			this.idleSpot = idleSpot;
			this.target = target;
		}

		// Token: 0x06005A05 RID: 23045 RVA: 0x00212028 File Offset: 0x00210228
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.idleSpot);
			stateGraph.AddToil(lordToil_Travel);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_WanderNearby lordToil_WanderNearby = new LordToil_WanderNearby();
			stateGraph.AddToil(lordToil_WanderNearby);
			LordToil_End toil = new LordToil_End();
			stateGraph.AddToil(toil);
			Transition transition = new Transition(lordToil_Travel, lordToil_WanderNearby, false, true);
			transition.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Travel, lordToil_WanderNearby, false, true);
			transition2.AddTrigger(new Trigger_Memo("SpokenTo"));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_WanderNearby, toil, false, true);
			transition3.AddSource(lordToil_Travel);
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x06005A06 RID: 23046 RVA: 0x002120EC File Offset: 0x002102EC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.idleSpot, "idleSpot", default(IntVec3), false);
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		// Token: 0x0400339E RID: 13214
		private IntVec3 idleSpot;

		// Token: 0x0400339F RID: 13215
		private Pawn target;
	}
}
