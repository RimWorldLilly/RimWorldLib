using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A2E RID: 6702
	public class CompProperties_UseEffectAddHediff : CompProperties_UseEffect
	{
		// Token: 0x0600A840 RID: 43072 RVA: 0x003BCBF3 File Offset: 0x003BADF3
		public CompProperties_UseEffectAddHediff()
		{
			this.compClass = typeof(CompUseEffect_AddHediff);
		}

		// Token: 0x040062DC RID: 25308
		public HediffDef hediffDef;

		// Token: 0x040062DD RID: 25309
		public bool allowRepeatedUse;
	}
}
