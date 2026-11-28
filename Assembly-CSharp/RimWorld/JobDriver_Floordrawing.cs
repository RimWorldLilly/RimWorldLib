using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A06 RID: 2566
	public class JobDriver_Floordrawing : JobDriver
	{
		// Token: 0x06004D93 RID: 19859 RVA: 0x001DAEC3 File Offset: 0x001D90C3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(base.TargetA, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(base.TargetB, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x001DAF01 File Offset: 0x001D9101
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Floordrawing.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_Floordrawing.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400306D RID: 12397
		private const int DrawingIntervalTicks = 2500;
	}
}
