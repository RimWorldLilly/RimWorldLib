using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E43 RID: 7747
	public static class CellFinderUtility
	{
		// Token: 0x1700250F RID: 9487
		// (get) Token: 0x0600C8E7 RID: 51431 RVA: 0x0049BBA9 File Offset: 0x00499DA9
		public static ByteGrid DistToColonyBuilding
		{
			get
			{
				return CellFinderUtility.distToColonyBuilding;
			}
		}

		// Token: 0x0600C8E8 RID: 51432 RVA: 0x0049BBB0 File Offset: 0x00499DB0
		public static void CalculateDistanceToColonyBuildingGrid(Map map)
		{
			if (CellFinderUtility.distToColonyBuilding == null)
			{
				CellFinderUtility.distToColonyBuilding = new ByteGrid(map);
			}
			else if (!CellFinderUtility.distToColonyBuilding.MapSizeMatches(map))
			{
				CellFinderUtility.distToColonyBuilding.ClearAndResizeTo(map);
			}
			CellFinderUtility.distToColonyBuilding.Clear(byte.MaxValue);
			CellFinderUtility.tmpColonyBuildingsLocs.Clear();
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if (!building.IsClearableFreeBuilding)
				{
					CellFinderUtility.tmpColonyBuildingsLocs.Add(building.Position);
				}
			}
			Dijkstra<IntVec3>.Run(CellFinderUtility.tmpColonyBuildingsLocs, (IntVec3 x) => DijkstraUtility.AdjacentCellsNeighborsGetter(x, map), delegate(IntVec3 a, IntVec3 b)
			{
				if (a.x == b.x || a.z == b.z)
				{
					return 1f;
				}
				return 1.4142135f;
			}, CellFinderUtility.tmpDistanceResult, null);
			for (int j = 0; j < CellFinderUtility.tmpDistanceResult.Count; j++)
			{
				CellFinderUtility.distToColonyBuilding[CellFinderUtility.tmpDistanceResult[j].Key] = (byte)Mathf.Min(CellFinderUtility.tmpDistanceResult[j].Value, 254.999f);
			}
		}

		// Token: 0x04007272 RID: 29298
		private static ByteGrid distToColonyBuilding;

		// Token: 0x04007273 RID: 29299
		private static readonly List<IntVec3> tmpColonyBuildingsLocs = new List<IntVec3>();

		// Token: 0x04007274 RID: 29300
		private static readonly List<KeyValuePair<IntVec3, float>> tmpDistanceResult = new List<KeyValuePair<IntVec3, float>>();
	}
}
