using System;

namespace RimWorld
{
	// Token: 0x02001ED5 RID: 7893
	[DefOf]
	public static class AbilityDefOf
	{
		// Token: 0x0600CA90 RID: 51856 RVA: 0x004A6252 File Offset: 0x004A4452
		static AbilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(AbilityDefOf));
		}

		// Token: 0x04007F95 RID: 32661
		[MayRequireRoyalty]
		public static AbilityDef Speech;

		// Token: 0x04007F96 RID: 32662
		[MayRequireBiotech]
		public static AbilityDef ReimplantXenogerm;

		// Token: 0x04007F97 RID: 32663
		[MayRequireBiotech]
		public static AbilityDef ResurrectionMech;

		// Token: 0x04007F98 RID: 32664
		[MayRequireAnomaly]
		public static AbilityDef EntitySkip;

		// Token: 0x04007F99 RID: 32665
		[MayRequireAnomaly]
		public static AbilityDef UnnaturalCorpseSkip;

		// Token: 0x04007F9A RID: 32666
		[MayRequireAnomaly]
		public static AbilityDef ConsumeLeap_Devourer;

		// Token: 0x04007F9B RID: 32667
		[MayRequireOdyssey]
		public static AbilityDef SludgeSpew;

		// Token: 0x04007F9C RID: 32668
		[MayRequireOdyssey]
		public static AbilityDef EggSpew;
	}
}
