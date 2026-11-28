using System;

namespace RimWorld
{
	// Token: 0x02001446 RID: 5190
	public class CompProperties_AbilitySocialInteraction : CompProperties_AbilityEffect
	{
		// Token: 0x06007FCA RID: 32714 RVA: 0x002D9832 File Offset: 0x002D7A32
		public CompProperties_AbilitySocialInteraction()
		{
			this.compClass = typeof(CompAbilityEffect_SocialInteraction);
		}

		// Token: 0x04004FEC RID: 20460
		public InteractionDef interactionDef;

		// Token: 0x04004FED RID: 20461
		public bool canApplyToMentallyBroken;

		// Token: 0x04004FEE RID: 20462
		public bool canApplyToUnconscious;

		// Token: 0x04004FEF RID: 20463
		public bool canApplyToAsleep;
	}
}
