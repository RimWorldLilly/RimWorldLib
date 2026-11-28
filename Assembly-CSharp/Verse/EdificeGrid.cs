using System;

namespace Verse
{
	// Token: 0x0200027D RID: 637
	public sealed class EdificeGrid
	{
		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060012AB RID: 4779 RVA: 0x0006A678 File Offset: 0x00068878
		public Building[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x1700038D RID: 909
		public Building this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x1700038E RID: 910
		public Building this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0006A6A4 File Offset: 0x000688A4
		public EdificeGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Building[map.cellIndices.NumGridCells];
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0006A6CC File Offset: 0x000688CC
		public void Register(Building ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.innerArray[cellIndices.CellToIndex(c)] = ed;
				}
			}
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0006A734 File Offset: 0x00068934
		public void DeRegister(Building ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.innerArray[cellIndices.CellToIndex(j, i)] = null;
				}
			}
		}

		// Token: 0x040010B1 RID: 4273
		private Map map;

		// Token: 0x040010B2 RID: 4274
		private Building[] innerArray;
	}
}
