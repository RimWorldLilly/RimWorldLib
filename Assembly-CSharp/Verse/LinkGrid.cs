using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000585 RID: 1413
	public class LinkGrid
	{
		// Token: 0x060029BD RID: 10685 RVA: 0x000FF65B File Offset: 0x000FD85B
		public LinkGrid(Map map)
		{
			this.map = map;
			this.linkGrid = new LinkFlags[map.cellIndices.NumGridCells];
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x000FF680 File Offset: 0x000FD880
		public LinkFlags LinkFlagsAt(IntVec3 c)
		{
			return this.linkGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x000FF69C File Offset: 0x000FD89C
		public void Notify_LinkerCreatedOrDestroyed(Thing linker)
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 c in linker.OccupiedRect())
			{
				LinkFlags linkFlags = LinkFlags.None;
				List<Thing> list = this.map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def.graphicData != null)
					{
						linkFlags |= list[i].def.graphicData.linkFlags;
					}
				}
				this.linkGrid[cellIndices.CellToIndex(c)] = linkFlags;
			}
		}

		// Token: 0x04001CEC RID: 7404
		private Map map;

		// Token: 0x04001CED RID: 7405
		private LinkFlags[] linkGrid;
	}
}
