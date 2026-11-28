using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E7 RID: 5095
	public class CompProperties_AbilityCounsel : CompProperties_AbilityEffect
	{
		// Token: 0x06007E9E RID: 32414 RVA: 0x002D417C File Offset: 0x002D237C
		public CompProperties_AbilityCounsel()
		{
			this.compClass = typeof(CompAbilityEffect_Counsel);
		}

		// Token: 0x04004F6A RID: 20330
		public float minMoodOffset = -10f;

		// Token: 0x04004F6B RID: 20331
		[MustTranslate]
		public string successMessage;

		// Token: 0x04004F6C RID: 20332
		[MustTranslate]
		public string successMessageNoNegativeThought;

		// Token: 0x04004F6D RID: 20333
		[MustTranslate]
		public string failMessage;

		// Token: 0x04004F6E RID: 20334
		public ThoughtDef failedThoughtRecipient;
	}
}
