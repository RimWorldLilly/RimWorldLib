using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C82 RID: 3202
	public class LordJob_SightstealerAssault : LordJob
	{
		// Token: 0x06005AC0 RID: 23232 RVA: 0x002161DC File Offset: 0x002143DC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil toil = new LordToil_SightstealerAssault();
			stateGraph.AddToil(toil);
			return stateGraph;
		}
	}
}
