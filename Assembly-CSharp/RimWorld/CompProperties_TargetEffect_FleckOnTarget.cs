using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A10 RID: 6672
	public class CompProperties_TargetEffect_FleckOnTarget : CompProperties
	{
		// Token: 0x0600A7DD RID: 42973 RVA: 0x003BBAF4 File Offset: 0x003B9CF4
		public CompProperties_TargetEffect_FleckOnTarget()
		{
			this.compClass = typeof(ComTargetEffect_FleckOnTarget);
		}

		// Token: 0x040062B5 RID: 25269
		public FleckDef fleckDef;
	}
}
