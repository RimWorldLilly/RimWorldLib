using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B12 RID: 2834
	public class JobGiver_StandAndBeSociallyActive : ThinkNode_JobGiver
	{
		// Token: 0x060053EB RID: 21483 RVA: 0x001F3B7F File Offset: 0x001F1D7F
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_StandAndBeSociallyActive jobGiver_StandAndBeSociallyActive = (JobGiver_StandAndBeSociallyActive)base.DeepCopy(resolve);
			jobGiver_StandAndBeSociallyActive.ticksRange = this.ticksRange;
			jobGiver_StandAndBeSociallyActive.lookDirection = this.lookDirection;
			jobGiver_StandAndBeSociallyActive.maintainFacing = this.maintainFacing;
			return jobGiver_StandAndBeSociallyActive;
		}

		// Token: 0x060053EC RID: 21484 RVA: 0x001F3BB1 File Offset: 0x001F1DB1
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.StandAndBeSociallyActive);
			job.expiryInterval = this.ticksRange.RandomInRange;
			job.lookDirection = this.lookDirection;
			job.forceMaintainFacing = this.maintainFacing;
			return job;
		}

		// Token: 0x04003243 RID: 12867
		public IntRange ticksRange = new IntRange(300, 600);

		// Token: 0x04003244 RID: 12868
		public Direction8Way lookDirection;

		// Token: 0x04003245 RID: 12869
		public bool maintainFacing;
	}
}
