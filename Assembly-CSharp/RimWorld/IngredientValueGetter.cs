using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F09 RID: 3849
	public abstract class IngredientValueGetter
	{
		// Token: 0x060063FE RID: 25598
		public abstract float ValuePerUnitOf(ThingDef t);

		// Token: 0x060063FF RID: 25599
		public abstract string BillRequirementsDescription(RecipeDef r, IngredientCount ing);

		// Token: 0x06006400 RID: 25600 RVA: 0x0023CF5C File Offset: 0x0023B15C
		public virtual string ExtraDescriptionLine(RecipeDef r)
		{
			return null;
		}
	}
}
