using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002061 RID: 8289
	public class PlaceWorker_RitualPosition : PlaceWorker
	{
		// Token: 0x0600D135 RID: 53557 RVA: 0x004CED77 File Offset: 0x004CCF77
		public static List<Thing> GetRitualFocusInRange(IntVec3 ritualPosition, Thing ritualPositionThing = null)
		{
			return Find.CurrentMap.listerBuildingWithTagInProximity.GetForCell(ritualPosition, 4.9f, "RitualFocus", ritualPositionThing);
		}

		// Token: 0x0600D136 RID: 53558 RVA: 0x004CED94 File Offset: 0x004CCF94
		public static IEnumerable<Thing> RitualSeatsInRange(ThingDef def, IntVec3 center, Rot4 rot, Thing thing = null)
		{
			PlaceWorker_RitualPosition.<RitualSeatsInRange>d__5 <RitualSeatsInRange>d__ = new PlaceWorker_RitualPosition.<RitualSeatsInRange>d__5(-2);
			<RitualSeatsInRange>d__.<>3__def = def;
			<RitualSeatsInRange>d__.<>3__center = center;
			<RitualSeatsInRange>d__.<>3__rot = rot;
			<RitualSeatsInRange>d__.<>3__thing = thing;
			return <RitualSeatsInRange>d__;
		}

		// Token: 0x0600D137 RID: 53559 RVA: 0x004CEDBC File Offset: 0x004CCFBC
		public static void DrawRitualSeatConnections(ThingDef def, IntVec3 center, Rot4 rot, Thing thing = null, List<Thing> except = null)
		{
			foreach (Thing thing2 in PlaceWorker_RitualPosition.RitualSeatsInRange(def, center, rot, thing))
			{
				if (except == null || !except.Contains(thing2))
				{
					GenDraw.DrawLineBetween(GenThing.TrueCenter(center, rot, def.size, AltitudeLayer.MetaOverlays.AltitudeFor()), thing2.TrueCenter(), SimpleColor.Yellow, 0.2f);
				}
			}
		}

		// Token: 0x0600D138 RID: 53560 RVA: 0x004CEE38 File Offset: 0x004CD038
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			List<Thing> ritualFocusInRange = PlaceWorker_RitualPosition.GetRitualFocusInRange(center, thing);
			for (int i = 0; i < ritualFocusInRange.Count; i++)
			{
				Thing thing2 = ritualFocusInRange[i];
				if (thing2.def != def)
				{
					GenDraw.DrawLineBetween(GenThing.TrueCenter(center, Rot4.North, def.size, def.Altitude), thing2.TrueCenter(), SimpleColor.Green, 0.2f);
				}
			}
			PlaceWorker_RitualPosition.DrawRitualSeatConnections(def, center, rot, thing, null);
		}

		// Token: 0x040084E0 RID: 34016
		public const float RitualFocusRange = 4.9f;

		// Token: 0x040084E1 RID: 34017
		public const SimpleColor RitualFocusConnectionColor = SimpleColor.Green;

		// Token: 0x040084E2 RID: 34018
		public const SimpleColor RitualSeatConnectionColor = SimpleColor.Yellow;

		// Token: 0x040084E3 RID: 34019
		private static List<Thing> tmpRitualSeatsInRoom = new List<Thing>();
	}
}
