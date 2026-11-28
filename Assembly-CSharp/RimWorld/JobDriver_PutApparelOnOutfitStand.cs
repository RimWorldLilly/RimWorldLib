using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A61 RID: 2657
	public class JobDriver_PutApparelOnOutfitStand : JobDriver
	{
		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x0600501C RID: 20508 RVA: 0x001E2F3C File Offset: 0x001E113C
		private Apparel Apparel
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Apparel;
			}
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x0600501D RID: 20509 RVA: 0x001E2F64 File Offset: 0x001E1164
		private Building_OutfitStand DestOutfitStand
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing as Building_OutfitStand;
			}
		}

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x0600501E RID: 20510 RVA: 0x001E2F8C File Offset: 0x001E118C
		private bool TargetIsOnApparelSource
		{
			get
			{
				Apparel apparel = this.Apparel;
				if (apparel != null && !apparel.Spawned)
				{
					IApparelSource apparelSource = apparel.ParentHolder as IApparelSource;
					if (apparelSource != null)
					{
						return apparelSource is Thing;
					}
				}
				return false;
			}
		}

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x0600501F RID: 20511 RVA: 0x001E2FC8 File Offset: 0x001E11C8
		private IApparelSource ApparelSource
		{
			get
			{
				return (IApparelSource)this.job.GetTarget(TargetIndex.C).Thing;
			}
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x001E2FEE File Offset: 0x001E11EE
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.TargetIsOnApparelSource)
			{
				this.job.targetC = (Thing)this.Apparel.ParentHolder;
			}
		}

		// Token: 0x06005021 RID: 20513 RVA: 0x001E3020 File Offset: 0x001E1220
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.TargetIsOnApparelSource)
			{
				return this.pawn.Reserve((Thing)this.Apparel.ParentHolder, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.DestOutfitStand, this.job, 1, -1, null, errorOnFailed, false);
			}
			return this.pawn.Reserve(this.Apparel, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.DestOutfitStand, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005022 RID: 20514 RVA: 0x001E30CC File Offset: 0x001E12CC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PutApparelOnOutfitStand.<MakeNewToils>d__13 <MakeNewToils>d__ = new JobDriver_PutApparelOnOutfitStand.<MakeNewToils>d__13(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003127 RID: 12583
		private const TargetIndex ApparelInd = TargetIndex.A;

		// Token: 0x04003128 RID: 12584
		private const TargetIndex OutfitStandInd = TargetIndex.B;

		// Token: 0x04003129 RID: 12585
		private const TargetIndex ApparelSourceIndex = TargetIndex.C;
	}
}
