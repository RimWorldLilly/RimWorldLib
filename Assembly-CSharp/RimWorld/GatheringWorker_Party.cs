using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F63 RID: 3939
	public class GatheringWorker_Party : GatheringWorker
	{
		// Token: 0x06006547 RID: 25927 RVA: 0x00241850 File Offset: 0x0023FA50
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			return new LordJob_Joinable_Party(spot, organizer, this.def);
		}

		// Token: 0x06006548 RID: 25928 RVA: 0x0024185F File Offset: 0x0023FA5F
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			return RCellFinder.TryFindGatheringSpot(organizer, this.def, false, out spot);
		}
	}
}
