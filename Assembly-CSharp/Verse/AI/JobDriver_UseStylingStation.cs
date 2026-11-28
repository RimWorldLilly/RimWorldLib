using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200082C RID: 2092
	public class JobDriver_UseStylingStation : JobDriver
	{
		// Token: 0x06004194 RID: 16788 RVA: 0x0019FA3D File Offset: 0x0019DC3D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004195 RID: 16789 RVA: 0x0019FA60 File Offset: 0x0019DC60
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_UseStylingStation.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_UseStylingStation.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
