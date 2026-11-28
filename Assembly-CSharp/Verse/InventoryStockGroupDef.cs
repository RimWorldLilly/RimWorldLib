using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200017D RID: 381
	public class InventoryStockGroupDef : Def
	{
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x0003C889 File Offset: 0x0003AA89
		public ThingDef DefaultThingDef
		{
			get
			{
				return this.defaultThingDef ?? this.thingDefs.First<ThingDef>();
			}
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0003C8A0 File Offset: 0x0003AAA0
		public override IEnumerable<string> ConfigErrors()
		{
			InventoryStockGroupDef.<ConfigErrors>d__6 <ConfigErrors>d__ = new InventoryStockGroupDef.<ConfigErrors>d__6(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04000924 RID: 2340
		public List<ThingDef> thingDefs;

		// Token: 0x04000925 RID: 2341
		public int min;

		// Token: 0x04000926 RID: 2342
		public int max = 3;

		// Token: 0x04000927 RID: 2343
		public ThingDef defaultThingDef;
	}
}
