using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000309 RID: 777
	public class RoofCollapseBuffer
	{
		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060017E7 RID: 6119 RVA: 0x0008F7DC File Offset: 0x0008D9DC
		public List<IntVec3> CellsMarkedToCollapse
		{
			get
			{
				return this.cellsToCollapse;
			}
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x0008F7E4 File Offset: 0x0008D9E4
		public bool IsMarkedToCollapse(IntVec3 c)
		{
			return this.cellsToCollapse.Contains(c);
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x0008F7F2 File Offset: 0x0008D9F2
		public void MarkToCollapse(IntVec3 c)
		{
			if (!this.cellsToCollapse.Contains(c))
			{
				this.cellsToCollapse.Add(c);
			}
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x0008F80E File Offset: 0x0008DA0E
		public void Clear()
		{
			this.cellsToCollapse.Clear();
		}

		// Token: 0x04001426 RID: 5158
		private List<IntVec3> cellsToCollapse = new List<IntVec3>();
	}
}
