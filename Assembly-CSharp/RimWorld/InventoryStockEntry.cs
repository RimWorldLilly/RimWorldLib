using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200155A RID: 5466
	public class InventoryStockEntry : IExposable
	{
		// Token: 0x060085BB RID: 34235 RVA: 0x00304D23 File Offset: 0x00302F23
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
		}

		// Token: 0x04005268 RID: 21096
		public ThingDef thingDef;

		// Token: 0x04005269 RID: 21097
		public int count;
	}
}
