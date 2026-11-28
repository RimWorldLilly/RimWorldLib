using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000649 RID: 1609
	public static class GhostDrawer
	{
		// Token: 0x0600329F RID: 12959 RVA: 0x00142EE0 File Offset: 0x001410E0
		public static void DrawGhostThing(IntVec3 center, Rot4 rot, ThingDef thingDef, Graphic baseGraphic, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null, bool drawPlaceWorkers = true, ThingDef stuff = null)
		{
			if (baseGraphic == null)
			{
				baseGraphic = thingDef.graphic;
			}
			Graphic graphic = GhostUtility.GhostGraphicFor(baseGraphic, thingDef, ghostCol, stuff);
			Vector3 loc = GenThing.TrueCenter(center, rot, thingDef.Size, drawAltitude.AltitudeFor());
			graphic.DrawFromDef(loc, rot, thingDef, 0f);
			for (int i = 0; i < thingDef.comps.Count; i++)
			{
				thingDef.comps[i].DrawGhost(center, rot, thingDef, ghostCol, drawAltitude, thing);
			}
			if (drawPlaceWorkers && thingDef.PlaceWorkers != null)
			{
				for (int j = 0; j < thingDef.PlaceWorkers.Count; j++)
				{
					thingDef.PlaceWorkers[j].DrawGhost(thingDef, center, rot, ghostCol, thing);
				}
			}
		}
	}
}
