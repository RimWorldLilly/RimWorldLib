using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200203E RID: 8254
	public class PlaceWorker_ShowCauseHediffAoE : PlaceWorker
	{
		// Token: 0x0600D0D4 RID: 53460 RVA: 0x004CCE38 File Offset: 0x004CB038
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_CauseHediff_AoE compProperties = def.GetCompProperties<CompProperties_CauseHediff_AoE>();
			if (compProperties != null)
			{
				GenDraw.DrawRadiusRing(center, compProperties.range, Color.white, null);
			}
		}
	}
}
