using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009E2 RID: 2530
	public class JobDriver_EatAtCannibalPlatter : JobDriver, IEatingDriver
	{
		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06004CB2 RID: 19634 RVA: 0x001D7D09 File Offset: 0x001D5F09
		public bool GainingNutritionNow
		{
			get
			{
				return base.CurToil == this.eating;
			}
		}

		// Token: 0x06004CB3 RID: 19635 RVA: 0x001D7D19 File Offset: 0x001D5F19
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.ReserveSittableOrSpot(this.job.targetB.Cell, this.job, errorOnFailed);
		}

		// Token: 0x06004CB4 RID: 19636 RVA: 0x001D7D3D File Offset: 0x001D5F3D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_EatAtCannibalPlatter.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_EatAtCannibalPlatter.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003022 RID: 12322
		private Toil eating;

		// Token: 0x04003023 RID: 12323
		private const TargetIndex PlatterIndex = TargetIndex.A;

		// Token: 0x04003024 RID: 12324
		private const TargetIndex CellIndex = TargetIndex.B;

		// Token: 0x04003025 RID: 12325
		private const int BloodFilthIntervalTick = 40;

		// Token: 0x04003026 RID: 12326
		private const float ChanceToProduceBloodFilth = 0.25f;
	}
}
