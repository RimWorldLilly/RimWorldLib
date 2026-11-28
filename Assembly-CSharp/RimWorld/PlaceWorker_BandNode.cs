using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002066 RID: 8294
	public class PlaceWorker_BandNode : PlaceWorker
	{
		// Token: 0x0600D143 RID: 53571 RVA: 0x004CF0E8 File Offset: 0x004CD2E8
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(center, rot, def.Size).ExpandedBy(1))
			{
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i].def == def || thingList[i].def.entityDefToBuild == def)
						{
							return "MustNotBePlacedCloseToAnother".Translate(def);
						}
					}
				}
			}
			return true;
		}
	}
}
