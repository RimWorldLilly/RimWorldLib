using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B0D RID: 2829
	public class JobGiver_PrepareSkylantern : JobGiver_GotoAndStandSociallyActive
	{
		// Token: 0x060053DF RID: 21471 RVA: 0x001F37A8 File Offset: 0x001F19A8
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 dest = base.GetDest(pawn);
			Job job = JobMaker.MakeJob(JobDefOf.PrepareSkylantern, dest);
			job.locomotionUrgency = this.locomotionUrgency;
			job.expiryInterval = this.expiryInterval;
			job.checkOverrideOnExpire = true;
			job.thingDefToCarry = this.def;
			job.count = this.count;
			return job;
		}

		// Token: 0x060053E0 RID: 21472 RVA: 0x001F3804 File Offset: 0x001F1A04
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_PrepareSkylantern jobGiver_PrepareSkylantern = (JobGiver_PrepareSkylantern)base.DeepCopy(resolve);
			jobGiver_PrepareSkylantern.def = this.def;
			jobGiver_PrepareSkylantern.count = this.count;
			return jobGiver_PrepareSkylantern;
		}

		// Token: 0x04003241 RID: 12865
		public ThingDef def;

		// Token: 0x04003242 RID: 12866
		public int count = 1;
	}
}
