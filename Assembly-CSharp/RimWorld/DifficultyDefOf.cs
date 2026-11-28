using System;

namespace RimWorld
{
	// Token: 0x02001E8D RID: 7821
	[DefOf]
	public static class DifficultyDefOf
	{
		// Token: 0x0600CA48 RID: 51784 RVA: 0x004A5D8A File Offset: 0x004A3F8A
		static DifficultyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DifficultyDefOf));
		}

		// Token: 0x04007ABB RID: 31419
		public static DifficultyDef Easy;

		// Token: 0x04007ABC RID: 31420
		public static DifficultyDef Rough;
	}
}
