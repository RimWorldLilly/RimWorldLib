using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A1A RID: 6682
	public class CompProperties_TargetEffect_MoteOnTarget : CompProperties
	{
		// Token: 0x0600A7F1 RID: 42993 RVA: 0x003BBD9C File Offset: 0x003B9F9C
		public CompProperties_TargetEffect_MoteOnTarget()
		{
			this.compClass = typeof(ComTargetEffect_MoteOnTarget);
		}

		// Token: 0x040062BD RID: 25277
		public ThingDef moteDef;
	}
}
