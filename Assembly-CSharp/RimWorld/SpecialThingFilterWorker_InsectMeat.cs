using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F67 RID: 8039
	public class SpecialThingFilterWorker_InsectMeat : SpecialThingFilterWorker
	{
		// Token: 0x0600CCC0 RID: 52416 RVA: 0x004B61CE File Offset: 0x004B43CE
		public override bool Matches(Thing t)
		{
			return FoodUtility.IsInsectCorpseOrInsectMeatIngredient(t);
		}

		// Token: 0x0600CCC1 RID: 52417 RVA: 0x004B61D6 File Offset: 0x004B43D6
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsCorpse || def.HasComp(typeof(CompIngredients)) || FoodUtility.GetMeatSourceCategory(def) == MeatSourceCategory.Insect;
		}
	}
}
