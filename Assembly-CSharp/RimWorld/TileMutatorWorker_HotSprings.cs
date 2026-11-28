using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001329 RID: 4905
	public class TileMutatorWorker_HotSprings : TileMutatorWorker
	{
		// Token: 0x0600799F RID: 31135 RVA: 0x002B4945 File Offset: 0x002B2B45
		public TileMutatorWorker_HotSprings(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079A0 RID: 31136 RVA: 0x002B4950 File Offset: 0x002B2B50
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.springNoise = new Perlin(0.05000000074505806, 1.5, 0.5, 4, Rand.Int, QualityMode.Medium);
			this.springNoise = new ScaleBias(0.5, 0.5, this.springNoise);
			ModuleBase moduleBase = MapNoiseUtility.CreateFalloffRadius((float)map.Size.x * 0.35f, map.Center.ToVector2(), 0.05f, true);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.015f, 25f, 1, -1);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "hot springs area");
			this.springNoise = new Multiply(this.springNoise, moduleBase);
			NoiseDebugUI.StoreNoiseRender(this.springNoise, "hot springs");
		}

		// Token: 0x060079A1 RID: 31137 RVA: 0x002B4A24 File Offset: 0x002B2C24
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.springNoise.GetValue(intVec) > 0.65f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x060079A2 RID: 31138 RVA: 0x002B4A90 File Offset: 0x002B2C90
		public override void GeneratePostTerrain(Map map)
		{
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.springNoise.GetValue(intVec);
				if (value > 0.85f)
				{
					map.terrainGrid.SetTerrain(intVec, TerrainDefOf.HotSpring);
				}
				else if (value > 0.65f)
				{
					map.terrainGrid.SetTerrain(intVec, GenStep_RocksFromGrid.RockDefAt(intVec).building.naturalTerrain);
				}
			}
		}

		// Token: 0x04004CA2 RID: 19618
		private const float ShapeNoiseFrequency = 0.015f;

		// Token: 0x04004CA3 RID: 19619
		private const float ShapeNoiseStrength = 25f;

		// Token: 0x04004CA4 RID: 19620
		private const float NoiseFrequency = 0.05f;

		// Token: 0x04004CA5 RID: 19621
		private const float NoiseLacunarity = 1.5f;

		// Token: 0x04004CA6 RID: 19622
		private const float NoisePersistence = 0.5f;

		// Token: 0x04004CA7 RID: 19623
		private const int NoiseOctaves = 4;

		// Token: 0x04004CA8 RID: 19624
		private const float PoolsRadius = 0.35f;

		// Token: 0x04004CA9 RID: 19625
		private const float SpringThreshold = 0.85f;

		// Token: 0x04004CAA RID: 19626
		private const float StoneThreshold = 0.65f;

		// Token: 0x04004CAB RID: 19627
		private const float FalloffExp = 0.05f;

		// Token: 0x04004CAC RID: 19628
		private ModuleBase springNoise;
	}
}
