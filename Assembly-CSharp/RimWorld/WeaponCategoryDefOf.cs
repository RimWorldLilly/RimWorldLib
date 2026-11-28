using System;

namespace RimWorld
{
	// Token: 0x02001F0F RID: 7951
	[DefOf]
	public static class WeaponCategoryDefOf
	{
		// Token: 0x0600CAC7 RID: 51911 RVA: 0x004A65F9 File Offset: 0x004A47F9
		static WeaponCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeaponCategoryDefOf));
		}

		// Token: 0x04008162 RID: 33122
		[MayRequireRoyalty]
		public static WeaponCategoryDef BladeLink;
	}
}
