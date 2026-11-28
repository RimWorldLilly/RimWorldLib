using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B0A RID: 2826
	public class JobGiver_HateChant : ThinkNode_JobGiver
	{
		// Token: 0x060053D8 RID: 21464 RVA: 0x001F360C File Offset: 0x001F180C
		protected override Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(JobDefOf.HateChanting);
		}
	}
}
