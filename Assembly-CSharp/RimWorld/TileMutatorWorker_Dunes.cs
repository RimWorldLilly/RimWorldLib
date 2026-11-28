using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001325 RID: 4901
	public class TileMutatorWorker_Dunes : TileMutatorWorker
	{
		// Token: 0x0600798B RID: 31115 RVA: 0x002B3A00 File Offset: 0x002B1C00
		public TileMutatorWorker_Dunes(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600798C RID: 31116 RVA: 0x002B3A0C File Offset: 0x002B1C0C
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.dunesNoise = new Voronoi2D(0.0024999999441206455, Rand.Int, 0.15f, 0.5f, true);
			this.dunesNoise = MapNoiseUtility.AddDisplacementNoise(this.dunesNoise, 0.015f, 20f, 2, -1);
			this.dunesNoise = new Scale(1.0, 1.0, 15.0, this.dunesNoise);
			this.dunesNoise = new Scale(1.0, 1.0, 1.0, this.dunesNoise);
			ModuleBase moduleBase = new Perlin(0.019999999552965164, 2.0, 0.5, 2, Rand.Int, QualityMode.Medium);
			moduleBase = new Multiply(moduleBase, new Const(7.5));
			this.dunesNoise = new Displace(this.dunesNoise, new Const(0.0), new Const(0.0), moduleBase);
			this.dunesNoise = new Rotate(0.0, (double)Rand.Range(0f, 180f), 0.0, this.dunesNoise);
		}

		// Token: 0x0600798D RID: 31117 RVA: 0x002B3B58 File Offset: 0x002B1D58
		public override void GeneratePostTerrain(Map map)
		{
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.dunesNoise.GetValue(intVec) <= 0.2f && intVec.GetTerrain(map) == TerrainDefOf.Sand)
				{
					map.terrainGrid.SetTerrain(intVec, TerrainDefOf.SoftSand);
				}
			}
		}

		// Token: 0x04004C7D RID: 19581
		private ModuleBase dunesNoise;

		// Token: 0x04004C7E RID: 19582
		private const float VoronoiFrequency = 0.0025f;

		// Token: 0x04004C7F RID: 19583
		private const float VoronoiRandomnessX = 0.15f;

		// Token: 0x04004C80 RID: 19584
		private const float VoronoiRandomnessZ = 0.5f;

		// Token: 0x04004C81 RID: 19585
		private const float NoiseFrequency = 0.015f;

		// Token: 0x04004C82 RID: 19586
		private const float NoiseStrength = 20f;

		// Token: 0x04004C83 RID: 19587
		private const int NoiseOctaves = 2;

		// Token: 0x04004C84 RID: 19588
		private const float StretchFactor = 15f;

		// Token: 0x04004C85 RID: 19589
		private const float ScaleFactor = 1f;

		// Token: 0x04004C86 RID: 19590
		private const float Threshold = 0.2f;

		// Token: 0x04004C87 RID: 19591
		private const float DisplaceFrequency = 0.02f;

		// Token: 0x04004C88 RID: 19592
		private const float DisplaceMagnitude = 7.5f;
	}
}
