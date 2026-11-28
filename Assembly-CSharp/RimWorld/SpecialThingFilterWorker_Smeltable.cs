using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F52 RID: 8018
	public class SpecialThingFilterWorker_Smeltable : SpecialThingFilterWorker
	{
		// Token: 0x0600CC7D RID: 52349 RVA: 0x004B5CCB File Offset: 0x004B3ECB
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && t.Smeltable;
		}

		// Token: 0x0600CC7E RID: 52350 RVA: 0x004B5CE3 File Offset: 0x004B3EE3
		public override bool CanEverMatch(ThingDef def)
		{
			return def.smeltable;
		}

		// Token: 0x0600CC7F RID: 52351 RVA: 0x004B5CEB File Offset: 0x004B3EEB
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.smeltable && !def.MadeFromStuff;
		}
	}
}
