using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F00 RID: 3840
	public class IngestibleProperties
	{
		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x060063DE RID: 25566 RVA: 0x0023CAEE File Offset: 0x0023ACEE
		public JoyKindDef JoyKind
		{
			get
			{
				return this.joyKind ?? JoyKindDefOf.Gluttonous;
			}
		}

		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x060063DF RID: 25567 RVA: 0x0023CAFF File Offset: 0x0023ACFF
		public bool HumanEdible
		{
			get
			{
				return (FoodTypeFlags.OmnivoreHuman & this.foodType) > FoodTypeFlags.None;
			}
		}

		// Token: 0x17001101 RID: 4353
		// (get) Token: 0x060063E0 RID: 25568 RVA: 0x0023CB10 File Offset: 0x0023AD10
		public bool IsMeal
		{
			get
			{
				return this.preferability >= FoodPreferability.MealAwful && this.preferability <= FoodPreferability.MealLavish;
			}
		}

		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x060063E1 RID: 25569 RVA: 0x0023CB2A File Offset: 0x0023AD2A
		public float CachedNutrition
		{
			get
			{
				if (this.cachedNutrition == -1f)
				{
					this.cachedNutrition = this.parent.GetStatValueAbstract(StatDefOf.Nutrition, null);
				}
				return this.cachedNutrition;
			}
		}

		// Token: 0x060063E2 RID: 25570 RVA: 0x0023CB56 File Offset: 0x0023AD56
		public IEnumerable<string> ConfigErrors()
		{
			IngestibleProperties.<ConfigErrors>d__44 <ConfigErrors>d__ = new IngestibleProperties.<ConfigErrors>d__44(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x060063E3 RID: 25571 RVA: 0x0023CB68 File Offset: 0x0023AD68
		public RoyalTitleDef MaxSatisfiedTitle()
		{
			return (from t in DefDatabase<FactionDef>.AllDefsListForReading.SelectMany((FactionDef f) => f.RoyalTitlesAwardableInSeniorityOrderForReading)
			where t.foodRequirement.Defined && t.foodRequirement.Acceptable(this.parent)
			orderby t.seniority descending
			select t).FirstOrDefault<RoyalTitleDef>();
		}

		// Token: 0x060063E4 RID: 25572 RVA: 0x0023CBD8 File Offset: 0x0023ADD8
		internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			IngestibleProperties.<SpecialDisplayStats>d__46 <SpecialDisplayStats>d__ = new IngestibleProperties.<SpecialDisplayStats>d__46(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x060063E5 RID: 25573 RVA: 0x0023CBE8 File Offset: 0x0023ADE8
		private bool TryGetOutcomeDoer<T>(out T doer) where T : IngestionOutcomeDoer
		{
			doer = default(T);
			if (this.outcomeDoers == null)
			{
				return false;
			}
			foreach (IngestionOutcomeDoer ingestionOutcomeDoer in this.outcomeDoers)
			{
				T t = ingestionOutcomeDoer as T;
				if (t != null)
				{
					doer = t;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400389A RID: 14490
		[Unsaved(false)]
		public ThingDef parent;

		// Token: 0x0400389B RID: 14491
		public int maxNumToIngestAtOnce;

		// Token: 0x0400389C RID: 14492
		public int defaultNumToIngestAtOnce = 20;

		// Token: 0x0400389D RID: 14493
		public List<IngestionOutcomeDoer> outcomeDoers;

		// Token: 0x0400389E RID: 14494
		public int baseIngestTicks = 500;

		// Token: 0x0400389F RID: 14495
		public float chairSearchRadius = 32f;

		// Token: 0x040038A0 RID: 14496
		public bool useEatingSpeedStat = true;

		// Token: 0x040038A1 RID: 14497
		public bool babiesCanIngest;

		// Token: 0x040038A2 RID: 14498
		public bool humanlikeOnly;

		// Token: 0x040038A3 RID: 14499
		public bool nonDrugIngestibleWithoutFoodNeed;

		// Token: 0x040038A4 RID: 14500
		public ThoughtDef tasteThought;

		// Token: 0x040038A5 RID: 14501
		public ThoughtDef specialThoughtDirect;

		// Token: 0x040038A6 RID: 14502
		public ThoughtDef specialThoughtAsIngredient;

		// Token: 0x040038A7 RID: 14503
		public HistoryEventDef ateEvent;

		// Token: 0x040038A8 RID: 14504
		public EffecterDef ingestEffect;

		// Token: 0x040038A9 RID: 14505
		public EffecterDef ingestEffectEat;

		// Token: 0x040038AA RID: 14506
		public SoundDef ingestSound;

		// Token: 0x040038AB RID: 14507
		[MustTranslate]
		public string ingestCommandString;

		// Token: 0x040038AC RID: 14508
		[MustTranslate]
		public string ingestReportString;

		// Token: 0x040038AD RID: 14509
		[MustTranslate]
		public string ingestReportStringEat;

		// Token: 0x040038AE RID: 14510
		public HoldOffsetSet ingestHoldOffsetStanding;

		// Token: 0x040038AF RID: 14511
		public bool ingestHoldUsesTable = true;

		// Token: 0x040038B0 RID: 14512
		public bool tableDesired = true;

		// Token: 0x040038B1 RID: 14513
		public bool showIngestFloatOption = true;

		// Token: 0x040038B2 RID: 14514
		public FoodTypeFlags foodType;

		// Token: 0x040038B3 RID: 14515
		public float joy;

		// Token: 0x040038B4 RID: 14516
		public JoyKindDef joyKind;

		// Token: 0x040038B5 RID: 14517
		public ThingDef sourceDef;

		// Token: 0x040038B6 RID: 14518
		public FoodPreferability preferability;

		// Token: 0x040038B7 RID: 14519
		public bool nurseable;

		// Token: 0x040038B8 RID: 14520
		public float optimalityOffsetHumanlikes;

		// Token: 0x040038B9 RID: 14521
		public float optimalityOffsetFeedingAnimals;

		// Token: 0x040038BA RID: 14522
		public DrugCategory drugCategory;

		// Token: 0x040038BB RID: 14523
		public bool canAutoSelectAsFoodForCaravan = true;

		// Token: 0x040038BC RID: 14524
		public bool lowPriorityCaravanFood;

		// Token: 0x040038BD RID: 14525
		[Unsaved(false)]
		private float cachedNutrition = -1f;
	}
}
