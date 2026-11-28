using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009F4 RID: 2548
	public class JobDriver_GoForWalk : JobDriver
	{
		// Token: 0x06004D34 RID: 19764 RVA: 0x001D9C69 File Offset: 0x001D7E69
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x001D9C6C File Offset: 0x001D7E6C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_GoForWalk.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_GoForWalk.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
