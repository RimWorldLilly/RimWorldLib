using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001318 RID: 4888
	public class TileMutatorWorker_Archipelago : TileMutatorWorker_Coast
	{
		// Token: 0x170014AC RID: 5292
		// (get) Token: 0x06007941 RID: 31041 RVA: 0x002B1674 File Offset: 0x002AF874
		protected override float MaxForDeepWater
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x170014AD RID: 5293
		// (get) Token: 0x06007942 RID: 31042 RVA: 0x002B167B File Offset: 0x002AF87B
		protected override FloatRange CoastOffset
		{
			get
			{
				return new FloatRange(0.2f);
			}
		}

		// Token: 0x06007943 RID: 31043 RVA: 0x002B1687 File Offset: 0x002AF887
		public TileMutatorWorker_Archipelago(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007944 RID: 31044 RVA: 0x002B1690 File Offset: 0x002AF890
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			this.archNoise = new Perlin(0.019999999552965164, 2.0, 0.5, 2, Rand.Int, QualityMode.Medium);
			this.archNoise = new ScaleBias(0.5, 0.5, this.archNoise);
			this.archNoise = MapNoiseUtility.AddDisplacementNoise(this.archNoise, 0.015f, 25f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.archNoise, "archipelago");
			this.coastNoise = new SmoothMin(this.coastNoise, this.archNoise, 0.2);
		}

		// Token: 0x06007945 RID: 31045 RVA: 0x002B174C File Offset: 0x002AF94C
		public override void GeneratePostElevationFertility(Map map)
		{
			base.GeneratePostElevationFertility(map);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.archNoise.GetValue(intVec) < this.MaxForSand)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x04004C14 RID: 19476
		private const float ArchNoiseFrequency = 0.02f;

		// Token: 0x04004C15 RID: 19477
		private ModuleBase archNoise;
	}
}
