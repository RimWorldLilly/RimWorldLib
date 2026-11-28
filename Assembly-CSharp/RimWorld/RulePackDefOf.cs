using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EA7 RID: 7847
	[DefOf]
	public static class RulePackDefOf
	{
		// Token: 0x0600CA62 RID: 51810 RVA: 0x004A5F44 File Offset: 0x004A4144
		static RulePackDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RulePackDefOf));
		}

		// Token: 0x04007DF4 RID: 32244
		public static RulePackDef Sentence_SocialFightStarted;

		// Token: 0x04007DF5 RID: 32245
		public static RulePackDef Sentence_RomanceAttemptAccepted;

		// Token: 0x04007DF6 RID: 32246
		public static RulePackDef Sentence_RomanceAttemptRejected;

		// Token: 0x04007DF7 RID: 32247
		public static RulePackDef Sentence_MarriageProposalAccepted;

		// Token: 0x04007DF8 RID: 32248
		public static RulePackDef Sentence_MarriageProposalRejected;

		// Token: 0x04007DF9 RID: 32249
		public static RulePackDef Sentence_MarriageProposalRejectedBrokeUp;

		// Token: 0x04007DFA RID: 32250
		public static RulePackDef Sentence_RecruitAttemptAccepted;

		// Token: 0x04007DFB RID: 32251
		public static RulePackDef Sentence_RecruitAttemptRejected;

		// Token: 0x04007DFC RID: 32252
		[MayRequireIdeology]
		public static RulePackDef Sentence_ConvertIdeoAttemptSuccess;

		// Token: 0x04007DFD RID: 32253
		[MayRequireIdeology]
		public static RulePackDef Sentence_ConvertIdeoAttemptFail;

		// Token: 0x04007DFE RID: 32254
		[MayRequireIdeology]
		public static RulePackDef Sentence_ConvertIdeoAttemptFailSocialFight;

		// Token: 0x04007DFF RID: 32255
		[MayRequireIdeology]
		public static RulePackDef Sentence_ConvertIdeoAttemptFailResentment;

		// Token: 0x04007E00 RID: 32256
		[MayRequireBiotech]
		public static RulePackDef NamerGenepack;

		// Token: 0x04007E01 RID: 32257
		[MayRequireBiotech]
		public static RulePackDef NamerXenotype;

		// Token: 0x04007E02 RID: 32258
		[MayRequireBiotech]
		public static RulePackDef GrowthMomentFlavor;

		// Token: 0x04007E03 RID: 32259
		public static RulePackDef ArtDescriptionRoot_HasTale;

		// Token: 0x04007E04 RID: 32260
		public static RulePackDef ArtDescriptionRoot_Taleless;

		// Token: 0x04007E05 RID: 32261
		public static RulePackDef ArtDescriptionUtility_Global;

		// Token: 0x04007E06 RID: 32262
		public static RulePackDef GlobalUtility;

		// Token: 0x04007E07 RID: 32263
		public static RulePackDef TalelessImages;

		// Token: 0x04007E08 RID: 32264
		public static RulePackDef NamerWorld;

		// Token: 0x04007E09 RID: 32265
		public static RulePackDef NamerTraderGeneral;

		// Token: 0x04007E0A RID: 32266
		public static RulePackDef NamerScenario;

		// Token: 0x04007E0B RID: 32267
		public static RulePackDef NamerQuestDefault;

		// Token: 0x04007E0C RID: 32268
		public static RulePackDef NamerArtSculpture;

		// Token: 0x04007E0D RID: 32269
		public static RulePackDef ArtDescription_Sculpture;

		// Token: 0x04007E0E RID: 32270
		public static RulePackDef NamerArtWeaponMelee;

		// Token: 0x04007E0F RID: 32271
		public static RulePackDef ArtDescription_WeaponMelee;

		// Token: 0x04007E10 RID: 32272
		public static RulePackDef NamerArtWeaponGun;

		// Token: 0x04007E11 RID: 32273
		public static RulePackDef ArtDescription_WeaponGun;

		// Token: 0x04007E12 RID: 32274
		public static RulePackDef NamerArtFurniture;

		// Token: 0x04007E13 RID: 32275
		public static RulePackDef ArtDescription_Furniture;

		// Token: 0x04007E14 RID: 32276
		public static RulePackDef NamerArtSarcophagusPlate;

		// Token: 0x04007E15 RID: 32277
		public static RulePackDef ArtDescription_SarcophagusPlate;

		// Token: 0x04007E16 RID: 32278
		public static RulePackDef SeedGenerator;

		// Token: 0x04007E17 RID: 32279
		public static RulePackDef Combat_RangedFire;

		// Token: 0x04007E18 RID: 32280
		public static RulePackDef Combat_RangedDamage;

		// Token: 0x04007E19 RID: 32281
		public static RulePackDef Combat_RangedDeflect;

		// Token: 0x04007E1A RID: 32282
		public static RulePackDef Combat_RangedMiss;

		// Token: 0x04007E1B RID: 32283
		public static RulePackDef Combat_ExplosionImpact;

		// Token: 0x04007E1C RID: 32284
		public static RulePackDef Transition_Downed;

		// Token: 0x04007E1D RID: 32285
		public static RulePackDef Transition_Died;

		// Token: 0x04007E1E RID: 32286
		public static RulePackDef Transition_DiedExplosive;

		// Token: 0x04007E1F RID: 32287
		public static RulePackDef DamageEvent_Ceiling;

		// Token: 0x04007E20 RID: 32288
		public static RulePackDef DamageEvent_Fire;

		// Token: 0x04007E21 RID: 32289
		public static RulePackDef DamageEvent_PowerBeam;

		// Token: 0x04007E22 RID: 32290
		public static RulePackDef DamageEvent_Tornado;

		// Token: 0x04007E23 RID: 32291
		public static RulePackDef DamageEvent_TrapSpike;

		// Token: 0x04007E24 RID: 32292
		[MayRequireAnomaly]
		public static RulePackDef DamageEvent_UnnaturalDarkness;

		// Token: 0x04007E25 RID: 32293
		public static RulePackDef Event_Stun;

		// Token: 0x04007E26 RID: 32294
		public static RulePackDef Event_AbilityUsed;

		// Token: 0x04007E27 RID: 32295
		public static RulePackDef Event_ItemUsed;

		// Token: 0x04007E28 RID: 32296
		public static RulePackDef Battle_Solo;

		// Token: 0x04007E29 RID: 32297
		public static RulePackDef Battle_Duel;

		// Token: 0x04007E2A RID: 32298
		public static RulePackDef Battle_Internal;

		// Token: 0x04007E2B RID: 32299
		public static RulePackDef Battle_War;

		// Token: 0x04007E2C RID: 32300
		public static RulePackDef Battle_Brawl;

		// Token: 0x04007E2D RID: 32301
		public static RulePackDef DynamicWrapper;

		// Token: 0x04007E2E RID: 32302
		[MayRequireAnomaly]
		public static RulePackDef LabyrinthRamblings;

		// Token: 0x04007E2F RID: 32303
		[MayRequireAnomaly]
		public static RulePackDef NamerBiosignature;

		// Token: 0x04007E30 RID: 32304
		[MayRequireAnomaly]
		public static RulePackDef NamerArtCubeSculpture;

		// Token: 0x04007E31 RID: 32305
		[MayRequireAnomaly]
		public static RulePackDef ArtDescription_CubeSculpture;

		// Token: 0x04007E32 RID: 32306
		[MayRequireAnomaly]
		public static RulePackDef NamerArtVoidSculpture;

		// Token: 0x04007E33 RID: 32307
		[MayRequireAnomaly]
		public static RulePackDef ArtDescription_VoidSculpture;

		// Token: 0x04007E34 RID: 32308
		[MayRequireAnomaly]
		public static RulePackDef RevenantNoises;

		// Token: 0x04007E35 RID: 32309
		[MayRequireAnomaly]
		public static RulePackDef NamerPersonCreepjoiner;

		// Token: 0x04007E36 RID: 32310
		[MayRequireAnomaly]
		public static RulePackDef Event_UnnaturalCorpseAttack;

		// Token: 0x04007E37 RID: 32311
		[MayRequireAnomaly]
		public static RulePackDef Event_DevourerConsumeLeap;

		// Token: 0x04007E38 RID: 32312
		[MayRequireAnomaly]
		public static RulePackDef Event_DevourerDigestionAborted;

		// Token: 0x04007E39 RID: 32313
		[MayRequireAnomaly]
		public static RulePackDef Event_DevourerDigestionCompleted;

		// Token: 0x04007E3A RID: 32314
		[MayRequireAnomaly]
		public static RulePackDef Event_MetalhorrorEmerged;

		// Token: 0x04007E3B RID: 32315
		[MayRequireAnomaly]
		public static RulePackDef Event_Hypnotized;

		// Token: 0x04007E3C RID: 32316
		[MayRequireOdyssey]
		public static RulePackDef NamerGravship;

		// Token: 0x04007E3D RID: 32317
		[MayRequireOdyssey]
		public static RulePackDef NamerUniqueWeapon;
	}
}
