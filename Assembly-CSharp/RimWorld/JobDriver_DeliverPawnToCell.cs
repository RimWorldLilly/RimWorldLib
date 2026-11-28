using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009E1 RID: 2529
	public class JobDriver_DeliverPawnToCell : JobDriver
	{
		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06004CAB RID: 19627 RVA: 0x001D7BD0 File Offset: 0x001D5DD0
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004CAC RID: 19628 RVA: 0x001D7BF6 File Offset: 0x001D5DF6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			base.Map.reservationManager.ReleaseAllForTarget(this.Takee);
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004CAD RID: 19629 RVA: 0x001D7C2F File Offset: 0x001D5E2F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_DeliverPawnToCell.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_DeliverPawnToCell.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003020 RID: 12320
		private const TargetIndex TakeeIndex = TargetIndex.A;

		// Token: 0x04003021 RID: 12321
		private const TargetIndex TargetCellIndex = TargetIndex.B;
	}
}
