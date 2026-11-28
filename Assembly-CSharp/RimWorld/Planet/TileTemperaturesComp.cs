using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02002338 RID: 9016
	public class TileTemperaturesComp : WorldComponent
	{
		// Token: 0x0600DEC2 RID: 57026 RVA: 0x0052DF25 File Offset: 0x0052C125
		public TileTemperaturesComp(World world) : base(world)
		{
			this.ClearCaches();
		}

		// Token: 0x0600DEC3 RID: 57027 RVA: 0x0052DF34 File Offset: 0x0052C134
		public override void WorldComponentTick()
		{
			foreach (KeyValuePair<PlanetLayerDef, List<int>> keyValuePair in this.usedSlots)
			{
				PlanetLayerDef planetLayerDef;
				List<int> list;
				keyValuePair.Deconstruct(out planetLayerDef, out list);
				PlanetLayerDef key = planetLayerDef;
				foreach (int num in list)
				{
					this.cache[key][num].CheckCache();
				}
			}
			if (Find.TickManager.TicksGame % 300 == 84)
			{
				foreach (KeyValuePair<PlanetLayerDef, List<int>> keyValuePair in this.usedSlots)
				{
					PlanetLayerDef planetLayerDef;
					List<int> list;
					keyValuePair.Deconstruct(out planetLayerDef, out list);
					PlanetLayerDef key2 = planetLayerDef;
					List<int> list2 = list;
					if (list2.Any<int>())
					{
						this.cache[key2][list2[0]] = null;
						list2.RemoveAt(0);
					}
				}
			}
		}

		// Token: 0x0600DEC4 RID: 57028 RVA: 0x0052E064 File Offset: 0x0052C264
		public float GetOutdoorTemp(PlanetTile tile)
		{
			return this.RetrieveCachedData(tile).GetOutdoorTemp();
		}

		// Token: 0x0600DEC5 RID: 57029 RVA: 0x0052E072 File Offset: 0x0052C272
		public float GetSeasonalTemp(PlanetTile tile)
		{
			return this.RetrieveCachedData(tile).GetSeasonalTemp();
		}

		// Token: 0x0600DEC6 RID: 57030 RVA: 0x0052E080 File Offset: 0x0052C280
		public float OutdoorTemperatureAt(PlanetTile tile, int absTick)
		{
			return this.RetrieveCachedData(tile).OutdoorTemperatureAt(absTick);
		}

		// Token: 0x0600DEC7 RID: 57031 RVA: 0x0052E08F File Offset: 0x0052C28F
		public float OffsetFromDailyRandomVariation(PlanetTile tile, int absTick)
		{
			return this.RetrieveCachedData(tile).OffsetFromDailyRandomVariation(absTick);
		}

		// Token: 0x0600DEC8 RID: 57032 RVA: 0x0052E09E File Offset: 0x0052C29E
		public float AverageTemperatureForTwelfth(PlanetTile tile, Twelfth twelfth)
		{
			return this.RetrieveCachedData(tile).AverageTemperatureForTwelfth(twelfth);
		}

		// Token: 0x0600DEC9 RID: 57033 RVA: 0x0052E0B0 File Offset: 0x0052C2B0
		public bool SeasonAcceptableFor(PlanetTile tile, ThingDef animalRace)
		{
			if (!tile.Valid)
			{
				return true;
			}
			float seasonalTemp = this.GetSeasonalTemp(tile);
			return seasonalTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) && seasonalTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null);
		}

		// Token: 0x0600DECA RID: 57034 RVA: 0x0052E0F0 File Offset: 0x0052C2F0
		public bool OutdoorTemperatureAcceptableFor(PlanetTile tile, ThingDef animalRace)
		{
			if (!tile.Valid)
			{
				return true;
			}
			float outdoorTemp = this.GetOutdoorTemp(tile);
			return outdoorTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) && outdoorTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null);
		}

		// Token: 0x0600DECB RID: 57035 RVA: 0x0052E130 File Offset: 0x0052C330
		public bool SeasonAndOutdoorTemperatureAcceptableFor(PlanetTile tile, ThingDef animalRace)
		{
			return this.SeasonAcceptableFor(tile, animalRace) && this.OutdoorTemperatureAcceptableFor(tile, animalRace);
		}

		// Token: 0x0600DECC RID: 57036 RVA: 0x0052E146 File Offset: 0x0052C346
		public void ClearCaches()
		{
			this.cache = new Dictionary<PlanetLayerDef, TileTemperaturesComp.CachedTileTemperatureData[]>();
			this.usedSlots = new Dictionary<PlanetLayerDef, List<int>>();
		}

		// Token: 0x0600DECD RID: 57037 RVA: 0x0052E160 File Offset: 0x0052C360
		private TileTemperaturesComp.CachedTileTemperatureData RetrieveCachedData(PlanetTile tile)
		{
			if (!this.cache.ContainsKey(tile.LayerDef))
			{
				this.cache[tile.LayerDef] = new TileTemperaturesComp.CachedTileTemperatureData[tile.Layer.TilesCount];
			}
			if (!this.usedSlots.ContainsKey(tile.LayerDef))
			{
				this.usedSlots[tile.LayerDef] = new List<int>();
			}
			if (this.cache[tile.LayerDef][tile.tileId] != null)
			{
				return this.cache[tile.LayerDef][tile.tileId];
			}
			this.cache[tile.LayerDef][tile.tileId] = new TileTemperaturesComp.CachedTileTemperatureData(tile);
			this.usedSlots[tile.LayerDef].Add(tile.tileId);
			return this.cache[tile.LayerDef][tile.tileId];
		}

		// Token: 0x04008F0D RID: 36621
		private Dictionary<PlanetLayerDef, TileTemperaturesComp.CachedTileTemperatureData[]> cache;

		// Token: 0x04008F0E RID: 36622
		private Dictionary<PlanetLayerDef, List<int>> usedSlots;

		// Token: 0x02003E1B RID: 15899
		private class CachedTileTemperatureData
		{
			// Token: 0x06016087 RID: 90247 RVA: 0x006E0844 File Offset: 0x006DEA44
			public CachedTileTemperatureData(PlanetTile tile)
			{
				this.tile = tile;
				int seed = Gen.HashCombineInt(tile.GetHashCode(), 199372327);
				this.dailyVariationPerlinCached = new Perlin(4.999999873689376E-06, 2.0, 0.5, 3, seed, QualityMode.Medium);
				this.twelfthlyTempAverages = new float[12];
				for (int i = 0; i < 12; i++)
				{
					this.twelfthlyTempAverages[i] = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				}
				this.CheckCache();
			}

			// Token: 0x06016088 RID: 90248 RVA: 0x006E08F0 File Offset: 0x006DEAF0
			public float GetOutdoorTemp()
			{
				return this.cachedOutdoorTemp;
			}

			// Token: 0x06016089 RID: 90249 RVA: 0x006E08F8 File Offset: 0x006DEAF8
			public float GetSeasonalTemp()
			{
				return this.cachedSeasonalTemp;
			}

			// Token: 0x0601608A RID: 90250 RVA: 0x006E0900 File Offset: 0x006DEB00
			public float OutdoorTemperatureAt(int absTick)
			{
				return this.CalculateOutdoorTemperatureAtTile(absTick, true);
			}

			// Token: 0x0601608B RID: 90251 RVA: 0x006E090A File Offset: 0x006DEB0A
			public float OffsetFromDailyRandomVariation(int absTick)
			{
				return (float)this.dailyVariationPerlinCached.GetValue((double)absTick, 0.0, 0.0) * 7f;
			}

			// Token: 0x0601608C RID: 90252 RVA: 0x006E0932 File Offset: 0x006DEB32
			public float AverageTemperatureForTwelfth(Twelfth twelfth)
			{
				return this.twelfthlyTempAverages[(int)twelfth];
			}

			// Token: 0x0601608D RID: 90253 RVA: 0x006E093C File Offset: 0x006DEB3C
			public void CheckCache()
			{
				if (this.tickCachesNeedReset <= Find.TickManager.TicksGame)
				{
					this.tickCachesNeedReset = Find.TickManager.TicksGame + 60;
					Map map = Current.Game.FindMap(this.tile);
					this.cachedOutdoorTemp = this.OutdoorTemperatureAt(Find.TickManager.TicksAbs);
					if (map != null)
					{
						this.cachedOutdoorTemp += map.gameConditionManager.AggregateTemperatureOffset();
					}
					this.cachedSeasonalTemp = this.CalculateOutdoorTemperatureAtTile(Find.TickManager.TicksAbs, false);
				}
			}

			// Token: 0x0601608E RID: 90254 RVA: 0x006E09C8 File Offset: 0x006DEBC8
			private float CalculateOutdoorTemperatureAtTile(int absTick, bool includeDailyVariations)
			{
				if (absTick == 0)
				{
					absTick = 1;
				}
				float num = Find.WorldGrid[this.tile].temperature + GenTemperature.OffsetFromSeasonCycle(absTick, this.tile);
				if (includeDailyVariations)
				{
					num += this.OffsetFromDailyRandomVariation(absTick) + GenTemperature.OffsetFromSunCycle(absTick, this.tile);
				}
				return num;
			}

			// Token: 0x0400F515 RID: 62741
			private readonly PlanetTile tile;

			// Token: 0x0400F516 RID: 62742
			private int tickCachesNeedReset = int.MinValue;

			// Token: 0x0400F517 RID: 62743
			private float cachedOutdoorTemp = float.MinValue;

			// Token: 0x0400F518 RID: 62744
			private float cachedSeasonalTemp = float.MinValue;

			// Token: 0x0400F519 RID: 62745
			private float[] twelfthlyTempAverages;

			// Token: 0x0400F51A RID: 62746
			private Perlin dailyVariationPerlinCached;

			// Token: 0x0400F51B RID: 62747
			private const int CachedTempUpdateInterval = 60;
		}
	}
}
