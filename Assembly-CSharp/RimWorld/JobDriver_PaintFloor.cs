using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A5E RID: 2654
	public class JobDriver_PaintFloor : JobDriver
	{
		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x06005003 RID: 20483 RVA: 0x001E2B04 File Offset: 0x001E0D04
		private IntVec3 PaintTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		// Token: 0x06005004 RID: 20484 RVA: 0x001E2B28 File Offset: 0x001E0D28
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, ReservationLayerDefOf.Floor);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06005005 RID: 20485 RVA: 0x001E2B7A File Offset: 0x001E0D7A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PaintFloor.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_PaintFloor.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06005006 RID: 20486 RVA: 0x001E2B8A File Offset: 0x001E0D8A
		private IEnumerable<Toil> CollectDyeToils()
		{
			return new JobDriver_PaintFloor.<CollectDyeToils>d__8(-2);
		}

		// Token: 0x06005007 RID: 20487 RVA: 0x001E2B93 File Offset: 0x001E0D93
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.paintingTime, "paintingTime", 0f, false);
		}

		// Token: 0x0400311F RID: 12575
		private float paintingTime;

		// Token: 0x04003120 RID: 12576
		private const TargetIndex PaintTargetIndex = TargetIndex.A;

		// Token: 0x04003121 RID: 12577
		private const TargetIndex DyeIndex = TargetIndex.B;

		// Token: 0x04003122 RID: 12578
		private const float PaintTimeSecondsBase = 1f;
	}
}
