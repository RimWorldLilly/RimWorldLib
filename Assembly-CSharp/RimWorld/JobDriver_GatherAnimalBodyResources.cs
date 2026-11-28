using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009A4 RID: 2468
	public abstract class JobDriver_GatherAnimalBodyResources : JobDriver
	{
		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x06004AC1 RID: 19137
		protected abstract float WorkTotal { get; }

		// Token: 0x06004AC2 RID: 19138
		protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

		// Token: 0x06004AC3 RID: 19139 RVA: 0x001D1C01 File Offset: 0x001CFE01
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.gatherProgress, "gatherProgress", 0f, false);
		}

		// Token: 0x06004AC4 RID: 19140 RVA: 0x001D1C1F File Offset: 0x001CFE1F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004AC5 RID: 19141 RVA: 0x001D1C43 File Offset: 0x001CFE43
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_GatherAnimalBodyResources.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_GatherAnimalBodyResources.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002FA1 RID: 12193
		private float gatherProgress;

		// Token: 0x04002FA2 RID: 12194
		protected const TargetIndex AnimalInd = TargetIndex.A;
	}
}
