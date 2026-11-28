using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A2D RID: 2605
	public class JobDriver_EnterCryptosleepCasket : JobDriver
	{
		// Token: 0x06004EAC RID: 20140 RVA: 0x001DE731 File Offset: 0x001DC931
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004EAD RID: 20141 RVA: 0x001DE754 File Offset: 0x001DC954
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_EnterCryptosleepCasket.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_EnterCryptosleepCasket.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
