using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F5D RID: 8029
	public class SpecialThingFilterWorker_ChildOnlyApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600CCA4 RID: 52388 RVA: 0x004B5FDA File Offset: 0x004B41DA
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.IsApparel && def.apparel.developmentalStageFilter == DevelopmentalStage.Child;
		}

		// Token: 0x0600CCA5 RID: 52389 RVA: 0x004B5FF4 File Offset: 0x004B41F4
		public override bool Matches(Thing t)
		{
			return this.AlwaysMatches(t.def);
		}

		// Token: 0x0600CCA6 RID: 52390 RVA: 0x004B6002 File Offset: 0x004B4202
		public override bool CanEverMatch(ThingDef def)
		{
			return this.AlwaysMatches(def);
		}
	}
}
