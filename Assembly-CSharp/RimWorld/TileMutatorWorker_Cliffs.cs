using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200131F RID: 4895
	public class TileMutatorWorker_Cliffs : TileMutatorWorker
	{
		// Token: 0x0600796A RID: 31082 RVA: 0x002B2B76 File Offset: 0x002B0D76
		public TileMutatorWorker_Cliffs(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600796B RID: 31083 RVA: 0x002B2B80 File Offset: 0x002B0D80
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float num2 = num ?? ((float)Rand.Range(0, 360));
			this.cliffNoise = new DistFromPoint((float)map.Size.x * 0.5f);
			this.cliffNoise = new Scale(1.0, 1.0, 0.5, this.cliffNoise);
			this.cliffNoise = new Translate(0.0, 0.0, (double)((float)(-(float)map.Size.x) * 0.5f), this.cliffNoise);
			this.cliffNoise = new Rotate(0.0, (double)(num2 + 90f), 0.0, this.cliffNoise);
			this.cliffNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.cliffNoise);
			if (num != null)
			{
				ModuleBase rhs = MapNoiseUtility.FalloffAtAngle(num.Value, 0.1f, map);
				this.cliffNoise = new SmoothMin(this.cliffNoise, rhs, 0.1);
			}
			this.cliffNoise = MapNoiseUtility.AddDisplacementNoise(this.cliffNoise, 0.015f, 40f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.cliffNoise, "cliff");
		}

		// Token: 0x0600796C RID: 31084 RVA: 0x002B2D18 File Offset: 0x002B0F18
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.cliffNoise.GetValue(intVec);
				if (value > 0.75f)
				{
					elevation[intVec] = value;
				}
			}
		}

		// Token: 0x04004C50 RID: 19536
		private ModuleBase cliffNoise;

		// Token: 0x04004C51 RID: 19537
		private const float CliffSpan = 0.5f;

		// Token: 0x04004C52 RID: 19538
		private const float CliffOffset = 0.5f;

		// Token: 0x04004C53 RID: 19539
		private const float CliffSquash = 0.5f;

		// Token: 0x04004C54 RID: 19540
		private const float CliffNoiseFreq = 0.015f;

		// Token: 0x04004C55 RID: 19541
		private const float CliffNoiseStrength = 40f;

		// Token: 0x04004C56 RID: 19542
		private const float CliffThreshold = 0.75f;

		// Token: 0x04004C57 RID: 19543
		private const float CoastFalloff = 0.1f;
	}
}
