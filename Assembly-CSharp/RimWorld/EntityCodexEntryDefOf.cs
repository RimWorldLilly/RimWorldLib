using System;

namespace RimWorld
{
	// Token: 0x02001F01 RID: 7937
	[DefOf]
	public static class EntityCodexEntryDefOf
	{
		// Token: 0x0600CAB9 RID: 51897 RVA: 0x004A650B File Offset: 0x004A470B
		static EntityCodexEntryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(EntityCodexEntryDefOf));
		}

		// Token: 0x04008119 RID: 33049
		[MayRequireAnomaly]
		public static EntityCodexEntryDef UnnaturalCorpse;
	}
}
