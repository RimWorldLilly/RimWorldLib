using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001415 RID: 5141
	public class CompProperties_AbilityGiveRandomHediff : CompProperties_AbilityEffect
	{
		// Token: 0x06007F2A RID: 32554 RVA: 0x002D70F7 File Offset: 0x002D52F7
		public CompProperties_AbilityGiveRandomHediff()
		{
			this.compClass = typeof(CompAbilityEffect_GiveRandomHediff);
		}

		// Token: 0x04004FB7 RID: 20407
		public List<HediffOption> options;

		// Token: 0x04004FB8 RID: 20408
		public bool allowDuplicates;
	}
}
