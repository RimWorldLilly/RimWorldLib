using System;

namespace RimWorld
{
	// Token: 0x02001ED4 RID: 7892
	[DefOf]
	public static class RoyalTitleDefOf
	{
		// Token: 0x0600CA8F RID: 51855 RVA: 0x004A6241 File Offset: 0x004A4441
		static RoyalTitleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoyalTitleDefOf));
		}

		// Token: 0x04007F93 RID: 32659
		[MayRequireRoyalty]
		public static RoyalTitleDef Knight;

		// Token: 0x04007F94 RID: 32660
		[MayRequireRoyalty]
		public static RoyalTitleDef Count;
	}
}
