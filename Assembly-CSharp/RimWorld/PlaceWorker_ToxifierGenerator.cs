using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002065 RID: 8293
	public class PlaceWorker_ToxifierGenerator : PlaceWorker
	{
		// Token: 0x0600D141 RID: 53569 RVA: 0x004CF0B4 File Offset: 0x004CD2B4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_Toxifier compProperties = def.GetCompProperties<CompProperties_Toxifier>();
			if (compProperties != null)
			{
				GenDraw.DrawRadiusRing(center, compProperties.radius, Color.white, null);
			}
		}
	}
}
