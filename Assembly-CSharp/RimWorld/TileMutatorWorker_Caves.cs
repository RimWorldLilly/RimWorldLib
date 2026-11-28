using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200131D RID: 4893
	public class TileMutatorWorker_Caves : TileMutatorWorker
	{
		// Token: 0x170014B3 RID: 5299
		// (get) Token: 0x0600795C RID: 31068 RVA: 0x002B254E File Offset: 0x002B074E
		protected virtual float BranchChance
		{
			get
			{
				return 0.02f;
			}
		}

		// Token: 0x170014B4 RID: 5300
		// (get) Token: 0x0600795D RID: 31069 RVA: 0x002B2555 File Offset: 0x002B0755
		protected virtual float WidthOffsetPerCell
		{
			get
			{
				return 0.034f;
			}
		}

		// Token: 0x170014B5 RID: 5301
		// (get) Token: 0x0600795E RID: 31070 RVA: 0x002B255C File Offset: 0x002B075C
		protected virtual float MinTunnelWidth
		{
			get
			{
				return 1.4f;
			}
		}

		// Token: 0x0600795F RID: 31071 RVA: 0x002B2563 File Offset: 0x002B0763
		public TileMutatorWorker_Caves(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007960 RID: 31072 RVA: 0x002B256C File Offset: 0x002B076C
		public override void Init(Map map)
		{
			this.directionNoise = new Perlin(0.002050000010058284, 2.0, 0.5, 4, Rand.Int, QualityMode.Medium);
		}

		// Token: 0x06007961 RID: 31073 RVA: 0x002B259B File Offset: 0x002B079B
		public override void GeneratePostElevationFertility(Map map)
		{
			this.GenerateCaves(map);
		}

		// Token: 0x06007962 RID: 31074 RVA: 0x002B25A4 File Offset: 0x002B07A4
		protected void GenerateCaves(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			BoolGrid visited = new BoolGrid(map);
			List<IntVec3> group = new List<IntVec3>();
			MapGenCavesUtility.CaveGenParms @default = MapGenCavesUtility.CaveGenParms.Default;
			@default.widthOffsetPerCell = this.WidthOffsetPerCell;
			@default.minTunnelWidth = this.MinTunnelWidth;
			@default.branchChance = this.BranchChance;
			@default.allowBranchingAfterThisManyCells = 10;
			MapGenCavesUtility.GenerateCaves(map, visited, group, this.directionNoise, @default, (IntVec3 cell) => this.ShouldCarve(cell, elevation, map));
		}

		// Token: 0x06007963 RID: 31075 RVA: 0x002B2638 File Offset: 0x002B0838
		public override void GeneratePostTerrain(Map map)
		{
			Perlin perlin = new Perlin(0.07999999821186066, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			Perlin perlin2 = new Perlin(0.1599999964237213, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			MapGenFloatGrid caves = MapGenerator.Caves;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (caves[intVec] > 0f && elevation[intVec] > 0f && !intVec.GetTerrain(map).IsRiver)
				{
					float num = (float)perlin.GetValue((double)intVec.x, 0.0, (double)intVec.z);
					float num2 = (float)perlin2.GetValue((double)intVec.x, 0.0, (double)intVec.z);
					if (num > 0.93f)
					{
						map.terrainGrid.SetTerrain(intVec, MapGenUtility.ShallowFreshWaterTerrainAt(intVec, map));
					}
					else if (num2 > 0.55f)
					{
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Gravel);
					}
				}
			}
		}

		// Token: 0x06007964 RID: 31076 RVA: 0x002B2794 File Offset: 0x002B0994
		protected virtual bool ShouldCarve(IntVec3 c, MapGenFloatGrid elevation, Map map)
		{
			return TileMutatorWorker_Caves.IsRock(c, elevation, map);
		}

		// Token: 0x06007965 RID: 31077 RVA: 0x002B279E File Offset: 0x002B099E
		public static bool IsRock(IntVec3 c, MapGenFloatGrid elevation, Map map)
		{
			return c.InBounds(map) && elevation[c] > 0.7f;
		}

		// Token: 0x04004C3C RID: 19516
		protected ModuleBase directionNoise;

		// Token: 0x04004C3D RID: 19517
		public const float DirectionNoiseFrequency = 0.00205f;

		// Token: 0x04004C3E RID: 19518
		private const int AllowBranchingAfterThisManyCells = 10;

		// Token: 0x04004C3F RID: 19519
		private const float WaterFrequency = 0.08f;

		// Token: 0x04004C40 RID: 19520
		private const float GravelFrequency = 0.16f;

		// Token: 0x04004C41 RID: 19521
		private const float WaterThreshold = 0.93f;

		// Token: 0x04004C42 RID: 19522
		private const float GravelThreshold = 0.55f;
	}
}
