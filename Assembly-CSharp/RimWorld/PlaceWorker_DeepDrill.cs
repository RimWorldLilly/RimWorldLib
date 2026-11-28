using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200203A RID: 8250
	public class PlaceWorker_DeepDrill : PlaceWorker_ShowDeepResources
	{
		// Token: 0x0600D0CA RID: 53450 RVA: 0x004CCC4B File Offset: 0x004CAE4B
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			if (DeepDrillUtility.GetNextResource(loc, map) == null)
			{
				return "MustPlaceOnDrillable".Translate();
			}
			return true;
		}
	}
}
