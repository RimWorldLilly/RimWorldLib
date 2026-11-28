using System;
using LudeonTK;
using Unity.Collections;

namespace Verse
{
	// Token: 0x020002DE RID: 734
	public class MapGenFloatGrid : IDisposable
	{
		// Token: 0x17000434 RID: 1076
		public float this[IntVec3 c]
		{
			get
			{
				return this.grid[this.map.cellIndices.CellToIndex(c)];
			}
			set
			{
				this.grid[this.map.cellIndices.CellToIndex(c)] = value;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001655 RID: 5717 RVA: 0x0008652D File Offset: 0x0008472D
		internal ref NativeArray<float> Grid_Unsafe
		{
			get
			{
				return ref this.grid;
			}
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00086535 File Offset: 0x00084735
		public MapGenFloatGrid(Map map)
		{
			this.map = map;
			this.grid = new NativeArray<float>(map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x0008655C File Offset: 0x0008475C
		public void Clear()
		{
			NativeArrayUtility.MemClear<float>(this.grid);
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x00086569 File Offset: 0x00084769
		public void Dispose()
		{
			this.grid.Dispose();
		}

		// Token: 0x04001350 RID: 4944
		private readonly Map map;

		// Token: 0x04001351 RID: 4945
		private NativeArray<float> grid;
	}
}
