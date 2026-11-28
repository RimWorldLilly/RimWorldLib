using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000803 RID: 2051
	public class JobDriver_ExitMapFlying : JobDriver
	{
		// Token: 0x060040AA RID: 16554 RVA: 0x0019BAD4 File Offset: 0x00199CD4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x0019BAD7 File Offset: 0x00199CD7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ExitMapFlying.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_ExitMapFlying.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x0019BAE7 File Offset: 0x00199CE7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.waitTicks, "waitTicks", -1, false);
		}

		// Token: 0x04002AEA RID: 10986
		private int waitTicks = -1;
	}
}
