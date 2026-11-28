using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002075 RID: 8309
	public class PlaceWorker_InRangeOfGravEngine : PlaceWorker
	{
		// Token: 0x0600D16F RID: 53615 RVA: 0x004CFD20 File Offset: 0x004CDF20
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			CompProperties_GravshipFacility compProperties = def.GetCompProperties<CompProperties_GravshipFacility>();
			foreach (Thing thing2 in currentMap.listerThings.ThingsOfDef(ThingDefOf.GravEngine))
			{
				SimpleColor color;
				if (center.InHorDistOf(thing2.Position, compProperties.maxDistance))
				{
					color = SimpleColor.Green;
				}
				else
				{
					color = SimpleColor.Red;
				}
				GenDraw.DrawLineBetween(center.ToVector3Shifted(), thing2.TrueCenter(), color, 0.2f);
			}
		}

		// Token: 0x0600D170 RID: 53616 RVA: 0x004CFDB4 File Offset: 0x004CDFB4
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			ThingDef thingDef = checkingDef as ThingDef;
			if (thingDef == null)
			{
				return AcceptanceReport.WasRejected;
			}
			CompProperties_GravshipFacility compProperties = thingDef.GetCompProperties<CompProperties_GravshipFacility>();
			foreach (Thing thing2 in map.listerThings.ThingsOfDef(ThingDefOf.GravEngine))
			{
				if (loc.InHorDistOf(thing2.Position, compProperties.maxDistance))
				{
					return AcceptanceReport.WasAccepted;
				}
			}
			return "MessageMustBePlacedInRangeOfGravEngine".Translate();
		}
	}
}
