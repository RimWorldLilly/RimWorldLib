using System;
using System.Collections.Generic;
using RimWorld.SketchGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E9 RID: 4841
	public class GenStep_AncientStockpile_Gravcore : GenStep
	{
		// Token: 0x17001484 RID: 5252
		// (get) Token: 0x06007825 RID: 30757 RVA: 0x002A7B41 File Offset: 0x002A5D41
		public override int SeedPart
		{
			get
			{
				return 341298741;
			}
		}

		// Token: 0x06007826 RID: 30758 RVA: 0x002A7B48 File Offset: 0x002A5D48
		public override void Generate(Map map, GenStepParams parms)
		{
			for (int i = 0; i < 3; i++)
			{
				this.GenerateHatch(map, i == 0);
			}
		}

		// Token: 0x06007827 RID: 30759 RVA: 0x002A7B6C File Offset: 0x002A5D6C
		private void GenerateHatch(Map map, bool spawnGravcore = false)
		{
			GenStep_AncientStockpile_Gravcore.<>c__DisplayClass7_0 CS$<>8__locals1 = new GenStep_AncientStockpile_Gravcore.<>c__DisplayClass7_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			SketchResolveParams parms = default(SketchResolveParams);
			parms.sketch = new Sketch();
			CS$<>8__locals1.sketch = SketchGen.Generate(SketchResolverDefOf.AncientHatch, parms);
			CellRect item;
			if (!MapGenUtility.TryGetRandomClearRect(CS$<>8__locals1.sketch.OccupiedSize.x, CS$<>8__locals1.sketch.OccupiedSize.z, out item, -1, -1, new Predicate<CellRect>(CS$<>8__locals1.<GenerateHatch>g__Validator|0), -1f, 0.7f, MapGenUtility.SearchWeightMode.Largest, 1))
			{
				IntVec3 center;
				if (!CellFinder.TryFindRandomCell(CS$<>8__locals1.map, (IntVec3 cell) => base.<GenerateHatch>g__Validator|0(CellRect.CenteredOn(cell, CS$<>8__locals1.sketch.OccupiedSize)), out center))
				{
					return;
				}
				item = CellRect.CenteredOn(center, CS$<>8__locals1.sketch.OccupiedSize);
			}
			GenStep_AncientStockpile_Gravcore.spawnedThings.Clear();
			CS$<>8__locals1.sketch.Spawn(CS$<>8__locals1.map, item.Min, null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, true, false, true, GenStep_AncientStockpile_Gravcore.spawnedThings, false, false, null, null, null);
			CS$<>8__locals1.usedRects.Add(item);
			AncientHatch ancientHatch = GenStep_AncientStockpile_Gravcore.spawnedThings.FirstOrDefault((Thing t) => t.def == ThingDefOf.AncientHatch) as AncientHatch;
			if (spawnGravcore)
			{
				ancientHatch.stockpileType = TileMutatorWorker_Stockpile.StockpileType.Gravcore;
			}
			else
			{
				ancientHatch.layout = LayoutDefOf.AncientStockpileFake;
			}
			this.hatches.Add(ancientHatch);
		}

		// Token: 0x04004B55 RID: 19285
		private List<Thing> hatches = new List<Thing>();

		// Token: 0x04004B56 RID: 19286
		private const int NumHatches = 3;

		// Token: 0x04004B57 RID: 19287
		private const int MinDistanceBetweenHatches = 20;

		// Token: 0x04004B58 RID: 19288
		private static List<Thing> spawnedThings = new List<Thing>();
	}
}
