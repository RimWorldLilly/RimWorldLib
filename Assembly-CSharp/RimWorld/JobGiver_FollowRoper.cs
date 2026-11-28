using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B40 RID: 2880
	public class JobGiver_FollowRoper : ThinkNode_JobGiver
	{
		// Token: 0x06005475 RID: 21621 RVA: 0x001F6264 File Offset: 0x001F4464
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2;
			if (pawn == null)
			{
				pawn2 = null;
			}
			else
			{
				Pawn_RopeTracker roping = pawn.roping;
				pawn2 = ((roping != null) ? roping.RopedByPawn : null);
			}
			Pawn pawn3 = pawn2;
			if (pawn3 == null)
			{
				return null;
			}
			if (!(pawn3.jobs.curDriver is JobDriver_RopeToDestination))
			{
				return null;
			}
			if (!pawn3.CurJob.GetTarget(TargetIndex.B).Cell.IsValid)
			{
				return null;
			}
			if (!pawn3.Spawned || !pawn.CanReach(pawn3, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.FollowRoper, pawn3);
			job.expiryInterval = 140;
			job.checkOverrideOnExpire = true;
			return job;
		}

		// Token: 0x04003281 RID: 12929
		private const int FollowJobExpireInterval = 140;
	}
}
