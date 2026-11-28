using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A66 RID: 2662
	public class JobDriver_ReleaseEntity : JobDriver
	{
		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x06005056 RID: 20566 RVA: 0x001E3EE1 File Offset: 0x001E20E1
		private Thing Platform
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x06005057 RID: 20567 RVA: 0x001E3EE9 File Offset: 0x001E20E9
		private Pawn InnerPawn
		{
			get
			{
				Building_HoldingPlatform building_HoldingPlatform = this.Platform as Building_HoldingPlatform;
				if (building_HoldingPlatform == null)
				{
					return null;
				}
				return building_HoldingPlatform.HeldPawn;
			}
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x06005058 RID: 20568 RVA: 0x001E3F04 File Offset: 0x001E2104
		private bool EntityShouldBeReleased
		{
			get
			{
				Pawn innerPawn = this.InnerPawn;
				CompHoldingPlatformTarget compHoldingPlatformTarget = (innerPawn != null) ? innerPawn.TryGetComp<CompHoldingPlatformTarget>() : null;
				return compHoldingPlatformTarget != null && (compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Release || this.job.ignoreDesignations);
			}
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x001E3F3F File Offset: 0x001E213F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Platform, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x001E3F62 File Offset: 0x001E2162
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ReleaseEntity.<MakeNewToils>d__10 <MakeNewToils>d__ = new JobDriver_ReleaseEntity.<MakeNewToils>d__10(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400313E RID: 12606
		private const TargetIndex PlatformIndex = TargetIndex.A;

		// Token: 0x0400313F RID: 12607
		private const TargetIndex EntityIndex = TargetIndex.B;

		// Token: 0x04003140 RID: 12608
		private const int TransferTicks = 300;
	}
}
