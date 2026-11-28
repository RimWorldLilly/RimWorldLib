using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B13 RID: 2835
	public class JobGiver_WanderInGatheringArea : JobGiver_Wander
	{
		// Token: 0x060053EE RID: 21486 RVA: 0x001F3C04 File Offset: 0x001F1E04
		protected override IntVec3 GetExactWanderDest(Pawn pawn)
		{
			Predicate<IntVec3> cellValidator = (IntVec3 x) => this.allowUnroofed || !x.Roofed(pawn.Map);
			IntVec3 result;
			if (this.desiredRadius > 0f && GatheringsUtility.TryFindRandomCellInGatheringAreaWithRadius(pawn, this.desiredRadius, cellValidator, out result))
			{
				return result;
			}
			if (GatheringsUtility.TryFindRandomCellInGatheringArea(pawn, cellValidator, out result))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x060053EF RID: 21487 RVA: 0x001F3C6E File Offset: 0x001F1E6E
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060053F0 RID: 21488 RVA: 0x001F3C75 File Offset: 0x001F1E75
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_WanderInGatheringArea jobGiver_WanderInGatheringArea = (JobGiver_WanderInGatheringArea)base.DeepCopy(resolve);
			jobGiver_WanderInGatheringArea.allowUnroofed = this.allowUnroofed;
			jobGiver_WanderInGatheringArea.desiredRadius = this.desiredRadius;
			return jobGiver_WanderInGatheringArea;
		}

		// Token: 0x04003246 RID: 12870
		public bool allowUnroofed = true;

		// Token: 0x04003247 RID: 12871
		public float desiredRadius = -1f;
	}
}
