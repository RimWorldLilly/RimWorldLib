using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A8B RID: 2699
	public class JobDriver_UseItemResearchBench : JobDriver_UseItem
	{
		// Token: 0x06005153 RID: 20819 RVA: 0x001E6C90 File Offset: 0x001E4E90
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return base.TryMakePreToilReservations(errorOnFailed) && this.pawn.Reserve(this.job.targetB, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x001E6CBE File Offset: 0x001E4EBE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_UseItemResearchBench.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_UseItemResearchBench.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003188 RID: 12680
		private const TargetIndex Item = TargetIndex.A;

		// Token: 0x04003189 RID: 12681
		private const TargetIndex ResearchBenchInd = TargetIndex.B;
	}
}
