using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A81 RID: 2689
	public class JobDriver_TakeFromOtherInventory : JobDriver
	{
		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x0600510E RID: 20750 RVA: 0x001E5D64 File Offset: 0x001E3F64
		public Pawn ItemHoldingPawn
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Pawn;
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x0600510F RID: 20751 RVA: 0x001E5D85 File Offset: 0x001E3F85
		public Pawn_InventoryTracker ItemHoldingInventory
		{
			get
			{
				return base.TargetThingA.ParentHolder as Pawn_InventoryTracker;
			}
		}

		// Token: 0x06005110 RID: 20752 RVA: 0x001E5D97 File Offset: 0x001E3F97
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x001E5D9A File Offset: 0x001E3F9A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TakeFromOtherInventory.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_TakeFromOtherInventory.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003177 RID: 12663
		public const TargetIndex ItemIndex = TargetIndex.A;

		// Token: 0x04003178 RID: 12664
		public const TargetIndex ItemHolderIndex = TargetIndex.B;
	}
}
