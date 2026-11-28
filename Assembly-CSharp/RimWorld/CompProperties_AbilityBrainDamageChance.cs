using System;

namespace RimWorld
{
	// Token: 0x020013DC RID: 5084
	public class CompProperties_AbilityBrainDamageChance : CompProperties_AbilityEffect
	{
		// Token: 0x06007E75 RID: 32373 RVA: 0x002D2F6B File Offset: 0x002D116B
		public CompProperties_AbilityBrainDamageChance()
		{
			this.compClass = typeof(CompAbilityEffect_BrainDamageChance);
		}

		// Token: 0x04004F5D RID: 20317
		public float brainDamageChance = 0.3f;
	}
}
