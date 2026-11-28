using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BF0 RID: 3056
	public class WorkGiver_Breastfeed : WorkGiver_Scanner
	{
		// Token: 0x0600577C RID: 22396 RVA: 0x00206CCE File Offset: 0x00204ECE
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedHungryPawns;
		}

		// Token: 0x0600577D RID: 22397 RVA: 0x00206CE0 File Offset: 0x00204EE0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced)
		{
			if (forced)
			{
				return null;
			}
			Pawn pawn2 = t as Pawn;
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanMomAutoBreastfeedBabyNow(pawn, pawn2, forced, out breastfeedFailReason))
			{
				return null;
			}
			if (pawn2.mindState.AutofeedSetting(pawn) != AutofeedMode.Childcare)
			{
				return null;
			}
			return ChildcareUtility.MakeBreastfeedJob(pawn2, null);
		}
	}
}
