using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002044 RID: 8260
	public class PlaceWorker_NeverAdjacentUnstandable : PlaceWorker
	{
		// Token: 0x0600D0E3 RID: 53475 RVA: 0x004CD440 File Offset: 0x004CB640
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawFieldEdges(GenAdj.OccupiedRect(center, rot, def.size).ExpandedBy(1).Cells.ToList<IntVec3>(), Color.white, null, null, 2900);
		}

		// Token: 0x0600D0E4 RID: 53476 RVA: 0x004CD48C File Offset: 0x004CB68C
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(center, rot, def.Size).ExpandedBy(1))
			{
				if (c.InBounds(map))
				{
					List<Thing> list = map.thingGrid.ThingsListAt(c);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i] != thingToIgnore && list[i].def.passability != Traversability.Standable)
						{
							return "MustPlaceAdjacentStandable".Translate();
						}
					}
				}
			}
			return true;
		}
	}
}
