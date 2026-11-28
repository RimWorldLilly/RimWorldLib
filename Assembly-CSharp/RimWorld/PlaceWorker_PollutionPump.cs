using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002067 RID: 8295
	public class PlaceWorker_PollutionPump : PlaceWorker
	{
		// Token: 0x0600D145 RID: 53573 RVA: 0x004CF1C4 File Offset: 0x004CD3C4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_PollutionPump compProperties = def.GetCompProperties<CompProperties_PollutionPump>();
			if (compProperties == null)
			{
				return;
			}
			GenDraw.DrawRadiusRing(center, compProperties.radius, Color.white, null);
		}
	}
}
