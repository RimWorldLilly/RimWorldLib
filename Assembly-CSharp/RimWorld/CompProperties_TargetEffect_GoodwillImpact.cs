using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A12 RID: 6674
	public class CompProperties_TargetEffect_GoodwillImpact : CompProperties
	{
		// Token: 0x0600A7E1 RID: 42977 RVA: 0x003BBB50 File Offset: 0x003B9D50
		public CompProperties_TargetEffect_GoodwillImpact()
		{
			this.compClass = typeof(CompTargetEffect_GoodwillImpact);
		}

		// Token: 0x040062B6 RID: 25270
		public int goodwillImpact = -200;
	}
}
