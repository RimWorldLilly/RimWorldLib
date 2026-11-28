using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A32 RID: 2610
	public class JobDriver_ExtractBioferrite : JobDriver
	{
		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x06004ECB RID: 20171 RVA: 0x001DECDB File Offset: 0x001DCEDB
		private Thing Platform
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x06004ECC RID: 20172 RVA: 0x001DECE3 File Offset: 0x001DCEE3
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

		// Token: 0x06004ECD RID: 20173 RVA: 0x001DECFB File Offset: 0x001DCEFB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Platform, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004ECE RID: 20174 RVA: 0x001DED23 File Offset: 0x001DCF23
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ExtractBioferrite.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_ExtractBioferrite.<MakeNewToils>d__9(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004ECF RID: 20175 RVA: 0x001DED33 File Offset: 0x001DCF33
		public override string GetReport()
		{
			return JobUtility.GetResolvedJobReport(this.job.def.reportString, this.InnerPawn, this.job.targetB, this.job.targetC);
		}

		// Token: 0x040030CB RID: 12491
		private const TargetIndex PlatformIndex = TargetIndex.A;

		// Token: 0x040030CC RID: 12492
		private const int ExtractDuration = 2000;

		// Token: 0x040030CD RID: 12493
		private const int CutDamage = 4;

		// Token: 0x040030CE RID: 12494
		private const int BioferriteModifier = 4;
	}
}
