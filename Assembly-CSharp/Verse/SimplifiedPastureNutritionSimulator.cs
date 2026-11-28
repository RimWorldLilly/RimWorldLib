using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200023A RID: 570
	public static class SimplifiedPastureNutritionSimulator
	{
		// Token: 0x060010A1 RID: 4257 RVA: 0x0005FA94 File Offset: 0x0005DC94
		public static float NutritionProducedPerDay(Map map, ThingDef plantDef, float averageGrowthRate, float mapRespawnChance)
		{
			float result;
			using (ProfilerBlock.Scope("NutritionProducedPerDay"))
			{
				if (Mathf.Approximately(averageGrowthRate, 0f))
				{
					result = 0f;
				}
				else
				{
					if (SimplifiedPastureNutritionSimulator.cachedGame != Current.Game)
					{
						SimplifiedPastureNutritionSimulator.cachedGame = Current.Game;
						SimplifiedPastureNutritionSimulator.nutritionCache.Clear();
					}
					SimplifiedPastureNutritionSimulator.CacheKey key = new SimplifiedPastureNutritionSimulator.CacheKey
					{
						map = map,
						mapRespawnChance = mapRespawnChance,
						plantDef = plantDef
					};
					ValueTuple<float, float> valueTuple;
					if (SimplifiedPastureNutritionSimulator.nutritionCache.TryGetValue(key, out valueTuple) && Mathf.Abs(valueTuple.Item1 - averageGrowthRate) <= 0.05f)
					{
						result = valueTuple.Item2;
					}
					else
					{
						using (ProfilerBlock.Scope("Cache_NutritionProducedPerDay"))
						{
							float num = map.Biome.wildPlantRegrowDays / mapRespawnChance;
							float num2 = plantDef.plant.growDays / averageGrowthRate * plantDef.plant.harvestMinGrowth;
							float num3 = plantDef.GetStatValueAbstract(StatDefOf.Nutrition, null) * PlantUtility.NutritionFactorFromGrowth(plantDef, plantDef.plant.harvestMinGrowth) / (num + num2) * map.wildPlantSpawner.GetCommonalityPctOfPlant(plantDef) * 0.85f;
							SimplifiedPastureNutritionSimulator.nutritionCache[key] = new ValueTuple<float, float>(averageGrowthRate, num3);
							result = num3;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0005FC14 File Offset: 0x0005DE14
		public static float NutritionConsumedPerDay(Pawn animal)
		{
			return SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animal.def, animal.ageTracker.CurLifeStage);
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0005FC2C File Offset: 0x0005DE2C
		public static float NutritionConsumedPerDay(ThingDef animalDef)
		{
			LifeStageAge lifeStageAge = animalDef.race.lifeStageAges.Last<LifeStageAge>();
			return SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animalDef, lifeStageAge.def);
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0005FC56 File Offset: 0x0005DE56
		public static float NutritionConsumedPerDay(ThingDef animalDef, LifeStageDef lifeStageDef)
		{
			return Need_Food.BaseHungerRate(lifeStageDef, animalDef) * 60000f;
		}

		// Token: 0x04000F69 RID: 3945
		private const float UnderEstimateNutritionFactor = 0.85f;

		// Token: 0x04000F6A RID: 3946
		private const float CacheGrowthRateRange = 0.05f;

		// Token: 0x04000F6B RID: 3947
		private static Game cachedGame;

		// Token: 0x04000F6C RID: 3948
		[TupleElementNames(new string[]
		{
			"growthRate",
			"perDay"
		})]
		private static readonly Dictionary<SimplifiedPastureNutritionSimulator.CacheKey, ValueTuple<float, float>> nutritionCache = new Dictionary<SimplifiedPastureNutritionSimulator.CacheKey, ValueTuple<float, float>>();

		// Token: 0x020025B9 RID: 9657
		private struct CacheKey : IEquatable<SimplifiedPastureNutritionSimulator.CacheKey>
		{
			// Token: 0x0600F09E RID: 61598 RVA: 0x0058879C File Offset: 0x0058699C
			public bool Equals(SimplifiedPastureNutritionSimulator.CacheKey other)
			{
				return object.Equals(this.map, other.map) && object.Equals(this.plantDef, other.plantDef) && this.mapRespawnChance.Equals(other.mapRespawnChance);
			}

			// Token: 0x0600F09F RID: 61599 RVA: 0x005887D8 File Offset: 0x005869D8
			public override bool Equals(object obj)
			{
				if (obj is SimplifiedPastureNutritionSimulator.CacheKey)
				{
					SimplifiedPastureNutritionSimulator.CacheKey other = (SimplifiedPastureNutritionSimulator.CacheKey)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x0600F0A0 RID: 61600 RVA: 0x005887FD File Offset: 0x005869FD
			public override int GetHashCode()
			{
				return HashCode.Combine<Map, ThingDef, float>(this.map, this.plantDef, this.mapRespawnChance);
			}

			// Token: 0x04009AAD RID: 39597
			public Map map;

			// Token: 0x04009AAE RID: 39598
			public ThingDef plantDef;

			// Token: 0x04009AAF RID: 39599
			public float mapRespawnChance;
		}
	}
}
