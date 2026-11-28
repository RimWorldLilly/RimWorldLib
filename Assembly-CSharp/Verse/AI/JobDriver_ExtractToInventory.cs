using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000804 RID: 2052
	public class JobDriver_ExtractToInventory : JobDriver
	{
		// Token: 0x060040AF RID: 16559 RVA: 0x0019BB86 File Offset: 0x00199D86
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x0019BBAA File Offset: 0x00199DAA
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ExtractToInventory.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_ExtractToInventory.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002AEB RID: 10987
		private const TargetIndex ContainerInd = TargetIndex.A;

		// Token: 0x04002AEC RID: 10988
		private const TargetIndex ContentsInd = TargetIndex.B;

		// Token: 0x04002AED RID: 10989
		public const int OpenTicks = 300;
	}
}
