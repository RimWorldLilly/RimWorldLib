using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002034 RID: 8244
	public class PlaceWorker_NextToHopperAccepter : PlaceWorker
	{
		// Token: 0x0600D0BE RID: 53438 RVA: 0x004CCA00 File Offset: 0x004CAC00
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = loc + GenAdj.CardinalDirections[i];
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						ThingDef thingDef = GenConstruct.BuiltDefOf(thingList[j].def) as ThingDef;
						if (thingDef != null && thingDef.building != null && thingDef.building.wantsHopperAdjacent)
						{
							return true;
						}
					}
				}
			}
			return "MustPlaceNextToHopperAccepter".Translate();
		}
	}
}
