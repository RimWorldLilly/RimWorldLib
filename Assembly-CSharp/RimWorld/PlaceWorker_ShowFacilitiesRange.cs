using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002048 RID: 8264
	public class PlaceWorker_ShowFacilitiesRange : PlaceWorker
	{
		// Token: 0x0600D0EE RID: 53486 RVA: 0x004CD84C File Offset: 0x004CBA4C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_Facility compProperties = def.GetCompProperties<CompProperties_Facility>();
			if (compProperties != null)
			{
				GenDraw.DrawRadiusRing(center, compProperties.maxDistance - 0.1f);
			}
		}
	}
}
