using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002045 RID: 8261
	public class PlaceWorker_NeverAdjacentTrap : PlaceWorker
	{
		// Token: 0x0600D0E6 RID: 53478 RVA: 0x004CD55C File Offset: 0x004CB75C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
		}

		// Token: 0x0600D0E7 RID: 53479 RVA: 0x004CD560 File Offset: 0x004CB760
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(center, rot, def.Size).ExpandedBy(1))
			{
				if (c.InBounds(map))
				{
					List<Thing> list = map.thingGrid.ThingsListAt(c);
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing2 = list[i];
						if (thing2 != thingToIgnore && ((thing2.def.category == ThingCategory.Building && thing2.def.building.isTrap) || ((thing2.def.IsBlueprint || thing2.def.IsFrame) && thing2.def.entityDefToBuild is ThingDef && ((ThingDef)thing2.def.entityDefToBuild).building.isTrap)))
						{
							return "CannotPlaceAdjacentTrap".Translate();
						}
					}
				}
			}
			return true;
		}
	}
}
