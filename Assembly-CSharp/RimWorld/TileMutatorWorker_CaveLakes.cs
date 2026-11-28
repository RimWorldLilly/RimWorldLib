using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200131B RID: 4891
	public class TileMutatorWorker_CaveLakes : TileMutatorWorker_Caves
	{
		// Token: 0x06007950 RID: 31056 RVA: 0x002B1D80 File Offset: 0x002AFF80
		public TileMutatorWorker_CaveLakes(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007951 RID: 31057 RVA: 0x002B1D8C File Offset: 0x002AFF8C
		public override void GeneratePostTerrain(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			MapGenFloatGrid caves = MapGenerator.Caves;
			base.GeneratePostTerrain(map);
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (caves[intVec] > 0f && !intVec.GetTerrain(map).IsWater && Rand.Chance(0.005f))
				{
					ModuleBase moduleBase = new DistFromPoint(TileMutatorWorker_CaveLakes.LakeRadius.RandomInRange);
					moduleBase = new ScaleBias(-1.0, 1.0, moduleBase);
					moduleBase = new Scale((double)TileMutatorWorker_CaveLakes.LakeSquash.RandomInRange, 1.0, 1.0, moduleBase);
					moduleBase = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, moduleBase);
					moduleBase = new Translate((double)(-(double)intVec.x), 0.0, (double)(-(double)intVec.z), moduleBase);
					moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.03f, 5f, 4, map.Tile.tileId);
					CellRect cellRect = intVec.RectAbout(30, 30);
					cellRect.ClipInsideMap(map);
					foreach (IntVec3 intVec2 in cellRect)
					{
						float num = (float)moduleBase.GetValue((double)intVec2.x, 0.0, (double)intVec2.z);
						if (num >= 0.5f)
						{
							Thing edifice = intVec2.GetEdifice(map);
							if (edifice != null)
							{
								edifice.Destroy(DestroyMode.Vanish);
							}
							if (num > 0.8f)
							{
								map.terrainGrid.SetTerrain(intVec2, MapGenUtility.DeepFreshWaterTerrainAt(intVec2, map));
							}
							else if (num > 0.5f)
							{
								map.terrainGrid.SetTerrain(intVec2, MapGenUtility.ShallowFreshWaterTerrainAt(intVec2, map));
							}
						}
					}
				}
			}
		}

		// Token: 0x04004C28 RID: 19496
		private static readonly FloatRange LakeRadius = new FloatRange(10f, 15f);

		// Token: 0x04004C29 RID: 19497
		private static readonly FloatRange LakeSquash = new FloatRange(1f, 1.25f);

		// Token: 0x04004C2A RID: 19498
		private const float LakeNoiseFrequency = 0.03f;

		// Token: 0x04004C2B RID: 19499
		private const float LakeNoiseStrength = 5f;

		// Token: 0x04004C2C RID: 19500
		private const float LakeChancePerCell = 0.005f;

		// Token: 0x04004C2D RID: 19501
		private const float DeepWaterThreshold = 0.8f;

		// Token: 0x04004C2E RID: 19502
		private const float WaterThreshold = 0.5f;
	}
}
