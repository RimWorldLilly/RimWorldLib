using System;

namespace RimWorld
{
	// Token: 0x02001E7F RID: 7807
	[DefOf]
	public static class MainButtonDefOf
	{
		// Token: 0x0600CA3A RID: 51770 RVA: 0x004A5C9C File Offset: 0x004A3E9C
		static MainButtonDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MainButtonDefOf));
		}

		// Token: 0x04007A0D RID: 31245
		public static MainButtonDef Inspect;

		// Token: 0x04007A0E RID: 31246
		public static MainButtonDef Architect;

		// Token: 0x04007A0F RID: 31247
		public static MainButtonDef Research;

		// Token: 0x04007A10 RID: 31248
		public static MainButtonDef Menu;

		// Token: 0x04007A11 RID: 31249
		public static MainButtonDef World;

		// Token: 0x04007A12 RID: 31250
		public static MainButtonDef Quests;

		// Token: 0x04007A13 RID: 31251
		public static MainButtonDef Factions;

		// Token: 0x04007A14 RID: 31252
		[MayRequireIdeology]
		public static MainButtonDef Ideos;
	}
}
