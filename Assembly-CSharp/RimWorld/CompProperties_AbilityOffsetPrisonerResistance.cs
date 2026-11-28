using System;

namespace RimWorld
{
	// Token: 0x02001423 RID: 5155
	public class CompProperties_AbilityOffsetPrisonerResistance : CompProperties_AbilityEffect
	{
		// Token: 0x06007F59 RID: 32601 RVA: 0x002D8054 File Offset: 0x002D6254
		public CompProperties_AbilityOffsetPrisonerResistance()
		{
			this.compClass = typeof(CompAbilityEffect_OffsetPrisonerResistance);
		}

		// Token: 0x04004FCA RID: 20426
		public float offset;
	}
}
