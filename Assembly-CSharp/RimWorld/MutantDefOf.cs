using System;

namespace RimWorld
{
	// Token: 0x02001EFB RID: 7931
	[DefOf]
	public static class MutantDefOf
	{
		// Token: 0x0600CAB3 RID: 51891 RVA: 0x004A64A5 File Offset: 0x004A46A5
		static MutantDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MutantDefOf));
		}

		// Token: 0x040080F8 RID: 33016
		[MayRequireAnomaly]
		public static MutantDef Shambler;

		// Token: 0x040080F9 RID: 33017
		[MayRequireAnomaly]
		public static MutantDef Ghoul;

		// Token: 0x040080FA RID: 33018
		[MayRequireAnomaly]
		public static MutantDef AwokenCorpse;
	}
}
