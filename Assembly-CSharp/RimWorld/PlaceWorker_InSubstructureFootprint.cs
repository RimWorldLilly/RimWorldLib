using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002072 RID: 8306
	public class PlaceWorker_InSubstructureFootprint : PlaceWorker
	{
		// Token: 0x0600D163 RID: 53603 RVA: 0x004CFA41 File Offset: 0x004CDC41
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			SubstructureGrid.DrawSubstructureFootprint(null, null);
		}

		// Token: 0x0600D164 RID: 53604 RVA: 0x004CFA4A File Offset: 0x004CDC4A
		public override void DrawOnGUIExtra(BuildableDef def)
		{
			SubstructureGrid.DrawSubstructureCountOnGUI();
		}

		// Token: 0x0600D165 RID: 53605 RVA: 0x004CFA54 File Offset: 0x004CDC54
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			RoofDef roofDef = map.roofGrid.RoofAt(loc);
			if (roofDef != null && roofDef.isNatural)
			{
				return "MessageCannotPlaceUnderRockRoof".Translate();
			}
			if (!GravshipUtility.InsideFootprint(loc, map, null, null))
			{
				return "MessageMustBePlacedInRangeOfGravEngineOrExtender".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600D166 RID: 53606 RVA: 0x004CFAAC File Offset: 0x004CDCAC
		public override void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef bdef, IntVec3 center, Rot4 rot)
		{
			if (!center.InBounds(Find.CurrentMap) || center.Fogged(Find.CurrentMap))
			{
				return;
			}
			GUI.color = ColorLibrary.RedReadable;
			RoofDef roofDef = Find.CurrentMap.roofGrid.RoofAt(center);
			if (roofDef != null && roofDef.isNatural)
			{
				PlaceWorker.DrawTextLine(curX, ref curY, "MessageCannotPlaceUnderRockRoof".Translate());
			}
			if (!GravshipUtility.InsideFootprint(center, Find.CurrentMap, null, null))
			{
				PlaceWorker.DrawTextLine(curX, ref curY, "MessageMustBePlacedInRangeOfGravEngineOrExtender".Translate());
			}
			GUI.color = Color.white;
		}

		// Token: 0x0600D167 RID: 53607 RVA: 0x004CFB44 File Offset: 0x004CDD44
		public override bool ForceAllowPlaceOver(BuildableDef otherDef)
		{
			ThingDef thingDef = otherDef as ThingDef;
			return (thingDef != null && thingDef.Fillage == FillCategory.Full) || base.ForceAllowPlaceOver(otherDef);
		}
	}
}
