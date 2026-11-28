using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200081A RID: 2074
	public class JobDriver_FollowClose : JobDriver
	{
		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x0600413A RID: 16698 RVA: 0x0019E5F8 File Offset: 0x0019C7F8
		private Pawn Followee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x0600413B RID: 16699 RVA: 0x0019E61E File Offset: 0x0019C81E
		private bool CurrentlyWalkingToFollowee
		{
			get
			{
				return this.pawn.pather.Moving && this.pawn.pather.Destination == this.Followee;
			}
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x0019E654 File Offset: 0x0019C854
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x0019E658 File Offset: 0x0019C858
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.job.followRadius <= 0f)
			{
				Log.Error("Follow radius is <= 0. pawn=" + this.pawn.ToStringSafe<Pawn>());
				this.job.followRadius = 10f;
			}
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x0019E6A7 File Offset: 0x0019C8A7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_FollowClose.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_FollowClose.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x0019E6B7 File Offset: 0x0019C8B7
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x0019E6D4 File Offset: 0x0019C8D4
		public static bool FarEnoughAndPossibleToStartJob(Pawn follower, Pawn followee, float radius)
		{
			if (radius <= 0f)
			{
				string text = "Checking follow job with radius <= 0. pawn=" + follower.ToStringSafe<Pawn>();
				if (follower.mindState != null && follower.mindState.duty != null)
				{
					string str = text;
					string str2 = " duty=";
					DutyDef def = follower.mindState.duty.def;
					text = str + str2 + ((def != null) ? def.ToString() : null);
				}
				Log.ErrorOnce(text, follower.thingIDNumber ^ 843254009);
				return false;
			}
			if (!follower.CanReach(followee, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return false;
			}
			float radius2 = radius * 1.2f;
			return !JobDriver_FollowClose.NearFollowee(follower, followee, radius2) || (!JobDriver_FollowClose.NearDestinationOrNotMoving(follower, followee, radius2) && follower.CanReach(followee.pather.LastPassableCellInPath, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn));
		}

		// Token: 0x06004141 RID: 16705 RVA: 0x0019E79C File Offset: 0x0019C99C
		private static bool NearFollowee(Pawn follower, Pawn followee, float radius)
		{
			return follower.Position.AdjacentTo8WayOrInside(followee.Position) || (follower.Position.InHorDistOf(followee.Position, radius) && GenSight.LineOfSight(follower.Position, followee.Position, follower.Map));
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x0019E7F0 File Offset: 0x0019C9F0
		private static bool NearDestinationOrNotMoving(Pawn follower, Pawn followee, float radius)
		{
			if (!followee.pather.Moving)
			{
				return true;
			}
			IntVec3 lastPassableCellInPath = followee.pather.LastPassableCellInPath;
			return !lastPassableCellInPath.IsValid || follower.Position.AdjacentTo8WayOrInside(lastPassableCellInPath) || follower.Position.InHorDistOf(lastPassableCellInPath, radius);
		}

		// Token: 0x04002B23 RID: 11043
		private const TargetIndex FolloweeInd = TargetIndex.A;

		// Token: 0x04002B24 RID: 11044
		private const int CheckPathIntervalTicks = 30;
	}
}
