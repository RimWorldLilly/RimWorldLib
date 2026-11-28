using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200082D RID: 2093
	public class JobDriver_UseStylingStationAutomatic : JobDriver
	{
		// Token: 0x06004197 RID: 16791 RVA: 0x0019FA78 File Offset: 0x0019DC78
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004198 RID: 16792 RVA: 0x0019FA9B File Offset: 0x0019DC9B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_UseStylingStationAutomatic.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_UseStylingStationAutomatic.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
