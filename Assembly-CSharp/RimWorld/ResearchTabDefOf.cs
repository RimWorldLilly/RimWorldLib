using System;

namespace RimWorld
{
	// Token: 0x02001E84 RID: 7812
	[DefOf]
	public static class ResearchTabDefOf
	{
		// Token: 0x0600CA3F RID: 51775 RVA: 0x004A5CF1 File Offset: 0x004A3EF1
		static ResearchTabDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ResearchTabDefOf));
		}

		// Token: 0x04007A29 RID: 31273
		public static ResearchTabDef Main;

		// Token: 0x04007A2A RID: 31274
		[MayRequireAnomaly]
		public static ResearchTabDef Anomaly;
	}
}
