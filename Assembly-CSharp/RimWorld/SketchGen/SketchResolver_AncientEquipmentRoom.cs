using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020B9 RID: 8377
	public class SketchResolver_AncientEquipmentRoom : SketchResolver
	{
		// Token: 0x170025DF RID: 9695
		// (get) Token: 0x0600D311 RID: 54033 RVA: 0x004DAA30 File Offset: 0x004D8C30
		private static IEnumerable<ThingDef> CentralThings
		{
			get
			{
				return new SketchResolver_AncientEquipmentRoom.<get_CentralThings>d__1(-2);
			}
		}

		// Token: 0x170025E0 RID: 9696
		// (get) Token: 0x0600D312 RID: 54034 RVA: 0x004DAA39 File Offset: 0x004D8C39
		private static IEnumerable<ThingDef> EdgeThings
		{
			get
			{
				return new SketchResolver_AncientEquipmentRoom.<get_EdgeThings>d__3(-2);
			}
		}

		// Token: 0x0600D313 RID: 54035 RVA: 0x004DAA42 File Offset: 0x004D8C42
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0600D314 RID: 54036 RVA: 0x004DAA60 File Offset: 0x004D8C60
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient equipment room"))
			{
				return;
			}
			SketchResolveParams parms2 = parms;
			parms2.cornerThing = ThingDefOf.AncientLamp;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddCornerThings.Resolve(parms2);
			foreach (ThingDef thingCentral in SketchResolver_AncientEquipmentRoom.CentralThings)
			{
				SketchResolveParams parms3 = parms;
				parms3.thingCentral = thingCentral;
				parms3.requireFloor = new bool?(true);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms3);
			}
			foreach (ThingDef wallEdgeThing in SketchResolver_AncientEquipmentRoom.EdgeThings)
			{
				SketchResolveParams parms4 = parms;
				parms4.wallEdgeThing = wallEdgeThing;
				parms4.requireFloor = new bool?(true);
				SketchResolverDefOf.AddWallEdgeThings.Resolve(parms4);
			}
		}
	}
}
