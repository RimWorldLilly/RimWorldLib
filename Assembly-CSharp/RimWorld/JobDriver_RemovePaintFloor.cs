using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A5F RID: 2655
	public class JobDriver_RemovePaintFloor : JobDriver
	{
		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x0600500F RID: 20495 RVA: 0x001E2D28 File Offset: 0x001E0F28
		private IntVec3 PaintTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		// Token: 0x06005010 RID: 20496 RVA: 0x001E2D49 File Offset: 0x001E0F49
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x001E2D6C File Offset: 0x001E0F6C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RemovePaintFloor.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_RemovePaintFloor.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003123 RID: 12579
		private float removalTime;

		// Token: 0x04003124 RID: 12580
		private const TargetIndex PaintTargetIndex = TargetIndex.A;

		// Token: 0x04003125 RID: 12581
		private const float RemoveTimeSecondsBase = 1f;
	}
}
