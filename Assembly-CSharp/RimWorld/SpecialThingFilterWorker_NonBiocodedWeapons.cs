using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F5A RID: 8026
	public class SpecialThingFilterWorker_NonBiocodedWeapons : SpecialThingFilterWorker
	{
		// Token: 0x0600CC9B RID: 52379 RVA: 0x004B5F41 File Offset: 0x004B4141
		public override bool Matches(Thing t)
		{
			return t.def.IsWeapon && (!CompBiocodable.IsBiocoded(t) || t.TryGetComp<CompBladelinkWeapon>() != null);
		}

		// Token: 0x0600CC9C RID: 52380 RVA: 0x004B5F65 File Offset: 0x004B4165
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsWeapon;
		}
	}
}
