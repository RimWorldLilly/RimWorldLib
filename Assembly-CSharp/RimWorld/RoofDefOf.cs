using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E88 RID: 7816
	[DefOf]
	public static class RoofDefOf
	{
		// Token: 0x0600CA43 RID: 51779 RVA: 0x004A5D35 File Offset: 0x004A3F35
		static RoofDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoofDefOf));
		}

		// Token: 0x04007A8C RID: 31372
		public static RoofDef RoofConstructed;

		// Token: 0x04007A8D RID: 31373
		public static RoofDef RoofRockThick;

		// Token: 0x04007A8E RID: 31374
		public static RoofDef RoofRockThin;
	}
}
