using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A75 RID: 2677
	public class JobDriver_Seal : JobDriver
	{
		// Token: 0x060050BC RID: 20668 RVA: 0x001E5007 File Offset: 0x001E3207
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060050BD RID: 20669 RVA: 0x001E502B File Offset: 0x001E322B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Seal.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_Seal.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400315C RID: 12636
		private const TargetIndex HatchInd = TargetIndex.A;
	}
}
