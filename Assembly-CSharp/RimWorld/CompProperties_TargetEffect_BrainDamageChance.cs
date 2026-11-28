using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A0C RID: 6668
	public class CompProperties_TargetEffect_BrainDamageChance : CompProperties
	{
		// Token: 0x0600A7D5 RID: 42965 RVA: 0x003BB9DC File Offset: 0x003B9BDC
		public CompProperties_TargetEffect_BrainDamageChance()
		{
			this.compClass = typeof(CompTargetEffect_BrainDamageChance);
		}

		// Token: 0x040062B3 RID: 25267
		public float brainDamageChance = 0.3f;
	}
}
