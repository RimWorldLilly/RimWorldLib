using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009EA RID: 2538
	public class JobDriver_Spectate : JobDriver
	{
		// Token: 0x06004CE7 RID: 19687 RVA: 0x001D85F4 File Offset: 0x001D67F4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.ReserveSittableOrSpot(this.job.GetTarget(TargetIndex.A).Cell, this.job, errorOnFailed);
		}

		// Token: 0x06004CE8 RID: 19688 RVA: 0x001D8627 File Offset: 0x001D6827
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Spectate.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_Spectate.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003036 RID: 12342
		private const TargetIndex MySpotOrChairInd = TargetIndex.A;

		// Token: 0x04003037 RID: 12343
		private const TargetIndex WatchTargetInd = TargetIndex.B;

		// Token: 0x04003038 RID: 12344
		private const TargetIndex ChairInd = TargetIndex.C;
	}
}
