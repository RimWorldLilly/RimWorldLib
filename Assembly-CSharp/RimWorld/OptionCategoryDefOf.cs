using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E95 RID: 7829
	[DefOf]
	public static class OptionCategoryDefOf
	{
		// Token: 0x0600CA50 RID: 51792 RVA: 0x004A5E12 File Offset: 0x004A4012
		static OptionCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(OptionCategoryDefOf));
		}

		// Token: 0x04007AF6 RID: 31478
		public static OptionCategoryDef General;

		// Token: 0x04007AF7 RID: 31479
		public static OptionCategoryDef Graphics;

		// Token: 0x04007AF8 RID: 31480
		public static OptionCategoryDef Audio;

		// Token: 0x04007AF9 RID: 31481
		public static OptionCategoryDef Gameplay;

		// Token: 0x04007AFA RID: 31482
		public static OptionCategoryDef Interface;

		// Token: 0x04007AFB RID: 31483
		public static OptionCategoryDef Controls;

		// Token: 0x04007AFC RID: 31484
		public static OptionCategoryDef Dev;

		// Token: 0x04007AFD RID: 31485
		public static OptionCategoryDef Mods;
	}
}
