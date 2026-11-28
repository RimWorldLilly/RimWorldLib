using System;

namespace RimWorld
{
	// Token: 0x02001EFE RID: 7934
	[DefOf]
	public static class MonolithLevelDefOf
	{
		// Token: 0x0600CAB6 RID: 51894 RVA: 0x004A64D8 File Offset: 0x004A46D8
		static MonolithLevelDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MonolithLevelDefOf));
		}

		// Token: 0x040080FE RID: 33022
		[MayRequireAnomaly]
		public static MonolithLevelDef Inactive;

		// Token: 0x040080FF RID: 33023
		[MayRequireAnomaly]
		public static MonolithLevelDef Waking;

		// Token: 0x04008100 RID: 33024
		[MayRequireAnomaly]
		public static MonolithLevelDef VoidAwakened;

		// Token: 0x04008101 RID: 33025
		[MayRequireAnomaly]
		public static MonolithLevelDef Gleaming;

		// Token: 0x04008102 RID: 33026
		[MayRequireAnomaly]
		public static MonolithLevelDef Embraced;

		// Token: 0x04008103 RID: 33027
		[MayRequireAnomaly]
		public static MonolithLevelDef Disrupted;
	}
}
