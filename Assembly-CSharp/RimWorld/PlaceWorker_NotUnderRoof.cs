using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002030 RID: 8240
	public class PlaceWorker_NotUnderRoof : PlaceWorker
	{
		// Token: 0x0600D0B4 RID: 53428 RVA: 0x004CC6F4 File Offset: 0x004CA8F4
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			if (checkingDef.Size.x == 1 && checkingDef.Size.z == 1)
			{
				if (map.roofGrid.Roofed(loc))
				{
					return new AcceptanceReport("MustPlaceUnroofed".Translate());
				}
			}
			else
			{
				foreach (IntVec3 c in GenAdj.OccupiedRect(loc, rot, checkingDef.Size))
				{
					if (map.roofGrid.Roofed(c))
					{
						return new AcceptanceReport("MustPlaceUnroofed".Translate());
					}
				}
			}
			return true;
		}
	}
}
