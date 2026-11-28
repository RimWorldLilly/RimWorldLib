using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200206C RID: 8300
	public class PlaceWorker_NoiseSource : PlaceWorker
	{
		// Token: 0x0600D150 RID: 53584 RVA: 0x004CF4E4 File Offset: 0x004CD6E4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_NoiseSource compProperties = def.GetCompProperties<CompProperties_NoiseSource>();
			if (compProperties != null)
			{
				GenDraw.DrawRadiusRing(center, compProperties.radius, Color.white, null);
			}
		}
	}
}
