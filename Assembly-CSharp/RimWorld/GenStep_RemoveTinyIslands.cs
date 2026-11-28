using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B9 RID: 4793
	public class GenStep_RemoveTinyIslands : GenStep
	{
		// Token: 0x17001438 RID: 5176
		// (get) Token: 0x060076F7 RID: 30455 RVA: 0x0029EE52 File Offset: 0x0029D052
		public override int SeedPart
		{
			get
			{
				return 56712394;
			}
		}

		// Token: 0x060076F8 RID: 30456 RVA: 0x0029EE5C File Offset: 0x0029D05C
		public override void Generate(Map map, GenStepParams parms)
		{
			GenStep_RemoveTinyIslands.<>c__DisplayClass4_0 CS$<>8__locals1 = new GenStep_RemoveTinyIslands.<>c__DisplayClass4_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.mapRect = CellRect.WholeMap(CS$<>8__locals1.map);
			int num = 0;
			GenStep_RemoveTinyIslands.tmpVisited.Clear();
			foreach (IntVec3 intVec in CS$<>8__locals1.map.AllCells)
			{
				if (!GenStep_RemoveTinyIslands.tmpVisited.Contains(intVec) && !CS$<>8__locals1.<Generate>g__Impassable|0(intVec))
				{
					int area = 0;
					bool touchesMapEdge = false;
					FloodFiller floodFiller = CS$<>8__locals1.map.floodFiller;
					IntVec3 root = intVec;
					Predicate<IntVec3> passCheck;
					if ((passCheck = CS$<>8__locals1.<>9__1) == null)
					{
						passCheck = (CS$<>8__locals1.<>9__1 = ((IntVec3 x) => !base.<Generate>g__Impassable|0(x)));
					}
					floodFiller.FloodFill(root, passCheck, delegate(IntVec3 x)
					{
						GenStep_RemoveTinyIslands.tmpVisited.Add(x);
						int area = area;
						area++;
						if (CS$<>8__locals1.mapRect.IsOnEdge(x))
						{
							touchesMapEdge = true;
						}
					}, int.MaxValue, false, null);
					if (touchesMapEdge)
					{
						num = Mathf.Max(num, area);
					}
				}
			}
			if (num < 30)
			{
				return;
			}
			GenStep_RemoveTinyIslands.tmpVisited.Clear();
			foreach (IntVec3 intVec2 in CS$<>8__locals1.map.AllCells)
			{
				if (!GenStep_RemoveTinyIslands.tmpVisited.Contains(intVec2) && !CS$<>8__locals1.<Generate>g__Impassable|0(intVec2))
				{
					GenStep_RemoveTinyIslands.tmpIsland.Clear();
					TerrainDef adjacentImpassableTerrain = null;
					bool touchesMapEdge = false;
					CS$<>8__locals1.map.floodFiller.FloodFill(intVec2, delegate(IntVec3 x)
					{
						if (CS$<>8__locals1.<Generate>g__Impassable|0(x))
						{
							adjacentImpassableTerrain = x.GetTerrain(CS$<>8__locals1.map);
							return false;
						}
						return true;
					}, delegate(IntVec3 x)
					{
						GenStep_RemoveTinyIslands.tmpVisited.Add(x);
						GenStep_RemoveTinyIslands.tmpIsland.Add(x);
						if (CS$<>8__locals1.mapRect.IsOnEdge(x))
						{
							touchesMapEdge = true;
						}
					}, int.MaxValue, false, null);
					if ((GenStep_RemoveTinyIslands.tmpIsland.Count <= num / 20 || (!touchesMapEdge && GenStep_RemoveTinyIslands.tmpIsland.Count < num / 2)) && adjacentImpassableTerrain != null)
					{
						for (int i = 0; i < GenStep_RemoveTinyIslands.tmpIsland.Count; i++)
						{
							CS$<>8__locals1.map.terrainGrid.SetTerrain(GenStep_RemoveTinyIslands.tmpIsland[i], adjacentImpassableTerrain);
						}
					}
				}
			}
		}

		// Token: 0x04004AA9 RID: 19113
		private static HashSet<IntVec3> tmpVisited = new HashSet<IntVec3>();

		// Token: 0x04004AAA RID: 19114
		private static List<IntVec3> tmpIsland = new List<IntVec3>();
	}
}
