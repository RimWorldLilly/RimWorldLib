using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200132C RID: 4908
	public class TileMutatorWorker_IceDunes : TileMutatorWorker
	{
		// Token: 0x060079AC RID: 31148 RVA: 0x002B4F1B File Offset: 0x002B311B
		public TileMutatorWorker_IceDunes(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079AD RID: 31149 RVA: 0x002B4F24 File Offset: 0x002B3124
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.iceDunesNoise = new Voronoi2D(0.007000000216066837, Rand.Int, 0.5f, true);
			this.iceDunesNoise = MapNoiseUtility.AddDisplacementNoise(this.iceDunesNoise, 0.03f, 10f, 3, -1);
			this.iceDunesNoise = new Scale(3.0, 1.0, 1.0, this.iceDunesNoise);
			ModuleBase moduleBase = new Perlin(0.019999999552965164, 2.0, 0.5, 2, Rand.Int, QualityMode.Medium);
			moduleBase = new Multiply(moduleBase, new Const(6.5));
			this.iceDunesNoise = new Displace(this.iceDunesNoise, moduleBase, new Const(0.0), new Const(0.0));
			float num = Rand.Range(0f, 180f);
			this.iceDunesNoise = new Rotate(0.0, (double)num, 0.0, this.iceDunesNoise);
		}

		// Token: 0x060079AE RID: 31150 RVA: 0x002B5044 File Offset: 0x002B3244
		public override void GeneratePostTerrain(Map map)
		{
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.iceDunesNoise.GetValue(intVec) <= 0.2f && !intVec.GetTerrain(map).IsWater)
				{
					GenSpawn.Spawn(ThingDefOf.SolidIce, intVec, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x04004CB9 RID: 19641
		private ModuleBase iceDunesNoise;

		// Token: 0x04004CBA RID: 19642
		private const float VoronoiFrequency = 0.007f;

		// Token: 0x04004CBB RID: 19643
		private const float VoronoiRandomness = 0.5f;

		// Token: 0x04004CBC RID: 19644
		private const float NoiseFrequency = 0.03f;

		// Token: 0x04004CBD RID: 19645
		private const float NoiseStrength = 10f;

		// Token: 0x04004CBE RID: 19646
		private const float ScaleFactor = 3f;

		// Token: 0x04004CBF RID: 19647
		private const float Threshold = 0.2f;

		// Token: 0x04004CC0 RID: 19648
		private const float DisplaceFrequency = 0.02f;

		// Token: 0x04004CC1 RID: 19649
		private const float DisplaceMagnitude = 6.5f;
	}
}
