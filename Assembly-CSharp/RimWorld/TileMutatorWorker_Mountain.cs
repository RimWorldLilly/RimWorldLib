using System;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200133D RID: 4925
	public class TileMutatorWorker_Mountain : TileMutatorWorker
	{
		// Token: 0x060079FC RID: 31228 RVA: 0x002B6AE3 File Offset: 0x002B4CE3
		public TileMutatorWorker_Mountain(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079FD RID: 31229 RVA: 0x002B6AEC File Offset: 0x002B4CEC
		public override void GeneratePostElevationFertility(Map map)
		{
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float num2 = ((num != null) ? num : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake)) ?? ((float)Rand.Range(0, 360));
			ModuleBase moduleBase = new DistFromAxis_Directional((float)map.Size.x * 0.15f);
			moduleBase = new Translate((double)((float)(-(float)map.Size.x) * 0.2f), 0.0, 0.0, moduleBase);
			moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
			moduleBase = new Clamp(0.0, 1.0, moduleBase);
			moduleBase = new Rotate(0.0, (double)num2, 0.0, moduleBase);
			moduleBase = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.003f, 20f, 2, -1);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "mountain");
			float b = map.TileInfo.WaterCovered ? 0f : float.MaxValue;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				elevation[intVec] = Mathf.Min(elevation[intVec] + moduleBase.GetValue(intVec), b);
			}
		}

		// Token: 0x04004CF6 RID: 19702
		private const float MountainNoiseFrequency = 0.003f;

		// Token: 0x04004CF7 RID: 19703
		private const float MountainNoiseStrength = 20f;

		// Token: 0x04004CF8 RID: 19704
		private const float EdgeMountainSpan = 0.15f;

		// Token: 0x04004CF9 RID: 19705
		private const float EdgeMountainOffset = 0.2f;
	}
}
