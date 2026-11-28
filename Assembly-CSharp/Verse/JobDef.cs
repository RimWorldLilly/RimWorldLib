using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200017F RID: 383
	public class JobDef : Def
	{
		// Token: 0x06000B1D RID: 2845 RVA: 0x0003C8C7 File Offset: 0x0003AAC7
		public override IEnumerable<string> ConfigErrors()
		{
			JobDef.<ConfigErrors>d__37 <ConfigErrors>d__ = new JobDef.<ConfigErrors>d__37(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400092C RID: 2348
		public Type driverClass;

		// Token: 0x0400092D RID: 2349
		[MustTranslate]
		public string reportString = "Doing something.";

		// Token: 0x0400092E RID: 2350
		public bool playerInterruptible = true;

		// Token: 0x0400092F RID: 2351
		public bool forceCompleteBeforeNextJob;

		// Token: 0x04000930 RID: 2352
		public CheckJobOverrideOnDamageMode checkOverrideOnDamage = CheckJobOverrideOnDamageMode.Always;

		// Token: 0x04000931 RID: 2353
		public bool alwaysShowWeapon;

		// Token: 0x04000932 RID: 2354
		public bool neverShowWeapon;

		// Token: 0x04000933 RID: 2355
		public bool suspendable = true;

		// Token: 0x04000934 RID: 2356
		public bool casualInterruptible = true;

		// Token: 0x04000935 RID: 2357
		public bool allowOpportunisticPrefix;

		// Token: 0x04000936 RID: 2358
		public bool collideWithPawns;

		// Token: 0x04000937 RID: 2359
		public bool isIdle;

		// Token: 0x04000938 RID: 2360
		public TaleDef taleOnCompletion;

		// Token: 0x04000939 RID: 2361
		public bool neverFleeFromEnemies;

		// Token: 0x0400093A RID: 2362
		public bool sleepCanInterrupt = true;

		// Token: 0x0400093B RID: 2363
		public bool makeTargetPrisoner;

		// Token: 0x0400093C RID: 2364
		public int waitAfterArriving;

		// Token: 0x0400093D RID: 2365
		public bool carryThingAfterJob;

		// Token: 0x0400093E RID: 2366
		public bool dropThingBeforeJob = true;

		// Token: 0x0400093F RID: 2367
		public bool isCrawlingIfDowned = true;

		// Token: 0x04000940 RID: 2368
		public bool alwaysShowReport;

		// Token: 0x04000941 RID: 2369
		public bool abilityCasting;

		// Token: 0x04000942 RID: 2370
		public bool tryStartFlying;

		// Token: 0x04000943 RID: 2371
		public bool ifFlyingKeepFlying;

		// Token: 0x04000944 RID: 2372
		public float overrideFlyChance = -1f;

		// Token: 0x04000945 RID: 2373
		public bool displayAsAreaInFloatMenu = true;

		// Token: 0x04000946 RID: 2374
		public bool ignoreFenceBlocked;

		// Token: 0x04000947 RID: 2375
		public int joyDuration = 4000;

		// Token: 0x04000948 RID: 2376
		public int joyMaxParticipants = 1;

		// Token: 0x04000949 RID: 2377
		public float joyGainRate = 1f;

		// Token: 0x0400094A RID: 2378
		public SkillDef joySkill;

		// Token: 0x0400094B RID: 2379
		public float joyXpPerTick;

		// Token: 0x0400094C RID: 2380
		public JoyKindDef joyKind;

		// Token: 0x0400094D RID: 2381
		public Rot4 faceDir = Rot4.Invalid;

		// Token: 0x0400094E RID: 2382
		public int learningDuration = 20000;

		// Token: 0x0400094F RID: 2383
		public ReservationLayerDef containerReservationLayer;

		// Token: 0x04000950 RID: 2384
		public bool boardingGravship;
	}
}
