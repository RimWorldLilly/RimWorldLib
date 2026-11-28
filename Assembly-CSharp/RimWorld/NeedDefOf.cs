using System;

namespace RimWorld
{
	// Token: 0x02001E70 RID: 7792
	[DefOf]
	public static class NeedDefOf
	{
		// Token: 0x0600CA2B RID: 51755 RVA: 0x004A5B9D File Offset: 0x004A3D9D
		static NeedDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(NeedDefOf));
		}

		// Token: 0x04007718 RID: 30488
		public static NeedDef Food;

		// Token: 0x04007719 RID: 30489
		public static NeedDef Rest;

		// Token: 0x0400771A RID: 30490
		public static NeedDef Indoors;

		// Token: 0x0400771B RID: 30491
		[MayRequireBiotech]
		public static NeedDef Learning;
	}
}
