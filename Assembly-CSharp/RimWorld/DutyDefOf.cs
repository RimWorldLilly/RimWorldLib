using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001E87 RID: 7815
	[DefOf]
	public static class DutyDefOf
	{
		// Token: 0x0600CA42 RID: 51778 RVA: 0x004A5D24 File Offset: 0x004A3F24
		static DutyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DutyDefOf));
		}

		// Token: 0x04007A45 RID: 31301
		public static DutyDef TravelOrLeave;

		// Token: 0x04007A46 RID: 31302
		public static DutyDef TravelOrWait;

		// Token: 0x04007A47 RID: 31303
		public static DutyDef Kidnap;

		// Token: 0x04007A48 RID: 31304
		public static DutyDef Steal;

		// Token: 0x04007A49 RID: 31305
		public static DutyDef TakeWoundedGuest;

		// Token: 0x04007A4A RID: 31306
		public static DutyDef Follow;

		// Token: 0x04007A4B RID: 31307
		public static DutyDef PrisonerEscape;

		// Token: 0x04007A4C RID: 31308
		public static DutyDef PrisonerEscapeSapper;

		// Token: 0x04007A4D RID: 31309
		public static DutyDef DefendAndExpandHive;

		// Token: 0x04007A4E RID: 31310
		public static DutyDef DefendHiveAggressively;

		// Token: 0x04007A4F RID: 31311
		public static DutyDef LoadAndEnterTransporters;

		// Token: 0x04007A50 RID: 31312
		public static DutyDef EnterTransporterAndDefendSelf;

		// Token: 0x04007A51 RID: 31313
		public static DutyDef LoadAndEnterPortal;

		// Token: 0x04007A52 RID: 31314
		public static DutyDef ManClosestTurret;

		// Token: 0x04007A53 RID: 31315
		public static DutyDef SleepForever;

		// Token: 0x04007A54 RID: 31316
		public static DutyDef Idle;

		// Token: 0x04007A55 RID: 31317
		public static DutyDef IdleNoInteraction;

		// Token: 0x04007A56 RID: 31318
		public static DutyDef WanderClose;

		// Token: 0x04007A57 RID: 31319
		public static DutyDef WanderClose_NoNeeds;

		// Token: 0x04007A58 RID: 31320
		[MayRequireAnomaly]
		public static DutyDef ShamblerSwarm;

		// Token: 0x04007A59 RID: 31321
		[MayRequireAnomaly]
		public static DutyDef SightstealerSwarm;

		// Token: 0x04007A5A RID: 31322
		[MayRequireAnomaly]
		public static DutyDef SightstealerAssault;

		// Token: 0x04007A5B RID: 31323
		[MayRequireAnomaly]
		public static DutyDef GorehulkAssault;

		// Token: 0x04007A5C RID: 31324
		[MayRequireAnomaly]
		public static DutyDef DevourerAssault;

		// Token: 0x04007A5D RID: 31325
		[MayRequireAnomaly]
		public static DutyDef FleshbeastAssault;

		// Token: 0x04007A5E RID: 31326
		[MayRequireAnomaly]
		public static DutyDef PerformHateChant;

		// Token: 0x04007A5F RID: 31327
		[MayRequireAnomaly]
		public static DutyDef ChimeraStalkFlee;

		// Token: 0x04007A60 RID: 31328
		[MayRequireAnomaly]
		public static DutyDef ChimeraStalkWander;

		// Token: 0x04007A61 RID: 31329
		[MayRequireAnomaly]
		public static DutyDef ChimeraAttack;

		// Token: 0x04007A62 RID: 31330
		[MayRequireAnomaly]
		public static DutyDef DefendFleshmassHeart;

		// Token: 0x04007A63 RID: 31331
		[MayRequireAnomaly]
		public static DutyDef VoidAwakeningWander;

		// Token: 0x04007A64 RID: 31332
		[MayRequireOdyssey]
		public static DutyDef WanderNest;

		// Token: 0x04007A65 RID: 31333
		[MayRequireOdyssey]
		public static DutyDef NestAssault;

		// Token: 0x04007A66 RID: 31334
		public static DutyDef AssaultColony;

		// Token: 0x04007A67 RID: 31335
		public static DutyDef Breaching;

		// Token: 0x04007A68 RID: 31336
		public static DutyDef Sapper;

		// Token: 0x04007A69 RID: 31337
		public static DutyDef Escort;

		// Token: 0x04007A6A RID: 31338
		public static DutyDef Defend;

		// Token: 0x04007A6B RID: 31339
		[MayRequireAnomaly]
		public static DutyDef DefendInvoker;

		// Token: 0x04007A6C RID: 31340
		public static DutyDef Build;

		// Token: 0x04007A6D RID: 31341
		public static DutyDef HuntEnemiesIndividual;

		// Token: 0x04007A6E RID: 31342
		public static DutyDef DefendBase;

		// Token: 0x04007A6F RID: 31343
		[MayRequireRoyalty]
		public static DutyDef AssaultThing;

		// Token: 0x04007A70 RID: 31344
		public static DutyDef PrisonerAssaultColony;

		// Token: 0x04007A71 RID: 31345
		public static DutyDef HuntDownColonists;

		// Token: 0x04007A72 RID: 31346
		public static DutyDef ExitMapRandom;

		// Token: 0x04007A73 RID: 31347
		public static DutyDef ExitMapBest;

		// Token: 0x04007A74 RID: 31348
		public static DutyDef ExitMapBestAndDefendSelf;

		// Token: 0x04007A75 RID: 31349
		public static DutyDef ExitMapNearDutyTarget;

		// Token: 0x04007A76 RID: 31350
		public static DutyDef MarryPawn;

		// Token: 0x04007A77 RID: 31351
		public static DutyDef GiveSpeech;

		// Token: 0x04007A78 RID: 31352
		public static DutyDef Spectate;

		// Token: 0x04007A79 RID: 31353
		public static DutyDef BestowingCeremony_MoveInPlace;

		// Token: 0x04007A7A RID: 31354
		[MayRequireRoyalty]
		public static DutyDef Bestow;

		// Token: 0x04007A7B RID: 31355
		[MayRequireIdeology]
		public static DutyDef Pilgrims_Spectate;

		// Token: 0x04007A7C RID: 31356
		[MayRequireIdeology]
		public static DutyDef PlayTargetInstrument;

		// Token: 0x04007A7D RID: 31357
		[MayRequireBiotech]
		public static DutyDef SocialMeeting;

		// Token: 0x04007A7E RID: 31358
		public static DutyDef PrepareCaravan_GatherItems;

		// Token: 0x04007A7F RID: 31359
		public static DutyDef PrepareCaravan_Wait;

		// Token: 0x04007A80 RID: 31360
		public static DutyDef PrepareCaravan_GatherAnimals;

		// Token: 0x04007A81 RID: 31361
		public static DutyDef PrepareCaravan_CollectAnimals;

		// Token: 0x04007A82 RID: 31362
		public static DutyDef PrepareCaravan_GatherDownedPawns;

		// Token: 0x04007A83 RID: 31363
		public static DutyDef PrepareCaravan_Pause;

		// Token: 0x04007A84 RID: 31364
		public static DutyDef ReturnedCaravan_PenAnimals;

		// Token: 0x04007A85 RID: 31365
		[MayRequireAnomaly]
		public static DutyDef Goto;

		// Token: 0x04007A86 RID: 31366
		[MayRequireAnomaly]
		public static DutyDef Goto_NoZeroLengthPaths;

		// Token: 0x04007A87 RID: 31367
		[MayRequireAnomaly]
		public static DutyDef Invoke;

		// Token: 0x04007A88 RID: 31368
		[MayRequireAnomaly]
		public static DutyDef PsychicRitualDance;

		// Token: 0x04007A89 RID: 31369
		[MayRequireAnomaly]
		public static DutyDef WaitForRitualParticipants;

		// Token: 0x04007A8A RID: 31370
		[MayRequireAnomaly]
		public static DutyDef DeliverPawnToPsychicRitualCell;

		// Token: 0x04007A8B RID: 31371
		[MayRequireAnomaly]
		public static DutyDef GatherOfferingsForPsychicRitual;
	}
}
