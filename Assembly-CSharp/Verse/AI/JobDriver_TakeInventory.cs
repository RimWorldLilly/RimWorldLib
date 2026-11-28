using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200081E RID: 2078
	public class JobDriver_TakeInventory : JobDriver
	{
		// Token: 0x06004154 RID: 16724 RVA: 0x0019EBD9 File Offset: 0x0019CDD9
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 10, this.job.count, null, errorOnFailed, false);
		}

		// Token: 0x06004155 RID: 16725 RVA: 0x0019EC07 File Offset: 0x0019CE07
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TakeInventory.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_TakeInventory.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
