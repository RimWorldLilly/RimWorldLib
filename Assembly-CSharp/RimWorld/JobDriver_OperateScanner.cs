using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A5B RID: 2651
	public class JobDriver_OperateScanner : JobDriver
	{
		// Token: 0x06004FEE RID: 20462 RVA: 0x001E2760 File Offset: 0x001E0960
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x001E2783 File Offset: 0x001E0983
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_OperateScanner.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_OperateScanner.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
