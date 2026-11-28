using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001322 RID: 4898
	public class TileMutatorWorker_CoastalIsland : TileMutatorWorker_Coast
	{
		// Token: 0x170014BC RID: 5308
		// (get) Token: 0x06007980 RID: 31104 RVA: 0x002B32F4 File Offset: 0x002B14F4
		protected override FloatRange CoastOffset
		{
			get
			{
				return new FloatRange(0.6f);
			}
		}

		// Token: 0x06007981 RID: 31105 RVA: 0x002B3300 File Offset: 0x002B1500
		public TileMutatorWorker_CoastalIsland(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007982 RID: 31106 RVA: 0x002B330C File Offset: 0x002B150C
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float randomInRange = TileMutatorWorker_CoastalIsland.IslandSquashRange.RandomInRange;
			float num = Rand.Range(0f, 360f);
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * 0.95f);
			moduleBase = new Clamp((double)this.MaxForDeepWater, 1.0, moduleBase);
			moduleBase = new Scale((double)randomInRange, 1.0, 1.0, moduleBase);
			moduleBase = new Rotate(0.0, (double)num, 0.0, moduleBase);
			moduleBase = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.015f, 35f, 4, map.Tile.tileId);
			ModuleBase moduleBase2 = new DistFromPoint((float)map.Size.x * 0.65f);
			moduleBase2 = new ScaleBias(-1.0, 1.0, moduleBase2);
			moduleBase2 = new Scale((double)randomInRange, 1.0, 1.0, moduleBase2);
			moduleBase2 = new Rotate(0.0, (double)num, 0.0, moduleBase2);
			moduleBase2 = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), moduleBase2);
			moduleBase2 = MapNoiseUtility.AddDisplacementNoise(moduleBase2, 0.015f, 35f, 4, map.Tile.tileId);
			this.coastNoise = new SmoothMin(this.coastNoise, moduleBase, 0.25);
			this.coastNoise = new Max(this.coastNoise, moduleBase2);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "island & coast");
		}

		// Token: 0x04004C67 RID: 19559
		private const float IslandRadius = 0.65f;

		// Token: 0x04004C68 RID: 19560
		private const float WaterRadius = 0.95f;

		// Token: 0x04004C69 RID: 19561
		private const float IslandNoiseFrequency = 0.015f;

		// Token: 0x04004C6A RID: 19562
		private const float IslandNoiseStrength = 35f;

		// Token: 0x04004C6B RID: 19563
		private const float WaterSmoothMinStrength = 0.25f;

		// Token: 0x04004C6C RID: 19564
		private static readonly FloatRange IslandSquashRange = new FloatRange(0.65f, 1f);
	}
}
