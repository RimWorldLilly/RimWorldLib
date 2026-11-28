using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200132A RID: 4906
	public class TileMutatorWorker_Iceberg : TileMutatorWorker_Coast
	{
		// Token: 0x170014BD RID: 5309
		// (get) Token: 0x060079A3 RID: 31139 RVA: 0x002B4B24 File Offset: 0x002B2D24
		protected override FloatRange CoastOffset
		{
			get
			{
				return new FloatRange(0.6f);
			}
		}

		// Token: 0x060079A4 RID: 31140 RVA: 0x002B4B30 File Offset: 0x002B2D30
		public TileMutatorWorker_Iceberg(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079A5 RID: 31141 RVA: 0x002B4B3C File Offset: 0x002B2D3C
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * 0.95f);
			moduleBase = new Clamp((double)this.MaxForDeepWater, 1.0, moduleBase);
			moduleBase = new Scale(1.0, 1.0, 0.75, moduleBase);
			moduleBase = new Rotate(0.0, (double)this.GetCoastAngle(map.Tile), 0.0, moduleBase);
			moduleBase = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.015f, 25f, 4, -1);
			this.coastNoise = new SmoothMin(this.coastNoise, moduleBase, 0.25);
			NoiseDebugUI.StoreNoiseRender(this.coastNoise, "coast with space for iceberg");
			this.icebergNoise = new DistFromPoint((float)map.Size.x * 0.25f);
			this.icebergNoise = new ScaleBias(-1.0, 1.0, this.icebergNoise);
			this.icebergNoise = new Scale((double)TileMutatorWorker_Iceberg.BergSquashRange.RandomInRange, 1.0, 1.0, this.icebergNoise);
			this.icebergNoise = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, this.icebergNoise);
			this.icebergNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.icebergNoise);
			this.icebergNoise = MapNoiseUtility.AddDisplacementNoise(this.icebergNoise, 0.015f, 20f, 4, -1);
			this.icebergNoise = new Clamp(0.0, 1.0, this.icebergNoise);
			this.icebergNoise = new Power(this.icebergNoise, new Const(0.4000000059604645));
			NoiseDebugUI.StoreNoiseRender(this.icebergNoise, "iceberg");
		}

		// Token: 0x060079A6 RID: 31142 RVA: 0x002B4D8C File Offset: 0x002B2F8C
		public override void GeneratePostTerrain(Map map)
		{
			base.GeneratePostTerrain(map);
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.icebergNoise.GetValue(intVec);
				if (value > 0.35f)
				{
					map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Ice);
				}
				else if (value > 0f)
				{
					map.terrainGrid.SetTerrain(intVec, MapGenUtility.ShallowOceanWaterTerrainAt(intVec, map));
				}
				if (value > 0.55f)
				{
					GenSpawn.Spawn(ThingDefOf.SolidIce, intVec, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x04004CAD RID: 19629
		private ModuleBase icebergNoise;

		// Token: 0x04004CAE RID: 19630
		private const float Radius = 0.25f;

		// Token: 0x04004CAF RID: 19631
		private const float BergNoiseFrequency = 0.015f;

		// Token: 0x04004CB0 RID: 19632
		private const float BergNoiseStrength = 20f;

		// Token: 0x04004CB1 RID: 19633
		private const float Exponent = 0.4f;

		// Token: 0x04004CB2 RID: 19634
		private const float BergDeepWaterThreshold = 0f;

		// Token: 0x04004CB3 RID: 19635
		private const float BergThreshold = 0.35f;

		// Token: 0x04004CB4 RID: 19636
		private const float BergWallThreshold = 0.55f;

		// Token: 0x04004CB5 RID: 19637
		private const float WaterSmoothMinStrength = 0.25f;

		// Token: 0x04004CB6 RID: 19638
		private const float WaterRadius = 0.95f;

		// Token: 0x04004CB7 RID: 19639
		private const float WaterSquash = 0.75f;

		// Token: 0x04004CB8 RID: 19640
		private static readonly FloatRange BergSquashRange = new FloatRange(0.65f, 0.9f);
	}
}
