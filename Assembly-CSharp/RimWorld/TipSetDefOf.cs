using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EA9 RID: 7849
	[DefOf]
	public static class TipSetDefOf
	{
		// Token: 0x0600CA64 RID: 51812 RVA: 0x004A5F66 File Offset: 0x004A4166
		static TipSetDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TipSetDefOf));
		}

		// Token: 0x04007E44 RID: 32324
		public static TipSetDef GameplayTips;
	}
}
