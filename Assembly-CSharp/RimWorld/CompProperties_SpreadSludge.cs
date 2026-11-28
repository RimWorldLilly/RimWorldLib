using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001995 RID: 6549
	public class CompProperties_SpreadSludge : CompProperties
	{
		// Token: 0x0600A468 RID: 42088 RVA: 0x003ABEA8 File Offset: 0x003AA0A8
		public CompProperties_SpreadSludge()
		{
			this.compClass = typeof(CompSpreadSludge);
		}

		// Token: 0x04006130 RID: 24880
		public int mtbTicks = 2500;

		// Token: 0x04006131 RID: 24881
		public AbilityDef abilityDef;

		// Token: 0x04006132 RID: 24882
		public bool tryAvoidSludge;
	}
}
