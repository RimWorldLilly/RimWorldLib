using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.SketchGen;
using Verse;

namespace RimWorld
{
	// Token: 0x0200134A RID: 4938
	public class TileMutatorWorker_Stockpile : TileMutatorWorker
	{
		// Token: 0x06007A4B RID: 31307 RVA: 0x002B94FB File Offset: 0x002B76FB
		public TileMutatorWorker_Stockpile(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A4C RID: 31308 RVA: 0x002B9504 File Offset: 0x002B7704
		public override string GetLabel(PlanetTile tile)
		{
			return ("Stockpile" + TileMutatorWorker_Stockpile.GetStockpileType(tile).ToString()).Translate();
		}

		// Token: 0x06007A4D RID: 31309 RVA: 0x002B953C File Offset: 0x002B773C
		public override void GeneratePostFog(Map map)
		{
			TileMutatorWorker_Stockpile.<>c__DisplayClass4_0 CS$<>8__locals1 = new TileMutatorWorker_Stockpile.<>c__DisplayClass4_0();
			CS$<>8__locals1.map = map;
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			SketchResolveParams parms = default(SketchResolveParams);
			parms.sketch = new Sketch();
			CS$<>8__locals1.sketch = SketchGen.Generate(SketchResolverDefOf.AncientHatch, parms);
			CellRect item;
			if (!MapGenUtility.TryGetRandomClearRect(CS$<>8__locals1.sketch.OccupiedSize.x, CS$<>8__locals1.sketch.OccupiedSize.z, out item, -1, -1, new Predicate<CellRect>(CS$<>8__locals1.<GeneratePostFog>g__Validator|0), -1f, 0.7f, MapGenUtility.SearchWeightMode.Largest, 1))
			{
				IntVec3 center;
				if (!CellFinder.TryFindRandomCell(CS$<>8__locals1.map, (IntVec3 c) => base.<GeneratePostFog>g__Validator|0(CellRect.CenteredOn(c, CS$<>8__locals1.sketch.OccupiedSize)), out center))
				{
					return;
				}
				item = CellRect.CenteredOn(center, CS$<>8__locals1.sketch.OccupiedSize);
			}
			List<Thing> list = new List<Thing>();
			CS$<>8__locals1.sketch.Spawn(CS$<>8__locals1.map, item.Min, null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, true, false, true, list, false, false, null, null, null);
			CS$<>8__locals1.usedRects.Add(item);
			(list.FirstOrDefault((Thing t) => t.def == ThingDefOf.AncientHatch) as AncientHatch).stockpileType = TileMutatorWorker_Stockpile.GetStockpileType(CS$<>8__locals1.map.Tile);
		}

		// Token: 0x06007A4E RID: 31310 RVA: 0x002B967A File Offset: 0x002B787A
		private static TileMutatorWorker_Stockpile.StockpileType GetStockpileType(PlanetTile tile)
		{
			Rand.PushState(tile.tileId);
			TileMutatorWorker_Stockpile.StockpileType result = TileMutatorWorker_Stockpile.GeneratableStockpileTypes.RandomElement<TileMutatorWorker_Stockpile.StockpileType>();
			Rand.PopState();
			return result;
		}

		// Token: 0x04004D3E RID: 19774
		public static readonly List<TileMutatorWorker_Stockpile.StockpileType> GeneratableStockpileTypes = new List<TileMutatorWorker_Stockpile.StockpileType>
		{
			TileMutatorWorker_Stockpile.StockpileType.Medicine,
			TileMutatorWorker_Stockpile.StockpileType.Chemfuel,
			TileMutatorWorker_Stockpile.StockpileType.Component,
			TileMutatorWorker_Stockpile.StockpileType.Weapons,
			TileMutatorWorker_Stockpile.StockpileType.Drugs
		};

		// Token: 0x02003256 RID: 12886
		public enum StockpileType
		{
			// Token: 0x0400CA3E RID: 51774
			Medicine,
			// Token: 0x0400CA3F RID: 51775
			Chemfuel,
			// Token: 0x0400CA40 RID: 51776
			Component,
			// Token: 0x0400CA41 RID: 51777
			Weapons,
			// Token: 0x0400CA42 RID: 51778
			Gravcore,
			// Token: 0x0400CA43 RID: 51779
			Drugs
		}
	}
}
