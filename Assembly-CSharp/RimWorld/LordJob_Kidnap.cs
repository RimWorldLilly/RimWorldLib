using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C71 RID: 3185
	public class LordJob_Kidnap : LordJob
	{
		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x06005A5E RID: 23134 RVA: 0x00214444 File Offset: 0x00212644
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005A60 RID: 23136 RVA: 0x00214450 File Offset: 0x00212650
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_KidnapCover lordToil_KidnapCover = new LordToil_KidnapCover();
			lordToil_KidnapCover.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_KidnapCover);
			LordToil_KidnapCover lordToil_KidnapCover2 = new LordToil_KidnapCover();
			lordToil_KidnapCover2.cover = false;
			lordToil_KidnapCover2.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_KidnapCover2);
			Transition transition = new Transition(lordToil_KidnapCover, lordToil_KidnapCover2, false, true);
			transition.AddTrigger(new Trigger_TicksPassed(1200));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06005A61 RID: 23137 RVA: 0x002144B3 File Offset: 0x002126B3
		public override void ExposeData()
		{
		}
	}
}
