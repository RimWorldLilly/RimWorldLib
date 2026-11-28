using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002059 RID: 8281
	public class PlaceWorker_DrawLinesToDeathrestBuildings : PlaceWorker
	{
		// Token: 0x0600D118 RID: 53528 RVA: 0x004CE2E4 File Offset: 0x004CC4E4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			Room room = center.GetRoom(Find.CurrentMap);
			if (room == null)
			{
				return;
			}
			foreach (Region region in room.Regions)
			{
				foreach (Thing t in region.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
				{
					if (this.CanShowConnectionTo(def, t, center))
					{
						GenDraw.DrawLineBetween(center.ToVector3Shifted(), t.TrueCenter());
					}
				}
			}
		}

		// Token: 0x0600D119 RID: 53529 RVA: 0x004CE3A8 File Offset: 0x004CC5A8
		public override void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef bdef, IntVec3 center, Rot4 rot)
		{
			PlaceWorker_DrawLinesToDeathrestBuildings.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.curX = curX;
			if (ModsConfig.BiotechActive)
			{
				PlaceWorker_DrawLinesToDeathrestBuildings.tmpLinkedThings.Clear();
				Room room = center.GetRoom(Find.CurrentMap);
				if (room != null)
				{
					foreach (Region region in room.Regions)
					{
						foreach (Thing thing in region.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
						{
							if (this.CanShowConnectionTo((ThingDef)bdef, thing, center) && !PlaceWorker_DrawLinesToDeathrestBuildings.tmpLinkedThings.Contains(thing))
							{
								PlaceWorker_DrawLinesToDeathrestBuildings.tmpLinkedThings.Add(thing);
								if (PlaceWorker_DrawLinesToDeathrestBuildings.tmpLinkedThings.Count == 1)
								{
									PlaceWorker_DrawLinesToDeathrestBuildings.<DrawPlaceMouseAttachments>g__DrawTextLine|2_0(ref curY, "FacilityPotentiallyLinkedTo".Translate() + ":", ref CS$<>8__locals1);
								}
								PlaceWorker_DrawLinesToDeathrestBuildings.<DrawPlaceMouseAttachments>g__DrawTextLine|2_0(ref curY, "  - " + thing.LabelCap, ref CS$<>8__locals1);
							}
						}
					}
				}
			}
			base.DrawPlaceMouseAttachments(CS$<>8__locals1.curX, ref curY, bdef, center, rot);
		}

		// Token: 0x0600D11A RID: 53530 RVA: 0x004CE4F4 File Offset: 0x004CC6F4
		private bool CanShowConnectionTo(ThingDef def, Thing t, IntVec3 center)
		{
			if (t.def == def)
			{
				return false;
			}
			CompDeathrestBindable compDeathrestBindable = t.TryGetComp<CompDeathrestBindable>();
			return compDeathrestBindable != null && compDeathrestBindable.BoundPawn == null && GenSight.LineOfSight(center, t.OccupiedRect().CenterCell, Find.CurrentMap);
		}

		// Token: 0x0600D11D RID: 53533 RVA: 0x004CE554 File Offset: 0x004CC754
		[CompilerGenerated]
		internal static void <DrawPlaceMouseAttachments>g__DrawTextLine|2_0(ref float y, string text, ref PlaceWorker_DrawLinesToDeathrestBuildings.<>c__DisplayClass2_0 A_2)
		{
			float lineHeight = Text.LineHeight;
			Widgets.Label(new Rect(A_2.curX, y, 999f, lineHeight), text);
			y += lineHeight;
		}

		// Token: 0x040084D9 RID: 34009
		private static List<Thing> tmpLinkedThings = new List<Thing>();
	}
}
