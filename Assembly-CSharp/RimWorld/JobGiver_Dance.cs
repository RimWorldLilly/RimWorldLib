using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AFA RID: 2810
	public class JobGiver_Dance : ThinkNode_JobGiver
	{
		// Token: 0x060053A8 RID: 21416 RVA: 0x001F250B File Offset: 0x001F070B
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Dance jobGiver_Dance = (JobGiver_Dance)base.DeepCopy(resolve);
			jobGiver_Dance.ticksRange = this.ticksRange;
			return jobGiver_Dance;
		}

		// Token: 0x060053A9 RID: 21417 RVA: 0x001F2525 File Offset: 0x001F0725
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Dance);
			job.expiryInterval = this.ticksRange.RandomInRange;
			return job;
		}

		// Token: 0x0400322B RID: 12843
		public IntRange ticksRange = new IntRange(300, 600);
	}
}
