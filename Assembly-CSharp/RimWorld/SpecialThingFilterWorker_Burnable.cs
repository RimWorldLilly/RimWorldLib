using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F56 RID: 8022
	public class SpecialThingFilterWorker_Burnable : SpecialThingFilterWorker
	{
		// Token: 0x0600CC8C RID: 52364 RVA: 0x004B5DD5 File Offset: 0x004B3FD5
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && t.BurnableByRecipe;
		}

		// Token: 0x0600CC8D RID: 52365 RVA: 0x004B5DED File Offset: 0x004B3FED
		public override bool CanEverMatch(ThingDef def)
		{
			return def.burnableByRecipe;
		}

		// Token: 0x0600CC8E RID: 52366 RVA: 0x004B5DF5 File Offset: 0x004B3FF5
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
