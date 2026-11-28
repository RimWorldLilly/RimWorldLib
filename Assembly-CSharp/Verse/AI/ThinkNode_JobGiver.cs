using System;

namespace Verse.AI
{
	// Token: 0x020008F2 RID: 2290
	public abstract class ThinkNode_JobGiver : ThinkNode
	{
		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x0600462D RID: 17965 RVA: 0x001B8841 File Offset: 0x001B6A41
		public virtual string CrawlingReportStringOverride
		{
			get
			{
				return this.crawlingReportStringOverride;
			}
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x001B8849 File Offset: 0x001B6A49
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_JobGiver thinkNode_JobGiver = (ThinkNode_JobGiver)base.DeepCopy(resolve);
			thinkNode_JobGiver.reportStringOverride = this.reportStringOverride;
			thinkNode_JobGiver.crawlingReportStringOverride = this.crawlingReportStringOverride;
			return thinkNode_JobGiver;
		}

		// Token: 0x0600462F RID: 17967
		protected abstract Job TryGiveJob(Pawn pawn);

		// Token: 0x06004630 RID: 17968 RVA: 0x001B8870 File Offset: 0x001B6A70
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			Job job = this.TryGiveJob(pawn);
			if (job == null)
			{
				return ThinkResult.NoJob;
			}
			if (this.reportStringOverride != null)
			{
				job.reportStringOverride = this.reportStringOverride;
			}
			if (this.CrawlingReportStringOverride != null)
			{
				job.crawlingReportStringOverride = this.CrawlingReportStringOverride;
			}
			return new ThinkResult(job, this, null, false);
		}

		// Token: 0x04002D8D RID: 11661
		[MustTranslate]
		protected string reportStringOverride;

		// Token: 0x04002D8E RID: 11662
		[MustTranslate]
		protected string crawlingReportStringOverride;
	}
}
