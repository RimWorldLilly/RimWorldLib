using System;

namespace Verse
{
	// Token: 0x0200027A RID: 634
	public class CellGrid
	{
		// Token: 0x17000385 RID: 901
		public IntVec3 this[IntVec3 c]
		{
			get
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				return CellIndicesUtility.IndexToCell(this.grid[num], this.mapSizeX);
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				this.grid[num] = CellIndicesUtility.CellToIndex(value, this.mapSizeX);
			}
		}

		// Token: 0x17000386 RID: 902
		public IntVec3 this[int index]
		{
			get
			{
				return CellIndicesUtility.IndexToCell(this.grid[index], this.mapSizeX);
			}
			set
			{
				this.grid[index] = CellIndicesUtility.CellToIndex(value, this.mapSizeX);
			}
		}

		// Token: 0x17000387 RID: 903
		public IntVec3 this[int x, int z]
		{
			get
			{
				int num = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
				return CellIndicesUtility.IndexToCell(this.grid[num], this.mapSizeX);
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
				this.grid[num] = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x0600128D RID: 4749 RVA: 0x00069F00 File Offset: 0x00068100
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00069F0A File Offset: 0x0006810A
		public CellGrid()
		{
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00069F12 File Offset: 0x00068112
		public CellGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x00069F21 File Offset: 0x00068121
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00069F4C File Offset: 0x0006814C
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear();
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new int[this.mapSizeX * this.mapSizeZ];
			this.Clear();
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00069FB4 File Offset: 0x000681B4
		public void Clear()
		{
			int num = CellIndicesUtility.CellToIndex(IntVec3.Invalid, this.mapSizeX);
			for (int i = 0; i < this.grid.Length; i++)
			{
				this.grid[i] = num;
			}
		}

		// Token: 0x040010A5 RID: 4261
		private int[] grid;

		// Token: 0x040010A6 RID: 4262
		private int mapSizeX;

		// Token: 0x040010A7 RID: 4263
		private int mapSizeZ;
	}
}
