using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000150 RID: 336
	public class RaceProperties
	{
		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x00038C73 File Offset: 0x00036E73
		public bool Humanlike
		{
			get
			{
				return this.intelligence >= Intelligence.Humanlike;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000A32 RID: 2610 RVA: 0x00038C81 File Offset: 0x00036E81
		public bool ToolUser
		{
			get
			{
				return this.intelligence >= Intelligence.ToolUser;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x00038C8F File Offset: 0x00036E8F
		public bool Animal
		{
			get
			{
				return !this.ToolUser && this.IsFlesh && !this.IsAnomalyEntity;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x00038CAC File Offset: 0x00036EAC
		public bool Insect
		{
			get
			{
				return this.FleshType == FleshTypeDefOf.Insectoid;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x00038CBB File Offset: 0x00036EBB
		public bool Dryad
		{
			get
			{
				return this.animalType == AnimalType.Dryad;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x00038CC6 File Offset: 0x00036EC6
		public bool ShouldHaveAbilityTracker
		{
			get
			{
				return this.Humanlike || this.IsMechanoid;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x00038CD8 File Offset: 0x00036ED8
		public bool EatsFood
		{
			get
			{
				return this.foodType > FoodTypeFlags.None;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x00038CE4 File Offset: 0x00036EE4
		public float FoodLevelPercentageWantEat
		{
			get
			{
				switch (this.ResolvedDietCategory)
				{
				case DietCategory.NeverEats:
					return 0.3f;
				case DietCategory.Herbivorous:
					return 0.45f;
				case DietCategory.Dendrovorous:
					return 0.45f;
				case DietCategory.Ovivorous:
					return 0.4f;
				case DietCategory.Omnivorous:
					return 0.3f;
				case DietCategory.Carnivorous:
					return 0.3f;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x00038D44 File Offset: 0x00036F44
		public DietCategory ResolvedDietCategory
		{
			get
			{
				if (!this.EatsFood)
				{
					return DietCategory.NeverEats;
				}
				if (this.Eats(FoodTypeFlags.Tree))
				{
					return DietCategory.Dendrovorous;
				}
				if (this.Eats(FoodTypeFlags.Meat))
				{
					if (this.Eats(FoodTypeFlags.VegetableOrFruit) || this.Eats(FoodTypeFlags.Plant))
					{
						return DietCategory.Omnivorous;
					}
					return DietCategory.Carnivorous;
				}
				else
				{
					if (this.Eats(FoodTypeFlags.AnimalProduct))
					{
						return DietCategory.Ovivorous;
					}
					return DietCategory.Herbivorous;
				}
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00038D97 File Offset: 0x00036F97
		public DeathActionWorker DeathActionWorker
		{
			get
			{
				return this.deathAction.Worker;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00038DA4 File Offset: 0x00036FA4
		public FleshTypeDef FleshType
		{
			get
			{
				return this.fleshType ?? FleshTypeDefOf.Normal;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x00038DB5 File Offset: 0x00036FB5
		public bool IsMechanoid
		{
			get
			{
				return this.FleshType == FleshTypeDefOf.Mechanoid;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x00038DC4 File Offset: 0x00036FC4
		public bool IsFlesh
		{
			get
			{
				return this.FleshType.isOrganic;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x00038DD1 File Offset: 0x00036FD1
		public bool IsAnomalyEntity
		{
			get
			{
				return ModsConfig.AnomalyActive && (this.FleshType == FleshTypeDefOf.EntityMechanical || this.FleshType == FleshTypeDefOf.EntityFlesh || this.FleshType == FleshTypeDefOf.Fleshbeast);
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x00038E05 File Offset: 0x00037005
		public bool IsDrone
		{
			get
			{
				return ModsConfig.OdysseyActive && this.FleshType == FleshTypeDefOf.Drone;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x00038E1D File Offset: 0x0003701D
		public ThingDef BloodDef
		{
			get
			{
				return this.bloodDef;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x00038E25 File Offset: 0x00037025
		public ThingDef BloodSmearDef
		{
			get
			{
				return this.bloodSmearDef;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x00038E2D File Offset: 0x0003702D
		public bool CanDoHerdMigration
		{
			get
			{
				return this.Animal && this.herdMigrationAllowed;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000A43 RID: 2627 RVA: 0x00038E3F File Offset: 0x0003703F
		public bool CanPassFences
		{
			get
			{
				return !this.FenceBlocked;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x00038E4A File Offset: 0x0003704A
		public bool FenceBlocked
		{
			get
			{
				return this.Roamer;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x00038E52 File Offset: 0x00037052
		public bool Roamer
		{
			get
			{
				return this.roamMtbDays != null;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x00038E5F File Offset: 0x0003705F
		public bool IsWorkMech
		{
			get
			{
				return !this.mechEnabledWorkTypes.NullOrEmpty<WorkTypeDef>();
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x00038E70 File Offset: 0x00037070
		public PawnKindDef AnyPawnKind
		{
			get
			{
				if (this.cachedAnyPawnKind == null)
				{
					List<PawnKindDef> allDefsListForReading = DefDatabase<PawnKindDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].race.race == this)
						{
							this.cachedAnyPawnKind = allDefsListForReading[i];
							break;
						}
					}
				}
				return this.cachedAnyPawnKind;
			}
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00038EC5 File Offset: 0x000370C5
		public RulePackDef GetNameGenerator(Gender gender)
		{
			if (gender == Gender.Female && this.nameGeneratorFemale != null)
			{
				return this.nameGeneratorFemale;
			}
			return this.nameGenerator;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00038EE0 File Offset: 0x000370E0
		public bool CanEverEat(Thing t)
		{
			return this.CanEverEat(t.def);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00038EF0 File Offset: 0x000370F0
		public bool CanEverEat(ThingDef t)
		{
			return this.EatsFood && t.ingestible != null && t.ingestible.preferability != FoodPreferability.Undefined && (this.willNeverEat == null || !this.willNeverEat.Contains(t)) && this.Eats(t.ingestible.foodType);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x00038F49 File Offset: 0x00037149
		public bool Eats(FoodTypeFlags food)
		{
			return this.EatsFood && (this.foodType & food) > FoodTypeFlags.None;
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00038F60 File Offset: 0x00037160
		public void ResolveReferencesSpecial()
		{
			if (this.specificMeatDef != null)
			{
				this.meatDef = this.specificMeatDef;
			}
			else if (this.useMeatFrom != null)
			{
				this.meatDef = this.useMeatFrom.race.meatDef;
			}
			if (this.useLeatherFrom != null)
			{
				this.leatherDef = this.useLeatherFrom.race.leatherDef;
			}
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00038FBF File Offset: 0x000371BF
		public IEnumerable<string> ConfigErrors(ThingDef thingDef)
		{
			RaceProperties.<ConfigErrors>d__171 <ConfigErrors>d__ = new RaceProperties.<ConfigErrors>d__171(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__thingDef = thingDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00038FD6 File Offset: 0x000371D6
		public IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef, StatRequest req)
		{
			RaceProperties.<SpecialDisplayStats>d__172 <SpecialDisplayStats>d__ = new RaceProperties.<SpecialDisplayStats>d__172(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__parentDef = parentDef;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00038FF4 File Offset: 0x000371F4
		public static string NutritionEatenPerDay(Pawn p)
		{
			return (p.needs.food.FoodFallPerTickAssumingCategory(HungerCategory.Fed, false) * 60000f).ToString("0.##");
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00039028 File Offset: 0x00037228
		public static string NutritionEatenPerDayExplanation(Pawn p, bool showDiet = false, bool showLegend = false, bool showCalculations = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("NutritionEatenPerDayTip".Translate(ThingDefOf.MealSimple.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("0.##")));
			stringBuilder.AppendLine();
			if (showDiet)
			{
				stringBuilder.AppendLine("CanEat".Translate() + ": " + p.RaceProps.foodType.ToHumanString());
				stringBuilder.AppendLine();
			}
			if (showLegend)
			{
				stringBuilder.AppendLine("Legend".Translate() + ":");
				stringBuilder.AppendLine("NoDietCategoryLetter".Translate() + " - " + DietCategory.Omnivorous.ToStringHuman());
				DietCategory[] array = (DietCategory[])Enum.GetValues(typeof(DietCategory));
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != DietCategory.NeverEats && array[i] != DietCategory.Omnivorous)
					{
						stringBuilder.AppendLine(array[i].ToStringHumanShort() + " - " + array[i].ToStringHuman());
					}
				}
				stringBuilder.AppendLine();
			}
			if (showCalculations)
			{
				stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": " + (p.ageTracker.CurLifeStage.hungerRateFactor * p.RaceProps.baseHungerRate * 2.6666667E-05f * 60000f).ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute));
				if (p.health.hediffSet.HungerRateFactor != 1f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_RelevantHediffs".Translate() + ": " + p.health.hediffSet.HungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
					foreach (Hediff hediff in p.health.hediffSet.hediffs)
					{
						if (hediff.CurStage != null && hediff.CurStage.hungerRateFactor != 1f)
						{
							stringBuilder.AppendLine("    " + hediff.LabelCap + ": " + hediff.CurStage.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
						}
					}
					foreach (Hediff hediff2 in p.health.hediffSet.hediffs)
					{
						if (hediff2.CurStage != null && hediff2.CurStage.hungerRateFactorOffset != 0f)
						{
							stringBuilder.AppendLine("    " + hediff2.LabelCap + ": " + hediff2.CurStage.hungerRateFactorOffset.ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Offset));
						}
					}
				}
				Pawn_StoryTracker story = p.story;
				if (((story != null) ? story.traits : null) != null && p.story.traits.HungerRateFactor != 1f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_RelevantTraits".Translate() + ": " + p.story.traits.HungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
					foreach (Trait trait in p.story.traits.allTraits)
					{
						if (!trait.Suppressed && trait.CurrentData.hungerRateFactor != 1f)
						{
							stringBuilder.AppendLine("    " + trait.LabelCap + ": " + trait.CurrentData.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
						}
					}
				}
				Building_Bed building_Bed = p.CurrentBed() ?? p.CurrentCaravanBed();
				if (building_Bed != null)
				{
					float statValue = building_Bed.GetStatValue(StatDefOf.BedHungerRateFactor, true, -1);
					if (statValue != 1f)
					{
						stringBuilder.AppendLine().AppendLine("StatsReport_InBed".Translate() + ": x" + statValue.ToStringPercent());
					}
				}
				if (ModsConfig.BiotechActive)
				{
					Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating, false);
					if (firstHediffOfDef != null)
					{
						HediffComp_Lactating hediffComp_Lactating = firstHediffOfDef.TryGetComp<HediffComp_Lactating>();
						if (hediffComp_Lactating != null)
						{
							float f = hediffComp_Lactating.AddedNutritionPerDay();
							stringBuilder.AppendLine();
							stringBuilder.AppendLine(firstHediffOfDef.LabelBaseCap + ": " + f.ToStringWithSign("0.##"));
						}
					}
				}
				if (p.genes != null)
				{
					int num = 0;
					foreach (Gene gene in p.genes.GenesListForReading)
					{
						if (!gene.Overridden)
						{
							num += gene.def.biostatMet;
						}
					}
					float num2 = GeneTuning.MetabolismToFoodConsumptionFactorCurve.Evaluate((float)num);
					if (num2 != 1f)
					{
						stringBuilder.AppendLine().AppendLine("FactorForMetabolism".Translate() + ": x" + num2.ToStringPercent());
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_FinalValue".Translate() + ": " + RaceProperties.NutritionEatenPerDay(p));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x04000687 RID: 1671
		public Intelligence intelligence;

		// Token: 0x04000688 RID: 1672
		private FleshTypeDef fleshType;

		// Token: 0x04000689 RID: 1673
		private ThingDef bloodDef;

		// Token: 0x0400068A RID: 1674
		private ThingDef bloodSmearDef;

		// Token: 0x0400068B RID: 1675
		public bool hasGenders = true;

		// Token: 0x0400068C RID: 1676
		public Gender forceGender;

		// Token: 0x0400068D RID: 1677
		public bool needsRest = true;

		// Token: 0x0400068E RID: 1678
		public ThinkTreeDef thinkTreeMain;

		// Token: 0x0400068F RID: 1679
		public ThinkTreeDef thinkTreeConstant;

		// Token: 0x04000690 RID: 1680
		public DutyDef dutyBoss;

		// Token: 0x04000691 RID: 1681
		public PawnNameCategory nameCategory;

		// Token: 0x04000692 RID: 1682
		public FoodTypeFlags foodType;

		// Token: 0x04000693 RID: 1683
		public BodyDef body;

		// Token: 0x04000694 RID: 1684
		public DeathActionProperties deathAction = new DeathActionProperties();

		// Token: 0x04000695 RID: 1685
		public List<AnimalBiomeRecord> wildBiomes;

		// Token: 0x04000696 RID: 1686
		public SimpleCurve ageGenerationCurve;

		// Token: 0x04000697 RID: 1687
		public bool makesFootprints;

		// Token: 0x04000698 RID: 1688
		public int executionRange = 2;

		// Token: 0x04000699 RID: 1689
		public float lifeExpectancy = 10f;

		// Token: 0x0400069A RID: 1690
		public List<HediffGiverSetDef> hediffGiverSets;

		// Token: 0x0400069B RID: 1691
		public float? roamMtbDays;

		// Token: 0x0400069C RID: 1692
		public bool allowedOnCaravan = true;

		// Token: 0x0400069D RID: 1693
		public bool canReleaseToWild = true;

		// Token: 0x0400069E RID: 1694
		public bool playerCanChangeMaster = true;

		// Token: 0x0400069F RID: 1695
		public bool showTrainables = true;

		// Token: 0x040006A0 RID: 1696
		public bool hideTrainingTab;

		// Token: 0x040006A1 RID: 1697
		public bool doesntMove;

		// Token: 0x040006A2 RID: 1698
		public PawnRenderTreeDef renderTree;

		// Token: 0x040006A3 RID: 1699
		public AnimationDef startingAnimation;

		// Token: 0x040006A4 RID: 1700
		public ThingDef linkedCorpseKind;

		// Token: 0x040006A5 RID: 1701
		public bool canOpenFactionlessDoors = true;

		// Token: 0x040006A6 RID: 1702
		public bool alwaysAwake;

		// Token: 0x040006A7 RID: 1703
		public bool alwaysViolent;

		// Token: 0x040006A8 RID: 1704
		public bool isImmuneToInfections;

		// Token: 0x040006A9 RID: 1705
		public float bleedRateFactor = 1f;

		// Token: 0x040006AA RID: 1706
		public bool canBecomeShambler;

		// Token: 0x040006AB RID: 1707
		public bool neverIncludeInQuests;

		// Token: 0x040006AC RID: 1708
		public bool canBeVacuumBurnt = true;

		// Token: 0x040006AD RID: 1709
		public bool herdAnimal;

		// Token: 0x040006AE RID: 1710
		public bool packAnimal;

		// Token: 0x040006AF RID: 1711
		public bool predator;

		// Token: 0x040006B0 RID: 1712
		public float maxPreyBodySize = 99999f;

		// Token: 0x040006B1 RID: 1713
		public float petness;

		// Token: 0x040006B2 RID: 1714
		public float nuzzleMtbHours = -1f;

		// Token: 0x040006B3 RID: 1715
		public float manhunterOnDamageChance;

		// Token: 0x040006B4 RID: 1716
		public float manhunterOnTameFailChance;

		// Token: 0x040006B5 RID: 1717
		public bool canBePredatorPrey = true;

		// Token: 0x040006B6 RID: 1718
		public bool herdMigrationAllowed = true;

		// Token: 0x040006B7 RID: 1719
		public AnimalType animalType;

		// Token: 0x040006B8 RID: 1720
		public List<ThingDef> willNeverEat;

		// Token: 0x040006B9 RID: 1721
		public bool giveNonToolUserBeatFireVerb;

		// Token: 0x040006BA RID: 1722
		public bool disableIgniteVerb;

		// Token: 0x040006BB RID: 1723
		public bool disableAreaControl;

		// Token: 0x040006BC RID: 1724
		public bool waterSeeker;

		// Token: 0x040006BD RID: 1725
		public int? waterCellCost;

		// Token: 0x040006BE RID: 1726
		public bool disableMating;

		// Token: 0x040006BF RID: 1727
		public List<ThingDef> canCrossBreedWith;

		// Token: 0x040006C0 RID: 1728
		public List<ThingDef> crossAggroWith;

		// Token: 0x040006C1 RID: 1729
		public bool canFishForFood;

		// Token: 0x040006C2 RID: 1730
		public bool canFlyInVacuum;

		// Token: 0x040006C3 RID: 1731
		public PawnKindDef manhunterPackUseLabelFrom;

		// Token: 0x040006C4 RID: 1732
		public float flightStartChanceOnJobStart;

		// Token: 0x040006C5 RID: 1733
		public float flightSpeedFactor = 2.8f;

		// Token: 0x040006C6 RID: 1734
		public bool canFlyIntoMap;

		// Token: 0x040006C7 RID: 1735
		public bool canLeaveMapFlying;

		// Token: 0x040006C8 RID: 1736
		public float leaveMapOnFleeChance;

		// Token: 0x040006C9 RID: 1737
		public int maxMechEnergy = 100;

		// Token: 0x040006CA RID: 1738
		public List<WorkTypeDef> mechEnabledWorkTypes = new List<WorkTypeDef>();

		// Token: 0x040006CB RID: 1739
		public int mechFixedSkillLevel = 10;

		// Token: 0x040006CC RID: 1740
		public List<MechWorkTypePriority> mechWorkTypePriorities;

		// Token: 0x040006CD RID: 1741
		public int? bulletStaggerDelayTicks;

		// Token: 0x040006CE RID: 1742
		public float? bulletStaggerSpeedFactor;

		// Token: 0x040006CF RID: 1743
		public EffecterDef bulletStaggerEffecterDef;

		// Token: 0x040006D0 RID: 1744
		public bool bulletStaggerIgnoreBodySize;

		// Token: 0x040006D1 RID: 1745
		public MechWeightClassDef mechWeightClass;

		// Token: 0x040006D2 RID: 1746
		public List<DetritusLeavingType> detritusLeavings = new List<DetritusLeavingType>();

		// Token: 0x040006D3 RID: 1747
		public float gestationPeriodDays = -1f;

		// Token: 0x040006D4 RID: 1748
		public SimpleCurve litterSizeCurve;

		// Token: 0x040006D5 RID: 1749
		public float mateMtbHours = 12f;

		// Token: 0x040006D6 RID: 1750
		[NoTranslate]
		public List<string> untrainableTags;

		// Token: 0x040006D7 RID: 1751
		[NoTranslate]
		public List<string> trainableTags;

		// Token: 0x040006D8 RID: 1752
		public TrainabilityDef trainability;

		// Token: 0x040006D9 RID: 1753
		public List<TrainableDef> specialTrainables;

		// Token: 0x040006DA RID: 1754
		private RulePackDef nameGenerator;

		// Token: 0x040006DB RID: 1755
		private RulePackDef nameGeneratorFemale;

		// Token: 0x040006DC RID: 1756
		public float nameOnTameChance;

		// Token: 0x040006DD RID: 1757
		public float baseBodySize = 1f;

		// Token: 0x040006DE RID: 1758
		public float baseHealthScale = 1f;

		// Token: 0x040006DF RID: 1759
		public float baseHungerRate = 1f;

		// Token: 0x040006E0 RID: 1760
		public List<LifeStageAge> lifeStageAges = new List<LifeStageAge>();

		// Token: 0x040006E1 RID: 1761
		public List<LifeStageWorkSettings> lifeStageWorkSettings = new List<LifeStageWorkSettings>();

		// Token: 0x040006E2 RID: 1762
		public bool hasMeat = true;

		// Token: 0x040006E3 RID: 1763
		[MustTranslate]
		public string meatLabel;

		// Token: 0x040006E4 RID: 1764
		public Color meatColor = Color.white;

		// Token: 0x040006E5 RID: 1765
		public float meatMarketValue = 2f;

		// Token: 0x040006E6 RID: 1766
		public ThingDef specificMeatDef;

		// Token: 0x040006E7 RID: 1767
		public ThingDef useMeatFrom;

		// Token: 0x040006E8 RID: 1768
		public ThingDef useLeatherFrom;

		// Token: 0x040006E9 RID: 1769
		public bool hasCorpse = true;

		// Token: 0x040006EA RID: 1770
		public bool hasUnnaturalCorpse;

		// Token: 0x040006EB RID: 1771
		public bool corpseHiddenWhileUndiscovered;

		// Token: 0x040006EC RID: 1772
		public ThingDef leatherDef;

		// Token: 0x040006ED RID: 1773
		public ShadowData specialShadowData;

		// Token: 0x040006EE RID: 1774
		public List<Vector3> headPosPerRotation;

		// Token: 0x040006EF RID: 1775
		public IntRange soundCallIntervalRange = new IntRange(2000, 4000);

		// Token: 0x040006F0 RID: 1776
		public float soundCallIntervalFriendlyFactor = 1f;

		// Token: 0x040006F1 RID: 1777
		public float soundCallIntervalAggressiveFactor = 0.25f;

		// Token: 0x040006F2 RID: 1778
		public SoundDef soundMeleeHitPawn;

		// Token: 0x040006F3 RID: 1779
		public SoundDef soundMeleeHitBuilding;

		// Token: 0x040006F4 RID: 1780
		public SoundDef soundMeleeMiss;

		// Token: 0x040006F5 RID: 1781
		public SoundDef soundMeleeDodge;

		// Token: 0x040006F6 RID: 1782
		public SoundDef soundAmbience;

		// Token: 0x040006F7 RID: 1783
		public SoundDef soundMoving;

		// Token: 0x040006F8 RID: 1784
		public SoundDef soundEating;

		// Token: 0x040006F9 RID: 1785
		public KnowledgeCategoryDef knowledgeCategory;

		// Token: 0x040006FA RID: 1786
		public int anomalyKnowledge;

		// Token: 0x040006FB RID: 1787
		[Unsaved(false)]
		public ThingDef meatDef;

		// Token: 0x040006FC RID: 1788
		[Unsaved(false)]
		public ThingDef corpseDef;

		// Token: 0x040006FD RID: 1789
		[Unsaved(false)]
		public ThingDef unnaturalCorpseDef;

		// Token: 0x040006FE RID: 1790
		[Unsaved(false)]
		private PawnKindDef cachedAnyPawnKind;
	}
}
