using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D8 RID: 5080
	public class CompProperties_AbilityBloodfeederBite : CompProperties_AbilityEffect
	{
		// Token: 0x06007E67 RID: 32359 RVA: 0x002D2A0C File Offset: 0x002D0C0C
		public CompProperties_AbilityBloodfeederBite()
		{
			this.compClass = typeof(CompAbilityEffect_BloodfeederBite);
		}

		// Token: 0x06007E68 RID: 32360 RVA: 0x002D2A3A File Offset: 0x002D0C3A
		public override IEnumerable<string> ExtraStatSummary()
		{
			CompProperties_AbilityBloodfeederBite.<ExtraStatSummary>d__8 <ExtraStatSummary>d__ = new CompProperties_AbilityBloodfeederBite.<ExtraStatSummary>d__8(-2);
			<ExtraStatSummary>d__.<>4__this = this;
			return <ExtraStatSummary>d__;
		}

		// Token: 0x04004F55 RID: 20309
		public float hemogenGain;

		// Token: 0x04004F56 RID: 20310
		public ThoughtDef thoughtDefToGiveTarget;

		// Token: 0x04004F57 RID: 20311
		public ThoughtDef opinionThoughtDefToGiveTarget;

		// Token: 0x04004F58 RID: 20312
		public float resistanceGain;

		// Token: 0x04004F59 RID: 20313
		public float nutritionGain = 0.1f;

		// Token: 0x04004F5A RID: 20314
		public float targetBloodLoss = 0.4499f;

		// Token: 0x04004F5B RID: 20315
		public IntRange bloodFilthToSpawnRange;
	}
}
