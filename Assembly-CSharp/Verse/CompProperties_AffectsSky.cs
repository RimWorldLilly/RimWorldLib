using System;

namespace Verse
{
	// Token: 0x02000136 RID: 310
	public class CompProperties_AffectsSky : CompProperties
	{
		// Token: 0x060009F4 RID: 2548 RVA: 0x00037048 File Offset: 0x00035248
		public CompProperties_AffectsSky()
		{
			this.compClass = typeof(CompAffectsSky);
		}

		// Token: 0x04000595 RID: 1429
		public float glow = 1f;

		// Token: 0x04000596 RID: 1430
		public SkyColorSet skyColors;

		// Token: 0x04000597 RID: 1431
		public float lightsourceShineSize = 1f;

		// Token: 0x04000598 RID: 1432
		public float lightsourceShineIntensity = 1f;

		// Token: 0x04000599 RID: 1433
		public bool lerpDarken;
	}
}
