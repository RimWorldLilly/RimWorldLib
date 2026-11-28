using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C65 RID: 3173
	public class LordJob_DefendAttackedTraderCaravan : LordJob
	{
		// Token: 0x06005A0F RID: 23055 RVA: 0x002123DE File Offset: 0x002105DE
		public LordJob_DefendAttackedTraderCaravan()
		{
		}

		// Token: 0x06005A10 RID: 23056 RVA: 0x002123E6 File Offset: 0x002105E6
		public LordJob_DefendAttackedTraderCaravan(IntVec3 defendSpot)
		{
			this.defendSpot = defendSpot;
		}

		// Token: 0x06005A11 RID: 23057 RVA: 0x002123F8 File Offset: 0x002105F8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendTraderCaravan lordToil_DefendTraderCaravan = new LordToil_DefendTraderCaravan(this.defendSpot);
			stateGraph.StartingToil = lordToil_DefendTraderCaravan;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			Transition transition = new Transition(lordToil_DefendTraderCaravan, lordToil_ExitMap, false, true);
			transition.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			transition.AddTrigger(new Trigger_TraderAndAllTraderCaravanGuardsLost());
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06005A12 RID: 23058 RVA: 0x00212458 File Offset: 0x00210658
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendSpot, "defendSpot", default(IntVec3), false);
		}

		// Token: 0x040033A1 RID: 13217
		private IntVec3 defendSpot;
	}
}
