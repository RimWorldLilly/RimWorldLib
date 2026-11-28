using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F5C RID: 8028
	public class SpecialThingFilterWorker_NonBiocodedApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600CCA1 RID: 52385 RVA: 0x004B5FB0 File Offset: 0x004B41B0
		public override bool Matches(Thing t)
		{
			return t.def.IsApparel && !CompBiocodable.IsBiocoded(t);
		}

		// Token: 0x0600CCA2 RID: 52386 RVA: 0x004B5FCA File Offset: 0x004B41CA
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel;
		}
	}
}
