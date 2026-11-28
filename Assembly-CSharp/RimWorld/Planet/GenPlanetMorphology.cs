using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002339 RID: 9017
	public static class GenPlanetMorphology
	{
		// Token: 0x0600DECE RID: 57038 RVA: 0x0052E25C File Offset: 0x0052C45C
		public static void Erode(PlanetLayer layer, List<PlanetTile> tiles, int count, Predicate<PlanetTile> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			WorldGrid worldGrid = Find.WorldGrid;
			GenPlanetMorphology.tilesSet.Clear();
			GenPlanetMorphology.tilesSet.AddRange(tiles);
			GenPlanetMorphology.tmpEdgeTiles.Clear();
			for (int i = 0; i < tiles.Count; i++)
			{
				worldGrid.GetTileNeighbors(tiles[i], GenPlanetMorphology.tmpNeighbors);
				for (int j = 0; j < GenPlanetMorphology.tmpNeighbors.Count; j++)
				{
					if (!GenPlanetMorphology.tilesSet.Contains(GenPlanetMorphology.tmpNeighbors[j]))
					{
						GenPlanetMorphology.tmpEdgeTiles.Add(tiles[i]);
						break;
					}
				}
			}
			if (!GenPlanetMorphology.tmpEdgeTiles.Any<PlanetTile>())
			{
				return;
			}
			GenPlanetMorphology.tmpOutput.Clear();
			Predicate<PlanetTile> passCheck;
			if (extraPredicate != null)
			{
				passCheck = ((PlanetTile x) => GenPlanetMorphology.tilesSet.Contains(x) && extraPredicate(x));
			}
			else
			{
				passCheck = ((PlanetTile x) => GenPlanetMorphology.tilesSet.Contains(x));
			}
			layer.Filler.FloodFill(PlanetTile.Invalid, passCheck, delegate(PlanetTile tile, int traversalDist)
			{
				if (traversalDist >= count)
				{
					GenPlanetMorphology.tmpOutput.Add(tile);
				}
				return false;
			}, int.MaxValue, GenPlanetMorphology.tmpEdgeTiles);
			tiles.Clear();
			tiles.AddRange(GenPlanetMorphology.tmpOutput);
		}

		// Token: 0x0600DECF RID: 57039 RVA: 0x0052E39C File Offset: 0x0052C59C
		public static void Dilate(PlanetLayer layer, List<PlanetTile> tiles, int count, Predicate<PlanetTile> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			WorldFloodFiller filler = layer.Filler;
			PlanetTile invalid = PlanetTile.Invalid;
			Predicate<PlanetTile> passCheck = extraPredicate;
			if (extraPredicate == null && (passCheck = GenPlanetMorphology.<>c.<>9__5_0) == null)
			{
				passCheck = (GenPlanetMorphology.<>c.<>9__5_0 = ((PlanetTile x) => true));
			}
			filler.FloodFill(invalid, passCheck, delegate(PlanetTile tile, int traversalDist)
			{
				if (traversalDist > count)
				{
					return true;
				}
				if (traversalDist != 0)
				{
					tiles.Add(tile);
				}
				return false;
			}, int.MaxValue, tiles);
		}

		// Token: 0x0600DED0 RID: 57040 RVA: 0x0052E412 File Offset: 0x0052C612
		public static void Open(PlanetLayer layer, List<PlanetTile> tiles, int count)
		{
			GenPlanetMorphology.Erode(layer, tiles, count, null);
			GenPlanetMorphology.Dilate(layer, tiles, count, null);
		}

		// Token: 0x0600DED1 RID: 57041 RVA: 0x0052E426 File Offset: 0x0052C626
		public static void Close(PlanetLayer layer, List<PlanetTile> tiles, int count)
		{
			GenPlanetMorphology.Dilate(layer, tiles, count, null);
			GenPlanetMorphology.Erode(layer, tiles, count, null);
		}

		// Token: 0x04008F0F RID: 36623
		private static readonly HashSet<PlanetTile> tmpOutput = new HashSet<PlanetTile>();

		// Token: 0x04008F10 RID: 36624
		private static readonly HashSet<PlanetTile> tilesSet = new HashSet<PlanetTile>();

		// Token: 0x04008F11 RID: 36625
		private static readonly List<PlanetTile> tmpEdgeTiles = new List<PlanetTile>();

		// Token: 0x04008F12 RID: 36626
		private static readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();
	}
}
