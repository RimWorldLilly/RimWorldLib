using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E6D RID: 7789
	[DefOf]
	public static class DesignationCategoryDefOf
	{
		// Token: 0x0600CA29 RID: 51753 RVA: 0x004A5B7B File Offset: 0x004A3D7B
		static DesignationCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DesignationCategoryDefOf));
		}

		// Token: 0x04007670 RID: 30320
		public static DesignationCategoryDef Production;

		// Token: 0x04007671 RID: 30321
		public static DesignationCategoryDef Floors;

		// Token: 0x04007672 RID: 30322
		public static DesignationCategoryDef Zone;
	}
}
