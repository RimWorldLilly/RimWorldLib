using System;

namespace Verse
{
	// Token: 0x020003F5 RID: 1013
	public class HealthTuning
	{
		// Token: 0x04001780 RID: 6016
		public const int StandardInterval = 60;

		// Token: 0x04001781 RID: 6017
		public const float SmallPawnFragmentedDamageHealthScaleThreshold = 0.5f;

		// Token: 0x04001782 RID: 6018
		public const int SmallPawnFragmentedDamageMinimumDamageAmount = 10;

		// Token: 0x04001783 RID: 6019
		public static float ChanceToAdditionallyDamageInnerSolidPart = 0.2f;

		// Token: 0x04001784 RID: 6020
		public const float MinBleedingRateToBleed = 0.1f;

		// Token: 0x04001785 RID: 6021
		public const float BleedSeverityRecoveryPerInterval = 0.00033333333f;

		// Token: 0x04001786 RID: 6022
		public const float BloodFilthDropChanceFactorStanding = 0.004f;

		// Token: 0x04001787 RID: 6023
		public const float BloodFilthDropChanceFactorLaying = 0.0004f;

		// Token: 0x04001788 RID: 6024
		public const float BloodSmearFilthDropChanceFactorCrawling = 0.015f;

		// Token: 0x04001789 RID: 6025
		public const int BaseTicksAfterInjuryToStopBleeding = 90000;

		// Token: 0x0400178A RID: 6026
		public const int TicksAfterMissingBodyPartToStopBeingFresh = 90000;

		// Token: 0x0400178B RID: 6027
		public const float DefaultPainShockThreshold = 0.8f;

		// Token: 0x0400178C RID: 6028
		public const int InjuryHealInterval = 600;

		// Token: 0x0400178D RID: 6029
		public const int RapidRegenInjuryHealInterval = 15;

		// Token: 0x0400178E RID: 6030
		public const float InjuryHealPerDay_Base = 8f;

		// Token: 0x0400178F RID: 6031
		public const float InjuryHealPerDayOffset_Laying = 4f;

		// Token: 0x04001790 RID: 6032
		public const float InjuryHealPerDayOffset_Tended = 8f;

		// Token: 0x04001791 RID: 6033
		public const int InjurySeverityTendedPerMedicine = 20;

		// Token: 0x04001792 RID: 6034
		public const float BaseTotalDamageLethalThreshold = 150f;

		// Token: 0x04001793 RID: 6035
		public const float BecomePermanentBaseChance = 0.02f;

