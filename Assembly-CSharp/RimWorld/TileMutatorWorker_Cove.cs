using System;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001323 RID: 4899
	public class TileMutatorWorker_Cove : TileMutatorWorker_Coast
	{
		// Token: 0x06007984 RID: 31108 RVA: 0x002B350D File Offset: 0x002B170D
		public TileMutatorWorker_Cove(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007985 RID: 31109 RVA: 0x002B3518 File Offset: 0x002B1718
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float coastAngle = this.GetCoastAngle(map.Tile);
			this.coveNoise = MapNoiseUtility.CreateFalloffRadius((float)map.Size.x * 0.5f, Vector2.zero, 1f, false);
			this.coveNoise = new Translate(0.0, 0.0, (double)((float)(-(float)map.Size.x) * -0.2f), this.coveNoise);
			this.coveNoise = new Rotate(0.0, (double)(coastAngle + 90f), 0.0, this.coveNoise);
			this.coveNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.coveNoise);
			float num = Rand.Range(-0.1f, 0.1f);
			ModuleBase moduleBase = new DistFromCone(5f, (float)map.Size.x * 0.1f);
			moduleBase = new Translate((double)((float)(-(float)map.Size.x) * num), 0.0, 0.0, moduleBase);
			moduleBase = new Rotate(0.0, (double)(coastAngle + 90f), 0.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
			moduleBase = new ScaleBias(0.25, 0.0, moduleBase);
			this.coveNoise = new SmoothMin(this.coveNoise, moduleBase, 0.2);
			NoiseDebugUI.StoreNoiseRender(this.coveNoise, "cove shape");
			this.coveNoise = MapNoiseUtility.AddDisplacementNoise(this.coveNoise, 0.003f, 30f, 2, -1);
			this.coveNoise = MapNoiseUtility.AddDisplacementNoise(this.coveNoise, 0.015f, 25f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.coveNoise, "cove noise");
			this.coveNoise = new Max(this.coveNoise, new Const((double)this.MaxForDeepWater));
			this.coastNoise = new SmoothMin(this.coveNoise, this.coastNoise, 0.5);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "cove & coast");
		}

		// Token: 0x06007986 RID: 31110 RVA: 0x002B377C File Offset: 0x002B197C
		public override void GeneratePostElevationFertility(Map map)
		{
			base.GeneratePostElevationFertility(map);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.coveNoise.GetValue(intVec) < 0.4f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x04004C6D RID: 19565
		private ModuleBase coveNoise;

		// Token: 0x04004C6E RID: 19566
		private const float CoveRadius = 0.5f;

		// Token: 0x04004C6F RID: 19567
		private const float CoveOffset = -0.2f;

		// Token: 0x04004C70 RID: 19568
		private const float CoveMacroNoiseFrequency = 0.003f;

		// Token: 0x04004C71 RID: 19569
		private const float CoveMacroNoiseStrength = 30f;

		// Token: 0x04004C72 RID: 19570
		private const float EntranceGradient = 5f;

		// Token: 0x04004C73 RID: 19571
		private const float EntranceSpan = 0.1f;

		// Token: 0x04004C74 RID: 19572
		private const float EntranceScale = 0.25f;

		// Token: 0x04004C75 RID: 19573
		private const float EntranceBias = 0f;

		// Token: 0x04004C76 RID: 19574
		private const float EntranceXOffset = 0.1f;

		// Token: 0x04004C77 RID: 19575
		private const float ElevationThreshold = 0.4f;
	}
}
