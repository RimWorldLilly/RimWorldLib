using System;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200134D RID: 4941
	public class TileMutatorWorker_Valley : TileMutatorWorker
	{
		// Token: 0x06007A57 RID: 31319 RVA: 0x002B9A37 File Offset: 0x002B7C37
		public TileMutatorWorker_Valley(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A58 RID: 31320 RVA: 0x002B9A40 File Offset: 0x002B7C40
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float? num2 = (num != null) ? num : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake);
			float num3 = num2 ?? ((float)Rand.Range(0, 360));
			this.valleyNoise = new DistFromAxis((float)map.Size.x * 0.3f);
			this.valleyNoise = new Power(this.valleyNoise, new Const(0.5));
			this.valleyNoise = new ScaleBias(2.0, -1.0, this.valleyNoise);
			this.valleyNoise = new Rotate(0.0, (double)(num3 + 90f), 0.0, this.valleyNoise);
			this.valleyNoise = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), this.valleyNoise);
			if (num2 != null)
			{
				ModuleBase rhs = MapNoiseUtility.FalloffAtAngle(num2.Value, 0.2f, map);
				this.valleyNoise = new SmoothMin(this.valleyNoise, rhs, 0.1);
			}
			this.valleyNoise = MapNoiseUtility.AddDisplacementNoise(this.valleyNoise, 0.015f, 30f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.valleyNoise, "valley");
		}

		// Token: 0x06007A59 RID: 31321 RVA: 0x002B9BE8 File Offset: 0x002B7DE8
		public override void GeneratePostElevationFertility(Map map)
		{
			float b = map.TileInfo.WaterCovered ? 0f : float.MaxValue;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				elevation[intVec] = Mathf.Min(elevation[intVec] + this.valleyNoise.GetValue(intVec), b);
			}
		}

		// Token: 0x04004D48 RID: 19784
		private ModuleBase valleyNoise;

		// Token: 0x04004D49 RID: 19785
		private const float ValleySpan = 0.3f;

		// Token: 0x04004D4A RID: 19786
		private const float ValleyNoiseFreq = 0.015f;

		// Token: 0x04004D4B RID: 19787
		private const float ValleyNoiseStrength = 30f;

		// Token: 0x04004D4C RID: 19788
		private const float ValleyFalloffExp = 0.5f;

		// Token: 0x04004D4D RID: 19789
		private const float CoastFalloff = 0.2f;
	}
}
