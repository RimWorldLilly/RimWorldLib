using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002064 RID: 8292
	public class PlaceWorker_MustBeIndoors : PlaceWorker
	{
		// Token: 0x0600D13F RID: 53567 RVA: 0x004CF074 File Offset: 0x004CD274
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			Room room = loc.GetRoom(map);
			if (room == null || room.TouchesMapEdge)
			{
				return "MustBePlacedIndoors".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}
	}
}
