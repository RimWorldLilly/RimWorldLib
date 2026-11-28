using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AD7 RID: 2775
	public abstract class JobGiver_AIFollowPawn : ThinkNode_JobGiver
	{
		// Token: 0x06005325 RID: 21285
		protected abstract Pawn GetFollowee(Pawn pawn);

		// Token: 0x06005326 RID: 21286
		protected abstract float GetRadius(Pawn pawn);

		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x06005327 RID: 21287 RVA: 0x001EF755 File Offset: 0x001ED955
		protected virtual int FollowJobExpireInterval
		{
			get
			{
				return 140;
			}
		}

		// Token: 0x06005328 RID: 21288 RVA: 0x001EF75C File Offset: 0x001ED95C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn followee = this.GetFollowee(pawn);
			if (followee == null)
			{
				Type type = base.GetType();
				Log.Error(((type != null) ? type.ToString() : null) + " has null followee. pawn=" + pawn.ToStringSafe<Pawn>());
				return null;
			}
			if (!followee.Spawned || !pawn.CanReach(followee, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			float radius = this.GetRadius(pawn);
			if (!JobDriver_FollowClose.FarEnoughAndPossibleToStartJob(pawn, followee, radius))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.FollowClose, followee);
			job.expiryInterval = this.FollowJobExpireInterval;
			job.checkOverrideOnExpire = true;
			job.followRadius = radius;
			return job;
		}
	}
}
