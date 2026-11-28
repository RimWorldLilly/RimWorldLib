using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC7 RID: 3783
	public class CompProperties_DarklightOverlay : CompProperties_FireOverlay
	{
		// Token: 0x06006365 RID: 25445 RVA: 0x0023B40C File Offset: 0x0023960C
		public CompProperties_DarklightOverlay()
		{
			this.compClass = typeof(CompDarklightOverlay);
		}

		// Token: 0x06006366 RID: 25446 RVA: 0x0023B424 File Offset: 0x00239624
		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
			GhostUtility.GhostGraphicFor(CompDarklightOverlay.DarklightGraphic, thingDef, ghostCol, null).DrawFromDef(center.ToVector3ShiftedWithAltitude(drawAltitude), rot, thingDef, 0f);
		}
	}
}
