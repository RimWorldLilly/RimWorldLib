using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A3A RID: 2618
	public class JobDriver_Flick : JobDriver
	{
		// Token: 0x06004F01 RID: 20225 RVA: 0x001DF9FC File Offset: 0x001DDBFC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004F02 RID: 20226 RVA: 0x001DFA1F File Offset: 0x001DDC1F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Flick.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_Flick.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
