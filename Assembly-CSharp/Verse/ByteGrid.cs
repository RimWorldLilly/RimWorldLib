using System;

namespace Verse
{
	// Token: 0x02000279 RID: 633
	public sealed class ByteGrid : IExposable
	{
		// Token: 0x17000381 RID: 897
		public byte this[IntVec3 c]
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

		// Token: 0x17000382 RID: 898
		public byte this[int index]
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

		// Token: 0x17000383 RID: 899
		public byte this[int x, int z]
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

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x0600127F RID: 4735 RVA: 0x00069CA1 File Offset: 0x00067EA1
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00069CAB File Offset: 0x00067EAB
		public ByteGrid()
		{
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00069CB3 File Offset: 0x00067EB3
		public ByteGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00069CC2 File Offset: 0x00067EC2
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00069CEC File Offset: 0x00067EEC
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear(0);
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new byte[this.mapSizeX * this.mapSizeZ];
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00069D4C File Offset: 0x00067F4C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.LookByteArray(ref this.grid, "grid");
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00069D84 File Offset: 0x00067F84
		public void Clear(byte value = 0)
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

		// Token: 0x06001286 RID: 4742 RVA: 0x00069DC8 File Offset: 0x00067FC8
		public void DebugDraw()
		{
			for (int i = 0; i < this.grid.Length; i++)
			{
				byte b = this.grid[i];
				if (b > 0)
				{
					CellRenderer.RenderCell(CellIndicesUtility.IndexToCell(i, this.mapSizeX), (float)b / 255f * 0.5f);
				}
			}
		}

		// Token: 0x040010A2 RID: 4258
		private byte[] grid;

		// Token: 0x040010A3 RID: 4259
		private int mapSizeX;

		// Token: 0x040010A4 RID: 4260
		private int mapSizeZ;
	}
}
