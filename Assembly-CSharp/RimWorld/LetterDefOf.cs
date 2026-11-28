using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EBC RID: 7868
	[DefOf]
	public static class LetterDefOf
	{
		// Token: 0x0600CA77 RID: 51831 RVA: 0x004A60A9 File Offset: 0x004A42A9
		static LetterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LetterDefOf));
		}

		// Token: 0x04007EE4 RID: 32484
		public static LetterDef ThreatBig;

		// Token: 0x04007EE5 RID: 32485
		public static LetterDef ThreatSmall;

		// Token: 0x04007EE6 RID: 32486
		public static LetterDef NegativeEvent;

		// Token: 0x04007EE7 RID: 32487
		public static LetterDef NeutralEvent;

		// Token: 0x04007EE8 RID: 32488
		public static LetterDef PositiveEvent;

		// Token: 0x04007EE9 RID: 32489
		public static LetterDef Death;

		// Token: 0x04007EEA RID: 32490
		public static LetterDef AcceptVisitors;

		// Token: 0x04007EEB RID: 32491
		public static LetterDef AcceptJoiner;

		// Token: 0x04007EEC RID: 32492
		public static LetterDef GameEnded;

		// Token: 0x04007EED RID: 32493
		public static LetterDef ChoosePawn;

		// Token: 0x04007EEE RID: 32494
		public static LetterDef RitualOutcomeNegative;

		// Token: 0x04007EEF RID: 32495
		public static LetterDef RitualOutcomePositive;

		// Token: 0x04007EF0 RID: 32496
		[MayRequireIdeology]
		public static LetterDef RelicHuntInstallationFound;

		// Token: 0x04007EF1 RID: 32497
		[MayRequireBiotech]
		public static LetterDef BabyBirth;

		// Token: 0x04007EF2 RID: 32498
		[MayRequireBiotech]
		public static LetterDef BabyToChild;

		// Token: 0x04007EF3 RID: 32499
		[MayRequireBiotech]
		public static LetterDef ChildToAdult;

		// Token: 0x04007EF4 RID: 32500
		[MayRequireBiotech]
		public static LetterDef ChildBirthday;

		// Token: 0x04007EF5 RID: 32501
		[MayRequireBiotech]
		public static LetterDef Bossgroup;

		// Token: 0x04007EF6 RID: 32502
		[MayRequireAnomaly]
		public static LetterDef AcceptCreepJoiner;

		// Token: 0x04007EF7 RID: 32503
		[MayRequireAnomaly]
		public static LetterDef EntityDiscovered;

		// Token: 0x04007EF8 RID: 32504
		public static LetterDef BundleLetter;
	}
}
