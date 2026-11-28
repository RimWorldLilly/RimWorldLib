using System;

namespace Verse.AI.Group
{
	// Token: 0x0200093C RID: 2364
	public class LordJob_ExitMapBest : LordJob
	{
		// Token: 0x060048B9 RID: 18617 RVA: 0x001C803B File Offset: 0x001C623B
		public LordJob_ExitMapBest()
		{
		}

		// Token: 0x060048BA RID: 18618 RVA: 0x001C804A File Offset: 0x001C624A
		public LordJob_ExitMapBest(LocomotionUrgency locomotion, bool canDig = false, bool canDefendSelf = false)
		{
			this.locomotion = locomotion;
			this.canDig = canDig;
			this.canDefendSelf = canDefendSelf;
		}

		// Token: 0x060048BB RID: 18619 RVA: 0x001C8070 File Offset: 0x001C6270
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(this.locomotion, this.canDig, false);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			if (this.canDefendSelf)
			{
				LordToil_ExitMapFighting lordToil_ExitMapFighting = new LordToil_ExitMapFighting(LocomotionUrgency.Jog, this.canDig, false);
				stateGraph.AddToil(lordToil_ExitMapFighting);
				Transition transition = new Transition(lordToil_ExitMap, lordToil_ExitMapFighting, false, true);
				transition.AddTrigger(new Trigger_PawnHarmed(1f, false, null, null, null));
				transition.AddPostAction(new TransitionAction_WakeAll());
				transition.AddPostAction(new TransitionAction_EndAllJobs());
				stateGraph.AddTransition(transition, false);
			}
			return stateGraph;
		}

		// Token: 0x060048BC RID: 18620 RVA: 0x001C8107 File Offset: 0x001C6307
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.canDefendSelf, "canDefendSelf", false, false);
		}

		// Token: 0x04002ED8 RID: 11992
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x04002ED9 RID: 11993
		private bool canDig;

		// Token: 0x04002EDA RID: 11994
		private bool canDefendSelf;
	}
}
