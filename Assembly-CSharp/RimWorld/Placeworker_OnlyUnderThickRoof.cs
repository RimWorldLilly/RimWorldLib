using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002031 RID: 8241
	public class Placeworker_OnlyUnderThickRoof : PlaceWorker
	{
		// Token: 0x0600D0B6 RID: 53430 RVA: 0x004CC7C0 File Offset: 0x004CA9C0
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			RoofDef roofDef = map.roofGrid.RoofAt(loc);
			if (roofDef == null || !roofDef.isThickRoof)
			{
				return new AcceptanceReport("MustPlaceUnderThickRoof".Translate());
			}
			return true;
		}
	}
}
