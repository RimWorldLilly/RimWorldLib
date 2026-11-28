using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld.Planet
{
	// Token: 0x0200239F RID: 9119
	public static class MultipleCaravansCellFinder
	{
		// Token: 0x0600E2F0 RID: 58096 RVA: 0x00546378 File Offset: 0x00544578
		public static void FindStartingCellsFor2Groups(Map map, out IntVec3 first, out IntVec3 second)
		{
			for (int i = 0; i < 10; i++)
			{
				if (MultipleCaravansCellFinder.TryFindOppositeSpots(map, 0.05f, out first, out second))
				{
					return;
				}
			}
			for (int j = 0; j < 10; j++)
			{
				if (MultipleCaravansCellFinder.TryFindOppositeSpots(map, 0.15f, out first, out second))
				{
					return;
				}
			}
			if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out first))
			{
				Log.Error("Could not find any valid starting cell for a caravan.");
				first = CellFinder.RandomCell(map);
				second = CellFinder.RandomCell(map);
				return;
			}
			IntVec3 localFirst = first;
			float tryMinDistBetweenSpots = (float)Mathf.Max(map.Size.x, map.Size.z) * 0.6f;
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false).WithFenceblocked(true);
			if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && !x.InHorDistOf(localFirst, tryMinDistBetweenSpots) && map.reachability.CanReach(x, localFirst, PathEndMode.OnCell, traverseParams), map, CellFinder.EdgeRoadChance_Neutral, out second) && !CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && map.reachability.CanReach(x, localFirst, PathEndMode.OnCell, traverseParams), map, 0.5f, out second))
			{
				Log.Error("Could not find any valid starting cell for a caravan.");
				second = CellFinder.RandomCell(map);
				return;
			}
			first = CellFinder.RandomClosewalkCellNear(first, map, 7, null);
			second = CellFinder.RandomClosewalkCellNear(second, map, 7, null);
		}

		// Token: 0x0600E2F1 RID: 58097 RVA: 0x00546508 File Offset: 0x00544708
		private static bool TryFindOppositeSpots(Map map, float maxDistPctToOppositeSpots, out IntVec3 first, out IntVec3 second)
		{
			IntVec3 intVec = MultipleCaravansCellFinder.RandomSpotNearEdge(map);
			IntVec3 intVec2 = MultipleCaravansCellFinder.OppositeSpot(intVec, map);
			int num = Mathf.Min(map.Size.x, map.Size.z);
			CellRect cellRect = CellRect.CenteredOn(intVec, Mathf.Max(Mathf.RoundToInt((float)num * maxDistPctToOppositeSpots), 1)).ClipInsideMap(map);
			CellRect cellRect2 = CellRect.CenteredOn(intVec2, Mathf.Max(Mathf.RoundToInt((float)num * maxDistPctToOppositeSpots), 1)).ClipInsideMap(map);
			for (int i = 0; i < 20; i++)
			{
				IntVec3 intVec3 = (i == 0) ? intVec : cellRect.RandomCell;
				IntVec3 intVec4 = (i == 0) ? intVec2 : cellRect2.RandomCell;
				if (intVec3.Standable(map) && !intVec3.Fogged(map) && intVec4.Standable(map) && !intVec4.Fogged(map) && map.reachability.CanReach(intVec3, intVec4, PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false).WithFenceblocked(true)))
				{
					first = intVec3;
					second = intVec4;
					return true;
				}
			}
			first = IntVec3.Invalid;
			second = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600E2F2 RID: 58098 RVA: 0x00546638 File Offset: 0x00544838
		private static IntVec3 RandomSpotNearEdge(Map map)
		{
			CellRect cellRect = CellRect.WholeMap(map);
			cellRect.minX += Mathf.RoundToInt((float)map.Size.x * 0.2f);
			cellRect.minZ += Mathf.RoundToInt((float)map.Size.z * 0.2f);
			cellRect.maxX -= Mathf.RoundToInt((float)map.Size.x * 0.2f);
			cellRect.maxZ -= Mathf.RoundToInt((float)map.Size.z * 0.2f);
			return cellRect.EdgeCells.RandomElement<IntVec3>();
		}

		// Token: 0x0600E2F3 RID: 58099 RVA: 0x005466E0 File Offset: 0x005448E0
		private static IntVec3 OppositeSpot(IntVec3 spot, Map map)
		{
			return new IntVec3(map.Size.x - spot.x, spot.y, map.Size.z - spot.z);
		}

		// Token: 0x040090D3 RID: 37075
		private const int TriesToFindPerfectOppositeSpots = 10;

		// Token: 0x040090D4 RID: 37076
		private const int TriesToFindGoodEnoughOppositeSpots = 10;

		// Token: 0x040090D5 RID: 37077
		private const int TriesToFindMatchingPair = 20;

		// Token: 0x040090D6 RID: 37078
		private const float PerfectIfDistPctToOppositeSpotsAtMost = 0.05f;

		// Token: 0x040090D7 RID: 37079
		private const float GoodEnoughIfDistPctToOppositeSpotsAtMost = 0.15f;

		// Token: 0x040090D8 RID: 37080
		private const float SpotDistPctToEdge = 0.2f;

		// Token: 0x040090D9 RID: 37081
		private const float TryMinDistPctBetweenFallbackEdgeCells = 0.6f;
	}
}
