using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B08 RID: 2824
	public class JobGiver_GotoAndStandSociallyActive : ThinkNode_JobGiver
	{
		// Token: 0x060053D2 RID: 21458 RVA: 0x001F33FC File Offset: 0x001F15FC
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 dest = this.GetDest(pawn);
			Job job = JobMaker.MakeJob(JobDefOf.GotoAndBeSociallyActive, dest, dest + pawn.Rotation.FacingCell);
			job.locomotionUrgency = this.locomotionUrgency;
			job.expiryInterval = this.expiryInterval;
			job.checkOverrideOnExpire = true;
			return job;
		}

		// Token: 0x060053D3 RID: 21459 RVA: 0x001F345C File Offset: 0x001F165C
		public IntVec3 GetDest(Pawn pawn)
		{
			Predicate<IntVec3> validatorRelaxed = (IntVec3 x) => this.allowUnroofed || !x.Roofed(pawn.Map);
			Predicate<IntVec3> cellValidator = delegate(IntVec3 x)
			{
				if (!RitualUtility.GoodSpectateCellForRitual(x, pawn, pawn.Map))
				{
					return false;
				}
				if (this.minDistanceToOtherReservedCell > 0)
				{
					foreach (IntVec3 c in GenRadial.RadialCellsAround(x, (float)this.minDistanceToOtherReservedCell, true))
					{
						if (!pawn.CanReserveAndReach(c, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false))
						{
							return false;
						}
					}
				}
				return validatorRelaxed(x);
			};
			IntVec3 result;
			if (this.desiredRadius > 0f && GatheringsUtility.TryFindRandomCellInGatheringAreaWithRadius(pawn, this.desiredRadius, cellValidator, out result))
			{
				return result;
			}
			if (GatheringsUtility.TryFindRandomCellInGatheringArea(pawn, validatorRelaxed, out result))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x060053D4 RID: 21460 RVA: 0x001F34E0 File Offset: 0x001F16E0
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GotoAndStandSociallyActive jobGiver_GotoAndStandSociallyActive = (JobGiver_GotoAndStandSociallyActive)base.DeepCopy(resolve);
			jobGiver_GotoAndStandSociallyActive.locomotionUrgency = this.locomotionUrgency;
			jobGiver_GotoAndStandSociallyActive.allowUnroofed = this.allowUnroofed;
			jobGiver_GotoAndStandSociallyActive.desiredRadius = this.desiredRadius;
			jobGiver_GotoAndStandSociallyActive.minDistanceToOtherReservedCell = this.minDistanceToOtherReservedCell;
			jobGiver_GotoAndStandSociallyActive.expiryInterval = this.expiryInterval;
			return jobGiver_GotoAndStandSociallyActive;
		}

		// Token: 0x0400323C RID: 12860
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x0400323D RID: 12861
		public bool allowUnroofed = true;

		// Token: 0x0400323E RID: 12862
		protected int expiryInterval = -1;

		// Token: 0x0400323F RID: 12863
		public float desiredRadius = -1f;

		// Token: 0x04003240 RID: 12864
		public int minDistanceToOtherReservedCell = -1;
	}
}
