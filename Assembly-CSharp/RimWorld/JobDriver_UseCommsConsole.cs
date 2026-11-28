using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A89 RID: 2697
	public class JobDriver_UseCommsConsole : JobDriver
	{
		// Token: 0x06005147 RID: 20807 RVA: 0x001E6921 File Offset: 0x001E4B21
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x001E6944 File Offset: 0x001E4B44
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_UseCommsConsole.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_UseCommsConsole.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
