using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000154 RID: 340
	public class RecipeWorkerCounter_MakeStoneBlocks : RecipeWorkerCounter
	{
		// Token: 0x06000A6A RID: 2666 RVA: 0x00039E24 File Offset: 0x00038024
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00039E28 File Offset: 0x00038028
		public override int CountProducts(Bill_Production bill)
		{
			int num = 0;
			List<ThingDef> childThingDefs = ThingCategoryDefOf.StoneBlocks.childThingDefs;
			for (int i = 0; i < childThingDefs.Count; i++)
			{
				num += bill.Map.resourceCounter.GetCount(childThingDefs[i]);
			}
			return num;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00039E6E File Offset: 0x0003806E
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.StoneBlocks.label;
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00039E7C File Offset: 0x0003807C
		public override bool CanPossiblyStore(Bill_Production bill, ISlotGroup slotGroup)
		{
			foreach (ThingDef thingDef in bill.ingredientFilter.AllowedThingDefs)
			{
				if (!thingDef.butcherProducts.NullOrEmpty<ThingDefCountClass>())
				{
					ThingDef thingDef2 = thingDef.butcherProducts[0].thingDef;
					if (!slotGroup.Settings.AllowedToAccept(thingDef2))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
