using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E4 RID: 5092
	public class CompProperties_AbilityConvert : CompProperties_AbilityEffect
	{
		// Token: 0x06007E92 RID: 32402 RVA: 0x002D3811 File Offset: 0x002D1A11
		public CompProperties_AbilityConvert()
		{
			this.compClass = typeof(CompAbilityEffect_Convert);
		}

		// Token: 0x06007E93 RID: 32403 RVA: 0x002D3834 File Offset: 0x002D1A34
		public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
		{
			CompProperties_AbilityConvert.<ConfigErrors>d__6 <ConfigErrors>d__ = new CompProperties_AbilityConvert.<ConfigErrors>d__6(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04004F65 RID: 20325
		[MustTranslate]
		public string successMessage;

		// Token: 0x04004F66 RID: 20326
		[MustTranslate]
		public string failMessage;

		// Token: 0x04004F67 RID: 20327
		public ThoughtDef failedThoughtInitiator;

		// Token: 0x04004F68 RID: 20328
		public ThoughtDef failedThoughtRecipient;

		// Token: 0x04004F69 RID: 20329
		public float convertPowerFactor = -1f;
	}
}
