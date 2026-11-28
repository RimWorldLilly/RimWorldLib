using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000239 RID: 569
	public class MapPlantGrowthRateCalculator
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06001091 RID: 4241 RVA: 0x0005F55A File Offset: 0x0005D75A
		public List<TerrainDef> TerrainDefs
		{
			get
			{
				this.ComputeIfDirty();
				return this.terrainDefs;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06001092 RID: 4242 RVA: 0x0005F568 File Offset: 0x0005D768
		public List<ThingDef> WildGrazingPlants
		{
			get
			{
				this.ComputeIfDirty();
				return this.wildGrazingPlants;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06001093 RID: 4243 RVA: 0x0005F576 File Offset: 0x0005D776
		public List<ThingDef> GrazingAnimals
		{
			get
			{
				this.ComputeIfDirty();
				return this.includeAnimalTypes;
			}
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x0005F584 File Offset: 0x0005D784
		public void BuildFor(PlanetTile tile)
		{
			this.tile = tile;
			this.longLat = Find.WorldGrid.LongLatOf(tile);
			this.biome = Find.WorldGrid[tile].PrimaryBiome;
			this.ComputeIfDirty();
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x0005F5BA File Offset: 0x0005D7BA
		public void BuildFor(Map map)
		{
			this.tile = map.Tile;
			this.longLat = Find.WorldGrid.LongLatOf(map.Tile);
			this.biome = map.Biome;
			this.ComputeIfDirty();
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0005F5F0 File Offset: 0x0005D7F0
		public float GrowthRateForDay(int nowTicks, ThingDef plantDef, TerrainDef terrainDef)
		{
			float result;
			using (ProfilerBlock.Scope("GrowthRateForDay"))
			{
				this.ComputeIfDirty();
				int index = nowTicks / 60000 % 60;
				result = MapPlantGrowthRateCalculator.ComputeGrowthRate(plantDef, terrainDef, this.dailyGrowthRates[index]);
			}
			return result;
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0005F650 File Offset: 0x0005D850
		public float QuadrumGrowthRateFor(Quadrum quadrum, ThingDef plantDef, TerrainDef terrainDef)
		{
			this.ComputeIfDirty();
			return MapPlantGrowthRateCalculator.ComputeGrowthRate(plantDef, terrainDef, this.seasonalGrowthRates[quadrum]);
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0005F66C File Offset: 0x0005D86C
		private static float ComputeGrowthRate(ThingDef plantDef, TerrainDef terrainDef, MapPlantGrowthRateCalculator.PlantGrowthRates rates)
		{
			if (!plantDef.plant.completelyIgnoreFertility && terrainDef.fertility < plantDef.plant.fertilityMin)
			{
				return 0f;
			}
			MapPlantGrowthRateCalculator.GrowthRateAccumulator growthRateAccumulator = rates.For(plantDef);
			return PlantUtility.GrowthRateFactorFor_Fertility(plantDef, terrainDef.fertility) * growthRateAccumulator.GrowthRateForTemperature * growthRateAccumulator.GrowthRateForGlow;
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0005F6C4 File Offset: 0x0005D8C4
		private void ComputeIfDirty()
		{
			if (!this.dirty)
			{
				return;
			}
			using (ProfilerBlock.Scope("MapPlantGrowthRateCalculator.ComputeIfDirty"))
			{
				this.dirty = false;
				this.includeAnimalTypes.Clear();
				this.seasonalGrowthRates.Clear();
				this.dailyGrowthRates.Clear();
				this.seasonalGrowthRates.Add(Quadrum.Aprimay, new MapPlantGrowthRateCalculator.PlantGrowthRates());
				this.seasonalGrowthRates.Add(Quadrum.Decembary, new MapPlantGrowthRateCalculator.PlantGrowthRates());
				this.seasonalGrowthRates.Add(Quadrum.Jugust, new MapPlantGrowthRateCalculator.PlantGrowthRates());
				this.seasonalGrowthRates.Add(Quadrum.Septober, new MapPlantGrowthRateCalculator.PlantGrowthRates());
				this.AddIncludedAnimals();
				this.terrainDefs = DefDatabase<TerrainDef>.AllDefsListForReading;
				this.wildGrazingPlants = this.biome.AllWildPlants.Where(new Func<ThingDef, bool>(MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals)).ToList<ThingDef>();
				this.CalculateDailyFertility();
				this.CalculateSeasonalFertility();
			}
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0005F7B8 File Offset: 0x0005D9B8
		private void CalculateDailyFertility()
		{
			for (int i = 0; i < 60; i++)
			{
				int num = i * 60000;
				int nowTicks = num - num % 60000;
				MapPlantGrowthRateCalculator.PlantGrowthRates plantGrowthRates = new MapPlantGrowthRateCalculator.PlantGrowthRates();
				this.dailyGrowthRates.Add(plantGrowthRates);
				foreach (ThingDef plantDef in this.wildGrazingPlants)
				{
					this.SimulateGrowthRateForDay(nowTicks, plantGrowthRates.For(plantDef));
				}
			}
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0005F844 File Offset: 0x0005DA44
		private void CalculateSeasonalFertility()
		{
			for (int i = 0; i < this.dailyGrowthRates.Count; i++)
			{
				Quadrum key = GenDate.Quadrum((long)(i * 60000), this.longLat.x);
				MapPlantGrowthRateCalculator.PlantGrowthRates plantGrowthRates = this.seasonalGrowthRates[key];
				foreach (KeyValuePair<ThingDef, MapPlantGrowthRateCalculator.GrowthRateAccumulator> keyValuePair in this.dailyGrowthRates[i].byPlant)
				{
					plantGrowthRates.For(keyValuePair.Value.plantDef).Accumulate(keyValuePair.Value);
				}
			}
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0005F8FC File Offset: 0x0005DAFC
		private void AddIncludedAnimals()
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (MapPlantGrowthRateCalculator.IsPastureAnimal(thingDef))
				{
					this.includeAnimalTypes.Add(thingDef);
				}
			}
			this.includeAnimalTypes.Sort((ThingDef a, ThingDef b) => string.CompareOrdinal(a.label, b.label));
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0005F980 File Offset: 0x0005DB80
		public static bool IsPastureAnimal(ThingDef td)
		{
			return !td.IsCorpse && td.race != null && td.race.Animal && td.race.Roamer;
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0005F9AC File Offset: 0x0005DBAC
		public static bool IsEdibleByPastureAnimals(ThingDef foodDef)
		{
			return foodDef.ingestible != null && foodDef.ingestible.preferability != FoodPreferability.Undefined && (FoodTypeFlags.VegetarianRoughAnimal & foodDef.ingestible.foodType) > FoodTypeFlags.None;
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0005F9DC File Offset: 0x0005DBDC
		private void SimulateGrowthRateForDay(int nowTicks, MapPlantGrowthRateCalculator.GrowthRateAccumulator growthRates)
		{
			int num = nowTicks - nowTicks % 60000;
			int num2 = 24;
			int num3 = 60000 / num2;
			for (int i = 0; i < num2; i++)
			{
				int num4 = num + i * num3;
				float cellTemp = Find.World.tileTemperatures.OutdoorTemperatureAt(this.tile, num4);
				float glow = GenCelestial.CelestialSunGlow(this.tile, num4);
				float grTemp = PlantUtility.GrowthRateFactorFor_Temperature(growthRates.plantDef, cellTemp);
				float grGlow = PlantUtility.GrowthRateFactorFor_Light(growthRates.plantDef, glow);
				growthRates.Accumulate(grTemp, grGlow);
			}
		}

		// Token: 0x04000F60 RID: 3936
		private Vector2 longLat;

		// Token: 0x04000F61 RID: 3937
		private PlanetTile tile;

		// Token: 0x04000F62 RID: 3938
		private BiomeDef biome;

		// Token: 0x04000F63 RID: 3939
		private bool dirty = true;

		// Token: 0x04000F64 RID: 3940
		private readonly List<ThingDef> includeAnimalTypes = new List<ThingDef>();

		// Token: 0x04000F65 RID: 3941
		private List<TerrainDef> terrainDefs;

		// Token: 0x04000F66 RID: 3942
		private List<ThingDef> wildGrazingPlants;

		// Token: 0x04000F67 RID: 3943
		private readonly Dictionary<Quadrum, MapPlantGrowthRateCalculator.PlantGrowthRates> seasonalGrowthRates = new Dictionary<Quadrum, MapPlantGrowthRateCalculator.PlantGrowthRates>();

		// Token: 0x04000F68 RID: 3944
		private readonly List<MapPlantGrowthRateCalculator.PlantGrowthRates> dailyGrowthRates = new List<MapPlantGrowthRateCalculator.PlantGrowthRates>();

		// Token: 0x020025B6 RID: 9654
		private class GrowthRateAccumulator
		{
			// Token: 0x0600F094 RID: 61588 RVA: 0x0058867D File Offset: 0x0058687D
			public GrowthRateAccumulator(ThingDef plantDef)
			{
				this.plantDef = plantDef;
			}

			// Token: 0x17002A72 RID: 10866
			// (get) Token: 0x0600F095 RID: 61589 RVA: 0x0058868C File Offset: 0x0058688C
			public float GrowthRateForTemperature
			{
				get
				{
					if (this.numSamples != 0)
					{
						return this.sumGrowthRateForTemperature / (float)this.numSamples;
					}
					return 0f;
				}
			}

			// Token: 0x17002A73 RID: 10867
			// (get) Token: 0x0600F096 RID: 61590 RVA: 0x005886AA File Offset: 0x005868AA
			public float GrowthRateForGlow
			{
				get
				{
					if (this.numSamples != 0)
					{
						return this.sumGrowthRateForGlow / (float)this.numSamples;
					}
					return 0f;
				}
			}

			// Token: 0x0600F097 RID: 61591 RVA: 0x005886C8 File Offset: 0x005868C8
			public void Accumulate(float grTemp, float grGlow)
			{
				this.sumGrowthRateForTemperature += grTemp;
				this.sumGrowthRateForGlow += grGlow;
				this.numSamples++;
			}

			// Token: 0x0600F098 RID: 61592 RVA: 0x005886F4 File Offset: 0x005868F4
			public void Accumulate(MapPlantGrowthRateCalculator.GrowthRateAccumulator other)
			{
				this.sumGrowthRateForTemperature += other.sumGrowthRateForTemperature;
				this.sumGrowthRateForGlow += other.sumGrowthRateForGlow;
				this.numSamples += other.numSamples;
			}

			// Token: 0x04009AA6 RID: 39590
			public readonly ThingDef plantDef;

			// Token: 0x04009AA7 RID: 39591
			private float sumGrowthRateForTemperature;

			// Token: 0x04009AA8 RID: 39592
			private float sumGrowthRateForGlow;

			// Token: 0x04009AA9 RID: 39593
			private int numSamples;
		}

		// Token: 0x020025B7 RID: 9655
		private class PlantGrowthRates
		{
			// Token: 0x0600F099 RID: 61593 RVA: 0x00588730 File Offset: 0x00586930
			public MapPlantGrowthRateCalculator.GrowthRateAccumulator For(ThingDef plantDef)
			{
				MapPlantGrowthRateCalculator.GrowthRateAccumulator growthRateAccumulator;
				if (!this.byPlant.TryGetValue(plantDef, out growthRateAccumulator))
				{
					growthRateAccumulator = new MapPlantGrowthRateCalculator.GrowthRateAccumulator(plantDef);
					this.byPlant.Add(plantDef, growthRateAccumulator);
				}
				return growthRateAccumulator;
			}

			// Token: 0x04009AAA RID: 39594
			public readonly Dictionary<ThingDef, MapPlantGrowthRateCalculator.GrowthRateAccumulator> byPlant = new Dictionary<ThingDef, MapPlantGrowthRateCalculator.GrowthRateAccumulator>();
		}
	}
}
