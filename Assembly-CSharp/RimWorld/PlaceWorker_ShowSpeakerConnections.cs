using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200203D RID: 8253
	public class PlaceWorker_ShowSpeakerConnections : PlaceWorker
	{
		// Token: 0x0600D0D1 RID: 53457 RVA: 0x004CCD8D File Offset: 0x004CAF8D
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			PlaceWorker_ShowSpeakerConnections.DrawConnections((ThingDef)checkingDef, loc, map);
			return true;
		}

		// Token: 0x0600D0D2 RID: 53458 RVA: 0x004CCDA4 File Offset: 0x004CAFA4
		public static void DrawConnections(ThingDef checker, IntVec3 loc, Map map)
		{
			ThingDef thingDef = (checker == ThingDefOf.LightBall) ? ThingDefOf.Loudspeaker : ThingDefOf.LightBall;
			int num = GenRadial.NumCellsInRadius(12f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = loc + GenRadial.RadialPattern[i];
				Thing firstThing = c.GetFirstThing(map, thingDef);
				if (firstThing != null && firstThing.def == thingDef)
				{
					GenDraw.DrawLineBetween(loc.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays.AltitudeFor()), c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays.AltitudeFor()), SimpleColor.Green, 0.2f);
				}
			}
		}
	}
}
