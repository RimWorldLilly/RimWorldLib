using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ECA RID: 7882
	[DefOf]
	public static class ClamorDefOf
	{
		// Token: 0x0600CA85 RID: 51845 RVA: 0x004A6197 File Offset: 0x004A4397
		static ClamorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ClamorDefOf));
		}

		// Token: 0x04007F54 RID: 32596
		public static ClamorDef Movement;

		// Token: 0x04007F55 RID: 32597
		public static ClamorDef Harm;

		// Token: 0x04007F56 RID: 32598
		public static ClamorDef Construction;

		// Token: 0x04007F57 RID: 32599
		public static ClamorDef Impact;

		// Token: 0x04007F58 RID: 32600
		public static ClamorDef Ability;

		// Token: 0x04007F59 RID: 32601
		[MayRequireBiotech]
		public static ClamorDef BabyCry;
	}
}
