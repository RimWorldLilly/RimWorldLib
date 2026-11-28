using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AA1 RID: 2721
	public class JobDriver_GetNeuralSupercharge : JobDriver
	{
		// Token: 0x060051E6 RID: 20966 RVA: 0x001E862C File Offset: 0x001E682C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A).Thing, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x001E8668 File Offset: 0x001E6868
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_GetNeuralSupercharge.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_GetNeuralSupercharge.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040031A9 RID: 12713
		private const TargetIndex ChargerInd = TargetIndex.A;
	}
}
