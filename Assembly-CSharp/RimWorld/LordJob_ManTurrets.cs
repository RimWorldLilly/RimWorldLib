using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C74 RID: 3188
	public class LordJob_ManTurrets : LordJob
	{
		// Token: 0x06005A70 RID: 23152 RVA: 0x0021458E File Offset: 0x0021278E
		public override StateGraph CreateGraph()
		{
			return new StateGraph
			{
				StartingToil = new LordToil_ManClosestTurrets()
			};
		}
	}
}
