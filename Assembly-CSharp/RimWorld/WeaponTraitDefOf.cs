using System;

namespace RimWorld
{
	// Token: 0x02001ED8 RID: 7896
	[DefOf]
	public static class WeaponTraitDefOf
	{
		// Token: 0x0600CA92 RID: 51858 RVA: 0x004A6274 File Offset: 0x004A4474
		static WeaponTraitDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeaponTraitDefOf));
		}

		// Token: 0x04007FD2 RID: 32722
		[MayRequireRoyalty]
		public static WeaponTraitDef NeedKill;
	}
}
