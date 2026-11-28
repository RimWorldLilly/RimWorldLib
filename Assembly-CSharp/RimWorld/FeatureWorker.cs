using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F5B RID: 3931
	public abstract class FeatureWorker
	{
		// Token: 0x06006520 RID: 25888
		public abstract void GenerateWhereAppropriate(PlanetLayer layer);

		// Token: 0x06006521 RID: 25889 RVA: 0x00240FA0 File Offset: 0x0023F1A0
		protected void AddFeature(PlanetLayer layer, List<PlanetTile> members, List<PlanetTile> tilesForTextDrawPosCalculation)
		{
			WorldFeature worldFeature = new WorldFeature(this.def, layer);
			worldFeature.name = NameGenerator.GenerateName(this.def.nameMaker, from x in Find.WorldFeatures.features
			select x.name, false, "r_name");
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < members.Count; i++)
			{
				worldGrid[members[i]].feature = worldFeature;
			}
			this.AssignBestDrawPos(layer, worldFeature, tilesForTextDrawPosCalculation);
			Find.WorldFeatures.features.Add(worldFeature);
		}

		// Token: 0x06006522 RID: 25890 RVA: 0x00241048 File Offset: 0x0023F248
		private void AssignBestDrawPos(PlanetLayer layer, WorldFeature newFeature, List<PlanetTile> tilesForTextDrawPosCalculation)
		{
			FeatureWorker.<>c__DisplayClass10_0 CS$<>8__locals1 = new FeatureWorker.<>c__DisplayClass10_0();
			WorldGrid worldGrid = Find.WorldGrid;
			FeatureWorker.tmpEdgeTiles.Clear();
			FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Clear();
			FeatureWorker.tmpTilesForTextDrawPosCalculationSet.AddRange(tilesForTextDrawPosCalculation);
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < tilesForTextDrawPosCalculation.Count; i++)
			{
				PlanetTile planetTile = tilesForTextDrawPosCalculation[i];
				vector += worldGrid.GetTileCenter(planetTile);
				bool flag = worldGrid.IsOnEdge(planetTile);
				if (!flag)
				{
					worldGrid.GetTileNeighbors(planetTile, FeatureWorker.tmpNeighbors);
					for (int j = 0; j < FeatureWorker.tmpNeighbors.Count; j++)
					{
						if (!FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Contains(FeatureWorker.tmpNeighbors[j]))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					FeatureWorker.tmpEdgeTiles.Add(planetTile);
				}
			}
			vector /= (float)tilesForTextDrawPosCalculation.Count;
			if (!FeatureWorker.tmpEdgeTiles.Any<PlanetTile>())
			{
				FeatureWorker.tmpEdgeTiles.Add(tilesForTextDrawPosCalculation.RandomElement<PlanetTile>());
			}
			CS$<>8__locals1.bestTileDist = 0;
			FeatureWorker.tmpTraversedTiles.Clear();
			layer.Filler.FloodFill(PlanetTile.Invalid, (PlanetTile tile) => FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Contains(tile), new Predicate<PlanetTile, int>(CS$<>8__locals1.<AssignBestDrawPos>g__Process|0), int.MaxValue, FeatureWorker.tmpEdgeTiles);
			PlanetTile tile2 = PlanetTile.Invalid;
			float num = -1f;
			for (int k = 0; k < FeatureWorker.tmpTraversedTiles.Count; k++)
			{
				if (FeatureWorker.tmpTraversedTiles[k].Item2 == CS$<>8__locals1.bestTileDist)
				{
					float sqrMagnitude = (worldGrid.GetTileCenter(FeatureWorker.tmpTraversedTiles[k].Item1) - vector).sqrMagnitude;
					if (!tile2.Valid || sqrMagnitude < num)
					{
						tile2 = FeatureWorker.tmpTraversedTiles[k].Item1;
						num = sqrMagnitude;
					}
				}
			}
			float maxDrawSizeInTiles = (float)CS$<>8__locals1.bestTileDist * 2f * 1.2f;
			newFeature.drawCenter = worldGrid.GetTileCenter(tile2);
			newFeature.maxDrawSizeInTiles = maxDrawSizeInTiles;
		}

		// Token: 0x06006523 RID: 25891 RVA: 0x00241251 File Offset: 0x0023F451
		protected static void ClearVisited(PlanetLayer layer)
		{
			FeatureWorker.ClearOrCreate<bool>(layer, ref FeatureWorker.visited);
		}

		// Token: 0x06006524 RID: 25892 RVA: 0x0024125E File Offset: 0x0023F45E
		protected static void ClearGroupSizes(PlanetLayer layer)
		{
			FeatureWorker.ClearOrCreate<int>(layer, ref FeatureWorker.groupSize);
		}

		// Token: 0x06006525 RID: 25893 RVA: 0x0024126B File Offset: 0x0023F46B
		protected static void ClearGroupIDs(PlanetLayer layer)
		{
			FeatureWorker.ClearOrCreate<int>(layer, ref FeatureWorker.groupID);
		}

		// Token: 0x06006526 RID: 25894 RVA: 0x00241278 File Offset: 0x0023F478
		private static void ClearOrCreate<T>(PlanetLayer layer, ref T[] array)
		{
			int tilesCount = layer.TilesCount;
			if (array == null || array.Length != tilesCount)
			{
				array = new T[tilesCount];
				return;
			}
			Array.Clear(array, 0, array.Length);
		}

		// Token: 0x04003B7C RID: 15228
		public FeatureDef def;

		// Token: 0x04003B7D RID: 15229
		protected static bool[] visited;

		// Token: 0x04003B7E RID: 15230
		protected static int[] groupSize;

		// Token: 0x04003B7F RID: 15231
		protected static int[] groupID;

		// Token: 0x04003B80 RID: 15232
		private static readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();

		// Token: 0x04003B81 RID: 15233
		private static readonly HashSet<PlanetTile> tmpTilesForTextDrawPosCalculationSet = new HashSet<PlanetTile>();

		// Token: 0x04003B82 RID: 15234
		private static readonly List<PlanetTile> tmpEdgeTiles = new List<PlanetTile>();

		// Token: 0x04003B83 RID: 15235
		[TupleElementNames(new string[]
		{
			"tile",
			"traversalDistance"
		})]
		private static readonly List<ValueTuple<PlanetTile, int>> tmpTraversedTiles = new List<ValueTuple<PlanetTile, int>>();
	}
}
