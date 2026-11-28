using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001333 RID: 4915
	public class TileMutatorWorker_LakeWithIslands : TileMutatorWorker_Lake
	{
		// Token: 0x170014C5 RID: 5317
		// (get) Token: 0x060079CB RID: 31179 RVA: 0x002B5875 File Offset: 0x002B3A75
		protected override float LakeRadius
		{
			get
			{
				return 0.75f;
			}
		}

		// Token: 0x170014C6 RID: 5318
		// (get) Token: 0x060079CC RID: 31180 RVA: 0x002B587C File Offset: 0x002B3A7C
		protected override bool GenerateDeepWater
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060079CD RID: 31181 RVA: 0x002B587F File Offset: 0x002B3A7F
		public TileMutatorWorker_LakeWithIslands(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079CE RID: 31182 RVA: 0x002B5888 File Offset: 0x002B3A88
		public override void Init(Map map)
		{
			base.Init(map);
			ModuleBase moduleBase = new Perlin(0.019999999552965164, 2.0, 0.5, 2, Rand.Int, QualityMode.Medium);
			moduleBase = new ScaleBias(0.6000000238418579, 0.699999988079071, moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.015f, 15f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "island base");
			ModuleBase moduleBase2 = new ScaleBias(2.0, -1.0, this.lakeNoise);
			moduleBase2 = new Clamp(0.0, 1.0, moduleBase2);
			moduleBase2 = new Power(moduleBase2, new Const(0.44999998807907104));
			NoiseDebugUI.StoreNoiseRender(moduleBase2, "island falloff");
			moduleBase = new Multiply(moduleBase, moduleBase2);
			moduleBase = new ScaleBias(-1.0, 1.0, moduleBase);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "island Noise");
			this.lakeNoise = new Min(this.lakeNoise, moduleBase);
			NoiseDebugUI.StoreNoiseRender(this.lakeNoise, "lake + island Noise");
		}

		// Token: 0x04004CD3 RID: 19667
		private const float IslandsPerlinFreq = 0.02f;

		// Token: 0x04004CD4 RID: 19668
		private const float IslandsPerlinLac = 2f;

		// Token: 0x04004CD5 RID: 19669
		private const float IslandsPerlinPers = 0.5f;

		// Token: 0x04004CD6 RID: 19670
		private const int IslandsPerlinOctaves = 2;

		// Token: 0x04004CD7 RID: 19671
		private const float IslandsScale = 0.6f;

		// Token: 0x04004CD8 RID: 19672
		private const float IslandsBias = 0.7f;

		// Token: 0x04004CD9 RID: 19673
		private const float FalloffScale = 2f;

		// Token: 0x04004CDA RID: 19674
		private const float FalloffBias = -1f;

		// Token: 0x04004CDB RID: 19675
		private const float FalloffExp = 0.45f;
	}
}
