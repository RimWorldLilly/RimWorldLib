using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F0A RID: 3850
	public class IngredientValueGetter_Volume : IngredientValueGetter
	{
		// Token: 0x06006402 RID: 25602 RVA: 0x0023CF67 File Offset: 0x0023B167
		public override float ValuePerUnitOf(ThingDef t)
		{
			if (t.IsStuff)
			{
				return t.VolumePerUnit;
			}
			return 1f;
		}

		// Token: 0x06006403 RID: 25603 RVA: 0x0023CF80 File Offset: 0x0023B180
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			if (!ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume) || ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td)))
			{
				return ing.GetBaseCount().ToString() + "x " + ing.filter.Summary;
			}
			return (ing.GetBaseCount() * 10f).ToString() + "x " + ing.filter.Summary;
		}

		// Token: 0x06006404 RID: 25604 RVA: 0x0023D038 File Offset: 0x0023B238
		public override string ExtraDescriptionLine(RecipeDef r)
		{
			Func<ThingDef, bool> <>9__1;
			if (r.ingredients.Any(delegate(IngredientCount ing)
			{
				IEnumerable<ThingDef> allowedThingDefs = ing.filter.AllowedThingDefs;
				Func<ThingDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td)));
				}
				return allowedThingDefs.Any(predicate);
			}))
			{
				return "BillRequiresMayVary".Translate(10.ToStringCached());
			}
			return null;
		}
	}
}
