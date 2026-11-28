using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C92 RID: 3218
	public class LordJob_WanderNest : LordJob
	{
		// Token: 0x06005B17 RID: 23319 RVA: 0x00218484 File Offset: 0x00216684
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_WanderNest toil = new LordToil_WanderNest();
			stateGraph.AddToil(toil);
			return stateGraph;
		}

		// Token: 0x06005B18 RID: 23320 RVA: 0x002184A3 File Offset: 0x002166A3
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return reason != PawnLostCondition.Incapped;
		}
	}
}
