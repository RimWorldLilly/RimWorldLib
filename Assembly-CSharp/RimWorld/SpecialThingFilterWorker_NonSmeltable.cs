using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F53 RID: 8019
	public class SpecialThingFilterWorker_NonSmeltable : SpecialThingFilterWorker
	{
		// Token: 0x0600CC81 RID: 52353 RVA: 0x004B5D08 File Offset: 0x004B3F08
		public override bool Matches(Thing t)
		{
			return !t.Smeltable;
		}

		// Token: 0x0600CC82 RID: 52354 RVA: 0x004B5D13 File Offset: 0x004B3F13
		public override bool AlwaysMatches(ThingDef def)
		{
			return !def.PotentiallySmeltable;
		}
	}
}
