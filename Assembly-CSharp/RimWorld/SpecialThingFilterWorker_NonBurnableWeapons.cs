using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F58 RID: 8024
	public class SpecialThingFilterWorker_NonBurnableWeapons : SpecialThingFilterWorker
	{
		// Token: 0x0600CC94 RID: 52372 RVA: 0x004B5E5C File Offset: 0x004B405C
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.BurnableByRecipe;
		}

		// Token: 0x0600CC95 RID: 52373 RVA: 0x004B5E78 File Offset: 0x004B4078
		public override bool CanEverMatch(ThingDef def)
		{
			if (!def.IsWeapon)
			{
				return false;
			}
			if (!def.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				for (int i = 0; i < def.thingCategories.Count; i++)
				{
					for (ThingCategoryDef thingCategoryDef = def.thingCategories[i]; thingCategoryDef != null; thingCategoryDef = thingCategoryDef.parent)
					{
						if (thingCategoryDef == ThingCategoryDefOf.Weapons)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600CC96 RID: 52374 RVA: 0x004B5ED6 File Offset: 0x004B40D6
		public override bool AlwaysMatches(ThingDef def)
		{
			return this.CanEverMatch(def) && !def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
