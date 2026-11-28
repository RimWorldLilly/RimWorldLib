using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E96 RID: 7830
	[DefOf]
	public static class ThinkTreeDefOf
	{
		// Token: 0x0600CA51 RID: 51793 RVA: 0x004A5E23 File Offset: 0x004A4023
		static ThinkTreeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThinkTreeDefOf));
		}

		// Token: 0x04007AFE RID: 31486
		[MayRequireAnomaly]
		public static ThinkTreeDef ShamblerConstant;
	}
}
