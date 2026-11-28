using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016FB RID: 5883
	public class RitualPosition_OnInteractionCell : RitualPosition_Cells
	{
		// Token: 0x060092F9 RID: 37625 RVA: 0x0035B540 File Offset: 0x00359740
		public override void FindCells(List<IntVec3> cells, Thing thing, CellRect rect, IntVec3 spot, Rot4 rotation)
		{
			cells.Add(thing.InteractionCell + this.offset);
		}

		// Token: 0x060092FA RID: 37626 RVA: 0x0035B55C File Offset: 0x0035975C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.offset, "offset", default(IntVec3), false);
		}

		// Token: 0x0400590C RID: 22796
		public IntVec3 offset = IntVec3.Zero;
	}
}
