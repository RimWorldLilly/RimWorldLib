using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020BB RID: 8379
	public class SketchResolver_AncientLab : SketchResolver
	{
		// Token: 0x0600D319 RID: 54041 RVA: 0x004DACEC File Offset: 0x004D8EEC
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0600D31A RID: 54042 RVA: 0x004DAD08 File Offset: 0x004D8F08
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient lab"))
			{
				return;
			}
			SketchResolveParams parms2 = parms;
			parms2.thingCentral = ThingDefOf.AncientOperatingTable;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddThingsCentral.Resolve(parms2);
			SketchResolveParams parms3 = parms;
			parms3.wallEdgeThing = ThingDefOf.AncientDisplayBank;
			parms3.requireFloor = new bool?(true);
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms3);
			foreach (IntVec3 pos in parms.rect.Value.Cells)
			{
				parms.sketch.AddTerrain(TerrainDefOf.AncientTile, pos, true);
			}
		}
	}
}
