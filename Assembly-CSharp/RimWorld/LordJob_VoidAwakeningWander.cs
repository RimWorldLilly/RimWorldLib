using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C90 RID: 3216
	public class LordJob_VoidAwakeningWander : LordJob
	{
		// Token: 0x06005B10 RID: 23312 RVA: 0x002183AC File Offset: 0x002165AC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil toil = new LordToil_VoidAwakeningWander();
			stateGraph.AddToil(toil);
			return stateGraph;
		}
	}
}
