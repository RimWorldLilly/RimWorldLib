using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F59 RID: 8025
	public class SpecialThingFilterWorker_BiocodedWeapons : SpecialThingFilterWorker
	{
		// Token: 0x0600CC98 RID: 52376 RVA: 0x004B5EFC File Offset: 0x004B40FC
		public override bool Matches(Thing t)
		{
			return t.def.IsWeapon && t.TryGetComp<CompBladelinkWeapon>() == null && CompBiocodable.IsBiocoded(t);
		}

		// Token: 0x0600CC99 RID: 52377 RVA: 0x004B5F1D File Offset: 0x004B411D
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsWeapon && def.HasComp(typeof(CompBiocodable));
		}
	}
}
