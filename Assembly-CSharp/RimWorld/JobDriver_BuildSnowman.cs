using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009F2 RID: 2546
	public class JobDriver_BuildSnowman : JobDriver
	{
		// Token: 0x06004D2A RID: 19754 RVA: 0x001D9BAC File Offset: 0x001D7DAC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004D2B RID: 19755 RVA: 0x001D9BCF File Offset: 0x001D7DCF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_BuildSnowman.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_BuildSnowman.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004D2C RID: 19756 RVA: 0x001D9BDF File Offset: 0x001D7DDF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x04003051 RID: 12369
		private float workLeft = -1000f;

		// Token: 0x04003052 RID: 12370
		protected const int BaseWorkAmount = 2300;
	}
}
