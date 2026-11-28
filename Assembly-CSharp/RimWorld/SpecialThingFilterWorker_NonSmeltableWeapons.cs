using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F54 RID: 8020
	public class SpecialThingFilterWorker_NonSmeltableWeapons : SpecialThingFilterWorker
	{
		// Token: 0x0600CC84 RID: 52356 RVA: 0x004B5D26 File Offset: 0x004B3F26
		public override bool Matches(Thing t)
		{
			return !t.Smeltable;
		}

		// Token: 0x0600CC85 RID: 52357 RVA: 0x004B5D34 File Offset: 0x004B3F34
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

		// Token: 0x0600CC86 RID: 52358 RVA: 0x004B5D92 File Offset: 0x004B3F92
		public override bool AlwaysMatches(ThingDef def)
		{
			return this.CanEverMatch(def) && !def.PotentiallySmeltable;
		}
	}
}
