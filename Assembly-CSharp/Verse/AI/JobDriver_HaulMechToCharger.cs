using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000806 RID: 2054
	public class JobDriver_HaulMechToCharger : JobDriver
	{
		// Token: 0x060040B5 RID: 16565 RVA: 0x0019BC28 File Offset: 0x00199E28
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x0019BC7D File Offset: 0x00199E7D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_HaulMechToCharger.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_HaulMechToCharger.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002AEE RID: 10990
		private const TargetIndex MechInd = TargetIndex.A;

		// Token: 0x04002AEF RID: 10991
		private const TargetIndex ChargerInd = TargetIndex.B;

		// Token: 0x04002AF0 RID: 10992
		private const TargetIndex ChargerCellInd = TargetIndex.C;
	}
}
