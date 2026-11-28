using System;

namespace RimWorld
{
	// Token: 0x02001F09 RID: 7945
	[DefOf]
	public static class PrefabDefOf
	{
		// Token: 0x0600CAC1 RID: 51905 RVA: 0x004A6593 File Offset: 0x004A4793
		static PrefabDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PrefabDefOf));
		}

		// Token: 0x04008149 RID: 33097
		[MayRequireOdyssey]
		public static PrefabDef AncientUplink;

		// Token: 0x0400814A RID: 33098
		[MayRequireOdyssey]
		public static PrefabDef SurveyScanner;

		// Token: 0x0400814B RID: 33099
		[MayRequireOdyssey]
		public static PrefabDef CerebrexCore;

		// Token: 0x0400814C RID: 33100
		[MayRequireOdyssey]
		public static PrefabDef CorridorBarricade;
	}
}
