using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001341 RID: 4929
	public class TileMutatorWorker_Peninsula : TileMutatorWorker_Coast
	{
		// Token: 0x06007A08 RID: 31240 RVA: 0x002B6FA8 File Offset: 0x002B51A8
		public TileMutatorWorker_Peninsula(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A09 RID: 31241 RVA: 0x002B6FB4 File Offset: 0x002B51B4
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			float coastAngle = this.GetCoastAngle(map.Tile);
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * 0.6f);
			moduleBase = new ScaleBias(-1.0, 1.0, moduleBase);
			moduleBase = new Multiply(moduleBase, new CutOff(true, false));
			ModuleBase moduleBase2 = new DistFromAxis((float)map.Size.x * 0.6f);
			moduleBase2 = new Rotate(0.0, 90.0, 0.0, moduleBase2);
			moduleBase2 = new ScaleBias(-1.0, 1.0, moduleBase2);
			moduleBase2 = new Multiply(moduleBase2, new CutOff());
			this.coastNoise = new Add(moduleBase, moduleBase2);
			this.coastNoise = new Translate((double)((float)map.Size.x * 0.1f), 0.0, 0.0, this.coastNoise);
			this.coastNoise = new Rotate(0.0, (double)coastAngle, 0.0, this.coastNoise);
			this.coastNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.coastNoise);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "Peninsula shape", map.Size.ToIntVec2);
			this.coastNoise = MapNoiseUtility.AddDisplacementNoise(this.coastNoise, 0.006f, 30f, 2, -1);
			this.coastNoise = MapNoiseUtility.AddDisplacementNoise(this.coastNoise, 0.015f, 25f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "Peninsula", map.Size.ToIntVec2);
		}

		// Token: 0x04004D01 RID: 19713
		private const float PeninsulaRadius = 0.6f;

		// Token: 0x04004D02 RID: 19714
		private const float PeninsulaOffset = 0.1f;
	}
}
