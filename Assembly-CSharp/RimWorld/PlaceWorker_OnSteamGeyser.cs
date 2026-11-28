using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002033 RID: 8243
	public class PlaceWorker_OnSteamGeyser : PlaceWorker
	{
		// Token: 0x0600D0BA RID: 53434 RVA: 0x004CC95C File Offset: 0x004CAB5C
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			Thing thing2 = map.thingGrid.ThingAt(loc, ThingDefOf.SteamGeyser);
			if (thing2 == null || thing2.Position != loc)
			{
				return "MustPlaceOnSteamGeyser".Translate();
			}
			return true;
		}

		// Token: 0x0600D0BB RID: 53435 RVA: 0x004CC9A3 File Offset: 0x004CABA3
		public override bool ForceAllowPlaceOver(BuildableDef otherDef)
		{
			return otherDef == ThingDefOf.SteamGeyser;
		}

		// Token: 0x0600D0BC RID: 53436 RVA: 0x004CC9B0 File Offset: 0x004CABB0
		public override void DrawMouseAttachments(BuildableDef def)
		{
			List<Thing> list = Find.CurrentMap.listerThings.ThingsOfDef(ThingDefOf.SteamGeyser);
			for (int i = 0; i < list.Count; i++)
			{
				TargetHighlighter.Highlight(list[i], true, true, false);
			}
		}
	}
}
