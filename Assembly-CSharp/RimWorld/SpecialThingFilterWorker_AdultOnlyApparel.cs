using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F5E RID: 8030
	public class SpecialThingFilterWorker_AdultOnlyApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600CCA8 RID: 52392 RVA: 0x004B6013 File Offset: 0x004B4213
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.IsApparel && def.apparel.developmentalStageFilter == DevelopmentalStage.Adult;
		}

		// Token: 0x0600CCA9 RID: 52393 RVA: 0x004B602D File Offset: 0x004B422D
		public override bool Matches(Thing t)
		{
			return this.AlwaysMatches(t.def);
		}

		// Token: 0x0600CCAA RID: 52394 RVA: 0x004B603B File Offset: 0x004B423B
		public override bool CanEverMatch(ThingDef def)
		{
			return this.AlwaysMatches(def);
		}
	}
}
