using System;

namespace RimWorld
{
	// Token: 0x02001EF4 RID: 7924
	[DefOf]
	public static class LearningDesireDefOf
	{
		// Token: 0x0600CAAC RID: 51884 RVA: 0x004A642E File Offset: 0x004A462E
		static LearningDesireDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LearningDesireDefOf));
		}

		// Token: 0x040080DD RID: 32989
		[MayRequireBiotech]
		public static LearningDesireDef Lessontaking;

		// Token: 0x040080DE RID: 32990
		[MayRequireBiotech]
		public static LearningDesireDef Workwatching;

		// Token: 0x040080DF RID: 32991
		[MayRequireBiotech]
		public static LearningDesireDef Reading;
	}
}
