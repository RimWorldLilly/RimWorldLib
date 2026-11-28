using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C69 RID: 3177
	public class LordJob_DevourerAssault : LordJob
	{
		// Token: 0x06005A20 RID: 23072 RVA: 0x00212788 File Offset: 0x00210988
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil toil = new LordToil_DevourerAssault();
			stateGraph.AddToil(toil);
			return stateGraph;
		}
	}
}
