using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B22 RID: 2850
	public class JobGiver_StandAndStare : ThinkNode_JobGiver
	{
		// Token: 0x06005419 RID: 21529 RVA: 0x001F47AE File Offset: 0x001F29AE
		public override ThinkNode DeepCopy(bool resolve)
		{
			JobGiver_StandAndStare jobGiver_StandAndStare = (JobGiver_StandAndStare)base.DeepCopy(resolve);
			jobGiver_StandAndStare.locomotionUrgency = this.locomotionUrgency;
			jobGiver_StandAndStare.maxDanger = this.maxDanger;
			return jobGiver_StandAndStare;
		}

		// Token: 0x0600541A RID: 21530 RVA: 0x001F47D4 File Offset: 0x001F29D4
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn_MindState mindState = pawn.mindState;
			LocalTargetInfo? localTargetInfo;
			if (mindState == null)
			{
				localTargetInfo = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				localTargetInfo = ((duty != null) ? new LocalTargetInfo?(duty.focusSecond) : null);
			}
			LocalTargetInfo? localTargetInfo2 = localTargetInfo;
			if (localTargetInfo2 != null)
			{
				LocalTargetInfo localTargetInfo3 = localTargetInfo2.GetValueOrDefault().Cell;
				if (pawn.Position != localTargetInfo3.Cell)
				{
					return null;
				}
			}
			if (pawn.Downed)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.StandAndStare);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			return job;
		}

		// Token: 0x04003258 RID: 12888
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x04003259 RID: 12889
		protected Danger maxDanger = Danger.Some;
	}
}
