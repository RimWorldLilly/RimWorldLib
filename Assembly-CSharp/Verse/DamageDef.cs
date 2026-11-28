using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200012D RID: 301
	public class DamageDef : Def
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x00036AB1 File Offset: 0x00034CB1
		public DamageWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (DamageWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00036AE4 File Offset: 0x00034CE4
		public bool ExternalViolenceFor(Thing thing)
		{
			if (this.externalViolence)
			{
				return true;
			}
			if (this.externalViolenceForMechanoids)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.RaceProps.IsMechanoid)
				{
					return true;
				}
				if (thing is Building_Turret)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400053B RID: 1339
		public Type workerClass = typeof(DamageWorker);

		// Token: 0x0400053C RID: 1340
		private bool externalViolence;

		// Token: 0x0400053D RID: 1341
		private bool externalViolenceForMechanoids;

		// Token: 0x0400053E RID: 1342
		public bool hasForcefulImpact = true;

		// Token: 0x0400053F RID: 1343
		public bool harmsHealth = true;

		// Token: 0x04000540 RID: 1344
		public bool makesBlood = true;

		// Token: 0x04000541 RID: 1345
		public bool canInterruptJobs = true;

		// Token: 0x04000542 RID: 1346
		public bool isRanged;

		// Token: 0x04000543 RID: 1347
		public bool makesAnimalsFlee;

		// Token: 0x04000544 RID: 1348
		public bool execution;

		// Token: 0x04000545 RID: 1349
		public RulePackDef combatLogRules;

		// Token: 0x04000546 RID: 1350
		public float buildingDamageFactor = 1f;

		// Token: 0x04000547 RID: 1351
		public float buildingDamageFactorPassable = 1f;

		// Token: 0x04000548 RID: 1352
		public float buildingDamageFactorImpassable = 1f;

		// Token: 0x04000549 RID: 1353
		public float plantDamageFactor = 1f;

		// Token: 0x0400054A RID: 1354
		public float corpseDamageFactor = 1f;

		// Token: 0x0400054B RID: 1355
		public bool causeStun;

		// Token: 0x0400054C RID: 1356
		public int stunAdaptationTicks;

		// Token: 0x0400054D RID: 1357
		public int? constantStunDurationTicks;

		// Token: 0x0400054E RID: 1358
		public StatDef stunResistStat;

		// Token: 0x0400054F RID: 1359
		public bool displayAdaptedTextMote = true;

		// Token: 0x04000550 RID: 1360
		[MustTranslate]
		public string adaptedText;

		// Token: 0x04000551 RID: 1361
		public bool canUseDeflectMetalEffect = true;

		// Token: 0x04000552 RID: 1362
		public ImpactSoundTypeDef impactSoundType;

		// Token: 0x04000553 RID: 1363
		[MustTranslate]
		public string deathMessage = "{0} has been killed.";

		// Token: 0x04000554 RID: 1364
		public EffecterDef damageEffecter;

		// Token: 0x04000555 RID: 1365
		public int defaultDamage = -1;

		// Token: 0x04000556 RID: 1366
		public float defaultArmorPenetration = -1f;

		// Token: 0x04000557 RID: 1367
		public float defaultStoppingPower;

		// Token: 0x04000558 RID: 1368
		public List<DamageDefAdditionalHediff> additionalHediffs;

		// Token: 0x04000559 RID: 1369
		public List<HediffDef> additionalHediffsThisPart;

		// Token: 0x0400055A RID: 1370
		public bool applyAdditionalHediffsIfHuntingForFood = true;

		// Token: 0x0400055B RID: 1371
		public DamageArmorCategoryDef armorCategory;

		// Token: 0x0400055C RID: 1372
		public int minDamageToFragment = 99999;

		// Token: 0x0400055D RID: 1373
		public FloatRange overkillPctToDestroyPart = new FloatRange(0f, 0.7f);

		// Token: 0x0400055E RID: 1374
		public bool consideredHelpful;

		// Token: 0x0400055F RID: 1375
		public SimpleCurve igniteChanceByTargetFlammability;

		// Token: 0x04000560 RID: 1376
		public float igniteCellChance;

		// Token: 0x04000561 RID: 1377
		public bool ignoreShields;

		// Token: 0x04000562 RID: 1378
		public bool harmAllLayersUntilOutside;

		// Token: 0x04000563 RID: 1379
		public HediffDef hediff;

		// Token: 0x04000564 RID: 1380
		public HediffDef hediffSkin;

		// Token: 0x04000565 RID: 1381
		public HediffDef hediffSolid;

		// Token: 0x04000566 RID: 1382
		public bool isExplosive;

		// Token: 0x04000567 RID: 1383
		public float explosionSnowMeltAmount = 1f;

		// Token: 0x04000568 RID: 1384
		public bool explosionAffectOutsidePartsOnly = true;

		// Token: 0x04000569 RID: 1385
		public ThingDef explosionCellMote;

		// Token: 0x0400056A RID: 1386
		public FleckDef explosionCellFleck;

		// Token: 0x0400056B RID: 1387
		public Color explosionColorCenter = Color.white;

		// Token: 0x0400056C RID: 1388
		public Color explosionColorEdge = Color.white;

		// Token: 0x0400056D RID: 1389
		public EffecterDef explosionInteriorEffecter;

		// Token: 0x0400056E RID: 1390
		public ThingDef explosionInteriorMote;

		// Token: 0x0400056F RID: 1391
		public FleckDef explosionInteriorFleck;

		// Token: 0x04000570 RID: 1392
		public ThingDef explosionCenterMote;

		// Token: 0x04000571 RID: 1393
		public FleckDef explosionCenterFleck;

		// Token: 0x04000572 RID: 1394
		public EffecterDef explosionCenterEffecter;

		// Token: 0x04000573 RID: 1395
		public EffecterDef explosionCellEffecter;

		// Token: 0x04000574 RID: 1396
		public float explosionCellEffecterChance;

		// Token: 0x04000575 RID: 1397
		public float explosionCellEffecterMaxRadius;

		// Token: 0x04000576 RID: 1398
		public float explosionHeatEnergyPerCell;

		// Token: 0x04000577 RID: 1399
		public float expolosionPropagationSpeed = 1f;

		// Token: 0x04000578 RID: 1400
		public SoundDef soundExplosion;

		// Token: 0x04000579 RID: 1401
		public float explosionInteriorCellCountMultiplier = 1f;

		// Token: 0x0400057A RID: 1402
		public float explosionInteriorCellDistanceMultiplier = 0.7f;

		// Token: 0x0400057B RID: 1403
		public float stabChanceOfForcedInternal;

		// Token: 0x0400057C RID: 1404
		public SimpleCurve cutExtraTargetsCurve;

		// Token: 0x0400057D RID: 1405
		public float cutCleaveBonus;

		// Token: 0x0400057E RID: 1406
		public float bluntInnerHitChance;

		// Token: 0x0400057F RID: 1407
		public FloatRange bluntInnerHitDamageFractionToConvert;

		// Token: 0x04000580 RID: 1408
		public FloatRange bluntInnerHitDamageFractionToAdd;

		// Token: 0x04000581 RID: 1409
		public float bluntStunDuration = 1f;

		// Token: 0x04000582 RID: 1410
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToHeadCurve;

		// Token: 0x04000583 RID: 1411
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToBodyCurve;

		// Token: 0x04000584 RID: 1412
		public float scratchSplitPercentage = 0.5f;

		// Token: 0x04000585 RID: 1413
		public bool scaleDamageToBuildingsBasedOnFlammability;

		// Token: 0x04000586 RID: 1414
		[Unsaved(false)]
		private DamageWorker workerInt;
	}
}
