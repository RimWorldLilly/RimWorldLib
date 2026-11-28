using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B62 RID: 2914
	public class JobGiver_RevenantSleep : ThinkNode_JobGiver
	{
		// Token: 0x060054F7 RID: 21751 RVA: 0x001F9DE8 File Offset: 0x001F7FE8
		protected override Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(JobDefOf.RevenantSleep);
		}
	}
}
