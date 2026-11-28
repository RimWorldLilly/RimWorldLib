using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200027B RID: 635
	public sealed class CoverGrid
	{
		// Token: 0x17000389 RID: 905
		public Thing this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x1700038A RID: 906
		public Thing this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x0006A012 File Offset: 0x00068212
		public CoverGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Thing[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0006A038 File Offset: 0x00068238
		public void Register(Thing t)
		{
			if (t.def.Fillage == FillCategory.None)
			{
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.RecalculateCell(c, null);
				}
			}
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0006A098 File Offset: 0x00068298
		public void DeRegister(Thing t)
		{
			if (t.def.Fillage == FillCategory.None)
			{
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.RecalculateCell(c, t);
				}
			}
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x0006A0F8 File Offset: 0x000682F8
		private void RecalculateCell(IntVec3 c, Thing ignoreThing = null)
		{
			Thing thing = null;
			float num = 0.001f;
			List<Thing> list = this.map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing2 != ignoreThing && !thing2.Destroyed && thing2.Spawned && thing2.def.fillPercent > num)
				{
					thing = thing2;
					num = thing2.def.fillPercent;
				}
			}
			this.innerArray[this.map.cellIndices.CellToIndex(c)] = thing;
		}

		// Token: 0x040010A8 RID: 4264
		private Map map;

		// Token: 0x040010A9 RID: 4265
		private Thing[] innerArray;
	}
}
