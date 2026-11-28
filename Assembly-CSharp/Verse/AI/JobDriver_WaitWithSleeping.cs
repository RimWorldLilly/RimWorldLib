using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000822 RID: 2082
	public class JobDriver_WaitWithSleeping : JobDriver_Wait
	{
		// Token: 0x0600416A RID: 16746 RVA: 0x0019F212 File Offset: 0x0019D412
		public override void DecorateWaitToil(Toil wait)
		{
			wait.FailOn(delegate()
			{
				Pawn pawn = base.TargetThingB as Pawn;
				return pawn == null || pawn.Dead || pawn.Awake();
			});
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x0019F227 File Offset: 0x0019D427
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_WaitWithSleeping.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_WaitWithSleeping.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
