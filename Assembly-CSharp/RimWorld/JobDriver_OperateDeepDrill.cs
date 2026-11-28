using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A5A RID: 2650
	public class JobDriver_OperateDeepDrill : JobDriver
	{
		// Token: 0x06004FEB RID: 20459 RVA: 0x001E2725 File Offset: 0x001E0925
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004FEC RID: 20460 RVA: 0x001E2748 File Offset: 0x001E0948
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_OperateDeepDrill.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_OperateDeepDrill.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
