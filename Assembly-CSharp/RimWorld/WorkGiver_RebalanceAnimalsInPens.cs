using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BBC RID: 3004
	public class WorkGiver_RebalanceAnimalsInPens : WorkGiver_TakeToPen
	{
		// Token: 0x06005696 RID: 22166 RVA: 0x00202E0C File Offset: 0x0020100C
		public WorkGiver_RebalanceAnimalsInPens()
		{
			this.ropingPriority = RopingPriority.Balanced;
		}
	}
}
