using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002047 RID: 8263
	public class PlaceWorker_ShowFacilitiesConnections : PlaceWorker
	{
		// Token: 0x0600D0EB RID: 53483 RVA: 0x004CD7C0 File Offset: 0x004CB9C0
		public override void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef bdef, IntVec3 center, Rot4 rot)
		{
			ThingDef thingDef = bdef as ThingDef;
			if (thingDef != null)
			{
				Map currentMap = Find.CurrentMap;
				if (thingDef.HasComp(typeof(CompAffectedByFacilities)))
				{
					CompAffectedByFacilities.DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo(curX, ref curY, thingDef, center, rot, currentMap);
					return;
				}
				CompFacility.DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo(curX, ref curY, thingDef, center, rot, currentMap);
			}
		}

		// Token: 0x0600D0EC RID: 53484 RVA: 0x004CD80C File Offset: 0x004CBA0C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			if (def.HasComp(typeof(CompAffectedByFacilities)))
			{
				CompAffectedByFacilities.DrawLinesToPotentialThingsToLinkTo(def, center, rot, currentMap);
				return;
			}
			CompFacility.DrawLinesToPotentialThingsToLinkTo(def, center, rot, currentMap);
		}
	}
}
