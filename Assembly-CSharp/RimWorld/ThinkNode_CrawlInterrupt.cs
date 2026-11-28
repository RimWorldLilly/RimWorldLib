using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D89 RID: 3465
	public class ThinkNode_CrawlInterrupt : ThinkNode_Priority
	{
		// Token: 0x06005F9B RID: 24475 RVA: 0x0022C9E4 File Offset: 0x0022ABE4
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			pawn.mindState.nextMoveOrderIsWait = false;
			ThinkResult thinkResult = base.TryIssueJobPackage(pawn, jobParams);
			if (thinkResult == ThinkResult.NoJob)
			{
				return ThinkResult.NoJob;
			}
			if (pawn.mindState.nextMoveOrderIsCrawlBreak)
			{
				pawn.mindState.nextMoveOrderIsCrawlBreak = false;
				Job job = JobMaker.MakeJob(JobDefOf.Wait_Downed);
				job.expiryInterval = this.ticksBetweenCrawlsRange.RandomInRange;
				return new ThinkResult(job, this, null, false);
			}
			pawn.mindState.nextMoveOrderIsCrawlBreak = true;
			thinkResult.Job.expiryInterval = this.crawlDurationTicksRange.RandomInRange;
			return thinkResult;
		}

		// Token: 0x040035D1 RID: 13777
		private IntRange crawlDurationTicksRange = new IntRange(720, 1440);

		// Token: 0x040035D2 RID: 13778
		private IntRange ticksBetweenCrawlsRange = new IntRange(240, 480);
	}
}
