using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF0 RID: 3824
	public class CompProperties_Ingredients : CompProperties
	{
		// Token: 0x060063CE RID: 25550 RVA: 0x0023C6A2 File Offset: 0x0023A8A2
		public CompProperties_Ingredients()
		{
			this.compClass = typeof(CompIngredients);
		}

		// Token: 0x04003849 RID: 14409
		public bool performMergeCompatibilityChecks = true;

		// Token: 0x0400384A RID: 14410
		public bool splitTransferableFoodKind;

		// Token: 0x0400384B RID: 14411
		public FoodKind noIngredientsFoodKind = FoodKind.Any;
	}
}
