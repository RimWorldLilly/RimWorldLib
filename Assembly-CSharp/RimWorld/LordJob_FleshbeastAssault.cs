using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C6D RID: 3181
	public class LordJob_FleshbeastAssault : LordJob
	{
		// Token: 0x06005A35 RID: 23093 RVA: 0x00212BE4 File Offset: 0x00210DE4
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_FleshbeastAssault toil = new LordToil_FleshbeastAssault();
			stateGraph.AddToil(toil);
			return stateGraph;
		}
	}
}
