using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200147F RID: 5247
	public static class AnimalProductionUtility
	{
		// Token: 0x060080BB RID: 32955 RVA: 0x002DED47 File Offset: 0x002DCF47
		public static IEnumerable<StatDrawEntry> AnimalProductionStats(ThingDef d)
		{
			AnimalProductionUtility.<AnimalProductionStats>d__0 <AnimalProductionStats>d__ = new AnimalProductionUtility.<AnimalProductionStats>d__0(-2);
			<AnimalProductionStats>d__.<>3__d = d;
			return <AnimalProductionStats>d__;
		}

		// Token: 0x060080BC RID: 32956 RVA: 0x002DED57 File Offset: 0x002DCF57
		public static float AdultMeatAmount(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.MeatAmount, null);
		}

		// Token: 0x060080BD RID: 32957 RVA: 0x002DED65 File Offset: 0x002DCF65
		public static float AdultLeatherAmount(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.LeatherAmount, null);
		}

		// Token: 0x060080BE RID: 32958 RVA: 0x002DED73 File Offset: 0x002DCF73
		public static bool Herbivore(ThingDef d)
		{
			return (d.race.foodType & FoodTypeFlags.Plant) > FoodTypeFlags.None;
		}

		// Token: 0x060080BF RID: 32959 RVA: 0x002DED88 File Offset: 0x002DCF88
		public static float NutritionToGestate(ThingDef d)
		{
			float num = 0f;
			LifeStageAge lifeStageAge = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1];
			return num + AnimalProductionUtility.GestationDaysEach(d) * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
		}

		// Token: 0x060080C0 RID: 32960 RVA: 0x002DEDDC File Offset: 0x002DCFDC
		public static float NutritionToAdulthood(ThingDef d)
		{
			float num = 0f;
			num += AnimalProductionUtility.NutritionToGestate(d);
			for (int i = 1; i < d.race.lifeStageAges.Count; i++)
			{
				LifeStageAge lifeStageAge = d.race.lifeStageAges[i];
				float num2 = (lifeStageAge.minAge - d.race.lifeStageAges[i - 1].minAge) * 60f;
				num += num2 * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
			}
			return num;
		}

		// Token: 0x060080C1 RID: 32961 RVA: 0x002DEE68 File Offset: 0x002DD068
		public static float GestationDaysEach(ThingDef d)
		{
			if (d.HasComp(typeof(CompEggLayer)))
			{
				CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
				return compProperties.eggLayIntervalDays / compProperties.eggCountRange.Average;
			}
			return d.race.gestationPeriodDays / ((d.race.litterSizeCurve != null) ? Rand.ByCurveAverage(d.race.litterSizeCurve) : 1f);
		}

		// Token: 0x060080C2 RID: 32962 RVA: 0x002DEED1 File Offset: 0x002DD0D1
		public static float GestationDaysLitter(ThingDef d)
		{
			if (d.HasComp(typeof(CompEggLayer)))
			{
				return d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays;
			}
			return d.race.gestationPeriodDays;
		}

		// Token: 0x060080C3 RID: 32963 RVA: 0x002DEEFC File Offset: 0x002DD0FC
		public static IntRange OffspringRange(ThingDef d)
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties != null)
			{
				return compProperties.eggCountRange;
			}
			if (d.race.litterSizeCurve != null)
			{
				int min = Mathf.Max(Mathf.RoundToInt(d.race.litterSizeCurve.First<CurvePoint>().x), 1);
				int max = Mathf.Max(Mathf.RoundToInt(d.race.litterSizeCurve.Last<CurvePoint>().x), 1);
				return new IntRange(min, max);
			}
			return new IntRange(1, 1);
		}

		// Token: 0x060080C4 RID: 32964 RVA: 0x002DEF7C File Offset: 0x002DD17C
		public static float GrassNutritionPerDay()
		{
			ThingDef plant_Grass = ThingDefOf.Plant_Grass;
			return plant_Grass.GetStatValueAbstract(StatDefOf.Nutrition, null) / (plant_Grass.plant.growDays / 0.5f);
		}

		// Token: 0x060080C5 RID: 32965 RVA: 0x002DEFAD File Offset: 0x002DD1AD
		public static float GrassToMaintain(ThingDef d)
		{
			if (!AnimalProductionUtility.Herbivore(d))
			{
				return -1f;
			}
			return 2.6666667E-05f * d.race.baseHungerRate * 60000f / AnimalProductionUtility.GrassNutritionPerDay();
		}

		// Token: 0x060080C6 RID: 32966 RVA: 0x002DEFDC File Offset: 0x002DD1DC
		public static float EggsPerYear(ThingDef d)
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties == null)
			{
				return 0f;
			}
			return 60f / compProperties.eggLayIntervalDays * compProperties.eggCountRange.Average;
		}

		// Token: 0x060080C7 RID: 32967 RVA: 0x002DF011 File Offset: 0x002DD211
		public static float EggNutritionPerYear(ThingDef d)
		{
			return AnimalProductionUtility.EggsPerYear(d) * AnimalProductionUtility.EggNutrition(d);
		}

		// Token: 0x060080C8 RID: 32968 RVA: 0x002DF020 File Offset: 0x002DD220
		public static float EggNutrition(ThingDef d)
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties == null)
			{
				return -1f;
			}
			return (compProperties.eggUnfertilizedDef ?? compProperties.eggFertilizedDef).GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x060080C9 RID: 32969 RVA: 0x002DF058 File Offset: 0x002DD258
		public static float EggMarketValue(ThingDef d)
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties == null)
			{
				return -1f;
			}
			return (compProperties.eggUnfertilizedDef ?? compProperties.eggFertilizedDef).BaseMarketValue;
		}

		// Token: 0x060080CA RID: 32970 RVA: 0x002DF08A File Offset: 0x002DD28A
		public static float EggMarketValuePerYear(ThingDef d)
		{
			return AnimalProductionUtility.EggsPerYear(d) * AnimalProductionUtility.EggMarketValue(d);
		}

		// Token: 0x060080CB RID: 32971 RVA: 0x002DF099 File Offset: 0x002DD299
		public static float DaysToAdulthood(ThingDef d)
		{
			return d.race.lifeStageAges.Last<LifeStageAge>().minAge * 60f;
		}

		// Token: 0x060080CC RID: 32972 RVA: 0x002DF0B8 File Offset: 0x002DD2B8
		public static float MeatPerDayDuringGrowth(ThingDef d)
		{
			float num = AnimalProductionUtility.AdultMeatAmount(d);
			float num2 = AnimalProductionUtility.DaysToAdulthood(d);
			return num / num2;
		}

		// Token: 0x060080CD RID: 32973 RVA: 0x002DF0D4 File Offset: 0x002DD2D4
		public static float MilkPerYear(ThingDef d)
		{
			CompProperties_Milkable compProperties = d.GetCompProperties<CompProperties_Milkable>();
			if (compProperties == null)
			{
				return 0f;
			}
			return 60f / (float)compProperties.milkIntervalDays * (float)compProperties.milkAmount;
		}

		// Token: 0x060080CE RID: 32974 RVA: 0x002DF108 File Offset: 0x002DD308
		public static float MilkMarketValue(ThingDef d)
		{
			CompProperties_Milkable compProperties = d.GetCompProperties<CompProperties_Milkable>();
			if (compProperties == null)
			{
				return -1f;
			}
			return compProperties.milkDef.BaseMarketValue;
		}

		// Token: 0x060080CF RID: 32975 RVA: 0x002DF130 File Offset: 0x002DD330
		public static float MilkMarketValuePerYear(ThingDef d)
		{
			return AnimalProductionUtility.MilkPerYear(d) * AnimalProductionUtility.MilkMarketValue(d);
		}

		// Token: 0x060080D0 RID: 32976 RVA: 0x002DF140 File Offset: 0x002DD340
		public static float MilkNutrition(ThingDef d)
		{
			CompProperties_Milkable compProperties = d.GetCompProperties<CompProperties_Milkable>();
			if (compProperties == null)
			{
				return 0f;
			}
			if (!compProperties.milkDef.IsIngestible)
			{
				return 0f;
			}
			return compProperties.milkDef.ingestible.CachedNutrition;
		}

		// Token: 0x060080D1 RID: 32977 RVA: 0x002DF180 File Offset: 0x002DD380
		public static float MilkNutritionPerYear(ThingDef d)
		{
			return AnimalProductionUtility.MilkPerYear(d) * AnimalProductionUtility.MilkNutrition(d);
		}

		// Token: 0x060080D2 RID: 32978 RVA: 0x002DF190 File Offset: 0x002DD390
		public static float WoolPerYear(ThingDef d)
		{
			CompProperties_Shearable compProperties = d.GetCompProperties<CompProperties_Shearable>();
			if (compProperties == null)
			{
				return 0f;
			}
			return 60f / (float)compProperties.shearIntervalDays * (float)compProperties.woolAmount;
		}

		// Token: 0x060080D3 RID: 32979 RVA: 0x002DF1C4 File Offset: 0x002DD3C4
		public static float WoolMarketValue(ThingDef d)
		{
			CompProperties_Shearable compProperties = d.GetCompProperties<CompProperties_Shearable>();
			if (compProperties == null)
			{
				return 0f;
			}
			return compProperties.woolDef.BaseMarketValue;
		}

		// Token: 0x060080D4 RID: 32980 RVA: 0x002DF1EC File Offset: 0x002DD3EC
		public static float WoolMarketValuePerYear(ThingDef d)
		{
			return AnimalProductionUtility.WoolPerYear(d) * AnimalProductionUtility.WoolMarketValue(d);
		}
	}
}
