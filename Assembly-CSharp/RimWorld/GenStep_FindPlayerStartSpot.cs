using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A0 RID: 4768
	public class GenStep_FindPlayerStartSpot : GenStep
	{
		// Token: 0x1700140B RID: 5131
		// (get) Token: 0x06007643 RID: 30275 RVA: 0x0029A049 File Offset: 0x00298249
		public override int SeedPart
		{
			get
			{
				return 1187186631;
			}
		}

		// Token: 0x06007644 RID: 30276 RVA: 0x0029A050 File Offset: 0x00298250
		public override void Generate(Map map, GenStepParams parms)
		{
			GenStep_FindPlayerStartSpot.<>c__DisplayClass3_0 CS$<>8__locals1 = new GenStep_FindPlayerStartSpot.<>c__DisplayClass3_0();
			CS$<>8__locals1.map = map;
			if (CS$<>8__locals1.map.wasSpawnedViaGravShipLanding)
			{
				return;
			}
			DeepProfiler.Start("RebuildAllRegions");
			CS$<>8__locals1.map.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			DeepProfiler.End();
			if (MapGenerator.PlayerStartSpotValid)
			{
				return;
			}
			CS$<>8__locals1.largestOpenArea = this.FindLargestContiguousOpenArea(CS$<>8__locals1.map);
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			MapGenerator.PlayerStartSpot = CellFinderLoose.TryFindCentralCell(CS$<>8__locals1.map, 7, 10, new Predicate<IntVec3>(CS$<>8__locals1.<Generate>g__Validator|0), false);
		}

		// Token: 0x06007645 RID: 30277 RVA: 0x0029A0E4 File Offset: 0x002982E4
		private HashSet<IntVec3> FindLargestContiguousOpenArea(Map map)
		{
			HashSet<IntVec3> checkedCells = new HashSet<IntVec3>();
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
			HashSet<IntVec3> current = new HashSet<IntVec3>();
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			using (IEnumerator<IntVec3> enumerator = map.AllCells.GetEnumerator())
			{
				Action<IntVec3> <>9__1;
				while (enumerator.MoveNext())
				{
					IntVec3 cell = enumerator.Current;
					if (cell.GetEdifice(map) == null && !checkedCells.Contains(cell))
					{
						current.Clear();
						FloodFiller floodFiller = map.floodFiller;
						IntVec3 cell2 = cell;
						Predicate<IntVec3> passCheck = (IntVec3 x) => cell.GetEdifice(map) == null;
						Action<IntVec3> processor;
						if ((processor = <>9__1) == null)
						{
							processor = (<>9__1 = delegate(IntVec3 x)
							{
								current.Add(x);
								checkedCells.Add(x);
							});
						}
						floodFiller.FloodFill(cell2, passCheck, processor, int.MaxValue, false, null);
						if (current.Count > hashSet.Count)
						{
							hashSet.Clear();
							hashSet.AddRange(current);
						}
					}
				}
			}
			return hashSet;
		}

		// Token: 0x04004A23 RID: 18979
		private const int MinRoomCellCount = 10;
	}
}
