using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A30 RID: 2608
	public class JobDriver_ExecuteEntity : JobDriver
	{
		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x06004EBD RID: 20157 RVA: 0x001DEACA File Offset: 0x001DCCCA
		private Thing Platform
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x06004EBE RID: 20158 RVA: 0x001DEAD2 File Offset: 0x001DCCD2
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

		// Token: 0x06004EBF RID: 20159 RVA: 0x001DEAEA File Offset: 0x001DCCEA
		public override string GetReport()
		{
			return JobUtility.GetResolvedJobReport(this.job.def.reportString, this.InnerPawn, this.job.targetB, this.job.targetC);
		}

		// Token: 0x06004EC0 RID: 20160 RVA: 0x001DEB22 File Offset: 0x001DCD22
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Platform, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004EC1 RID: 20161 RVA: 0x001DEB45 File Offset: 0x001DCD45
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ExecuteEntity.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_ExecuteEntity.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030CA RID: 12490
		private const TargetIndex PlatformIndex = TargetIndex.A;
	}
}
