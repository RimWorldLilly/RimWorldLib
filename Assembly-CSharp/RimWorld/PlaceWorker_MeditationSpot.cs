using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002055 RID: 8277
	public class PlaceWorker_MeditationSpot : PlaceWorker
	{
		// Token: 0x0600D10F RID: 53519 RVA: 0x004CE130 File Offset: 0x004CC330
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			MeditationUtility.DrawMeditationSpotOverlay(center, Find.CurrentMap);
		}
	}
}
