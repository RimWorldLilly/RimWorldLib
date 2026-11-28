using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200131A RID: 4890
	public class TileMutatorWorker_Bay : TileMutatorWorker_Coast
	{
		// Token: 0x170014AF RID: 5295
		// (get) Token: 0x0600794C RID: 31052 RVA: 0x002B1B3E File Offset: 0x002AFD3E
		protected override FloatRange CoastOffset
		{
			get
			{
				return new FloatRange(0.2f, 0.3f);
			}
		}

		// Token: 0x0600794D RID: 31053 RVA: 0x002B1B4F File Offset: 0x002AFD4F
		public TileMutatorWorker_Bay(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600794E RID: 31054 RVA: 0x002B1B58 File Offset: 0x002AFD58
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float coastAngle = this.GetCoastAngle(map.Tile);
			this.bayNoise = new DistFromPoint((float)map.Size.x * 0.6f);
			this.bayNoise = new Clamp((double)this.MaxForDeepWater, 1.0, this.bayNoise);
			this.bayNoise = new Scale(1.0, 1.0, 0.75, this.bayNoise);
			this.bayNoise = new Translate(0.0, 0.0, (double)((float)(-(float)map.Size.x) * 0.1f), this.bayNoise);
			this.bayNoise = new Rotate(0.0, (double)(coastAngle + 90f), 0.0, this.bayNoise);
			this.bayNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.bayNoise);
			NoiseDebugUI.StoreNoiseRender(this.bayNoise, "bay shape");
			this.bayNoise = MapNoiseUtility.AddDisplacementNoise(this.bayNoise, 0.003f, 70f, 2, -1);
			this.bayNoise = MapNoiseUtility.AddDisplacementNoise(this.bayNoise, 0.015f, 25f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.bayNoise, "bay noise");
			this.coastNoise = new SmoothMin(this.bayNoise, this.coastNoise, 0.5);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "bay & coast");
		}

		// Token: 0x0600794F RID: 31055 RVA: 0x002B1D0C File Offset: 0x002AFF0C
		public override void GeneratePostElevationFertility(Map map)
		{
			base.GeneratePostElevationFertility(map);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.bayNoise.GetValue(intVec) < 0.4f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x04004C21 RID: 19489
		private const float BaySpan = 0.6f;

		// Token: 0x04004C22 RID: 19490
		private const float BayOffset = 0.1f;

		// Token: 0x04004C23 RID: 19491
		private const float BaySquash = 0.75f;

		// Token: 0x04004C24 RID: 19492
		private const float BayMacroNoiseFrequency = 0.003f;

		// Token: 0x04004C25 RID: 19493
		private const float BayMacroNoiseStrength = 70f;

		// Token: 0x04004C26 RID: 19494
		private const float ElevationThreshold = 0.4f;

		// Token: 0x04004C27 RID: 19495
		private ModuleBase bayNoise;
	}
}
