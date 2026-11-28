using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001417 RID: 5143
	public class CompProperties_AbilityHemogenCost : CompProperties_AbilityEffect
	{
		// Token: 0x06007F31 RID: 32561 RVA: 0x002D733B File Offset: 0x002D553B
		public CompProperties_AbilityHemogenCost()
		{
			this.compClass = typeof(CompAbilityEffect_HemogenCost);
		}

		// Token: 0x06007F32 RID: 32562 RVA: 0x002D7353 File Offset: 0x002D5553
		public override IEnumerable<string> ExtraStatSummary()
		{
			CompProperties_AbilityHemogenCost.<ExtraStatSummary>d__2 <ExtraStatSummary>d__ = new CompProperties_AbilityHemogenCost.<ExtraStatSummary>d__2(-2);
			<ExtraStatSummary>d__.<>4__this = this;
			return <ExtraStatSummary>d__;
		}

		// Token: 0x04004FB9 RID: 20409
		public float hemogenCost;
	}
}
