using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002062 RID: 8290
	public class PlaceWorker_RitualSeat : PlaceWorker
	{
		// Token: 0x0600D13B RID: 53563 RVA: 0x004CEEB8 File Offset: 0x004CD0B8
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			ThingDef thingDef = (def.entityDefToBuild != null) ? ((ThingDef)def.entityDefToBuild) : def;
			List<Thing> list = Find.CurrentMap.listerThings.ThingsOfDef(ThingDefOf.Lectern);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (GatheringsUtility.InGatheringArea(center, thing2.Position, Find.CurrentMap) && SpectatorCellFinder.IsCorrectlyRotatedChair(center, rot, thingDef, thing2.OccupiedRect()))
				{
					GenDraw.DrawLineBetween(GenThing.TrueCenter(center, rot, thingDef.size, thingDef.Altitude), thing2.TrueCenter(), SimpleColor.Yellow, 0.2f);
					foreach (Thing thing3 in PlaceWorker_RitualPosition.GetRitualFocusInRange(thing2.Position, thing2))
					{
						if (GatheringsUtility.InGatheringArea(center, thing3.Position, Find.CurrentMap) && SpectatorCellFinder.IsCorrectlyRotatedChair(center, rot, thingDef, thing3.OccupiedRect()))
						{
							GenDraw.DrawLineBetween(thing2.TrueCenter(), thing3.TrueCenter(), SimpleColor.Green, 0.2f);
						}
					}
				}
			}
		}
	}
}
