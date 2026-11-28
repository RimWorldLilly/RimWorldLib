using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001321 RID: 4897
	public class TileMutatorWorker_CoastalAtoll : TileMutatorWorker_Coast
	{
		// Token: 0x170014BA RID: 5306
		// (get) Token: 0x0600797B RID: 31099 RVA: 0x002B2FCF File Offset: 0x002B11CF
		protected override FloatRange CoastOffset
		{
			get
			{
				return new FloatRange(0.6f);
			}
		}

		// Token: 0x170014BB RID: 5307
		// (get) Token: 0x0600797C RID: 31100 RVA: 0x002B2FDB File Offset: 0x002B11DB
		protected override float MaxForSand
		{
			get
			{
				return 0.53f;
			}
		}

		// Token: 0x0600797D RID: 31101 RVA: 0x002B2FE2 File Offset: 0x002B11E2
		public TileMutatorWorker_CoastalAtoll(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600797E RID: 31102 RVA: 0x002B2FEC File Offset: 0x002B11EC
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * 0.95f);
			moduleBase = new Clamp((double)this.MaxForDeepWater, 1.0, moduleBase);
			moduleBase = new Scale(1.0, 1.0, 0.800000011920929, moduleBase);
			moduleBase = new Rotate(0.0, (double)this.GetCoastAngle(map.Tile), 0.0, moduleBase);
			moduleBase = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.015f, 25f, 4, -1);
			ModuleBase moduleBase2 = new DistFromPoint((float)map.Size.x * 0.65f);
			moduleBase2 = new ScaleBias(-1.0, 1.0, moduleBase2);
			moduleBase2 = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), moduleBase2);
			moduleBase2 = MapNoiseUtility.AddDisplacementNoise(moduleBase2, 0.003f, 20f, 4, map.Tile.tileId);
			moduleBase2 = MapNoiseUtility.AddDisplacementNoise(moduleBase2, 0.015f, 5f, 6, map.Tile.tileId);
			ModuleBase moduleBase3 = new DistFromPoint((float)map.Size.x * 0.5f);
			moduleBase3 = new Clamp((double)this.MaxForDeepWater, 1.0, moduleBase3);
			moduleBase3 = new Translate((double)((float)(-(float)map.Size.x) * (0.5f + Rand.Range(-0.08f, 0.08f))), 0.0, (double)((float)(-(float)map.Size.z) * (0.5f + Rand.Range(-0.08f, 0.08f))), moduleBase3);
			moduleBase3 = MapNoiseUtility.AddDisplacementNoise(moduleBase3, 0.003f, 20f, 4, map.Tile.tileId);
			moduleBase3 = MapNoiseUtility.AddDisplacementNoise(moduleBase3, 0.015f, 5f, 6, map.Tile.tileId);
			this.coastNoise = new SmoothMin(this.coastNoise, moduleBase, 0.5);
			this.coastNoise = new Max(this.coastNoise, moduleBase2);
			this.coastNoise = new Min(this.coastNoise, moduleBase3);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "atoll & coast");
		}

		// Token: 0x0600797F RID: 31103 RVA: 0x002B3280 File Offset: 0x002B1480
		public override void GeneratePostElevationFertility(Map map)
		{
			base.GeneratePostElevationFertility(map);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.coastNoise.GetValue(intVec) < this.MaxForSand)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x04004C5D RID: 19549
		private ModuleBase innerWaterNoise;

		// Token: 0x04004C5E RID: 19550
		private const float WaterRadius = 0.95f;

		// Token: 0x04004C5F RID: 19551
		private const float WaterSquash = 0.8f;

		// Token: 0x04004C60 RID: 19552
		private const float IslandRadius = 0.65f;

		// Token: 0x04004C61 RID: 19553
		private const float IslandInnerRadius = 0.5f;

		// Token: 0x04004C62 RID: 19554
		private const float IslandMacroNoiseFrequency = 0.003f;

		// Token: 0x04004C63 RID: 19555
		private const float IslandMacroNoiseStrength = 20f;

		// Token: 0x04004C64 RID: 19556
		private const float IslandNoiseFrequency = 0.015f;

		// Token: 0x04004C65 RID: 19557
		private const float IslandNoiseStrength = 5f;

		// Token: 0x04004C66 RID: 19558
		private const float IslandMaxInnerOffset = 0.08f;
	}
}
