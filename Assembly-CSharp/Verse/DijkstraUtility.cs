using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006C4 RID: 1732
	public static class DijkstraUtility
	{
		// Token: 0x060036EC RID: 14060 RVA: 0x001605A4 File Offset: 0x0015E7A4
		public static IEnumerable<IntVec3> AdjacentCellsNeighborsGetter(IntVec3 cell, Map map)
		{
			DijkstraUtility.adjacentCells.Clear();
			IntVec3[] array = GenAdj.AdjacentCells;
			for (int i = 0; i < array.Length; i++)
			{
				IntVec3 intVec = cell + array[i];
				if (intVec.InBounds(map))
				{
					DijkstraUtility.adjacentCells.Add(intVec);
				}
			}
			return DijkstraUtility.adjacentCells;
		}

		// Token: 0x0400241E RID: 9246
		private static List<IntVec3> adjacentCells = new List<IntVec3>();
	}
}
