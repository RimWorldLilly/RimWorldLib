using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F5F RID: 8031
	public class SpecialThingFilterWorker_NonDeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600CCAC RID: 52396 RVA: 0x004B604C File Offset: 0x004B424C
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && !apparel.WornByCorpse;
		}

		// Token: 0x0600CCAD RID: 52397 RVA: 0x004B606E File Offset: 0x004B426E
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
