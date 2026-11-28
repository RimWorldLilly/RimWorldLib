using System;

namespace RimWorld
{
	// Token: 0x02001EB6 RID: 7862
	[DefOf]
	public static class BillRepeatModeDefOf
	{
		// Token: 0x0600CA71 RID: 51825 RVA: 0x004A6043 File Offset: 0x004A4243
		static BillRepeatModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillRepeatModeDefOf));
		}

		// Token: 0x04007ED1 RID: 32465
		public static BillRepeatModeDef RepeatCount;

		// Token: 0x04007ED2 RID: 32466
		public static BillRepeatModeDef TargetCount;

		// Token: 0x04007ED3 RID: 32467
		public static BillRepeatModeDef Forever;
	}
}
