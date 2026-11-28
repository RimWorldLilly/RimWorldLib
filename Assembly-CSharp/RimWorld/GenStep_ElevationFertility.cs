using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200129F RID: 4767
	public class GenStep_ElevationFertility : GenStep
	{
		// Token: 0x1700140A RID: 5130
		// (get) Token: 0x0600763F RID: 30271 RVA: 0x00299BAE File Offset: 0x00297DAE
		public override int SeedPart
		{
			get
			{
				return 826504671;
			}
		}

		// Token: 0x06007640 RID: 30272 RVA: 0x00299BB8 File Offset: 0x00297DB8
		public override void Generate(Map map, GenStepParams parms)
		{
			NoiseRenderer.renderSize = new IntVec2(map.Size.x, map.Size.z);
			bool flag = !map.TileInfo.Mutators.Any((TileMutatorDef m) => m.preventNaturalElevation);
			if (map.generatorDef.isUnderground)
			{
				MapGenFloatGrid elevation = MapGenerator.Elevation;
				using (IEnumerator<IntVec3> enumerator = map.AllCells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 c = enumerator.Current;
						elevation[c] = 1f;
					}
					goto IL_30A;
				}
			}
			if (flag)
			{
				ModuleBase moduleBase = new Perlin((double)GenStep_ElevationFertility.ElevationFreqRange.RandomInRange, 2.0, 0.5, 3, Rand.Range(0, int.MaxValue), QualityMode.High);
				NoiseDebugUI.StoreNoiseRender(moduleBase, "elev shape");
				moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, GenStep_ElevationFertility.DetailFreqRange.RandomInRange, GenStep_ElevationFertility.DetailStrengthRange.RandomInRange, GenStep_ElevationFertility.DetailOctavesRange.RandomInRange, -1);
				NoiseDebugUI.StoreNoiseRender(moduleBase, "elev shape + detail");
				moduleBase = new Scale((double)GenStep_ElevationFertility.PreTurbulenceStretchRange.RandomInRange, 1.0, 1.0, moduleBase);
				moduleBase = new Rotate(0.0, (double)Rand.Range(0f, 180f), 0.0, moduleBase);
				moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, GenStep_ElevationFertility.WarpFreqRange.RandomInRange, GenStep_ElevationFertility.WarpStrengthRange.RandomInRange, GenStep_ElevationFertility.WarpOctavesRange.RandomInRange, -1);
				moduleBase = new Scale((double)GenStep_ElevationFertility.PostTurbulenceStretchRange.RandomInRange, 1.0, 1.0, moduleBase);
				moduleBase = new Rotate(0.0, (double)Rand.Range(0f, 180f), 0.0, moduleBase);
				NoiseDebugUI.StoreNoiseRender(moduleBase, "elev shape + detail + warp");
				moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
				float num = 1f;
				switch (map.TileInfo.HillinessForElevationGen)
				{
				case Hilliness.Flat:
					num = MapGenTuning.ElevationFactorFlat;
					break;
				case Hilliness.SmallHills:
					num = MapGenTuning.ElevationFactorSmallHills;
					break;
				case Hilliness.LargeHills:
					num = MapGenTuning.ElevationFactorLargeHills;
					break;
				case Hilliness.Mountainous:
					num = MapGenTuning.ElevationFactorMountains;
					break;
				case Hilliness.Impassable:
					num = MapGenTuning.ElevationFactorImpassableMountains;
					break;
				}
				moduleBase = new Multiply(moduleBase, new Const((double)num));
				NoiseDebugUI.StoreNoiseRender(moduleBase, "elev world-factored");
				float b = map.TileInfo.WaterCovered ? 0f : float.MaxValue;
				MapGenFloatGrid elevation2 = MapGenerator.Elevation;
				foreach (IntVec3 intVec in map.AllCells)
				{
					elevation2[intVec] = Mathf.Min(moduleBase.GetValue(intVec), b);
				}
			}
			IL_30A:
			ModuleBase moduleBase2 = new Perlin(0.020999999716877937, 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			moduleBase2 = new ScaleBias(0.5, 0.5, moduleBase2);
			NoiseDebugUI.StoreNoiseRender(moduleBase2, "noiseFert base");
			MapGenFloatGrid fertility = MapGenerator.Fertility;
			foreach (IntVec3 intVec2 in map.AllCells)
			{
				fertility[intVec2] = moduleBase2.GetValue(intVec2);
			}
		}

		// Token: 0x04004A18 RID: 18968
		public static readonly FloatRange ElevationFreqRange = new FloatRange(0.015f, 0.0225f);

		// Token: 0x04004A19 RID: 18969
		public const int ElevationOctaves = 3;

		// Token: 0x04004A1A RID: 18970
		public static readonly FloatRange DetailFreqRange = new FloatRange(0.03f, 0.06f);

		// Token: 0x04004A1B RID: 18971
		public static readonly FloatRange DetailStrengthRange = new FloatRange(5f, 10f);

		// Token: 0x04004A1C RID: 18972
		public static readonly IntRange DetailOctavesRange = new IntRange(4, 5);

		// Token: 0x04004A1D RID: 18973
		private static readonly FloatRange WarpFreqRange = new FloatRange(0.01f, 0.02f);

		// Token: 0x04004A1E RID: 18974
		private static readonly FloatRange WarpStrengthRange = new FloatRange(0f, 15f);

		// Token: 0x04004A1F RID: 18975
		private static readonly IntRange WarpOctavesRange = new IntRange(3, 4);

		// Token: 0x04004A20 RID: 18976
		private static readonly FloatRange PreTurbulenceStretchRange = new FloatRange(1f, 1.15f);

		// Token: 0x04004A21 RID: 18977
		private static readonly FloatRange PostTurbulenceStretchRange = new FloatRange(1f, 1.15f);

		// Token: 0x04004A22 RID: 18978
		private const float FertilityFreq = 0.021f;
	}
}
