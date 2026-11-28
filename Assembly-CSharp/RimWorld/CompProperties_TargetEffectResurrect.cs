using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A1E RID: 6686
	public class CompProperties_TargetEffectResurrect : CompProperties
	{
		// Token: 0x0600A7FB RID: 43003 RVA: 0x003BBF3F File Offset: 0x003BA13F
		public CompProperties_TargetEffectResurrect()
		{
			this.compClass = typeof(CompTargetEffect_Resurrect);
		}

		// Token: 0x040062BF RID: 25279
		public ThingDef moteDef;

		// Token: 0x040062C0 RID: 25280
		public bool withSideEffects = true;

		// Token: 0x040062C1 RID: 25281
		public HediffDef addsHediff;
	}
}
