using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000807 RID: 2055
	public class JobDriver_HaulToAtomizer : JobDriver
	{
		// Token: 0x060040B9 RID: 16569 RVA: 0x0019BD38 File Offset: 0x00199F38
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x0019BD87 File Offset: 0x00199F87
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_HaulToAtomizer.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_HaulToAtomizer.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002AF1 RID: 10993
		private const TargetIndex AtomizerInd = TargetIndex.A;

		// Token: 0x04002AF2 RID: 10994
		private const TargetIndex WastepackInd = TargetIndex.B;

		// Token: 0x04002AF3 RID: 10995
		private const TargetIndex ChargerCellInd = TargetIndex.C;
	}
}
