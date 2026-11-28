using System;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001343 RID: 4931
	public class TileMutatorWorker_Plateau : TileMutatorWorker
	{
		// Token: 0x06007A11 RID: 31249 RVA: 0x002B72F4 File Offset: 0x002B54F4
		public TileMutatorWorker_Plateau(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A12 RID: 31250 RVA: 0x002B7300 File Offset: 0x002B5500
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Vector2 offset = map.Center.ToVector2() + new Vector2((float)map.Size.x * TileMutatorWorker_Plateau.CenterOffsetRange.RandomInRange, (float)map.Size.z * TileMutatorWorker_Plateau.CenterOffsetRange.RandomInRange);
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float? num2 = (num != null) ? num : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake);
			if (num2 != null)
			{
				offset = map.Center.ToVector2() + Vector2Utility.FromAngle(num2.Value) * (float)map.Size.x * TileMutatorWorker_Plateau.CenterOffsetRange.max;
			}
			this.plateauNoise = MapNoiseUtility.CreateFalloffRadius((float)map.Size.x * 0.4f, offset, 0.2f, true);
			this.plateauNoise = MapNoiseUtility.AddDisplacementNoise(this.plateauNoise, 0.015f, 35f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.plateauNoise, "plateau");
		}

		// Token: 0x06007A13 RID: 31251 RVA: 0x002B7438 File Offset: 0x002B5638
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.plateauNoise.GetValue(intVec);
				if (value > 0f)
				{
					elevation[intVec] = value;
				}
			}
		}

		// Token: 0x04004D05 RID: 19717
		private ModuleBase plateauNoise;

		// Token: 0x04004D06 RID: 19718
		private const float PlateauRadius = 0.4f;

		// Token: 0x04004D07 RID: 19719
		private const float PlateauExp = 0.2f;

		// Token: 0x04004D08 RID: 19720
		private const float PlateauNoiseFrequency = 0.015f;

		// Token: 0x04004D09 RID: 19721
		private const float PlateauNoiseStrength = 35f;

		// Token: 0x04004D0A RID: 19722
		private const float PlateauThreshold = 0f;

		// Token: 0x04004D0B RID: 19723
		private static readonly FloatRange CenterOffsetRange = new FloatRange(-0.2f, 0.2f);
	}
}
