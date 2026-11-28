using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002035 RID: 8245
	public class PlaceWorker_HeadOnShipBeam : PlaceWorker
	{
		// Token: 0x0600D0C0 RID: 53440 RVA: 0x004CCAA0 File Offset: 0x004CACA0
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			IntVec3 c = loc + rot.FacingCell * -1;
			if (!c.InBounds(map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			if (edifice == null || edifice.def != ThingDefOf.Ship_Beam)
			{
				return "MustPlaceHeadOnShipBeam".Translate();
			}
			return true;
		}
	}
}
