using System;

namespace RimWorld
{
	// Token: 0x02001EEA RID: 7914
	[DefOf]
	public static class RitualVisualEffectDefOf
	{
		// Token: 0x0600CAA4 RID: 51876 RVA: 0x004A63A6 File Offset: 0x004A45A6
		static RitualVisualEffectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RitualVisualEffectDefOf));
		}

		// Token: 0x040080C1 RID: 32961
		[MayRequireIdeology]
		public static RitualVisualEffectDef Basic;
	}
}
