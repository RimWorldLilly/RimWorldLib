using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009CF RID: 2511
	public class JobDriver_PlaceNoCostFrame : JobDriver
	{
		// Token: 0x06004C3A RID: 19514 RVA: 0x001D6A8B File Offset: 0x001D4C8B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x001D6AAE File Offset: 0x001D4CAE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PlaceNoCostFrame.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_PlaceNoCostFrame.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
