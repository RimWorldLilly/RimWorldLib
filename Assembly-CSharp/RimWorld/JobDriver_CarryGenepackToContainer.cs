using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A17 RID: 2583
	public class JobDriver_CarryGenepackToContainer : JobDriver
	{
		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x06004E00 RID: 19968 RVA: 0x001DC450 File Offset: 0x001DA650
		private Thing Container
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x06004E01 RID: 19969 RVA: 0x001DC471 File Offset: 0x001DA671
		private CompGenepackContainer ContainerComp
		{
			get
			{
				return this.Container.TryGetComp<CompGenepackContainer>();
			}
		}

		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x06004E02 RID: 19970 RVA: 0x001DC480 File Offset: 0x001DA680
		private Genepack Genepack
		{
			get
			{
				return (Genepack)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004E03 RID: 19971 RVA: 0x001DC4A6 File Offset: 0x001DA6A6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x001DC4CA File Offset: 0x001DA6CA
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CarryGenepackToContainer.<MakeNewToils>d__10 <MakeNewToils>d__ = new JobDriver_CarryGenepackToContainer.<MakeNewToils>d__10(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003092 RID: 12434
		private const TargetIndex GenepackInd = TargetIndex.A;

		// Token: 0x04003093 RID: 12435
		private const TargetIndex ContainerInd = TargetIndex.B;

		// Token: 0x04003094 RID: 12436
		private const int InsertTicks = 30;
	}
}
