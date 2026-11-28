using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A02 RID: 2562
	public abstract class JobDriver_VisitJoyThing : JobDriver
	{
		// Token: 0x06004D7F RID: 19839 RVA: 0x001DAA4B File Offset: 0x001D8C4B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004D80 RID: 19840 RVA: 0x001DAA6F File Offset: 0x001D8C6F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_VisitJoyThing.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_VisitJoyThing.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004D81 RID: 19841
		protected abstract void WaitTickAction(int delta);

		// Token: 0x0400306A RID: 12394
		protected const TargetIndex TargetThingIndex = TargetIndex.A;
	}
}
