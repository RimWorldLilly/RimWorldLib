using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200204B RID: 8267
	public class PlaceWorker_ShowPlantHarmRadius : PlaceWorker
	{
		// Token: 0x0600D0F4 RID: 53492 RVA: 0x004CD8D8 File Offset: 0x004CBAD8
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (thing == null)
			{
				return;
			}
			CompPlantHarmRadius compPlantHarmRadius = thing.TryGetComp<CompPlantHarmRadius>();
			if (compPlantHarmRadius == null)
			{
				return;
			}
			float currentRadius = compPlantHarmRadius.CurrentRadius;
			if (currentRadius < 50f)
			{
				GenDraw.DrawRadiusRing(center, currentRadius);
			}
		}
	}
}
