using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020B7 RID: 8375
	public class SketchResolver_AncientBandNodeRoom : SketchResolver
	{
		// Token: 0x0600D309 RID: 54025 RVA: 0x004DA56A File Offset: 0x004D876A
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0600D30A RID: 54026 RVA: 0x004DA588 File Offset: 0x004D8788
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckBiotech("Ancient band node room"))
			{
				return;
			}
			SketchResolveParams parms2 = parms;
			parms2.wallEdgeThing = ThingDefOf.AncientBandNode;
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms2);
		}
	}
}
