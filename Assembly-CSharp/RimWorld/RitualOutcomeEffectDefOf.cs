using System;

namespace RimWorld
{
	// Token: 0x02001EE3 RID: 7907
	[DefOf]
	public static class RitualOutcomeEffectDefOf
	{
		// Token: 0x0600CA9D RID: 51869 RVA: 0x004A632F File Offset: 0x004A452F
		static RitualOutcomeEffectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RitualOutcomeEffectDefOf));
		}

		// Token: 0x0400809A RID: 32922
		[MayRequireRoyalty]
		public static RitualOutcomeEffectDef BestowingCeremony;

		// Token: 0x0400809B RID: 32923
		[MayRequireBiotech]
		public static RitualOutcomeEffectDef ChildBirth;
	}
}
