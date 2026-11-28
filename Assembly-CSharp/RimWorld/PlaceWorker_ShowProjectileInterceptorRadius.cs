using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002053 RID: 8275
	public class PlaceWorker_ShowProjectileInterceptorRadius : PlaceWorker
	{
		// Token: 0x0600D10B RID: 53515 RVA: 0x004CE0C4 File Offset: 0x004CC2C4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_ProjectileInterceptor compProperties = def.GetCompProperties<CompProperties_ProjectileInterceptor>();
			if (compProperties != null)
			{
				GenDraw.DrawCircleOutline(center.ToVector3Shifted(), compProperties.radius);
			}
		}
	}
}
