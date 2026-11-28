using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000892 RID: 2194
	public class MentalStateWorker_BedroomTantrum : MentalStateWorker
	{
		// Token: 0x0600442C RID: 17452 RVA: 0x001AB99C File Offset: 0x001A9B9C
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			if (ownedBed == null || ownedBed.GetRoom(RegionType.Set_All) == null || ownedBed.GetRoom(RegionType.Set_All).PsychologicallyOutdoors)
			{
				return false;
			}
			MentalStateWorker_BedroomTantrum.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsIn(ownedBed.GetRoom(RegionType.Set_All), pawn, MentalStateWorker_BedroomTantrum.tmpThings, null, 0);
			bool result = MentalStateWorker_BedroomTantrum.tmpThings.Any<Thing>();
			MentalStateWorker_BedroomTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x04002C3F RID: 11327
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
