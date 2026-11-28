using System;

namespace Verse.AI.Group
{
	// Token: 0x0200093E RID: 2366
	public class LordJob_Travel : LordJob
	{
		// Token: 0x060048C1 RID: 18625 RVA: 0x001C823F File Offset: 0x001C643F
		public LordJob_Travel()
		{
		}

		// Token: 0x060048C2 RID: 18626 RVA: 0x001C8247 File Offset: 0x001C6447
		public LordJob_Travel(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x060048C3 RID: 18627 RVA: 0x001C8258 File Offset: 0x001C6458
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.travelDest);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(false);
			stateGraph.AddToil(lordToil_DefendPoint);
			Transition transition = new Transition(lordToil_Travel, lordToil_DefendPoint, false, true);
			transition.AddTrigger(new Trigger_PawnHarmed(1f, false, null, null, null));
			transition.AddPreAction(new TransitionAction_SetDefendLocalGroup());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_DefendPoint, lordToil_Travel, false, true);
			transition2.AddTrigger(new Trigger_TicksPassedWithoutHarm(1200));
			transition2.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			stateGraph.AddTransition(transition2, false);
			return stateGraph;
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x001C8300 File Offset: 0x001C6500
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x04002EE1 RID: 12001
		private IntVec3 travelDest;
	}
}
