using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200082A RID: 2090
	public class JobDriver_Kill : JobDriver
	{
		// Token: 0x0600418D RID: 16781 RVA: 0x0019F99F File Offset: 0x0019DB9F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600418E RID: 16782 RVA: 0x0019F9C3 File Offset: 0x0019DBC3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Kill.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_Kill.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002B2C RID: 11052
		private const TargetIndex VictimInd = TargetIndex.A;
	}
}
