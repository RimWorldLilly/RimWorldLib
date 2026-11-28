using System;

namespace RimWorld
{
	// Token: 0x02001E91 RID: 7825
	[DefOf]
	public static class RaidAgeRestrictionDefOf
	{
		// Token: 0x0600CA4C RID: 51788 RVA: 0x004A5DCE File Offset: 0x004A3FCE
		static RaidAgeRestrictionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RaidAgeRestrictionDefOf));
		}

		// Token: 0x04007AC5 RID: 31429
		[MayRequireBiotech]
		public static RaidAgeRestrictionDef Children;
	}
}
