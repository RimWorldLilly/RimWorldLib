using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001572 RID: 5490
	public class Need_Food : Need
	{
		// Token: 0x170016E1 RID: 5857
		// (get) Token: 0x0600869F RID: 34463 RVA: 0x0030AAF5 File Offset: 0x00308CF5
		public bool Starving
		{
			get
			{
				return this.CurCategory == HungerCategory.Starving;
			}
		}

		// Token: 0x170016E2 RID: 5858
		// (get) Token: 0x060086A0 RID: 34464 RVA: 0x0030AB00 File Offset: 0x00308D00
		public float PercentageThreshUrgentlyHungry
		{
			get
			{
				return this.pawn.RaceProps.FoodLevelPercentageWantEat * 0.4f;
			}
		}

		// Token: 0x170016E3 RID: 5859
		// (get) Token: 0x060086A1 RID: 34465 RVA: 0x0030AB18 File Offset: 0x00308D18
		public float PercentageThreshHungry
		{
			get
			{
				return this.pawn.RaceProps.FoodLevelPercentageWantEat * 0.8f;
			}
		}

		// Token: 0x170016E4 RID: 5860
		// (get) Token: 0x060086A2 RID: 34466 RVA: 0x0030AB30 File Offset: 0x00308D30
		public float NutritionBetweenHungryAndFed
		{
			get
			{
				return (1f - this.PercentageThreshHungry) * this.MaxLevel;
			}
		}

		// Token: 0x170016E5 RID: 5861
		// (get) Token: 0x060086A3 RID: 34467 RVA: 0x0030AB48 File Offset: 0x00308D48
		private CompHoldingPlatformTarget PlatformTarget
		{
			get
			{
				CompHoldingPlatformTarget result;
				if ((result = this.platformComp) == null)
				{
					result = (this.platformComp = this.pawn.TryGetComp<CompHoldingPlatformTarget>());
				}
				return result;
			}
		}

		// Token: 0x170016E6 RID: 5862
		// (get) Token: 0x060086A4 RID: 34468 RVA: 0x0030AB73 File Offset: 0x00308D73
		public HungerCategory CurCategory
		{
			get
			{
				if (base.CurLevelPercentage <= 0f)
				{
					return HungerCategory.Starving;
				}
				if (base.CurLevelPercentage < this.PercentageThreshUrgentlyHungry)
				{
					return HungerCategory.UrgentlyHungry;
				}
				if (base.CurLevelPercentage < this.PercentageThreshHungry)
				{
					return HungerCategory.Hungry;
				}
				return HungerCategory.Fed;
			}
		}

		// Token: 0x170016E7 RID: 5863
		// (get) Token: 0x060086A5 RID: 34469 RVA: 0x0030ABA5 File Offset: 0x00308DA5
		public float FoodFallPerTick
		{
			get
			{
				return this.FoodFallPerTickAssumingCategory(this.CurCategory, false);
			}
		}

		// Token: 0x170016E8 RID: 5864
		// (get) Token: 0x060086A6 RID: 34470 RVA: 0x0030ABB4 File Offset: 0x00308DB4
		public int TicksUntilHungryWhenFed
		{
			get
			{
				return Mathf.CeilToInt(this.NutritionBetweenHungryAndFed / this.FoodFallPerTickAssumingCategory(HungerCategory.Fed, false));
			}
		}

		// Token: 0x170016E9 RID: 5865
		// (get) Token: 0x060086A7 RID: 34471 RVA: 0x0030ABCA File Offset: 0x00308DCA
		public int TicksUntilHungryWhenFedIgnoringMalnutrition
		{
			get
			{
				return Mathf.CeilToInt(this.NutritionBetweenHungryAndFed / this.FoodFallPerTickAssumingCategory(HungerCategory.Fed, true));
			}
		}

		// Token: 0x170016EA RID: 5866
		// (get) Token: 0x060086A8 RID: 34472 RVA: 0x0030ABE0 File Offset: 0x00308DE0
		public override int GUIChangeArrow
		{
			get
			{
				if (this.GainingFood())
				{
					return 1;
				}
				if (this.FoodFallPerTickAssumingCategory(HungerCategory.Hungry, false) <= 0f)
				{
					return 0;
				}
				return -1;
			}
		}

		// Token: 0x170016EB RID: 5867
		// (get) Token: 0x060086A9 RID: 34473 RVA: 0x0030ABFE File Offset: 0x00308DFE
		public override float MaxLevel
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return this.pawn.BodySize * this.pawn.ageTracker.CurLifeStage.foodMaxFactor;
				}
				return this.pawn.GetStatValue(StatDefOf.MaxNutrition, true, 15);
			}
		}

		// Token: 0x170016EC RID: 5868
		// (get) Token: 0x060086AA RID: 34474 RVA: 0x0030AC3D File Offset: 0x00308E3D
		public float NutritionWanted
		{
			get
			{
				return this.MaxLevel - this.CurLevel;
			}
		}

		// Token: 0x170016ED RID: 5869
		// (get) Token: 0x060086AB RID: 34475 RVA: 0x0030AC4C File Offset: 0x00308E4C
		public int TicksStarving
		{
			get
			{
				return Mathf.Max(0, Find.TickManager.TicksGame - this.lastNonStarvingTick);
			}
		}

		// Token: 0x170016EE RID: 5870
		// (get) Token: 0x060086AC RID: 34476 RVA: 0x0030AC65 File Offset: 0x00308E65
		private float MalnutritionSeverityPerInterval
		{
			get
			{
				return 0.0011325f * Mathf.Lerp(0.8f, 1.2f, Rand.ValueSeeded(this.pawn.thingIDNumber ^ 2551674));
			}
		}

		// Token: 0x170016EF RID: 5871
		// (get) Token: 0x060086AD RID: 34477 RVA: 0x0030AC92 File Offset: 0x00308E92
		protected override bool IsFrozen
		{
			get
			{
				if (!base.IsFrozen && !this.pawn.Deathresting)
				{
					CompHoldingPlatformTarget platformTarget = this.PlatformTarget;
					return platformTarget != null && platformTarget.CurrentlyHeldOnPlatform;
				}
				return true;
			}
		}

		// Token: 0x060086AE RID: 34478 RVA: 0x0030ACBC File Offset: 0x00308EBC
		public Need_Food(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x060086AF RID: 34479 RVA: 0x0030ACD0 File Offset: 0x00308ED0
		public bool GainingFood()
		{
			Pawn_JobTracker jobs = this.pawn.jobs;
			IEatingDriver eatingDriver = ((jobs != null) ? jobs.curDriver : null) as IEatingDriver;
			if (eatingDriver != null && eatingDriver.GainingNutritionNow)
			{
				return true;
			}
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (ModsConfig.BiotechActive && ChildcareUtility.CanSuckle(this.pawn, out breastfeedFailReason))
			{
				Pawn carriedBy = this.pawn.CarriedBy;
				JobDriver_FeedBaby jobDriver_FeedBaby = ((carriedBy != null) ? carriedBy.jobs.curDriver : null) as JobDriver_FeedBaby;
				if (jobDriver_FeedBaby != null && jobDriver_FeedBaby.Feeding)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060086B0 RID: 34480 RVA: 0x0030AD50 File Offset: 0x00308F50
		public float FoodFallPerTickAssumingCategory(HungerCategory hunger, bool ignoreMalnutrition = false)
		{
			Building_Bed building_Bed = this.pawn.CurrentBed();
			float num = Need_Food.BaseHungerRate(this.pawn.ageTracker.CurLifeStage, this.pawn.def) * hunger.HungerMultiplier() * this.pawn.health.hediffSet.GetHungerRateFactor(ignoreMalnutrition ? HediffDefOf.Malnutrition : null);
			Pawn_StoryTracker story = this.pawn.story;
			float? num2;
			if (story == null)
			{
				num2 = null;
			}
			else
			{
				TraitSet traits = story.traits;
				num2 = ((traits != null) ? new float?(traits.HungerRateFactor) : null);
			}
			float? num3 = num2;
			float num4 = num * num3.GetValueOrDefault(1f) * ((building_Bed != null) ? building_Bed.GetStatValue(StatDefOf.BedHungerRateFactor, true, -1) : 1f);
			if (ModsConfig.BiotechActive)
			{
				Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating, false);
				if (firstHediffOfDef != null)
				{
					HediffComp_Lactating hediffComp_Lactating = firstHediffOfDef.TryGetComp<HediffComp_Lactating>();
					if (hediffComp_Lactating != null)
					{
						num4 += hediffComp_Lactating.AddedNutritionPerDay() / 60000f;
					}
				}
			}
			if (ModsConfig.BiotechActive && this.pawn.genes != null)
			{
				int num5 = 0;
				foreach (Gene gene in this.pawn.genes.GenesListForReading)
				{
					if (!gene.Overridden)
					{
						num5 += gene.def.biostatMet;
					}
				}
				num4 *= GeneTuning.MetabolismToFoodConsumptionFactorCurve.Evaluate((float)num5);
			}
			if (ModsConfig.AnomalyActive)
			{
				CompHoldingPlatformTarget platformTarget = this.PlatformTarget;
				if (platformTarget != null && platformTarget.CurrentlyHeldOnPlatform)
				{
					num4 = 0f;
				}
			}
			return num4;
		}

		// Token: 0x060086B1 RID: 34481 RVA: 0x0030AF00 File Offset: 0x00309100
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= this.FoodFallPerTick * 150f;
			}
			if (!this.Starving)
			{
				this.lastNonStarvingTick = Find.TickManager.TicksGame;
			}
			if (!this.IsFrozen || this.pawn.Deathresting)
			{
				if (this.Starving)
				{
					HealthUtility.AdjustSeverity(this.pawn, HediffDefOf.Malnutrition, this.MalnutritionSeverityPerInterval);
					return;
				}
				HealthUtility.AdjustSeverity(this.pawn, HediffDefOf.Malnutrition, -this.MalnutritionSeverityPerInterval);
			}
		}

		// Token: 0x060086B2 RID: 34482 RVA: 0x0030AF94 File Offset: 0x00309194
		public override void SetInitialLevel()
		{
			StatDefOf.MaxNutrition.Worker.ClearCacheForThing(this.pawn);
			base.CurLevelPercentage = (this.pawn.RaceProps.Humanlike ? 0.8f : Rand.Range(0.5f, 0.9f));
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.lastNonStarvingTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060086B3 RID: 34483 RVA: 0x0030AFFC File Offset: 0x003091FC
		public override void OnNeedRemoved()
		{
			Hediff hediff;
			if (this.pawn.health.hediffSet.TryGetHediff(HediffDefOf.Malnutrition, out hediff))
			{
				this.pawn.health.RemoveHediff(hediff);
			}
		}

		// Token: 0x060086B4 RID: 34484 RVA: 0x0030B038 File Offset: 0x00309238
		public override string GetTipString()
		{
			return string.Concat(new string[]
			{
				(base.LabelCap + ": " + base.CurLevelPercentage.ToStringPercent()).Colorize(ColoredText.TipSectionTitleColor),
				" (",
				this.CurLevel.ToString("0.##"),
				" / ",
				this.MaxLevel.ToString("0.##"),
				")\n",
				this.def.description
			});
		}

		// Token: 0x060086B5 RID: 34485 RVA: 0x0030B0CC File Offset: 0x003092CC
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true, Rect? rectForTooltip = null, bool drawLabel = true)
		{
			if (this.threshPercents == null)
			{
				this.threshPercents = new List<float>();
			}
			this.threshPercents.Clear();
			this.threshPercents.Add(this.PercentageThreshHungry);
			this.threshPercents.Add(this.PercentageThreshUrgentlyHungry);
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip, rectForTooltip, drawLabel);
		}

		// Token: 0x060086B6 RID: 34486 RVA: 0x0030B12A File Offset: 0x0030932A
		public static float BaseHungerRate(LifeStageDef lifeStage, ThingDef pawnDef)
		{
			return lifeStage.hungerRateFactor * pawnDef.race.baseHungerRate * 2.6666667E-05f;
		}

		// Token: 0x060086B7 RID: 34487 RVA: 0x0030B144 File Offset: 0x00309344
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastNonStarvingTick, "lastNonStarvingTick", -99999, false);
		}

		// Token: 0x04005300 RID: 21248
		public int lastNonStarvingTick = -99999;

		// Token: 0x04005301 RID: 21249
		public const float BaseFoodFallPerTick = 2.6666667E-05f;

		// Token: 0x04005302 RID: 21250
		private const float BaseMalnutritionSeverityPerDay = 0.453f;

		// Token: 0x04005303 RID: 21251
		private const float BaseMalnutritionSeverityPerInterval = 0.0011325f;

		// Token: 0x04005304 RID: 21252
		private CompHoldingPlatformTarget platformComp;
	}
}
