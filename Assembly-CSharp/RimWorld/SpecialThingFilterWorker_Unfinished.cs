using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F55 RID: 8021
	public class SpecialThingFilterWorker_Unfinished : SpecialThingFilterWorker
	{
		// Token: 0x0600CC88 RID: 52360 RVA: 0x004B5DB0 File Offset: 0x004B3FB0
		public override bool Matches(Thing t)
		{
			return t.def.isUnfinishedThing;
		}

		// Token: 0x0600CC89 RID: 52361 RVA: 0x004B5DBD File Offset: 0x004B3FBD
		public override bool CanEverMatch(ThingDef def)
		{
			return def.isUnfinishedThing;
		}

		// Token: 0x0600CC8A RID: 52362 RVA: 0x004B5DC5 File Offset: 0x004B3FC5
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.isUnfinishedThing;
		}
	}
}
