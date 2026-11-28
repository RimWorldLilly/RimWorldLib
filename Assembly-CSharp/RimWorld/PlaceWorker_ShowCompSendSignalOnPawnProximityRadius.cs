using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200204A RID: 8266
	public class PlaceWorker_ShowCompSendSignalOnPawnProximityRadius : PlaceWorker
	{
		// Token: 0x0600D0F2 RID: 53490 RVA: 0x004CD8AC File Offset: 0x004CBAAC
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_SendSignalOnMotion compProperties = def.GetCompProperties<CompProperties_SendSignalOnMotion>();
			if (compProperties == null)
			{
				return;
			}
			GenDraw.DrawRadiusRing(center, compProperties.radius);
		}
	}
}
