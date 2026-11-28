using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AFD RID: 2813
	public class JobGiver_DeliverPawnToCell : JobGiver_GotoTravelDestination
	{
		// Token: 0x060053B0 RID: 21424 RVA: 0x001F2780 File Offset: 0x001F0980
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_DeliverPawnToCell jobGiver_DeliverPawnToCell = (JobGiver_DeliverPawnToCell)base.DeepCopy(resolve);
			jobGiver_DeliverPawnToCell.addArrivalTagIfTargetIsDead = this.addArrivalTagIfTargetIsDead;
			jobGiver_DeliverPawnToCell.addArrivalTagIfInvalidTarget = this.addArrivalTagIfInvalidTarget;
			return jobGiver_DeliverPawnToCell;
		}

		// Token: 0x060053B1 RID: 21425 RVA: 0x001F27A8 File Offset: 0x001F09A8
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
			if ((this.addArrivalTagIfTargetIsDead && pawn2.Dead) || pawn2.Position == pawn.mindState.duty.focus.Cell || (this.addArrivalTagIfInvalidTarget && !pawn.mindState.duty.focus.IsValid))
			{
				RitualUtility.AddArrivalTag(pawn);
				RitualUtility.AddArrivalTag(pawn2);
				return null;
			}
			if (!pawn.CanReach(pawn2, PathEndMode.Touch, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.DeliverToCell, pawn2, pawn.mindState.duty.focus, pawn.mindState.duty.focusThird);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			job.expiryInterval = this.jobMaxDuration;
			job.count = 1;
			job.ritualTag = "Arrived";
			return job;
		}

		// Token: 0x0400322F RID: 12847
		public bool addArrivalTagIfTargetIsDead;

		// Token: 0x04003230 RID: 12848
		public bool addArrivalTagIfInvalidTarget;
	}
}
