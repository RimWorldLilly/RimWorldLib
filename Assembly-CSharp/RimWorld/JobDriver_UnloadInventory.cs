using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A85 RID: 2693
	public class JobDriver_UnloadInventory : JobDriver
	{
		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x06005133 RID: 20787 RVA: 0x001E64E0 File Offset: 0x001E46E0
		private Pawn OtherPawn
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Pawn;
			}
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x001E6501 File Offset: 0x001E4701
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.OtherPawn, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x001E6524 File Offset: 0x001E4724
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_UnloadInventory.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_UnloadInventory.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400317C RID: 12668
		private const TargetIndex OtherPawnInd = TargetIndex.A;

		// Token: 0x0400317D RID: 12669
		private const TargetIndex ItemToHaulInd = TargetIndex.B;

		// Token: 0x0400317E RID: 12670
		private const TargetIndex StoreCellInd = TargetIndex.C;

		// Token: 0x0400317F RID: 12671
		private const int UnloadDuration = 10;
	}
}
