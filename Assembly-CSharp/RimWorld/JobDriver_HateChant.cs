using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009E5 RID: 2533
	public class JobDriver_HateChant : JobDriver
	{
		// Token: 0x06004CC7 RID: 19655 RVA: 0x001D8170 File Offset: 0x001D6370
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004CC8 RID: 19656 RVA: 0x001D8173 File Offset: 0x001D6373
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_HateChant.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_HateChant.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
