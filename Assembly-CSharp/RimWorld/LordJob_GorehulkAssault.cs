using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C6F RID: 3183
	public class LordJob_GorehulkAssault : LordJob
	{
		// Token: 0x06005A4C RID: 23116 RVA: 0x002137B0 File Offset: 0x002119B0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil toil = new LordToil_GorehulkAssault();
			stateGraph.AddToil(toil);
			return stateGraph;
		}

		// Token: 0x06005A4D RID: 23117 RVA: 0x002137CF File Offset: 0x002119CF
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return reason != PawnLostCondition.Incapped;
		}
	}
}
