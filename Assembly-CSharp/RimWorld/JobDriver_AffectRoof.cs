using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009C8 RID: 2504
	public abstract class JobDriver_AffectRoof : JobDriver
	{
		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x06004C0D RID: 19469 RVA: 0x001D6508 File Offset: 0x001D4708
		protected IntVec3 Cell
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x06004C0E RID: 19470
		protected abstract PathEndMode PathEndMode { get; }

		// Token: 0x06004C0F RID: 19471
		protected abstract void DoEffect();

		// Token: 0x06004C10 RID: 19472
		protected abstract bool DoWorkFailOn();

		// Token: 0x06004C11 RID: 19473 RVA: 0x001D6529 File Offset: 0x001D4729
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x06004C12 RID: 19474 RVA: 0x001D6547 File Offset: 0x001D4747
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Cell, this.job, 1, -1, ReservationLayerDefOf.Ceiling, errorOnFailed, false);
		}

		// Token: 0x06004C13 RID: 19475 RVA: 0x001D656E File Offset: 0x001D476E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_AffectRoof.<MakeNewToils>d__12 <MakeNewToils>d__ = new JobDriver_AffectRoof.<MakeNewToils>d__12(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002FF9 RID: 12281
		private float workLeft;

		// Token: 0x04002FFA RID: 12282
		private const TargetIndex CellInd = TargetIndex.A;

		// Token: 0x04002FFB RID: 12283
		private const TargetIndex GotoTargetInd = TargetIndex.B;

		// Token: 0x04002FFC RID: 12284
		private const float BaseWorkAmount = 65f;
	}
}
