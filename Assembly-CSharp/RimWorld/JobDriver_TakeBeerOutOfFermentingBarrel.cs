using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A7E RID: 2686
	public class JobDriver_TakeBeerOutOfFermentingBarrel : JobDriver
	{
		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x060050FD RID: 20733 RVA: 0x001E5AA0 File Offset: 0x001E3CA0
		protected Building_FermentingBarrel Barrel
		{
			get
			{
				return (Building_FermentingBarrel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x060050FE RID: 20734 RVA: 0x001E5AC8 File Offset: 0x001E3CC8
		protected Thing Beer
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x001E5AE9 File Offset: 0x001E3CE9
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Barrel, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005100 RID: 20736 RVA: 0x001E5B0C File Offset: 0x001E3D0C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TakeBeerOutOfFermentingBarrel.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_TakeBeerOutOfFermentingBarrel.<MakeNewToils>d__9(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400316F RID: 12655
		private const TargetIndex BarrelInd = TargetIndex.A;

		// Token: 0x04003170 RID: 12656
		private const TargetIndex BeerToHaulInd = TargetIndex.B;

		// Token: 0x04003171 RID: 12657
		private const TargetIndex StorageCellInd = TargetIndex.C;

		// Token: 0x04003172 RID: 12658
		private const int Duration = 200;
	}
}
