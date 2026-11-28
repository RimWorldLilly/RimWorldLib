using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200206D RID: 8301
	public class PlaceWorker_DrawLinesToBookcasesInRoom : PlaceWorker
	{
		// Token: 0x0600D152 RID: 53586 RVA: 0x004CF518 File Offset: 0x004CD718
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Room room = center.GetRoom(Find.CurrentMap);
			if (room == null || !room.ProperRoom || room.PsychologicallyOutdoors)
			{
				return;
			}
			room.DrawFieldEdges();
			foreach (Region region in room.Regions)
			{
				foreach (Thing thing2 in region.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
				{
					if (thing2 is Building_Bookcase)
					{
						GenDraw.DrawLineBetween(center.ToVector3Shifted(), thing2.TrueCenter());
					}
				}
			}
		}

		// Token: 0x0600D153 RID: 53587 RVA: 0x004CF5E4 File Offset: 0x004CD7E4
		public override void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef bdef, IntVec3 center, Rot4 rot)
		{
			PlaceWorker_DrawLinesToBookcasesInRoom.tmpLinkedThings.Clear();
			Room room = center.GetRoom(Find.CurrentMap);
			if (room != null)
			{
				foreach (Region region in room.Regions)
				{
					foreach (Thing thing in region.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
					{
						if (!PlaceWorker_DrawLinesToBookcasesInRoom.tmpLinkedThings.Contains(thing) && thing is Building_Bookcase)
						{
							PlaceWorker_DrawLinesToBookcasesInRoom.tmpLinkedThings.Add(thing);
							if (PlaceWorker_DrawLinesToBookcasesInRoom.tmpLinkedThings.Count == 1)
							{
								PlaceWorker.DrawTextLine(curX, ref curY, "FacilityPotentiallyLinkedTo".Translate() + ":");
							}
							PlaceWorker.DrawTextLine(curX, ref curY, "  - " + thing.LabelCap);
						}
					}
				}
			}
			base.DrawPlaceMouseAttachments(curX, ref curY, bdef, center, rot);
		}

		// Token: 0x040084E5 RID: 34021
		private static readonly List<Thing> tmpLinkedThings = new List<Thing>();
	}
}
