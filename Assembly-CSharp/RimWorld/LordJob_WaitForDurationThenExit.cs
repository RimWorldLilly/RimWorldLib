using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020010A1 RID: 4257
	public class LordJob_WaitForDurationThenExit : LordJob
	{
		// Token: 0x06006A83 RID: 27267 RVA: 0x00257327 File Offset: 0x00255527
		public LordJob_WaitForDurationThenExit()
		{
		}

		// Token: 0x06006A84 RID: 27268 RVA: 0x0025732F File Offset: 0x0025552F
		public LordJob_WaitForDurationThenExit(IntVec3 point, int durationTicks)
		{
			this.point = point;
			this.durationTicks = durationTicks;
		}

		// Token: 0x06006A85 RID: 27269 RVA: 0x00257348 File Offset: 0x00255548
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_WanderClose lordToil_WanderClose = new LordToil_WanderClose(this.point);
			stateGraph.AddToil(lordToil_WanderClose);
			stateGraph.StartingToil = lordToil_WanderClose;
			LordToil_ExitMapRandom lordToil_ExitMapRandom = new LordToil_ExitMapRandom();
			stateGraph.AddToil(lordToil_ExitMapRandom);
			Transition transition = new Transition(lordToil_WanderClose, lordToil_ExitMapRandom, false, true);
			transition.AddTrigger(new Trigger_TicksPassed(this.durationTicks));
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06006A86 RID: 27270 RVA: 0x002573B0 File Offset: 0x002555B0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.durationTicks, "durationTicks", 0, false);
		}

		// Token: 0x040043B6 RID: 17334
		public IntVec3 point;

		// Token: 0x040043B7 RID: 17335
		public int durationTicks;
	}
}
