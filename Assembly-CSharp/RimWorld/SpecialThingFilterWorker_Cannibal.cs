using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F66 RID: 8038
	public class SpecialThingFilterWorker_Cannibal : SpecialThingFilterWorker
	{
		// Token: 0x0600CCBD RID: 52413 RVA: 0x004B6197 File Offset: 0x004B4397
		public override bool Matches(Thing t)
		{
			return FoodUtility.IsHumanlikeCorpseOrHumanlikeMeatOrIngredient(t);
		}

		// Token: 0x0600CCBE RID: 52414 RVA: 0x004B619F File Offset: 0x004B439F
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsCorpse || def.HasComp(typeof(CompIngredients)) || FoodUtility.GetMeatSourceCategory(def) == MeatSourceCategory.Humanlike;
		}
	}
}
