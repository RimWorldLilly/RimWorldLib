using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002058 RID: 8280
	public class PlaceWorker_MeditationOffsetBuildingsNear : PlaceWorker
	{
		// Token: 0x0600D115 RID: 53525 RVA: 0x004CE1DC File Offset: 0x004CC3DC
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return;
			}
			FocusStrengthOffset_BuildingDefs focusStrengthOffset_BuildingDefs = ((CompProperties_MeditationFocus)((def.IsFrame || def.IsBlueprint) ? ((ThingDef)def.entityDefToBuild).CompDefFor<CompMeditationFocus>() : def.CompDefFor<CompMeditationFocus>())).offsets.OfType<FocusStrengthOffset_BuildingDefs>().FirstOrDefault<FocusStrengthOffset_BuildingDefs>();
			if (focusStrengthOffset_BuildingDefs != null)
			{
				if (focusStrengthOffset_BuildingDefs.drawRingRadius)
				{
					GenDraw.DrawRadiusRing(center, focusStrengthOffset_BuildingDefs.radius, PlaceWorker_MeditationOffsetBuildingsNear.RingColor, null);
				}
				List<Thing> forCell = Find.CurrentMap.listerBuldingOfDefInProximity.GetForCell(center, focusStrengthOffset_BuildingDefs.radius, focusStrengthOffset_BuildingDefs.defs, null);
				int num = 0;
				while (num < forCell.Count && num < focusStrengthOffset_BuildingDefs.maxBuildings)
				{
					if (thing != forCell[num])
					{
						GenDraw.DrawLineBetween(GenThing.TrueCenter(center, Rot4.North, def.size, def.Altitude), forCell[num].TrueCenter(), SimpleColor.Green, 0.2f);
					}
					num++;
				}
			}
		}

		// Token: 0x040084D8 RID: 34008
		public static readonly Color RingColor = new Color(0.5f, 0.8f, 0.5f);
	}
}
