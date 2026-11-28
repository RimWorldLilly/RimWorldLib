using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016FA RID: 5882
	public class RitualPosition_PreferredAdjacentCellCardinal : RitualPosition_Cells
	{
		// Token: 0x060092F6 RID: 37622 RVA: 0x0035B4E3 File Offset: 0x003596E3
		public override void FindCells(List<IntVec3> cells, Thing thing, CellRect rect, IntVec3 spot, Rot4 rotation)
		{
			if (this.preferredCellSearchPattern != null)
			{
				this.preferredCellSearchPattern.AddCellsToList(cells, thing, rect, spot, (thing != null) ? thing.Rotation : Rot4.South);
				return;
			}
			cells.AddRange(rect.AdjacentCellsCardinal);
		}

		// Token: 0x060092F7 RID: 37623 RVA: 0x0035B51B File Offset: 0x0035971B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<CellSearchPattern>(ref this.preferredCellSearchPattern, "preferredCellSearchPattern", Array.Empty<object>());
		}

		// Token: 0x0400590B RID: 22795
		public CellSearchPattern preferredCellSearchPattern;
	}
}
