using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009C2 RID: 2498
	public class JobDriver_FertilizeOvum : JobDriver
	{
		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x06004BEA RID: 19434 RVA: 0x001D5FD8 File Offset: 0x001D41D8
		private HumanOvum ovum
		{
			get
			{
				return (HumanOvum)base.TargetThingA;
			}
		}

		// Token: 0x06004BEB RID: 19435 RVA: 0x001D5FE5 File Offset: 0x001D41E5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004BEC RID: 19436 RVA: 0x001D600D File Offset: 0x001D420D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_FertilizeOvum.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_FertilizeOvum.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002FF0 RID: 12272
		private const int FertilizeTicks = 180;

		// Token: 0x04002FF1 RID: 12273
		public const TargetIndex OvumIndex = TargetIndex.A;
	}
}
