using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002069 RID: 8297
	public class PlaceWorker_CocoonTriggerRadius : PlaceWorker
	{
		// Token: 0x0600D149 RID: 53577 RVA: 0x004CF2C0 File Offset: 0x004CD4C0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_WakeUpDormant compProperties = def.GetCompProperties<CompProperties_WakeUpDormant>();
			if (compProperties == null)
			{
				return;
			}
			GenDraw.DrawRadiusRing(center, compProperties.wakeUpCheckRadius, Color.white, null);
		}
	}
}
