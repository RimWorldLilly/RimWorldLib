using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F57 RID: 8023
	public class SpecialThingFilterWorker_NonBurnable : SpecialThingFilterWorker
	{
		// Token: 0x0600CC90 RID: 52368 RVA: 0x004B5E12 File Offset: 0x004B4012
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.BurnableByRecipe;
		}

		// Token: 0x0600CC91 RID: 52369 RVA: 0x004B5E2D File Offset: 0x004B402D
		public override bool CanEverMatch(ThingDef def)
		{
			return !def.burnableByRecipe || def.MadeFromStuff;
		}

		// Token: 0x0600CC92 RID: 52370 RVA: 0x004B5E3F File Offset: 0x004B403F
		public override bool AlwaysMatches(ThingDef def)
		{
			return !def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
