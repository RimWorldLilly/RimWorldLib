using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020C0 RID: 8384
	public class SketchResolver_AncientRechargeRoom : SketchResolver
	{
		// Token: 0x170025E2 RID: 9698
		// (get) Token: 0x0600D329 RID: 54057 RVA: 0x004DB278 File Offset: 0x004D9478
		private static IEnumerable<ThingDef> CentralThings
		{
			get
			{
				return new SketchResolver_AncientRechargeRoom.<get_CentralThings>d__1(-2);
			}
		}

		// Token: 0x0600D32A RID: 54058 RVA: 0x004DB281 File Offset: 0x004D9481
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0600D32B RID: 54059 RVA: 0x004DB29C File Offset: 0x004D949C
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckBiotech("Ancient recharger room"))
			{
				return;
			}
			SketchResolveParams parms2 = parms;
			parms2.cornerThing = ThingDefOf.AncientLamp;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddCornerThings.Resolve(parms2);
			foreach (ThingDef thingCentral in SketchResolver_AncientRechargeRoom.CentralThings)
			{
				SketchResolveParams parms3 = parms;
				parms3.thingCentral = thingCentral;
				parms3.requireFloor = new bool?(true);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms3);
			}
			SketchResolveParams parms4 = parms;
			parms4.wallEdgeThing = ThingDefOf.Table1x2c;
			parms4.allowWood = new bool?(false);
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms4);
		}
	}
}
