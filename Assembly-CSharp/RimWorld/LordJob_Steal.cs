using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C89 RID: 3209
	public class LordJob_Steal : LordJob
	{
		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x06005AEC RID: 23276 RVA: 0x00216F9E File Offset: 0x0021519E
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x00216FA4 File Offset: 0x002151A4
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_StealCover lordToil_StealCover = new LordToil_StealCover();
			lordToil_StealCover.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_StealCover);
			LordToil_StealCover lordToil_StealCover2 = new LordToil_StealCover();
			lordToil_StealCover2.cover = false;
			lordToil_StealCover2.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_StealCover2);
			Transition transition = new Transition(lordToil_StealCover, lordToil_StealCover2, false, true);
			transition.AddTrigger(new Trigger_TicksPassedAndNoRecentHarm(1200));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}
	}
}
