using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002057 RID: 8279
	public class PlaceWorker_ArtificialBuildingsNear : PlaceWorker
	{
		// Token: 0x0600D113 RID: 53523 RVA: 0x004CE194 File Offset: 0x004CC394
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			FocusStrengthOffset_ArtificialBuildings focusStrengthOffset_ArtificialBuildings = ((CompProperties_MeditationFocus)def.CompDefFor<CompMeditationFocus>()).offsets.OfType<FocusStrengthOffset_ArtificialBuildings>().FirstOrDefault<FocusStrengthOffset_ArtificialBuildings>();
			if (focusStrengthOffset_ArtificialBuildings != null)
			{
				MeditationUtility.DrawArtificialBuildingOverlay(center, def, Find.CurrentMap, focusStrengthOffset_ArtificialBuildings.radius);
			}
		}
	}
}
