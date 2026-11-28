using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020BD RID: 8381
	public class SketchResolver_AncientMechGeneratorRoom : SketchResolver
	{
		// Token: 0x0600D31F RID: 54047 RVA: 0x004DAF24 File Offset: 0x004D9124
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0600D320 RID: 54048 RVA: 0x004DAF40 File Offset: 0x004D9140
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckBiotech("Ancient mechanitor generator room"))
			{
				return;
			}
			SketchResolveParams parms2 = parms;
			parms2.thingCentral = ThingDefOf.AncientToxifierGenerator;
			SketchResolverDefOf.AddThingsCentral.Resolve(parms2);
		}
	}
}
