using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A7D RID: 2685
	public class JobDriver_TakeAndExitMap : JobDriver
	{
		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x060050F5 RID: 20725 RVA: 0x001E5938 File Offset: 0x001E3B38
		protected Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060050F6 RID: 20726 RVA: 0x001E5959 File Offset: 0x001E3B59
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060050F7 RID: 20727 RVA: 0x001E597C File Offset: 0x001E3B7C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TakeAndExitMap.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_TakeAndExitMap.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400316D RID: 12653
		private const TargetIndex ItemInd = TargetIndex.A;

		// Token: 0x0400316E RID: 12654
		private const TargetIndex ExitCellInd = TargetIndex.B;
	}
}
