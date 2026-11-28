using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200088E RID: 2190
	public class MentalStateWorker_WanderOwnRoom : MentalStateWorker
	{
		// Token: 0x06004421 RID: 17441 RVA: 0x001AB860 File Offset: 0x001A9A60
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			return ownedBed != null && ownedBed.GetRoom(RegionType.Set_All) != null && !ownedBed.GetRoom(RegionType.Set_All).PsychologicallyOutdoors;
		}
	}
}
