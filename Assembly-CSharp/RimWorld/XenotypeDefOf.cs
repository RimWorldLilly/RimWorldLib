using System;

namespace RimWorld
{
	// Token: 0x02001EEF RID: 7919
	[DefOf]
	public static class XenotypeDefOf
	{
		// Token: 0x0600CAA7 RID: 51879 RVA: 0x004A63D9 File Offset: 0x004A45D9
		static XenotypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(XenotypeDefOf));
		}

		// Token: 0x040080D3 RID: 32979
		[MayRequireBiotech]
		public static XenotypeDef Baseliner;

		// Token: 0x040080D4 RID: 32980
		[MayRequireBiotech]
		public static XenotypeDef Sanguophage;
	}
}
