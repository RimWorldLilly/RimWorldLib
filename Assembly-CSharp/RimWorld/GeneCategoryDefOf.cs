using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EF2 RID: 7922
	[DefOf]
	public static class GeneCategoryDefOf
	{
		// Token: 0x0600CAAA RID: 51882 RVA: 0x004A640C File Offset: 0x004A460C
		static GeneCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GeneCategoryDefOf));
		}

		// Token: 0x040080D9 RID: 32985
		[MayRequireBiotech]
		public static GeneCategoryDef Archite;

		// Token: 0x040080DA RID: 32986
		public static GeneCategoryDef Miscellaneous;
	}
}
