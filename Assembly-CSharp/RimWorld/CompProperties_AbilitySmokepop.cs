using System;

namespace RimWorld
{
	// Token: 0x02001444 RID: 5188
	public class CompProperties_AbilitySmokepop : CompProperties_AbilityEffect
	{
		// Token: 0x06007FC3 RID: 32707 RVA: 0x002D96C8 File Offset: 0x002D78C8
		public CompProperties_AbilitySmokepop()
		{
			this.compClass = typeof(CompAbilityEffect_Smokepop);
		}

		// Token: 0x04004FEB RID: 20459
		public float smokeRadius;
	}
}
