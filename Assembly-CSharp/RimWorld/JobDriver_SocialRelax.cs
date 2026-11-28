using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009FF RID: 2559
	public class JobDriver_SocialRelax : JobDriver
	{
		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06004D6E RID: 19822 RVA: 0x001DA73C File Offset: 0x001D893C
		private Thing GatherSpotParent
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06004D6F RID: 19823 RVA: 0x001DA760 File Offset: 0x001D8960
		private bool HasChair
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).HasThing;
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06004D70 RID: 19824 RVA: 0x001DA784 File Offset: 0x001D8984
		private bool HasDrink
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C).HasThing;
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06004D71 RID: 19825 RVA: 0x001DA7A8 File Offset: 0x001D89A8
		private IntVec3 ClosestGatherSpotParentCell
		{
			get
			{
				return this.GatherSpotParent.OccupiedRect().ClosestCellTo(this.pawn.Position);
			}
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x001DA7D4 File Offset: 0x001D89D4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			IntVec3 cell;
			if (!this.HasChair || !Toils_Ingest.TryFindFreeSittingSpotOnThing(this.job.GetTarget(TargetIndex.B).Thing, this.pawn, out cell))
			{
				cell = this.job.GetTarget(TargetIndex.B).Cell;
			}
			if (!this.pawn.ReserveSittableOrSpot(cell, this.pawn.CurJob, true))
			{
				return false;
			}
			this.job.SetTarget(TargetIndex.B, cell);
			return !this.HasDrink || this.pawn.Reserve(this.job.GetTarget(TargetIndex.C), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004D73 RID: 19827 RVA: 0x001DA87E File Offset: 0x001D8A7E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_SocialRelax.<MakeNewToils>d__12 <MakeNewToils>d__ = new JobDriver_SocialRelax.<MakeNewToils>d__12(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004D74 RID: 19828 RVA: 0x001DA890 File Offset: 0x001D8A90
		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool flip)
		{
			IntVec3 closestGatherSpotParentCell = this.ClosestGatherSpotParentCell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref flip, closestGatherSpotParentCell, this.pawn);
		}

		// Token: 0x04003067 RID: 12391
		private const TargetIndex GatherSpotParentInd = TargetIndex.A;

		// Token: 0x04003068 RID: 12392
		private const TargetIndex ChairOrSpotInd = TargetIndex.B;

		// Token: 0x04003069 RID: 12393
		private const TargetIndex OptionalIngestibleInd = TargetIndex.C;
	}
}
