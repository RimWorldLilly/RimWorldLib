using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020B6 RID: 8374
	public class SketchResolver_AncientArmory : SketchResolver
	{
		// Token: 0x0600D306 RID: 54022 RVA: 0x004DA4DF File Offset: 0x004D86DF
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0600D307 RID: 54023 RVA: 0x004DA4FC File Offset: 0x004D86FC
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient armory"))
			{
				return;
			}
			SketchResolveParams parms2 = parms;
			parms2.wallEdgeThing = ThingDefOf.AncientLockerBank;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms2);
			SketchResolveParams parms3 = parms;
			parms3.thingCentral = ThingDefOf.AncientSecurityTurret;
			parms3.requireFloor = new bool?(true);
			SketchResolverDefOf.AddThingsCentral.Resolve(parms3);
		}
	}
}
