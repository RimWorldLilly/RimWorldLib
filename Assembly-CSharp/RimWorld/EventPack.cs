using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E19 RID: 7705
	public struct EventPack
	{
		// Token: 0x170024F7 RID: 9463
		// (get) Token: 0x0600C82E RID: 51246 RVA: 0x004965DC File Offset: 0x004947DC
		public string Tag
		{
			get
			{
				return this.tagInt;
			}
		}

		// Token: 0x170024F8 RID: 9464
		// (get) Token: 0x0600C82F RID: 51247 RVA: 0x004965E4 File Offset: 0x004947E4
		public IntVec3 Cell
		{
			get
			{
				return this.cellInt;
			}
		}

		// Token: 0x170024F9 RID: 9465
		// (get) Token: 0x0600C830 RID: 51248 RVA: 0x004965EC File Offset: 0x004947EC
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				return this.cellsInt;
			}
		}

		// Token: 0x0600C831 RID: 51249 RVA: 0x004965F4 File Offset: 0x004947F4
		public EventPack(string tag)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = null;
		}

		// Token: 0x0600C832 RID: 51250 RVA: 0x0049660F File Offset: 0x0049480F
		public EventPack(string tag, IntVec3 cell)
		{
			this.tagInt = tag;
			this.cellInt = cell;
			this.cellsInt = null;
		}

		// Token: 0x0600C833 RID: 51251 RVA: 0x00496626 File Offset: 0x00494826
		public EventPack(string tag, IEnumerable<IntVec3> cells)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = cells;
		}

		// Token: 0x0600C834 RID: 51252 RVA: 0x00496641 File Offset: 0x00494841
		public static implicit operator EventPack(string s)
		{
			return new EventPack(s);
		}

		// Token: 0x0600C835 RID: 51253 RVA: 0x0049664C File Offset: 0x0049484C
		public override string ToString()
		{
			if (this.Cell.IsValid)
			{
				return this.Tag + "-" + this.Cell.ToString();
			}
			return this.Tag;
		}

		// Token: 0x040071E6 RID: 29158
		private string tagInt;

		// Token: 0x040071E7 RID: 29159
		private IntVec3 cellInt;

		// Token: 0x040071E8 RID: 29160
		private IEnumerable<IntVec3> cellsInt;
	}
}
