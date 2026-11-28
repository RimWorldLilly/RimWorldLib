using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200143D RID: 5181
	public class CompProperties_ResurrectMech : CompProperties_AbilityEffect
	{
		// Token: 0x06007FA0 RID: 32672 RVA: 0x002D8E10 File Offset: 0x002D7010
		public CompProperties_ResurrectMech()
		{
			this.compClass = typeof(CompAbilityEffect_ResurrectMech);
		}

		// Token: 0x06007FA1 RID: 32673 RVA: 0x002D8E3E File Offset: 0x002D703E
		public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
		{
			CompProperties_ResurrectMech.<ConfigErrors>d__5 <ConfigErrors>d__ = new CompProperties_ResurrectMech.<ConfigErrors>d__5(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04004FDE RID: 20446
		public int maxCorpseAgeTicks = int.MaxValue;

		// Token: 0x04004FDF RID: 20447
		public List<MechChargeCosts> costs = new List<MechChargeCosts>();

		// Token: 0x04004FE0 RID: 20448
		public EffecterDef appliedEffecterDef;

		// Token: 0x04004FE1 RID: 20449
		public EffecterDef centerEffecterDef;
	}
}
