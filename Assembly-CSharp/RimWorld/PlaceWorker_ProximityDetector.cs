using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002070 RID: 8304
	public class PlaceWorker_ProximityDetector : PlaceWorker
	{
		// Token: 0x0600D15E RID: 53598 RVA: 0x004CF9C3 File Offset: 0x004CDBC3
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawRadiusRing(center, 19.9f, Color.white, null);
		}
	}
}
