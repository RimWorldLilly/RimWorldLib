using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200032E RID: 814
	public static class ZoneMaker
	{
		// Token: 0x06001904 RID: 6404 RVA: 0x00095860 File Offset: 0x00093A60
		public static Zone MakeZoneWithCells(Zone z, IEnumerable<IntVec3> cells)
		{
			if (cells != null)
			{
				foreach (IntVec3 c in cells)
				{
					z.AddCell(c);
				}
			}
			return z;
		}
	}
}
