using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A80 RID: 2688
	public class JobDriver_TakeCountToInventory : JobDriver
	{
		// Token: 0x0600510B RID: 20747 RVA: 0x001E5D1A File Offset: 0x001E3F1A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 10, this.job.count, null, true, false);
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x001E5D49 File Offset: 0x001E3F49
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TakeCountToInventory.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_TakeCountToInventory.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
