using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F65 RID: 8037
	public class SpecialThingFilterWorker_Carnivore : SpecialThingFilterWorker
	{
		// Token: 0x0600CCBA RID: 52410 RVA: 0x004B6149 File Offset: 0x004B4349
		public override bool Matches(Thing t)
		{
			return !FoodUtility.AcceptableVegetarian(t) && FoodUtility.AcceptableCarnivore(t) && !FoodUtility.IsHumanlikeCorpseOrHumanlikeMeatOrIngredient(t) && !FoodUtility.IsInsectCorpseOrInsectMeatIngredient(t);
		}

		// Token: 0x0600CCBB RID: 52411 RVA: 0x004B616E File Offset: 0x004B436E
		public override bool CanEverMatch(ThingDef def)
		{
			return FoodUtility.UnacceptableVegetarian(def) && FoodUtility.GetMeatSourceCategory(def) != MeatSourceCategory.Humanlike && FoodUtility.GetMeatSourceCategory(def) != MeatSourceCategory.Insect;
		}
	}
}
