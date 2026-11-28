using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E44 RID: 7748
	public class CellSearchPattern : IExposable
	{
		// Token: 0x0600C8EA RID: 51434 RVA: 0x0049BD09 File Offset: 0x00499F09
		public virtual void AddCellsToList(List<IntVec3> orderedCells, Thing thing, CellRect rect, IntVec3 focus, Rot4 focusRotation)
		{
			orderedCells.AddRange(rect.AdjacentCells);
		}

		// Token: 0x0600C8EB RID: 51435 RVA: 0x0049BD18 File Offset: 0x00499F18
		public virtual void ExposeData()
		{
		}
	}
}
