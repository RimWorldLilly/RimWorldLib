using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200082B RID: 2091
	public class JobDriver_OpenStylingStationDialog : JobDriver
	{
		// Token: 0x06004190 RID: 16784 RVA: 0x0019F9DB File Offset: 0x0019DBDB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004191 RID: 16785 RVA: 0x0019F9FE File Offset: 0x0019DBFE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_OpenStylingStationDialog.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_OpenStylingStationDialog.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
