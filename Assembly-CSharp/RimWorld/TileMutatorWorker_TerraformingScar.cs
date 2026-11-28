using System;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200134B RID: 4939
	public class TileMutatorWorker_TerraformingScar : TileMutatorWorker
	{
		// Token: 0x06007A50 RID: 31312 RVA: 0x002B96C5 File Offset: 0x002B78C5
		public TileMutatorWorker_TerraformingScar(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A51 RID: 31313 RVA: 0x002B96D0 File Offset: 0x002B78D0
		public override void GeneratePostElevationFertility(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			ModuleBase moduleBase = new Perlin((double)GenStep_ElevationFertility.ElevationFreqRange.RandomInRange, 2.0, 0.5, 3, Rand.Range(0, int.MaxValue), QualityMode.High);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "elev shape");
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, GenStep_ElevationFertility.DetailFreqRange.RandomInRange, GenStep_ElevationFertility.DetailStrengthRange.RandomInRange, GenStep_ElevationFertility.DetailOctavesRange.RandomInRange, -1);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "elev shape + detail");
			moduleBase = new Scale(2.0, 1.0, 1.0, moduleBase);
			moduleBase = new Rotate(0.0, (double)Rand.Range(0f, 180f), 0.0, moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.005f, 75f, 4, -1);
			moduleBase = new Scale(1.0, 1.0, 1.0, moduleBase);
			moduleBase = new Rotate(0.0, (double)Rand.Range(0f, 180f), 0.0, moduleBase);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "elev shape + detail + warp");
			moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
			float elevationFactorMountains = MapGenTuning.ElevationFactorMountains;
			moduleBase = new Multiply(moduleBase, new Const((double)elevationFactorMountains));
			NoiseDebugUI.StoreNoiseRender(moduleBase, "elev world-factored");
			float b = map.TileInfo.WaterCovered ? 0f : float.MaxValue;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				elevation[intVec] = Mathf.Min(moduleBase.GetValue(intVec), b);
			}
		}

		// Token: 0x04004D3F RID: 19775
		private const float WarpFreq = 0.005f;

		// Token: 0x04004D40 RID: 19776
		private const float WarpStrength = 75f;

		// Token: 0x04004D41 RID: 19777
		private const int WarpOctaves = 4;

		// Token: 0x04004D42 RID: 19778
		private const float PreTurbulenceStretch = 2f;

		// Token: 0x04004D43 RID: 19779
		private const float PostTurbulenceStretch = 1f;
	}
}
