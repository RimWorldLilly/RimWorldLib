using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200204C RID: 8268
	public class PlaceWorker_ShowExplosionRadius : PlaceWorker
	{
		// Token: 0x0600D0F6 RID: 53494 RVA: 0x004CD914 File Offset: 0x004CBB14
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_Explosive compProperties = def.GetCompProperties<CompProperties_Explosive>();
			if (compProperties == null)
			{
				return;
			}
			GenDraw.DrawRadiusRing(center, compProperties.explosiveRadius);
		}
	}
}
