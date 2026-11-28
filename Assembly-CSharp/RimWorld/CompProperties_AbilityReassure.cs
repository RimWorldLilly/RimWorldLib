using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001430 RID: 5168
	public class CompProperties_AbilityReassure : CompProperties_AbilityEffect
	{
		// Token: 0x06007F79 RID: 32633 RVA: 0x002D850A File Offset: 0x002D670A
		public CompProperties_AbilityReassure()
		{
			this.compClass = typeof(CompAbilityEffect_Reassure);
		}

		// Token: 0x04004FD2 RID: 20434
		[MustTranslate]
		public string successMessage;

		// Token: 0x04004FD3 RID: 20435
		public float baseCertaintyGain = 0.075f;
	}
}
