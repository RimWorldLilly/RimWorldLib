using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002071 RID: 8305
	public class PlaceWorker_GlowRadius : PlaceWorker
	{
		// Token: 0x0600D160 RID: 53600 RVA: 0x004CF9E0 File Offset: 0x004CDBE0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (thing != null)
			{
				return;
			}
			CompProperties_Glower compProperties = def.GetCompProperties<CompProperties_Glower>();
			if (compProperties != null)
			{
				float num = compProperties.glowRadius + -2.1f;
				if (num > 0f)
				{
					GenDraw.DrawRadiusRing(center, num, PlaceWorker_GlowRadius.RingColor, null);
				}
			}
		}

		// Token: 0x040084E8 RID: 34024
		private static readonly Color RingColor = new Color(0.8f, 0.8f, 0.4f);

		// Token: 0x040084E9 RID: 34025
		private const float RadiusOffset = -2.1f;
	}
}
