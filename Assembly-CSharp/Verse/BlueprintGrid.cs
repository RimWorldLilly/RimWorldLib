using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000277 RID: 631
	public sealed class BlueprintGrid
	{
		// Token: 0x17000377 RID: 887
		public List<Blueprint> this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x17000378 RID: 888
		public List<Blueprint> this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x000697FB File Offset: 0x000679FB
		public List<Blueprint>[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x00069803 File Offset: 0x00067A03
		public BlueprintGrid(Map map)
		{
			this.map = map;
			this.innerArray = new List<Blueprint>[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x00069828 File Offset: 0x00067A28
		public void Register(Blueprint ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					int num = cellIndices.CellToIndex(j, i);
					List<Blueprint>[] array = this.innerArray;
					int num2 = num;
					if (array[num2] == null)
					{
						array[num2] = new List<Blueprint>();
					}
					this.innerArray[num].Add(ed);
				}
			}
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x000698AC File Offset: 0x00067AAC
		public void DeRegister(Blueprint ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					int num = cellIndices.CellToIndex(j, i);
					this.innerArray[num].Remove(ed);
					if (this.innerArray[num].Count == 0)
					{
						this.innerArray[num] = null;
					}
				}
			}
		}

		// Token: 0x0400109A RID: 4250
		private readonly Map map;

		// Token: 0x0400109B RID: 4251
		private readonly List<Blueprint>[] innerArray;
	}
}
