using System;

namespace Verse.AI.Group
{
	// Token: 0x0200093F RID: 2367
	public class LordJob_TravelAndExit : LordJob
	{
		// Token: 0x060048C5 RID: 18629 RVA: 0x001C8327 File Offset: 0x001C6527
		public LordJob_TravelAndExit()
		{
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x001C832F File Offset: 0x001C652F
		public LordJob_TravelAndExit(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x001C8340 File Offset: 0x001C6540
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Travel(this.travelDest).CreateGraph()).StartingToil;
			stateGraph.StartingToil = startingToil;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			stateGraph.AddTransition(new Transition(startingToil, lordToil_ExitMap, false, true)
			{
				triggers = 
				{
					new Trigger_Memo("TravelArrived")
				}
			}, false);
			return stateGraph;
		}

		// Token: 0x060048C8 RID: 18632 RVA: 0x001C83AC File Offset: 0x001C65AC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x04002EE2 RID: 12002
		private IntVec3 travelDest;
	}
}
