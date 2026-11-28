using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200184A RID: 6218
	public class CompProperties_CauseHediff_Apparel : CompProperties
	{
		// Token: 0x06009BA7 RID: 39847 RVA: 0x00381BDC File Offset: 0x0037FDDC
		public CompProperties_CauseHediff_Apparel()
		{
			this.compClass = typeof(CompCauseHediff_Apparel);
		}

		// Token: 0x04005C92 RID: 23698
		public HediffDef hediff;

		// Token: 0x04005C93 RID: 23699
		public BodyPartDef part;
	}
}
