using System;

namespace RimWorld
{
	// Token: 0x02001EF6 RID: 7926
	[DefOf]
	public static class KnowledgeCategoryDefOf
	{
		// Token: 0x0600CAAE RID: 51886 RVA: 0x004A6450 File Offset: 0x004A4650
		static KnowledgeCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(KnowledgeCategoryDefOf));
		}

		// Token: 0x040080E2 RID: 32994
		[MayRequireAnomaly]
		public static KnowledgeCategoryDef Basic;

		// Token: 0x040080E3 RID: 32995
		[MayRequireAnomaly]
		public static KnowledgeCategoryDef Advanced;
	}
}
