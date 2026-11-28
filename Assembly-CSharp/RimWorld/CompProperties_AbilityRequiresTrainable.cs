using System;

namespace RimWorld
{
	// Token: 0x02001438 RID: 5176
	public class CompProperties_AbilityRequiresTrainable : CompProperties_AbilityEffect
	{
		// Token: 0x06007F94 RID: 32660 RVA: 0x002D8C65 File Offset: 0x002D6E65
		public CompProperties_AbilityRequiresTrainable()
		{
			this.compClass = typeof(CompAbilityEffect_RequiresTrainable);
		}

		// Token: 0x04004FDA RID: 20442
		public TrainableDef trainableDef;

		// Token: 0x04004FDB RID: 20443
		public bool aiCanCastWithoutTrainable;
	}
}
