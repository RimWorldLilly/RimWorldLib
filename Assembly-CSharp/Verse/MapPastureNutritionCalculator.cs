using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000238 RID: 568
	public class MapPastureNutritionCalculator
	{
		// Token: 0x06001088 RID: 4232 RVA: 0x0005F208 File Offset: 0x0005D408
		public void Reset(Map map)
		{
			this.map = map;
			this.cachedSeasonalDetailed.Clear();
			this.cachedSeasonalByTerrain.Clear();
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0005F228 File Offset: 0x0005D428
		public MapPastureNutritionCalculator.NutritionPerDayPerQuadrum CalculateAverageNutritionPerDay(TerrainDef terrain)
		{
			MapPastureNutritionCalculator.NutritionPerDayPerQuadrum nutritionPerDayPerQuadrum;
			if (!this.cachedSeasonalByTerrain.TryGetValue(terrain, out nutritionPerDayPerQuadrum))
			{
				nutritionPerDayPerQuadrum = default(MapPastureNutritionCalculator.NutritionPerDayPerQuadrum);
				foreach (ThingDef plantDef in this.map.plantGrowthRateCalculator.WildGrazingPlants)
				{
					MapPastureNutritionCalculator.NutritionPerDayPerQuadrum other = this.CalculateAverageNutritionPerDay(plantDef, terrain);
					nutritionPerDayPerQuadrum.AddFrom(other);
				}
				this.cachedSeasonalByTerrain.TryAdd(terrain, nutritionPerDayPerQuadrum);
			}
			return nutritionPerDayPerQuadrum;
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x0005F2B8 File Offset: 0x0005D4B8
		private MapPastureNutritionCalculator.NutritionPerDayPerQuadrum CalculateAverageNutritionPerDay(ThingDef plantDef, TerrainDef terrain)
		{
			MapPastureNutritionCalculator.NutritionPerDayPerQuadrum result;
			using (ProfilerBlock.Scope("CalculateAverageNutritionPerDay"))
			{
				Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum> dictionary;
				if (!this.cachedSeasonalDetailed.TryGetValue(plantDef, out dictionary))
				{
					dictionary = new Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>();
					this.cachedSeasonalDetailed.Add(plantDef, dictionary);
				}
				MapPastureNutritionCalculator.NutritionPerDayPerQuadrum nutritionPerDayPerQuadrum;
				if (!dictionary.TryGetValue(terrain, out nutritionPerDayPerQuadrum))
				{
					nutritionPerDayPerQuadrum = default(MapPastureNutritionCalculator.NutritionPerDayPerQuadrum);
					dictionary.Add(terrain, nutritionPerDayPerQuadrum);
					nutritionPerDayPerQuadrum[Quadrum.Aprimay] = this.GetAverageNutritionPerDay(Quadrum.Aprimay, plantDef, terrain);
					nutritionPerDayPerQuadrum[Quadrum.Decembary] = this.GetAverageNutritionPerDay(Quadrum.Decembary, plantDef, terrain);
					nutritionPerDayPerQuadrum[Quadrum.Jugust] = this.GetAverageNutritionPerDay(Quadrum.Jugust, plantDef, terrain);
					nutritionPerDayPerQuadrum[Quadrum.Septober] = this.GetAverageNutritionPerDay(Quadrum.Septober, plantDef, terrain);
				}
				result = nutritionPerDayPerQuadrum;
			}
			return result;
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0005F378 File Offset: 0x0005D578
		public float GetAverageNutritionPerDayToday(TerrainDef terrainDef)
		{
			float result;
			using (ProfilerBlock.Scope("GetAverageNutritionPerDayToday"))
			{
				float num = 0f;
				foreach (ThingDef plantDef in this.map.plantGrowthRateCalculator.WildGrazingPlants)
				{
					num += this.GetAverageNutritionPerDayToday(plantDef, terrainDef);
				}
				result = num;
			}
			return result;
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0005F40C File Offset: 0x0005D60C
		private float GetAverageNutritionPerDayToday(ThingDef plantDef, TerrainDef terrainDef)
		{
			if (terrainDef.fertility <= 0f)
			{
				return 0f;
			}
			float result;
			using (ProfilerBlock.Scope("GetAverageNutritionPerDayToday Plant/Terrain"))
			{
				int ticksAbs = Find.TickManager.TicksAbs;
				int nowTicks = ticksAbs - ticksAbs % 60000;
				float growthRate = this.map.plantGrowthRateCalculator.GrowthRateForDay(nowTicks, plantDef, terrainDef);
				result = this.ComputeNutritionProducedPerDay(plantDef, growthRate);
			}
			return result;
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0005F488 File Offset: 0x0005D688
		public float GetAverageNutritionPerDay(Quadrum quadrum, TerrainDef terrainDef)
		{
			float num = 0f;
			foreach (ThingDef plantDef in this.map.plantGrowthRateCalculator.WildGrazingPlants)
			{
				num += this.GetAverageNutritionPerDay(quadrum, plantDef, terrainDef);
			}
			return num;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0005F4F4 File Offset: 0x0005D6F4
		public float GetAverageNutritionPerDay(Quadrum quadrum, ThingDef plantDef, TerrainDef terrainDef)
		{
			float growthRate = this.map.plantGrowthRateCalculator.QuadrumGrowthRateFor(quadrum, plantDef, terrainDef);
			return this.ComputeNutritionProducedPerDay(plantDef, growthRate);
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0005F51D File Offset: 0x0005D71D
		private float ComputeNutritionProducedPerDay(ThingDef plantDef, float growthRate)
		{
			return SimplifiedPastureNutritionSimulator.NutritionProducedPerDay(this.map, plantDef, growthRate, this.map.wildPlantSpawner.CachedChanceFromDensity);
		}

		// Token: 0x04000F5D RID: 3933
		public Map map;

		// Token: 0x04000F5E RID: 3934
		private readonly Dictionary<ThingDef, Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>> cachedSeasonalDetailed = new Dictionary<ThingDef, Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>>();

		// Token: 0x04000F5F RID: 3935
		private readonly Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum> cachedSeasonalByTerrain = new Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>();

		// Token: 0x020025B5 RID: 9653
		public struct NutritionPerDayPerQuadrum
		{
			// Token: 0x0600F08C RID: 61580 RVA: 0x00588582 File Offset: 0x00586782
			public float ForQuadrum(Quadrum q)
			{
				return this[q];
			}

			// Token: 0x0600F08D RID: 61581 RVA: 0x0058858C File Offset: 0x0058678C
			public void AddFrom(MapPastureNutritionCalculator.NutritionPerDayPerQuadrum other)
			{
				this[0] = this[0] + other[0];
				this[3] = this[3] + other[3];
				this[1] = this[1] + other[1];
				this[2] = this[2] + other[2];
			}

			// Token: 0x17002A70 RID: 10864
			public float this[int key]
			{
				get
				{
					return this.GetValue(key);
				}
				set
				{
					this.SetValue(key, value);
				}
			}

			// Token: 0x17002A71 RID: 10865
			public float this[Quadrum key]
			{
				get
				{
					return this.GetValue((int)key);
				}
				set
				{
					this.SetValue((int)key, value);
				}
			}

			// Token: 0x0600F092 RID: 61586 RVA: 0x0058861B File Offset: 0x0058681B
			public float GetValue(int key)
			{
				if (key == 0)
				{
					return this.quadrumOne;
				}
				if (key == 1)
				{
					return this.quadrumTwo;
				}
				if (key == 2)
				{
					return this.quadrumThree;
				}
				if (key == 3)
				{
					return this.quadrumFour;
				}
				return -1f;
			}

			// Token: 0x0600F093 RID: 61587 RVA: 0x0058864D File Offset: 0x0058684D
			public void SetValue(int key, float value)
			{
				if (key == 0)
				{
					this.quadrumOne = value;
					return;
				}
				if (key == 1)
				{
					this.quadrumTwo = value;
					return;
				}
				if (key == 2)
				{
					this.quadrumThree = value;
					return;
				}
				if (key == 3)
				{
					this.quadrumFour = value;
				}
			}

			// Token: 0x04009AA2 RID: 39586
			private float quadrumOne;

			// Token: 0x04009AA3 RID: 39587
			private float quadrumTwo;

			// Token: 0x04009AA4 RID: 39588
			private float quadrumThree;

			// Token: 0x04009AA5 RID: 39589
			private float quadrumFour;
		}
	}
}
