using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008EA RID: 2282
	public class JobGiver_IdleForever : ThinkNode_JobGiver
	{
		// Token: 0x06004607 RID: 17927 RVA: 0x001B829C File Offset: 0x001B649C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Wait_Downed);
			if (pawn.Deathresting)
			{
				job.forceSleep = true;
			}
			else
			{
				job.expiryInterval = 2500;
			}
			return job;
		}

		// Token: 0x04002D7C RID: 11644
		private const int checkCrawlInterval = 2500;
	}
}
