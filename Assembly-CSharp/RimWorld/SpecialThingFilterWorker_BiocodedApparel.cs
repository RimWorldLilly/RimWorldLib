using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F5B RID: 8027
	public class SpecialThingFilterWorker_BiocodedApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600CC9E RID: 52382 RVA: 0x004B5F75 File Offset: 0x004B4175
		public override bool Matches(Thing t)
		{
			return t.def.IsApparel && CompBiocodable.IsBiocoded(t);
		}

		// Token: 0x0600CC9F RID: 52383 RVA: 0x004B5F8C File Offset: 0x004B418C
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.HasComp(typeof(CompBiocodable));
		}
	}
}
