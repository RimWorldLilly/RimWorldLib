using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009E0 RID: 2528
	public class JobDriver_DeliverPawnToAltar : JobDriver
	{
		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06004CA3 RID: 19619 RVA: 0x001D79FC File Offset: 0x001D5BFC
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06004CA4 RID: 19620 RVA: 0x001D7A24 File Offset: 0x001D5C24
		protected Building DropAltar
		{
			get
			{
				return (Building)this.job.GetTarget(TargetIndex.C).Thing;
			}
		}

		// Token: 0x06004CA5 RID: 19621 RVA: 0x001D7A4C File Offset: 0x001D5C4C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			base.Map.reservationManager.ReleaseAllForTarget(this.Takee);
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.DropAltar, this.job, 1, 0, null, errorOnFailed, false);
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x001D7AB5 File Offset: 0x001D5CB5
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_DeliverPawnToAltar.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_DeliverPawnToAltar.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400301D RID: 12317
		private const TargetIndex TakeeIndex = TargetIndex.A;

		// Token: 0x0400301E RID: 12318
		private const TargetIndex TargetCellIndex = TargetIndex.B;

		// Token: 0x0400301F RID: 12319
		private const TargetIndex AltarIndex = TargetIndex.C;
	}
}
