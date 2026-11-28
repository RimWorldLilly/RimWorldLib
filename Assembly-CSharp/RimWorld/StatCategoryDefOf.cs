using System;

namespace RimWorld
{
	// Token: 0x02001E93 RID: 7827
	[DefOf]
	public static class StatCategoryDefOf
	{
		// Token: 0x0600CA4E RID: 51790 RVA: 0x004A5DF0 File Offset: 0x004A3FF0
		static StatCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StatCategoryDefOf));
		}

		// Token: 0x04007ACB RID: 31435
		public static StatCategoryDef Basics;

		// Token: 0x04007ACC RID: 31436
		public static StatCategoryDef BasicsImportant;

		// Token: 0x04007ACD RID: 31437
		public static StatCategoryDef BasicsPawnImportant;

		// Token: 0x04007ACE RID: 31438
		public static StatCategoryDef BasicsNonPawnImportant;

		// Token: 0x04007ACF RID: 31439
		public static StatCategoryDef BasicsNonPawn;

		// Token: 0x04007AD0 RID: 31440
		public static StatCategoryDef BasicsPawn;

		// Token: 0x04007AD1 RID: 31441
		public static StatCategoryDef Animals;

		// Token: 0x04007AD2 RID: 31442
		public static StatCategoryDef AnimalProductivity;

		// Token: 0x04007AD3 RID: 31443
		public static StatCategoryDef Source;

		// Token: 0x04007AD4 RID: 31444
		[MayRequireBiotech]
		public static StatCategoryDef Mechanoid;

		// Token: 0x04007AD5 RID: 31445
		[MayRequireAnomaly]
		public static StatCategoryDef PsychicRituals;

		// Token: 0x04007AD6 RID: 31446
		[MayRequireAnomaly]
		public static StatCategoryDef Containment;

		// Token: 0x04007AD7 RID: 31447
		[MayRequireAnomaly]
		public static StatCategoryDef Serum;

		// Token: 0x04007AD8 RID: 31448
		public static StatCategoryDef Apparel;

		// Token: 0x04007AD9 RID: 31449
		public static StatCategoryDef Implant;

		// Token: 0x04007ADA RID: 31450
		public static StatCategoryDef Weapon;

		// Token: 0x04007ADB RID: 31451
		public static StatCategoryDef Weapon_Ranged;

		// Token: 0x04007ADC RID: 31452
		public static StatCategoryDef Weapon_Melee;

		// Token: 0x04007ADD RID: 31453
		public static StatCategoryDef Ability;

		// Token: 0x04007ADE RID: 31454
		public static StatCategoryDef Building;

		// Token: 0x04007ADF RID: 31455
		public static StatCategoryDef Terrain;

		// Token: 0x04007AE0 RID: 31456
		public static StatCategoryDef PawnResistances;

		// Token: 0x04007AE1 RID: 31457
		public static StatCategoryDef PawnPsyfocus;

		// Token: 0x04007AE2 RID: 31458
		public static StatCategoryDef PawnHealth;

		// Token: 0x04007AE3 RID: 31459
		public static StatCategoryDef PawnFood;

		// Token: 0x04007AE4 RID: 31460
		public static StatCategoryDef PawnWork;

		// Token: 0x04007AE5 RID: 31461
		public static StatCategoryDef PawnCombat;

		// Token: 0x04007AE6 RID: 31462
		public static StatCategoryDef PawnSocial;

		// Token: 0x04007AE7 RID: 31463
		public static StatCategoryDef PawnMisc;

		// Token: 0x04007AE8 RID: 31464
		public static StatCategoryDef EquippedStatOffsets;

		// Token: 0x04007AE9 RID: 31465
		public static StatCategoryDef StuffStatFactors;

		// Token: 0x04007AEA RID: 31466
		public static StatCategoryDef StuffStatOffsets;

		// Token: 0x04007AEB RID: 31467
		public static StatCategoryDef Surgery;

		// Token: 0x04007AEC RID: 31468
		public static StatCategoryDef CapacityEffects;

		// Token: 0x04007AED RID: 31469
		public static StatCategoryDef Meditation;

		// Token: 0x04007AEE RID: 31470
		public static StatCategoryDef Drug;

		// Token: 0x04007AEF RID: 31471
		public static StatCategoryDef DrugAddiction;

		// Token: 0x04007AF0 RID: 31472
		public static StatCategoryDef Genetics;
	}
}
