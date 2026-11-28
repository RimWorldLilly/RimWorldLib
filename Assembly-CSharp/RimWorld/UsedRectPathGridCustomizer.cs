using System;
using Unity.Collections;
using Verse;

namespace RimWorld
{
	// Token: 0x020012DD RID: 4829
	public class UsedRectPathGridCustomizer : PathRequest.IPathGridCustomizer, IDisposable
	{
		// Token: 0x060077CA RID: 30666 RVA: 0x002A66FC File Offset: 0x002A48FC
		public UsedRectPathGridCustomizer(Map map)
		{
			this.grid = new NativeArray<ushort>(map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			foreach (CellRect cellRect in MapGenerator.UsedRects)
			{
				foreach (IntVec3 c in cellRect.ExpandedBy(2).Cells)
				{
					if (c.InBounds(map))
					{
						this.grid[map.cellIndices.CellToIndex(c)] = 10000;
					}
				}
			}
		}

		// Token: 0x060077CB RID: 30667 RVA: 0x002A67CC File Offset: 0x002A49CC
		public NativeArray<ushort> GetOffsetGrid()
		{
			return this.grid;
		}

		// Token: 0x060077CC RID: 30668 RVA: 0x002A67D4 File Offset: 0x002A49D4
		public void Dispose()
		{
			this.grid.Dispose();
		}

		// Token: 0x04004B3B RID: 19259
		private NativeArray<ushort> grid;
	}
}
