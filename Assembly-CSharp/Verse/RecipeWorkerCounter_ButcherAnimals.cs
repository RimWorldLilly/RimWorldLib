using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000155 RID: 341
	public class RecipeWorkerCounter_ButcherAnimals : RecipeWorkerCounter
	{
		// Token: 0x06000A6F RID: 2671 RVA: 0x00039F04 File Offset: 0x00038104
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00039F08 File Offset: 0x00038108
		public override int CountProducts(Bill_Production bill)
		{
			int num = 0;
			List<ThingDef> childThingDefs = ThingCategoryDefOf.MeatRaw.childThingDefs;
			for (int i = 0; i < childThingDefs.Count; i++)
			{
				num += bill.Map.resourceCounter.GetCount(childThingDefs[i]);
			}
			return num;
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00039F4E File Offset: 0x0003814E
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.MeatRaw.label;
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00039F5C File Offset: 0x0003815C
		public override bool CanPossiblyStore(Bill_Production bill, ISlotGroup slotGroup)
		{
			foreach (ThingDef thingDef in bill.ingredientFilter.AllowedThingDefs)
			{
				if (thingDef.ingestible != null && thingDef.ingestible.sourceDef != null)
				{
					RaceProperties race = thingDef.ingestible.sourceDef.race;
					if (race != null && race.meatDef != null && !slotGroup.Settings.AllowedToAccept(race.meatDef))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