		// Token: 0x04001794 RID: 6036
		public static readonly SimpleCurve BecomePermanentChanceFactorBySeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(4f, 0f),
				true
			},
			{
				new CurvePoint(14f, 1f),
				true
			}
		};

		// Token: 0x04001795 RID: 6037
		public static readonly HealthTuning.PainCategoryWeighted[] InjuryPainCategories = new HealthTuning.PainCategoryWeighted[]
		{
			new HealthTuning.PainCategoryWeighted(PainCategory.Painless, 0.5f),
			new HealthTuning.PainCategoryWeighted(PainCategory.LowPain, 0.2f),
			new HealthTuning.PainCategoryWeighted(PainCategory.MediumPain, 0.2f),
			new HealthTuning.PainCategoryWeighted(PainCategory.HighPain, 0.1f)
		};

		// Token: 0x04001796 RID: 6038
		public const float MinDamagePartPctForInfection = 0.2f;

		// Token: 0x04001797 RID: 6039
		public static readonly IntRange InfectionDelayRange = new IntRange(15000, 45000);

		// Token: 0x04001798 RID: 6040
		public const float AnimalsInfectionChanceFactor = 0.1f;

		// Token: 0x04001799 RID: 6041
		public const float HypothermiaGrowthPerDegreeUnder = 6.45E-05f;

		// Token: 0x0400179A RID: 6042
		public const float HeatstrokeGrowthPerDegreeOver = 6.45E-05f;

		// Token: 0x0400179B RID: 6043
		public const float MinHeatstrokeProgressPerInterval = 0.000375f;

		// Token: 0x0400179C RID: 6044
		public const float MinHypothermiaProgress = 0.00075f;

		// Token: 0x0400179D RID: 6045
		public const float HarmfulTemperatureOffset = 10f;

		// Token: 0x0400179E RID: 6046
		public const float MinTempOverComfyMaxForBurn = 150f;

		// Token: 0x0400179F RID: 6047
		public const float BurnDamagePerTempOverage = 0.06f;

		// Token: 0x040017A0 RID: 6048
		public const int MinBurnDamage = 3;

		// Token: 0x040017A1 RID: 6049
		public const float ImmunityGainRandomFactorMin = 0.8f;

		// Token: 0x040017A2 RID: 6050
		public const float ImmunityGainRandomFactorMax = 1.2f;

		// Token: 0x040017A3 RID: 6051
		public const float ImpossibleToFallSickIfAboveThisImmunityLevel = 0.6f;

		// Token: 0x040017A4 RID: 6052
		public const int HediffGiverUpdateInterval = 60;

		// Token: 0x040017A5 RID: 6053
		public const int VomitCheckInterval = 600;

		// Token: 0x040017A6 RID: 6054
		public const int DeathCheckInterval = 200;

		// Token: 0x040017A7 RID: 6055
		public const int ForgetRandomMemoryThoughtCheckInterval = 400;

		// Token: 0x040017A8 RID: 6056
		public const float PawnBaseHealthForSummary = 75f;

		// Token: 0x040017A9 RID: 6057
		public const float DeathOnDownedChance_NonColonyAnimal = 0.5f;

		// Token: 0x040017AA RID: 6058
		public const float DeathOnDownedChance_NonColonyMechanoid = 1f;

		// Token: 0x040017AB RID: 6059
		public static readonly SimpleCurve DeathOnDownedChance_EntityFromThreatCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 0f),
				true
			},
			{
				new CurvePoint(1000f, 0.5f),
				true
			},
			{
				new CurvePoint(5000f, 0.9f),
				true
			}
		};

		// Token: 0x040017AC RID: 6060
		public static readonly SimpleCurve DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 0.8667f),
				true
			},
			{
				new CurvePoint(0f, 0.8f),
				true
			},
			{
				new CurvePoint(1f, 0.5657f),
				true
			},
			{
				new CurvePoint(2f, 0.5f),
				true
			},
			{
				new CurvePoint(8f, 0.2632f),
				true
			}
		};

		// Token: 0x040017AD RID: 6061
		public static readonly SimpleCurve DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve_WaveringPrisoners = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 0.92f),
				true
			},
			{
				new CurvePoint(0f, 0.85f),
				true
			},
			{
				new CurvePoint(1f, 0.62f),
				true
			},
			{
				new CurvePoint(2f, 0.55f),
				true
			},
			{
				new CurvePoint(8f, 0.3f),
				true
			}
		};

		// Token: 0x040017AE RID: 6062
		public const int MaxPopulationAlwaysRecruitable = 3;

		// Token: 0x040017AF RID: 6063
		public static readonly SimpleCurve NonRecruitableChanceOverPopulationIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 0.4f),
				true
			},
			{
				new CurvePoint(0f, 0.25f),
				true
			},
			{
				new CurvePoint(1f, 0.13f),
				true
			},
			{
				new CurvePoint(2f, 0.1f),
				true
			},
			{
				new CurvePoint(8f, 0.05f),
				true
			}
		};

		// Token: 0x040017B0 RID: 6064
		public const float TendPriority_LifeThreateningDisease = 1f;

		// Token: 0x040017B1 RID: 6065
		public const float TendPriority_PerBleedRate = 1.5f;

		// Token: 0x040017B2 RID: 6066
		public const float TendPriority_DiseaseSeverityDecreasesWhenTended = 0.025f;

		// Token: 0x020026C2 RID: 9922
		public struct PainCategoryWeighted
		{
			// Token: 0x0600F4EF RID: 62703 RVA: 0x00595B8C File Offset: 0x00593D8C
			public PainCategoryWeighted(PainCategory category, float weight)
			{
				this.category = category;
				this.weight = weight;
			}

			// Token: 0x04009EDA RID: 40666
			public PainCategory category;

			// Token: 0x04009EDB RID: 40667
			public float weight;
		}
	}
}
