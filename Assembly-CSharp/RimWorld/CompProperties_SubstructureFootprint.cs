using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A9 RID: 6569
	public class CompProperties_SubstructureFootprint : CompProperties
	{
		// Token: 0x0600A4EE RID: 42222 RVA: 0x003AE885 File Offset: 0x003ACA85
		public CompProperties_SubstructureFootprint()
		{
			this.compClass = typeof(CompSubstructureFootprint);
		}

		// Token: 0x0600A4EF RID: 42223 RVA: 0x003AE8A4 File Offset: 0x003ACAA4
		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
			SubstructureGrid.DrawSubstructureFootprintWithExtra(this, center, thing);
		}

		// Token: 0x04006175 RID: 24949
		public float radius;

		// Token: 0x04006176 RID: 24950
		public bool displaySubstructureOverlayWhenSelected = true;
	}
}
