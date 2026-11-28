using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E99 RID: 7833
	[DefOf]
	public static class SpecialThingFilterDefOf
	{
		// Token: 0x0600CA54 RID: 51796 RVA: 0x004A5E56 File Offset: 0x004A4056
		static SpecialThingFilterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SpecialThingFilterDefOf));
		}

		// Token: 0x04007B44 RID: 31556
		public static SpecialThingFilterDef AllowFresh;

		// Token: 0x04007B45 RID: 31557
		public static SpecialThingFilterDef AllowDeadmansApparel;

		// Token: 0x04007B46 RID: 31558
		public static SpecialThingFilterDef AllowNonDeadmansApparel;

		// Token: 0x04007B47 RID: 31559
		public static SpecialThingFilterDef AllowLargeCorpses;

		// Token: 0x04007B48 RID: 31560
		[MayRequireIdeology]
		public static SpecialThingFilterDef AllowVegetarian;

		// Token: 0x04007B49 RID: 31561
		[MayRequireIdeology]
		public static SpecialThingFilterDef AllowCarnivore;

		// Token: 0x04007B4A RID: 31562
		[MayRequireIdeology]
		public static SpecialThingFilterDef AllowCannibal;

		// Token: 0x04007B4B RID: 31563
		[MayRequireIdeology]
		public static SpecialThingFilterDef AllowInsectMeat;

		// Token: 0x04007B4C RID: 31564
		[MayRequireAnomaly]
		public static SpecialThingFilterDef AllowCorpsesUnnatural;
	}
}
