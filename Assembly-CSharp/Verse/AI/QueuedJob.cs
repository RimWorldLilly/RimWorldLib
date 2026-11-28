using System;

namespace Verse.AI
{
	// Token: 0x0200084B RID: 2123
	public class QueuedJob : IExposable
	{
		// Token: 0x060042B6 RID: 17078 RVA: 0x001A4C6B File Offset: 0x001A2E6B
		public QueuedJob()
		{
		}

		// Token: 0x060042B7 RID: 17079 RVA: 0x001A4C73 File Offset: 0x001A2E73
		public QueuedJob(QueuedJob other) : this(other.job, other.tag)
		{
		}

		// Token: 0x060042B8 RID: 17080 RVA: 0x001A4C87 File Offset: 0x001A2E87
		public QueuedJob(Job job, JobTag? tag)
		{
			this.job = job;
			this.tag = tag;
		}

		// Token: 0x060042B9 RID: 17081 RVA: 0x001A4C9D File Offset: 0x001A2E9D
		public void Cleanup(Pawn pawn, bool canReturnToPool)
		{
			if (pawn != null)
			{
				pawn.ClearReservationsForJob(this.job);
			}
			if (canReturnToPool)
			{
				JobMaker.ReturnToPool(this.job);
			}
			this.job = null;
			this.tag = null;
		}

		// Token: 0x060042BA RID: 17082 RVA: 0x001A4CD0 File Offset: 0x001A2ED0
		public void ExposeData()
		{
			Scribe_Deep.Look<Job>(ref this.job, "job", Array.Empty<object>());
			Scribe_Values.Look<JobTag?>(ref this.tag, "tag", null, false);
		}

		// Token: 0x04002B6F RID: 11119
		public Job job;

		// Token: 0x04002B70 RID: 11120
		public JobTag? tag;
	}
}
