using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F64 RID: 8036
	public class SpecialThingFilterWorker_Vegetarian : SpecialThingFilterWorker
	{
		// Token: 0x0600CCB7 RID: 52407 RVA: 0x004B6124 File Offset: 0x004B4324
		public override bool Matches(Thing t)
		{
			return FoodUtility.AcceptableVegetarian(t) && !FoodUtility.AcceptableCarnivore(t);
		}

		// Token: 0x0600CCB8 RID: 52408 RVA: 0x004B6139 File Offset: 0x004B4339
		public override bool CanEverMatch(ThingDef def)
		{
			return FoodUtility.UnacceptableCarnivore(def);
		}
	}
}
