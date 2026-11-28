using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020D6 RID: 8406
	public class SketchResolver_MonumentRuin : SketchResolver
	{
		// Token: 0x0600D38E RID: 54158 RVA: 0x004E0871 File Offset: 0x004DEA71
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return true;
		}

		// Token: 0x0600D38F RID: 54159 RVA: 0x004E0874 File Offset: 0x004DEA74
		protected override void ResolveInt(SketchResolveParams parms)
		{
			SketchResolveParams sketchResolveParams = parms;
			sketchResolveParams.allowWood = new bool?(parms.allowWood.GetValueOrDefault());
			if (sketchResolveParams.allowedMonumentThings == null)
			{
				sketchResolveParams.allowedMonumentThings = new ThingFilter();
				sketchResolveParams.allowedMonumentThings.SetAllowAll(null, true);
			}
			if (ModsConfig.RoyaltyActive)
			{
				sketchResolveParams.allowedMonumentThings.SetAllow(ThingDefOf.Drape, false);
			}
			SketchResolverDefOf.Monument.Resolve(sketchResolveParams);
			SketchResolverDefOf.DamageBuildings.Resolve(parms);
		}
	}
}
