using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200206E RID: 8302
	public class PlaceWorker_DrawLinesToResearchTablesInRoom : PlaceWorker
	{
		// Token: 0x0600D156 RID: 53590 RVA: 0x004CF718 File Offset: 0x004CD918
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
					if (thing2 is Building_ResearchBench)
					{
						GenDraw.DrawLineBetween(center.ToVector3Shifted(), thing2.TrueCenter());
					}
				}
			}
		}

		// Token: 0x0600D157 RID: 53591 RVA: 0x004CF7E4 File Offset: 0x004CD9E4
		public override void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef bdef, IntVec3 center, Rot4 rot)
		{
			PlaceWorker_DrawLinesToResearchTablesInRoom.tmpLinkedThings.Clear();
			Room room = center.GetRoom(Find.CurrentMap);
			if (room != null)
			{
				foreach (Region region in room.Regions)
				{
					foreach (Thing thing in region.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
					{
						if (!PlaceWorker_DrawLinesToResearchTablesInRoom.tmpLinkedThings.Contains(thing) && thing is Building_ResearchBench)
						{
							PlaceWorker_DrawLinesToResearchTablesInRoom.tmpLinkedThings.Add(thing);
							if (PlaceWorker_DrawLinesToResearchTablesInRoom.tmpLinkedThings.Count == 1)
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

		// Token: 0x040084E6 RID: 34022
		private static readonly List<Thing> tmpLinkedThings = new List<Thing>();
	}
}
