using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200133A RID: 4922
	public class TileMutatorWorker_LavaCrater : TileMutatorWorker_LavaLake
	{
		// Token: 0x060079EB RID: 31211 RVA: 0x002B635D File Offset: 0x002B455D
		public TileMutatorWorker_LavaCrater(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079EC RID: 31212 RVA: 0x002B6368 File Offset: 0x002B4568
		public override void Init(Map map)
		{
			base.Init(map);
			ModuleBase lhs = new Perlin(0.014999999664723873, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * this.LakeRadius);
			moduleBase = new Clamp(0.0, 1.0, moduleBase);
			moduleBase = new Invert(moduleBase);
			moduleBase = new ScaleBias(1.0, 1.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.waterInfo.lakeCenter.x), 0.0, (double)(-(double)map.waterInfo.lakeCenter.z), moduleBase);
			this.craterNoise = new Blend(lhs, moduleBase, new Const(0.7));
			NoiseDebugUI.StoreNoiseRender(this.craterNoise, "crater");
		}

		// Token: 0x060079ED RID: 31213 RVA: 0x002B6458 File Offset: 0x002B4658
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if ((float)this.craterNoise.GetValue((double)intVec.x, 0.0, (double)intVec.z) > 0.15f)
				{
					elevation[intVec] = 1f;
				}
			}
			base.GeneratePostElevationFertility(map);
		}

		// Token: 0x04004CEB RID: 19691
		private const float RockThreshold = 0.15f;

		// Token: 0x04004CEC RID: 19692
		private const float NoiseFrequency = 0.015f;

		// Token: 0x04004CED RID: 19693
		private ModuleBase craterNoise;
	}
}
