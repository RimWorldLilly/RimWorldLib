using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EFD RID: 7933
	[DefOf]
	public static class CreepJoinerDownsideDefOf
	{
		// Token: 0x0600CAB5 RID: 51893 RVA: 0x004A64C7 File Offset: 0x004A46C7
		static CreepJoinerDownsideDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(CreepJoinerDownsideDefOf));
		}

		// Token: 0x040080FC RID: 33020
		[MayRequireAnomaly]
		public static CreepJoinerDownsideDef OrganDecay;

		// Token: 0x040080FD RID: 33021
		[MayRequireAnomaly]
		public static CreepJoinerDownsideDef CrumblingMind;
	}
}
