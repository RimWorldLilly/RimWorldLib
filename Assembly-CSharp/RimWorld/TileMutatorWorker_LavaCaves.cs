using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001337 RID: 4919
	public class TileMutatorWorker_LavaCaves : TileMutatorWorker_Caves
	{
		// Token: 0x060079DE RID: 31198 RVA: 0x002B5AED File Offset: 0x002B3CED
		public TileMutatorWorker_LavaCaves(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079DF RID: 31199 RVA: 0x002B5AF8 File Offset: 0x002B3CF8
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
					ModuleBase moduleBase = new DistFromPoint(TileMutatorWorker_LavaCaves.LakeRadius.RandomInRange);
					moduleBase = new ScaleBias(-1.0, 1.0, moduleBase);
					moduleBase = new Scale((double)TileMutatorWorker_LavaCaves.LakeSquash.RandomInRange, 1.0, 1.0, moduleBase);
					moduleBase = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, moduleBase);
					moduleBase = new Translate((double)(-(double)intVec.x), 0.0, (double)(-(double)intVec.z), moduleBase);
					moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.03f, 5f, 4, map.Tile.tileId);
					CellRect cellRect = intVec.RectAbout(20, 20);
					cellRect.ClipInsideMap(map);
					foreach (IntVec3 intVec2 in cellRect)
					{
						float num = (float)moduleBase.GetValue((double)intVec2.x, 0.0, (double)intVec2.z);
						if (num >= 0.2f)
						{
							Thing edifice = intVec2.GetEdifice(map);
							if (edifice != null)
							{
								edifice.Destroy(DestroyMode.Vanish);
							}
							if (num > 0.5f)
							{
								map.terrainGrid.SetTerrain(intVec2, TerrainDefOf.LavaDeep);
							}
							else if (num > 0.2f)
							{
								map.terrainGrid.SetTerrain(intVec2, TerrainDefOf.CooledLava);
							}
						}
					}
				}
			}
		}

		// Token: 0x04004CDC RID: 19676
		private static readonly FloatRange LakeRadius = new FloatRange(4f, 6f);

		// Token: 0x04004CDD RID: 19677
		private static readonly FloatRange LakeSquash = new FloatRange(1f, 1.25f);

		// Token: 0x04004CDE RID: 19678
		private const float LakeNoiseFrequency = 0.03f;

		// Token: 0x04004CDF RID: 19679
		private const float LakeNoiseStrength = 5f;

		// Token: 0x04004CE0 RID: 19680
		private const float LavaPocketChancePerCell = 0.005f;

		// Token: 0x04004CE1 RID: 19681
		private const float LavaThreshold = 0.5f;

		// Token: 0x04004CE2 RID: 19682
		private const float CooledLavaThreshold = 0.2f;
	}
}
