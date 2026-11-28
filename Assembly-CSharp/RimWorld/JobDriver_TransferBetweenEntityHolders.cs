using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A1B RID: 2587
	public class JobDriver_TransferBetweenEntityHolders : JobDriver
	{
		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x06004E2C RID: 20012 RVA: 0x001DCEE0 File Offset: 0x001DB0E0
		private Thing Takee
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C).Thing;
			}
		}

		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x06004E2D RID: 20013 RVA: 0x001DCF04 File Offset: 0x001DB104
		private CompEntityHolder SourceHolder
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompEntityHolder>();
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x06004E2E RID: 20014 RVA: 0x001DCF2C File Offset: 0x001DB12C
		private CompEntityHolder DestHolder
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing.TryGetComp<CompEntityHolder>();
			}
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x001DCF54 File Offset: 0x001DB154
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.SourceHolder.parent, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.DestHolder.parent, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x001DCFD4 File Offset: 0x001DB1D4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TransferBetweenEntityHolders.<MakeNewToils>d__10 <MakeNewToils>d__ = new JobDriver_TransferBetweenEntityHolders.<MakeNewToils>d__10(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400309F RID: 12447
		private const TargetIndex SourceHolderIndex = TargetIndex.A;

		// Token: 0x040030A0 RID: 12448
		private const TargetIndex DestHolderIndex = TargetIndex.B;

		// Token: 0x040030A1 RID: 12449
		private const TargetIndex TakeeIndex = TargetIndex.C;
	}
}
