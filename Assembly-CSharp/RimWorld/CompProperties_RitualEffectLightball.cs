using System;

namespace RimWorld
{
	// Token: 0x02001742 RID: 5954
	public class CompProperties_RitualEffectLightball : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x06009442 RID: 37954 RVA: 0x0036096C File Offset: 0x0035EB6C
		public CompProperties_RitualEffectLightball()
		{
			this.compClass = typeof(CompRitualEffect_Lightball);
		}

		// Token: 0x0400598A RID: 22922
		public float radius;
	}
}
