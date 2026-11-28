using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A54 RID: 2644
	public class JobDriver_Maintain : JobDriver
	{
		// Token: 0x06004FC8 RID: 20424 RVA: 0x001E21DD File Offset: 0x001E03DD
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004FC9 RID: 20425 RVA: 0x001E2200 File Offset: 0x001E0400
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Maintain.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_Maintain.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400310B RID: 12555
		private const int MaintainTicks = 180;
	}
}
