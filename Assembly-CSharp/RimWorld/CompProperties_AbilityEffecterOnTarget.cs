using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F5 RID: 5109
	public class CompProperties_AbilityEffecterOnTarget : CompProperties_AbilityEffect
	{
		// Token: 0x06007EC4 RID: 32452 RVA: 0x002D4E25 File Offset: 0x002D3025
		public CompProperties_AbilityEffecterOnTarget()
		{
			this.compClass = typeof(CompAbilityEffect_EffecterOnTarget);
		}

		// Token: 0x04004F7C RID: 20348
		public EffecterDef effecterDef;

		// Token: 0x04004F7D RID: 20349
		public int maintainForTicks = -1;

		// Token: 0x04004F7E RID: 20350
		public float scale = 1f;
	}
}
