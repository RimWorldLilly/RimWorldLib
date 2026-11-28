using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F60 RID: 8032
	public class SpecialThingFilterWorker_DeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600CCAF RID: 52399 RVA: 0x004B6090 File Offset: 0x004B4290
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && apparel.WornByCorpse;
		}

		// Token: 0x0600CCB0 RID: 52400 RVA: 0x004B60AF File Offset: 0x004B42AF
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
