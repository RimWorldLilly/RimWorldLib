using System;

namespace Verse
{
	// Token: 0x02000285 RID: 645
	public sealed class IntGrid
	{
		// Token: 0x17000395 RID: 917
		public int this[IntVec3 c]
		{
			get
			{
				return this.grid[CellIndicesUtility.CellToIndex(c, this.mapSizeX)];
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				this.grid[num] = value;
			}
		}

		// Token: 0x17000396 RID: 918
		public int this[int index]
		{
			get
			{
				return this.grid[index];
			}
			set
			{
				this.grid[index] = value;
			}
		}

		// Token: 0x17000397 RID: 919
		public int this[int x, int z]
		{
			get
			{
				return this.grid[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)];
			}
			set
			{
				this.grid[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)] = value;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x0600132E RID: 4910 RVA: 0x0006CA6D File Offset: 0x0006AC6D
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x0006CA77 File Offset: 0x0006AC77
		public IntGrid()
		{
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0006CA7F File Offset: 0x0006AC7F
		public IntGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0006CA8E File Offset: 0x0006AC8E
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0006CAB8 File Offset: 0x0006ACB8
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear(0);
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new int[this.mapSizeX * this.mapSizeZ];
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0006CB18 File Offset: 0x0006AD18
		public void Clear(int value = 0)
		{
			if (value == 0)
			{
				Array.Clear(this.grid, 0, this.grid.Length);
				return;
			}
			for (int i = 0; i < this.grid.Length; i++)
			{
				this.grid[i] = value;
			}
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0006CB5C File Offset: 0x0006AD5C
		public void DebugDraw()
		{
			for (int i = 0; i < this.grid.Length; i++)
			{
				int num = this.grid[i];
				if (num > 0)
				{
					CellRenderer.RenderCell(CellIndicesUtility.IndexToCell(i, this.mapSizeX), (float)(num % 100) / 100f * 0.5f);
				}
			}
		}

		// Token: 0x040010E2 RID: 4322
		private int[] grid;

		// Token: 0x040010E3 RID: 4323
		private int mapSizeX;

		// Token: 0x040010E4 RID: 4324
		private int mapSizeZ;
	}
}
