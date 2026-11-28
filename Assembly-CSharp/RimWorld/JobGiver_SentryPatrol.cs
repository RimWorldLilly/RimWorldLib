using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AC0 RID: 2752
	public class JobGiver_SentryPatrol : ThinkNode_JobGiver
	{
		// Token: 0x060052A6 RID: 21158 RVA: 0x001ECCB4 File Offset: 0x001EAEB4
		protected override Job TryGiveJob(Pawn pawn)
		{
			CompSentryDrone comp = pawn.GetComp<CompSentryDrone>();
			if (comp == null)
			{
				return null;
			}
			if (comp.Mode != CompSentryDrone.SentryDroneMode.Patrol)
			{
				return null;
			}
			if (pawn.CurJobDef == JobDefOf.GotoPatrolDest && pawn.CurJob.jobGiver == this)
			{
				LocalTargetInfo destination = pawn.pather.Destination;
				if (destination.IsValid && destination.Cell.DistanceTo(pawn.Position) >= 1f)
				{
					return pawn.CurJob;
				}
			}
			IntVec3 nextPatrolDest = comp.GetNextPatrolDest();
			if (!nextPatrolDest.IsValid)
			{
				return null;
			}
			if (!pawn.CanReach(nextPatrolDest, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.GotoPatrolDest, nextPatrolDest);
		}
	}
}
