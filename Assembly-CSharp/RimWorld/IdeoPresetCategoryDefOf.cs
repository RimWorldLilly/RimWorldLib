using System;

namespace RimWorld
{
	// Token: 0x02001EEB RID: 7915
	[DefOf]
	public static class IdeoPresetCategoryDefOf
	{
		// Token: 0x0600CAA5 RID: 51877 RVA: 0x004A63B7 File Offset: 0x004A45B7
		static IdeoPresetCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IdeoPresetCategoryDefOf));
		}

		// Token: 0x040080C2 RID: 32962
		[MayRequireIdeology]
		public static IdeoPresetCategoryDef Classic;

		// Token: 0x040080C3 RID: 32963
		[MayRequireIdeology]
		public static IdeoPresetCategoryDef Custom;

		// Token: 0x040080C4 RID: 32964
		[MayRequireIdeology]
		public static IdeoPresetCategoryDef Fluid;
	}
}
