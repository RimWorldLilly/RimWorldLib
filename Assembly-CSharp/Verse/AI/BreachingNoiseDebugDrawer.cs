using System;

namespace Verse.AI
{
	// Token: 0x02000912 RID: 2322
	public static class BreachingNoiseDebugDrawer
	{
		// Token: 0x060046C0 RID: 18112 RVA: 0x001BCCBC File Offset: 0x001BAEBC
		public static void DebugDrawNoise(BreachingGrid grid)
		{
			Map currentMap = Find.CurrentMap;
			BreachingNoiseDebugDrawer.CheckInitDebugDrawGrid(grid);
			foreach (IntVec3 c in currentMap.AllCells)
			{
				if (BreachingNoiseDebugDrawer.debugDrawGrid[c] > 0)
				{
					CellRenderer.RenderCell(c, (float)BreachingNoiseDebugDrawer.debugDrawGrid[c] / 100f);
				}
			}
		}

		// Token: 0x060046C1 RID: 18113 RVA: 0x001BCD34 File Offset: 0x001BAF34
		private static void CheckInitDebugDrawGrid(BreachingGrid grid)
		{
			if (grid != BreachingNoiseDebugDrawer.debugGrid)
			{
				BreachingNoiseDebugDrawer.debugDrawGrid = null;
				BreachingNoiseDebugDrawer.debugGrid = grid;
			}
			if (BreachingNoiseDebugDrawer.debugDrawGrid == null)
			{
				BreachingNoiseDebugDrawer.debugDrawGrid = new IntGrid(grid.Map);
				BreachingNoiseDebugDrawer.debugDrawGrid.Clear(0);
				foreach (IntVec3 intVec in grid.Map.AllCells)
				{
					if (BreachingNoiseDebugDrawer.debugGrid.WithinNoise(intVec))
					{
						BreachingNoiseDebugDrawer.debugDrawGrid[intVec] = 1;
					}
				}
			}
		}

		// Token: 0x04002E0B RID: 11787
		private static BreachingGrid debugGrid;

		// Token: 0x04002E0C RID: 11788
		private static IntGrid debugDrawGrid;
	}
}
