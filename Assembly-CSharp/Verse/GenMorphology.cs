using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x020006ED RID: 1773
	public static class GenMorphology
	{
		// Token: 0x06003875 RID: 14453 RVA: 0x00169B78 File Offset: 0x00167D78
		public static void Erode(List<IntVec3> cells, int count, Map map, Predicate<IntVec3> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			GenMorphology.cellsSet.Clear();
			GenMorphology.cellsSet.AddRange(cells);
			GenMorphology.tmpEdgeCells.Clear();
			for (int i = 0; i < cells.Count; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					IntVec3 item = cells[i] + cardinalDirections[j];
					if (!GenMorphology.cellsSet.Contains(item))
					{
						GenMorphology.tmpEdgeCells.Add(cells[i]);
						break;
					}
				}
			}
			if (!GenMorphology.tmpEdgeCells.Any<IntVec3>())
			{
				return;
			}
			GenMorphology.tmpOutput.Clear();
			Predicate<IntVec3> passCheck;
			if (extraPredicate != null)
			{
				passCheck = ((IntVec3 x) => GenMorphology.cellsSet.Contains(x) && extraPredicate(x));
			}
			else
			{
				passCheck = ((IntVec3 x) => GenMorphology.cellsSet.Contains(x));
			}
			map.floodFiller.FloodFill(IntVec3.Invalid, passCheck, delegate(IntVec3 cell, int traversalDist)
			{
				if (traversalDist >= count)
				{
					GenMorphology.tmpOutput.Add(cell);
				}
				return false;
			}, int.MaxValue, false, GenMorphology.tmpEdgeCells);
			cells.Clear();
			cells.AddRange(GenMorphology.tmpOutput);
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x00169CA8 File Offset: 0x00167EA8
		public static void Dilate(List<IntVec3> cells, int count, Map map, Predicate<IntVec3> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			FloodFiller floodFiller = map.floodFiller;
			IntVec3 invalid = IntVec3.Invalid;
			Predicate<IntVec3> passCheck = extraPredicate;
			if (extraPredicate == null && (passCheck = GenMorphology.<>c.<>9__4_0) == null)
			{
				passCheck = (GenMorphology.<>c.<>9__4_0 = ((IntVec3 x) => true));
			}
			floodFiller.FloodFill(invalid, passCheck, delegate(IntVec3 cell, int traversalDist)
			{
				if (traversalDist > count)
				{
					return true;
				}
				if (traversalDist != 0)
				{
					cells.Add(cell);
				}
				return false;
			}, int.MaxValue, false, cells);
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x00169D1F File Offset: 0x00167F1F
		public static void Open(List<IntVec3> cells, int count, Map map)
		{
			GenMorphology.Erode(cells, count, map, null);
			GenMorphology.Dilate(cells, count, map, null);
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x00169D33 File Offset: 0x00167F33
		public static void Close(List<IntVec3> cells, int count, Map map)
		{
			GenMorphology.Dilate(cells, count, map, null);
			GenMorphology.Erode(cells, count, map, null);
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x00169D47 File Offset: 0x00167F47
		public static void GenerateNaturalPatch(Map map, CellRect rect, GenMorphology.GenPatchParms? parms = null, Predicate<IntVec3> validator = null)
		{
			GenMorphology.GenerateNaturalPatch(map, new List<CellRect>
			{
				rect
			}, parms, validator);
		}

		// Token: 0x0600387A RID: 14458 RVA: 0x00169D60 File Offset: 0x00167F60
		public static void GenerateNaturalPatch(Map map, List<CellRect> rects, GenMorphology.GenPatchParms? parms = null, Predicate<IntVec3> validator = null)
		{
			GenMorphology.GenPatchParms genPatchParms = parms ?? GenMorphology.GenPatchParms.Default;
			ModuleBase lhs = new Perlin((double)genPatchParms.macroFrequency, 1.25, 0.5, 6, map.ConstantRandSeed, QualityMode.Medium);
			ModuleBase rhs = new Perlin((double)genPatchParms.microFrequency, 2.0, 0.5, 6, map.ConstantRandSeed + 1, QualityMode.Medium);
			ModuleBase moduleBase = new Blend(lhs, rhs, new Const((double)genPatchParms.blendMacroMicro));
			moduleBase = new ScaleBias(0.5, 0.6000000238418579, moduleBase);
			ModuleBase moduleBase2 = new DistFromPointRects(rects);
			moduleBase2 = new Clamp(0.0, 1.0, moduleBase2);
			moduleBase2 = new ScaleBias(2.200000047683716, -0.5, moduleBase2);
			moduleBase = new Blend(moduleBase2, moduleBase, new Const((double)genPatchParms.blendFalloff));
			foreach (CellRect cellRect in rects)
			{
				foreach (IntVec3 intVec in cellRect.Cells)
				{
					if (GenMorphology.visited.Add(intVec) && intVec.InBounds(map) && (validator == null || validator(intVec)))
					{
						float value = moduleBase.GetValue(intVec);
						if (genPatchParms.terrainDef != null && value >= genPatchParms.terrainThreshold)
						{
							map.terrainGrid.SetTerrain(intVec, genPatchParms.terrainDef);
						}
						if (genPatchParms.edificeDef != null && value >= genPatchParms.edificeThreshold)
						{
							GenSpawn.Spawn(genPatchParms.edificeDef, intVec, map, WipeMode.Vanish);
						}
						if (genPatchParms.roofDef != null && value >= genPatchParms.roofThreshold)
						{
							map.roofGrid.SetRoof(intVec, genPatchParms.roofDef);
						}
					}
				}
			}
			GenMorphology.visited.Clear();
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x00169F80 File Offset: 0x00168180
		private static float ValueAt(ModuleBase noise, IntVec3 cell, List<CellRect> rects)
		{
			float value = rects.Min(delegate(CellRect rect)
			{
				IntVec3 b = rect.ClosestCellTo(cell);
				return (cell - b).Magnitude;
			});
			float num = Mathf.Clamp01(Mathf.InverseLerp(0f, 12f, value));
			return noise.GetValue(cell) + 1f - 2f * num;
		}

		// Token: 0x040024CD RID: 9421
		private static HashSet<IntVec3> tmpOutput = new HashSet<IntVec3>();

		// Token: 0x040024CE RID: 9422
		private static HashSet<IntVec3> cellsSet = new HashSet<IntVec3>();

		// Token: 0x040024CF RID: 9423
		private static List<IntVec3> tmpEdgeCells = new List<IntVec3>();

		// Token: 0x040024D0 RID: 9424
		private static readonly HashSet<IntVec3> visited = new HashSet<IntVec3>();

		// Token: 0x02002A53 RID: 10835
		public struct GenPatchParms
		{
			// Token: 0x17002BC5 RID: 11205
			// (get) Token: 0x060105FF RID: 67071 RVA: 0x005C526F File Offset: 0x005C346F
			public static GenMorphology.GenPatchParms Default
			{
				get
				{
					return GenMorphology.GenPatchParms.For(ThingDefOf.Slate, ThingDefOf.Slate.building.naturalTerrain);
				}
			}

			// Token: 0x06010600 RID: 67072 RVA: 0x005C528A File Offset: 0x005C348A
			public static GenMorphology.GenPatchParms For(ThingDef edifice)
			{
				return GenMorphology.GenPatchParms.For(edifice, null);
			}

			// Token: 0x06010601 RID: 67073 RVA: 0x005C5293 File Offset: 0x005C3493
			public static GenMorphology.GenPatchParms For(TerrainDef terrain)
			{
				return GenMorphology.GenPatchParms.For(null, terrain);
			}

			// Token: 0x06010602 RID: 67074 RVA: 0x005C529C File Offset: 0x005C349C
			public static GenMorphology.GenPatchParms For(ThingDef edifice, TerrainDef terrain)
			{
				return new GenMorphology.GenPatchParms
				{
					edificeDef = edifice,
					terrainDef = terrain,
					roofDef = RoofDefOf.RoofRockThin,
					macroFrequency = 0.05f,
					microFrequency = 0.08f,
					blendMacroMicro = 0.01f,
					blendFalloff = 0.4f,
					terrainThreshold = 0.45f,
					edificeThreshold = 0.5f,
					roofThreshold = 0.55f
				};
			}

			// Token: 0x0400AD64 RID: 44388
			public ThingDef edificeDef;

			// Token: 0x0400AD65 RID: 44389
			public TerrainDef terrainDef;

			// Token: 0x0400AD66 RID: 44390
			public RoofDef roofDef;

			// Token: 0x0400AD67 RID: 44391
			public float macroFrequency;

			// Token: 0x0400AD68 RID: 44392
			public float microFrequency;

			// Token: 0x0400AD69 RID: 44393
			public float blendMacroMicro;

			// Token: 0x0400AD6A RID: 44394
			public float blendFalloff;

			// Token: 0x0400AD6B RID: 44395
			public float terrainThreshold;

			// Token: 0x0400AD6C RID: 44396
			public float edificeThreshold;

			// Token: 0x0400AD6D RID: 44397
			public float roofThreshold;
		}
	}
}
