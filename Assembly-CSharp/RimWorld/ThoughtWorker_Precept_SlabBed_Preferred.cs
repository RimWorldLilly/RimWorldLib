using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D93 RID: 3475
	public class ThoughtWorker_Precept_SlabBed_Preferred : ThoughtWorker_Precept
	{
		// Token: 0x06005FCA RID: 24522 RVA: 0x0022D551 File Offset: 0x0022B751
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return p.mindState.lastBedDefSleptIn != null && p.mindState.lastBedDefSleptIn.building != null && p.mindState.lastBedDefSleptIn.building.bed_slabBed;
		}
	}
}
