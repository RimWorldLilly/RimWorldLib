using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001332 RID: 4914
	public class TileMutatorWorker_LakeWithIsland : TileMutatorWorker_Lake
	{
		// Token: 0x170014C3 RID: 5315
		// (get) Token: 0x060079C7 RID: 31175 RVA: 0x002B578C File Offset: 0x002B398C
		protected override float LakeRadius
		{
			get
			{
				return 0.8f;
			}
		}

		// Token: 0x170014C4 RID: 5316
		// (get) Token: 0x060079C8 RID: 31176 RVA: 0x002B5793 File Offset: 0x002B3993
		protected override bool GenerateDeepWater
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060079C9 RID: 31177 RVA: 0x002B5796 File Offset: 0x002B3996
		public TileMutatorWorker_LakeWithIsland(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079CA RID: 31178 RVA: 0x002B57A0 File Offset: 0x002B39A0
		public override void Init(Map map)
		{
			base.Init(map);
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * 0.4f);
			moduleBase = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.waterInfo.lakeCenter.x), 0.0, (double)(-(double)map.waterInfo.lakeCenter.z), moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.006f, 40f, 2, map.Tile.tileId);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.015f, 15f, 4, map.Tile.tileId);
			this.lakeNoise = new Min(this.lakeNoise, moduleBase);
		}

		// Token: 0x04004CCE RID: 19662
		private const float LakeIslandNoiseFrequency = 0.015f;

		// Token: 0x04004CCF RID: 19663
		private const float LakeIslandNoiseStrength = 15f;

		// Token: 0x04004CD0 RID: 19664
		private const float LakeIslandMacroNoiseFrequency = 0.006f;

		// Token: 0x04004CD1 RID: 19665
		private const float LakeIslandMacroNoiseStrength = 40f;

		// Token: 0x04004CD2 RID: 19666
		private const float IslandRadius = 0.4f;
	}
}
