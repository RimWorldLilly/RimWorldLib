using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A53 RID: 2643
	public class JobDriver_Lovin : JobDriver
	{
		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06004FBA RID: 20410 RVA: 0x001E1A03 File Offset: 0x001DFC03
		private Pawn Partner
		{
			get
			{
				return (Pawn)((Thing)this.job.GetTarget(this.PartnerInd));
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x06004FBB RID: 20411 RVA: 0x001E1A20 File Offset: 0x001DFC20
		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)((Thing)this.job.GetTarget(this.BedInd));
			}
		}

		// Token: 0x06004FBC RID: 20412 RVA: 0x001E1A3D File Offset: 0x001DFC3D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x001E1A58 File Offset: 0x001DFC58
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Partner, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null, errorOnFailed, false);
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x001E1AB5 File Offset: 0x001DFCB5
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(this.BedInd));
		}

		// Token: 0x06004FBF RID: 20415 RVA: 0x001E1AD3 File Offset: 0x001DFCD3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Lovin.<MakeNewToils>d__12 <MakeNewToils>d__ = new JobDriver_Lovin.<MakeNewToils>d__12(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x001E1AE4 File Offset: 0x001DFCE4
		private int GenerateRandomMinTicksToNextLovin(Pawn pawn)
		{
			if (DebugSettings.alwaysDoLovin)
			{
				return 100;
			}
			float num = JobDriver_Lovin.LovinIntervalHoursFromAgeCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
			if (ModsConfig.BiotechActive && pawn.genes != null)
			{
				foreach (Gene gene in pawn.genes.GenesListForReading)
				{
					num *= gene.def.lovinMTBFactor;
				}
			}
			foreach (Hediff hd in pawn.health.hediffSet.hediffs)
			{
				HediffComp_GiveLovinMTBFactor hediffComp_GiveLovinMTBFactor = hd.TryGetComp<HediffComp_GiveLovinMTBFactor>();
				if (hediffComp_GiveLovinMTBFactor != null)
				{
					num *= hediffComp_GiveLovinMTBFactor.Props.lovinMTBFactor;
				}
			}
			num = Rand.Gaussian(num, 0.3f);
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			return (int)(num * 2500f);
		}

		// Token: 0x04003105 RID: 12549
		private int ticksLeft;

		// Token: 0x04003106 RID: 12550
		private TargetIndex PartnerInd = TargetIndex.A;

		// Token: 0x04003107 RID: 12551
		private TargetIndex BedInd = TargetIndex.B;

		// Token: 0x04003108 RID: 12552
		private const int TicksBetweenHeartMotes = 100;

		// Token: 0x04003109 RID: 12553
		private static float PregnancyChance = 0.05f;

		// Token: 0x0400310A RID: 12554
		private static readonly SimpleCurve LovinIntervalHoursFromAgeCurve = new SimpleCurve
		{
			{
				new CurvePoint(16f, 1.5f),
				true
			},
			{
				new CurvePoint(22f, 1.5f),
				true
			},
			{
				new CurvePoint(30f, 4f),
				true
			},
			{
				new CurvePoint(50f, 12f),
				true
			},
			{
				new CurvePoint(75f, 36f),
				true
			}
		};
	}
}
