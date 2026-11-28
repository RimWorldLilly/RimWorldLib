using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F0B RID: 3851
	public class IngredientValueGetter_Nutrition : IngredientValueGetter
	{
		// Token: 0x06006406 RID: 25606 RVA: 0x0023D095 File Offset: 0x0023B295
		public override float ValuePerUnitOf(ThingDef t)
		{
			if (!t.IsNutritionGivingIngestible)
			{
				return 0f;
			}
			return t.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x06006407 RID: 25607 RVA: 0x0023D0B4 File Offset: 0x0023B2B4
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return ing.GetBaseCount().ToString() + "x " + "BillNutrition".Translate() + " (" + ing.filter.Summary + ")";
		}
	}
}
