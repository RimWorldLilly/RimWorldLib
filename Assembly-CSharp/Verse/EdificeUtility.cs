using System;

namespace Verse
{
	// Token: 0x0200027E RID: 638
	public static class EdificeUtility
	{
		// Token: 0x060012B1 RID: 4785 RVA: 0x0006A794 File Offset: 0x00068994
		public static bool IsEdifice(this BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isEdifice;
		}
	}
}
