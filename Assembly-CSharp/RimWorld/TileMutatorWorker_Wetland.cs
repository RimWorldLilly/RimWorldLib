using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200134E RID: 4942
	public class TileMutatorWorker_Wetland : TileMutatorWorker
	{
		// Token: 0x06007A5A RID: 31322 RVA: 0x002B9C70 File Offset: 0x002B7E70
		public TileMutatorWorker_Wetland(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A5B RID: 31323 RVA: 0x002B9C7C File Offset: 0x002B7E7C
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.wetlandNoise = new RidgedMultifractal(0.029999999329447746, 2.0, 2, Rand.Int, QualityMode.High);
			this.wetlandNoise = new Clamp(0.0, 1.0, this.wetlandNoise);
			NoiseDebugUI.StoreNoiseRender(this.wetlandNoise, "wetland");
		}

		// Token: 0x06007A5C RID: 31324 RVA: 0x002B9CE8 File Offset: 0x002B7EE8
		public override void GeneratePostTerrain(Map map)
		{
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (intVec.GetEdifice(map) == null)
				{
					TerrainDef terrain = intVec.GetTerrain(map);
					if (!terrain.IsWater)
					{
						float value = this.wetlandNoise.GetValue(intVec);
						if (value > 0.35f)
						{
							if (terrain.IsRock)
							{
								map.terrainGrid.SetTerrain(intVec, MapGenUtility.MudTerrainAt(intVec, map));
							}
							else
							{
								map.terrainGrid.SetTerrain(intVec, MapGenUtility.ShallowFreshWaterTerrainAt(intVec, map));
							}
						}
						else if (value > 0.015f && MapGenUtility.ShouldGenerateBeachSand(intVec, map))
						{
							map.terrainGrid.SetTerrain(intVec, MapGenUtility.MudTerrainAt(intVec, map));
						}
					}
				}
			}
		}

		// Token: 0x04004D4E RID: 19790
		private const float RidgedFreq = 0.03f;

		// Token: 0x04004D4F RID: 19791
		private const float RidgedLac = 2f;

		// Token: 0x04004D50 RID: 19792
		private const int RidgedOctaves = 2;

		// Token: 0x04004D51 RID: 19793
		private const float MudThreshold = 0.015f;

		// Token: 0x04004D52 RID: 19794
		private const float WaterThreshold = 0.35f;

		// Token: 0x04004D53 RID: 19795
		private ModuleBase wetlandNoise;
	}
}
