using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F3 RID: 5107
	public class CompProperties_AbilityEffecterOnCaster : CompProperties_AbilityEffect
	{
		// Token: 0x06007EC0 RID: 32448 RVA: 0x002D4D61 File Offset: 0x002D2F61
		public CompProperties_AbilityEffecterOnCaster()
		{
			this.compClass = typeof(CompAbilityEffect_EffecterOnCaster);
		}

		// Token: 0x04004F79 RID: 20345
		public EffecterDef effecterDef;

		// Token: 0x04004F7A RID: 20346
		public float scale = 1f;

		// Token: 0x04004F7B RID: 20347
		public int maintainTicks;
	}
}
