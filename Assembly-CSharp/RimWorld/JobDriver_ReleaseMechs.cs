using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A67 RID: 2663
	public class JobDriver_ReleaseMechs : JobDriver
	{
		// Token: 0x0600505E RID: 20574 RVA: 0x001E3FD1 File Offset: 0x001E21D1
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x001E3FD4 File Offset: 0x001E21D4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ReleaseMechs.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_ReleaseMechs.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
