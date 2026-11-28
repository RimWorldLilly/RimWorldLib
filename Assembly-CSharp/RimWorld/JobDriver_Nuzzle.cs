using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009B6 RID: 2486
	public class JobDriver_Nuzzle : JobDriver
	{
		// Token: 0x06004B41 RID: 19265 RVA: 0x001D2E86 File Offset: 0x001D1086
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004B42 RID: 19266 RVA: 0x001D2E89 File Offset: 0x001D1089
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Nuzzle.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_Nuzzle.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
