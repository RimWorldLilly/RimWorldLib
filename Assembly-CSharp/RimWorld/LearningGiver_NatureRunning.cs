using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B91 RID: 2961
	public class LearningGiver_NatureRunning : LearningGiver
	{
		// Token: 0x060055DD RID: 21981 RVA: 0x001FF51C File Offset: 0x001FD71C
		public override bool CanDo(Pawn pawn)
		{
			LocalTargetInfo localTargetInfo;
			return base.CanDo(pawn) && NatureRunningUtility.TryFindNatureInterestTarget(pawn, out localTargetInfo);
		}

		// Token: 0x060055DE RID: 21982 RVA: 0x001FF53C File Offset: 0x001FD73C
		public override Job TryGiveJob(Pawn pawn)
		{
			LocalTargetInfo targetA;
			if (!NatureRunningUtility.TryFindNatureInterestTarget(pawn, out targetA))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(this.def.jobDef, targetA);
			job.locomotionUrgency = LocomotionUrgency.Sprint;
			return job;
		}
	}
}
