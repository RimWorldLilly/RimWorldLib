using System;
using RimWorld.Planet;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001320 RID: 4896
	public class TileMutatorWorker_Coast : TileMutatorWorker
	{
		// Token: 0x170014B6 RID: 5302
		// (get) Token: 0x0600796D RID: 31085 RVA: 0x002B2D84 File Offset: 0x002B0F84
		protected virtual float MaxForDeepWater
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x170014B7 RID: 5303
		// (get) Token: 0x0600796E RID: 31086 RVA: 0x002B2D8B File Offset: 0x002B0F8B
		protected virtual float MaxForShallowWater
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x170014B8 RID: 5304
		// (get) Token: 0x0600796F RID: 31087 RVA: 0x002B2D92 File Offset: 0x002B0F92
		protected virtual float MaxForSand
		{
			get
			{
				return 0.6f;
			}
		}

		// Token: 0x170014B9 RID: 5305
		// (get) Token: 0x06007970 RID: 31088 RVA: 0x002B2D99 File Offset: 0x002B0F99
		protected virtual FloatRange CoastOffset
		{
			get
			{
				return new FloatRange(0.1f, 0.2f);
			}
		}

		// Token: 0x06007971 RID: 31089 RVA: 0x002B2DAA File Offset: 0x002B0FAA
		public TileMutatorWorker_Coast(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007972 RID: 31090 RVA: 0x002B2DB4 File Offset: 0x002B0FB4
		public override void Init(Map map)
		{
			float coastAngle = this.GetCoastAngle(map.Tile);
			this.coastNoise = MapNoiseUtility.FalloffAtAngle(coastAngle, this.CoastOffset.RandomInRange, map);
			this.coastNoise = MapNoiseUtility.AddDisplacementNoise(this.coastNoise, 0.006f, 30f, 2, -1);
			this.coastNoise = MapNoiseUtility.AddDisplacementNoise(this.coastNoise, 0.015f, 25f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "coast");
		}

		// Token: 0x06007973 RID: 31091 RVA: 0x002B2E34 File Offset: 0x002B1034
		protected virtual float GetCoastAngle(PlanetTile tile)
		{
			return Find.World.CoastAngleAt(tile, BiomeDefOf.Ocean).GetValueOrDefault();
		}

		// Token: 0x06007974 RID: 31092 RVA: 0x002B2E5C File Offset: 0x002B105C
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.GetNoiseValue(intVec) < this.MaxForDeepWater)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x06007975 RID: 31093 RVA: 0x002B2EC4 File Offset: 0x002B10C4
		public override void GeneratePostTerrain(Map map)
		{
			foreach (IntVec3 intVec in map.AllCells)
			{
				TerrainDef terrainDef = this.CoastTerrainAt(intVec, map);
				if (terrainDef != null && (intVec.GetTerrain(map).categoryType != TerrainDef.TerrainCategoryType.Stone || (intVec.GetEdifice(map) == null && terrainDef.IsWater)))
				{
					map.terrainGrid.SetTerrain(intVec, terrainDef);
				}
			}
		}

		// Token: 0x06007976 RID: 31094 RVA: 0x002B2F44 File Offset: 0x002B1144
		protected virtual float GetNoiseValue(IntVec3 cell)
		{
			return this.coastNoise.GetValue(cell);
		}

		// Token: 0x06007977 RID: 31095 RVA: 0x002B2F52 File Offset: 0x002B1152
		protected virtual TerrainDef DeepWaterTerrainAt(IntVec3 cell, Map map)
		{
			return MapGenUtility.DeepOceanWaterTerrainAt(cell, map);
		}

		// Token: 0x06007978 RID: 31096 RVA: 0x002B2F5B File Offset: 0x002B115B
		protected virtual TerrainDef ShallowWaterTerrainAt(IntVec3 cell, Map map)
		{
			return MapGenUtility.ShallowOceanWaterTerrainAt(cell, map);
		}

		// Token: 0x06007979 RID: 31097 RVA: 0x002B2F64 File Offset: 0x002B1164
		protected virtual TerrainDef BeachTerrainAt(IntVec3 cell, Map map)
		{
			return MapGenUtility.BeachTerrainAt(cell, map);
		}

		// Token: 0x0600797A RID: 31098 RVA: 0x002B2F70 File Offset: 0x002B1170
		protected virtual TerrainDef CoastTerrainAt(IntVec3 cell, Map map)
		{
			if (this.coastNoise == null)
			{
				return null;
			}
			float noiseValue = this.GetNoiseValue(cell);
			if (noiseValue < this.MaxForDeepWater)
			{
				return this.DeepWaterTerrainAt(cell, map);
			}
			if (noiseValue < this.MaxForShallowWater)
			{
				return this.ShallowWaterTerrainAt(cell, map);
			}
			if (noiseValue < this.MaxForSand && MapGenUtility.ShouldGenerateBeachSand(cell, map))
			{
				return this.BeachTerrainAt(cell, map);
			}
			return null;
		}

		// Token: 0x04004C58 RID: 19544
		protected ModuleBase coastNoise;

		// Token: 0x04004C59 RID: 19545
		protected const float CoastMacroNoiseFrequency = 0.006f;

		// Token: 0x04004C5A RID: 19546
		protected const float CoastMacroNoiseStrength = 30f;

		// Token: 0x04004C5B RID: 19547
		protected const float CoastNoiseFrequency = 0.015f;

		// Token: 0x04004C5C RID: 19548
		protected const float CoastNoiseStrength = 25f;
	}
}
