using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009B3 RID: 2483
	public class JobDriver_Unrope : JobDriver
	{
		// Token: 0x06004B32 RID: 19250 RVA: 0x001D2C8A File Offset: 0x001D0E8A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004B33 RID: 19251 RVA: 0x001D2CAE File Offset: 0x001D0EAE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Unrope.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_Unrope.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002FB8 RID: 12216
		private const TargetIndex AnimalInd = TargetIndex.A;
	}
}
