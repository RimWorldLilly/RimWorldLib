using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001A0 RID: 416
	public class PawnKindDef : Def
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x0003DC9F File Offset: 0x0003BE9F
		public RaceProperties RaceProps
		{
			get
			{
				return this.race.race;
			}
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0003DCAC File Offset: 0x0003BEAC
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.lifeStages.Count; i++)
			{
				this.lifeStages[i].ResolveReferences();
			}
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0003DCE6 File Offset: 0x0003BEE6
		public string GetLabelPlural(int count = -1)
		{
			if (!this.labelPlural.NullOrEmpty())
			{
				return this.labelPlural;
			}
			return Find.ActiveLanguageWorker.Pluralize(this.label, count);
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0003DD0D File Offset: 0x0003BF0D
		public string GetLabelGendered(Gender gender)
		{
			if (gender == Gender.Female && !this.labelFemale.NullOrEmpty())
			{
				return this.labelFemale;
			}
			if (gender == Gender.Male && !this.labelMale.NullOrEmpty())
			{
				return this.labelMale;
			}
			return this.label;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0003DD45 File Offset: 0x0003BF45
		public RulePackDef GetNameMaker(Gender gender)
		{
			if (gender == Gender.Female && this.nameMakerFemale != null)
			{
				return this.nameMakerFemale;
			}
			if (this.nameMaker != null)
			{
				return this.nameMaker;
			}
			return null;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0003DD6C File Offset: 0x0003BF6C
		public override void PostLoad()
		{
			if (this.backstoryCategories != null && this.backstoryCategories.Count > 0)
			{
				if (this.backstoryFilters == null)
				{
					this.backstoryFilters = new List<BackstoryCategoryFilter>();
				}
				this.backstoryFilters.Add(new BackstoryCategoryFilter
				{
					categories = this.backstoryCategories
				});
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				foreach (PawnKindLifeStage pawnKindLifeStage in this.lifeStages)
				{
					if (pawnKindLifeStage.swimmingGraphicData != null && pawnKindLifeStage.swimmingGraphicData.shaderType == null)
					{
						pawnKindLifeStage.swimmingGraphicData.shaderType = ShaderTypeDefOf.Transparent;
					}
					if (pawnKindLifeStage.femaleSwimmingGraphicData != null && pawnKindLifeStage.femaleSwimmingGraphicData.shaderType == null)
					{
						pawnKindLifeStage.femaleSwimmingGraphicData.shaderType = ShaderTypeDefOf.Transparent;
					}
					if (pawnKindLifeStage.stationaryGraphicData != null && pawnKindLifeStage.stationaryGraphicData.shaderType == null)
					{
						pawnKindLifeStage.stationaryGraphicData.shaderType = ShaderTypeDefOf.Transparent;
					}
					if (pawnKindLifeStage.femaleStationaryGraphicData != null && pawnKindLifeStage.femaleStationaryGraphicData.shaderType == null)
					{
						pawnKindLifeStage.femaleStationaryGraphicData.shaderType = ShaderTypeDefOf.Transparent;
					}
				}
			});
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0003DDD0 File Offset: 0x0003BFD0
		public float GetAnimalPointsToHuntOrSlaughter()
		{
			return this.combatPower * 5f * (1f + this.RaceProps.manhunterOnDamageChance * 0.5f) * (1f + this.RaceProps.manhunterOnTameFailChance * 0.2f) * (1f + this.race.GetStatValueAbstract(StatDefOf.Wildness, null)) + this.race.BaseMarketValue;
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0003DE3D File Offset: 0x0003C03D
		public override IEnumerable<string> ConfigErrors()
		{
			PawnKindDef.<ConfigErrors>d__156 <ConfigErrors>d__ = new PawnKindDef.<ConfigErrors>d__156(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0003DE4D File Offset: 0x0003C04D
		public static PawnKindDef Named(string defName)
		{
			return DefDatabase<PawnKindDef>.GetNamed(defName, true);
		}

		// Token: 0x04000A0A RID: 2570
		public ThingDef race;

		// Token: 0x04000A0B RID: 2571
		[LoadAlias("defaultFactionType")]
		public FactionDef defaultFactionDef;

		// Token: 0x04000A0C RID: 2572
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFilters;

		// Token: 0x04000A0D RID: 2573
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFiltersOverride;

		// Token: 0x04000A0E RID: 2574
		[NoTranslate]
		public List<string> backstoryCategories;

		// Token: 0x04000A0F RID: 2575
		[MustTranslate]
		public string labelPlural;

		// Token: 0x04000A10 RID: 2576
		public List<PawnKindLifeStage> lifeStages = new List<PawnKindLifeStage>();

		// Token: 0x04000A11 RID: 2577
		public List<AlternateGraphic> alternateGraphics;

		// Token: 0x04000A12 RID: 2578
		[XmlInheritanceAllowDuplicateNodes]
		public List<TraitRequirement> forcedTraits;

		// Token: 0x04000A13 RID: 2579
		[XmlInheritanceAllowDuplicateNodes]
		public List<TraitRequirement> disallowedTraitsWithDegree;

		// Token: 0x04000A14 RID: 2580
		public List<TraitDef> disallowedTraits;

		// Token: 0x04000A15 RID: 2581
		public float alternateGraphicChance;

		// Token: 0x04000A16 RID: 2582
		public MutantDef mutant;

		// Token: 0x04000A17 RID: 2583
		public XenotypeSet xenotypeSet;

		// Token: 0x04000A18 RID: 2584
		public bool useFactionXenotypes = true;

		// Token: 0x04000A19 RID: 2585
		[LoadAlias("hairTags")]
		public List<StyleItemTagWeighted> styleItemTags;

		// Token: 0x04000A1A RID: 2586
		public HairDef forcedHair;

		// Token: 0x04000A1B RID: 2587
		public Color? forcedHairColor;

		// Token: 0x04000A1C RID: 2588
		public List<MissingPart> missingParts;

		// Token: 0x04000A1D RID: 2589
		public RulePackDef nameMaker;

		// Token: 0x04000A1E RID: 2590
		public RulePackDef nameMakerFemale;

		// Token: 0x04000A1F RID: 2591
		public List<AbilityDef> abilities;

		// Token: 0x04000A20 RID: 2592
		public bool preventIdeo;

		// Token: 0x04000A21 RID: 2593
		public bool studiableAsPrisoner;

		// Token: 0x04000A22 RID: 2594
		public bool isBoss;

		// Token: 0x04000A23 RID: 2595
		public Dictionary<string, float> moveSpeedFactorByTerrainTag = new Dictionary<string, float>();

		// Token: 0x04000A24 RID: 2596
		public List<BackstoryDef> fixedChildBackstories = new List<BackstoryDef>();

		// Token: 0x04000A25 RID: 2597
		public List<BackstoryDef> fixedAdultBackstories = new List<BackstoryDef>();

		// Token: 0x04000A26 RID: 2598
		public float backstoryCryptosleepCommonality;

		// Token: 0x04000A27 RID: 2599
		public FloatRange? chronologicalAgeRange;

		// Token: 0x04000A28 RID: 2600
		public int minGenerationAge;

		// Token: 0x04000A29 RID: 2601
		public int maxGenerationAge = 999999;

		// Token: 0x04000A2A RID: 2602
		public bool factionLeader;

		// Token: 0x04000A2B RID: 2603
		public Gender? fixedGender;

		// Token: 0x04000A2C RID: 2604
		public bool allowOldAgeInjuries = true;

		// Token: 0x04000A2D RID: 2605
		public bool generateInitialNonFamilyRelations = true;

		// Token: 0x04000A2E RID: 2606
		public DevelopmentalStage? pawnGroupDevelopmentStage;

		// Token: 0x04000A2F RID: 2607
		public bool destroyGearOnDrop;

		// Token: 0x04000A30 RID: 2608
		public bool canStrip = true;

		// Token: 0x04000A31 RID: 2609
		public float defendPointRadius = -1f;

		// Token: 0x04000A32 RID: 2610
		public bool factionHostileOnKill;

		// Token: 0x04000A33 RID: 2611
		public bool factionHostileOnDeath;

		// Token: 0x04000A34 RID: 2612
		public bool hostileToAll;

		// Token: 0x04000A35 RID: 2613
		public FloatRange? initialResistanceRange;

		// Token: 0x04000A36 RID: 2614
		public FloatRange? initialWillRange;

		// Token: 0x04000A37 RID: 2615
		public bool forceNoDeathNotification;

		// Token: 0x04000A38 RID: 2616
		public bool skipResistant;

		// Token: 0x04000A39 RID: 2617
		public float controlGroupPortraitZoom = 1f;

		// Token: 0x04000A3A RID: 2618
		public float? overrideDeathOnDownedChance;

		// Token: 0x04000A3B RID: 2619
		public bool forceDeathOnDowned;

		// Token: 0x04000A3C RID: 2620
		public bool immuneToGameConditionEffects;

		// Token: 0x04000A3D RID: 2621
		public bool immuneToTraps;

		// Token: 0x04000A3E RID: 2622
		public bool collidesWithPawns = true;

		// Token: 0x04000A3F RID: 2623
		public bool ignoresPainShock;

		// Token: 0x04000A40 RID: 2624
		public bool canMeleeAttack = true;

		// Token: 0x04000A41 RID: 2625
		public float basePrisonBreakMtbDays = 60f;

		// Token: 0x04000A42 RID: 2626
		public bool useFixedRotation;

		// Token: 0x04000A43 RID: 2627
		public Rot4 fixedRotation;

		// Token: 0x04000A44 RID: 2628
		public bool showInDebugSpawner = true;

		// Token: 0x04000A45 RID: 2629
		public bool canOpenAnyDoor;

		// Token: 0x04000A46 RID: 2630
		public bool canOpenDoors = true;

		// Token: 0x04000A47 RID: 2631
		[NoTranslate]
		public string overrideDebugActionCategory;

		// Token: 0x04000A48 RID: 2632
		public float royalTitleChance;

		// Token: 0x04000A49 RID: 2633
		public RoyalTitleDef titleRequired;

		// Token: 0x04000A4A RID: 2634
		public RoyalTitleDef minTitleRequired;

		// Token: 0x04000A4B RID: 2635
		public List<RoyalTitleDef> titleSelectOne;

		// Token: 0x04000A4C RID: 2636
		public bool allowRoyalRoomRequirements = true;

		// Token: 0x04000A4D RID: 2637
		public bool allowRoyalApparelRequirements = true;

		// Token: 0x04000A4E RID: 2638
		public List<InfectionPathwayDef> meleeAttackInfectionPathways;

		// Token: 0x04000A4F RID: 2639
		public List<InfectionPathwayDef> rangedAttackInfectionPathways;

		// Token: 0x04000A50 RID: 2640
		public bool isFighter = true;

		// Token: 0x04000A51 RID: 2641
		public float combatPower = -1f;

		// Token: 0x04000A52 RID: 2642
		public bool canArriveManhunter = true;

		// Token: 0x04000A53 RID: 2643
		public bool canBeSapper;

		// Token: 0x04000A54 RID: 2644
		public bool isGoodBreacher;

		// Token: 0x04000A55 RID: 2645
		public bool allowInMechClusters = true;

		// Token: 0x04000A56 RID: 2646
		public int maxPerGroup = int.MaxValue;

		// Token: 0x04000A57 RID: 2647
		public bool isGoodPsychicRitualInvoker;

		// Token: 0x04000A58 RID: 2648
		public bool canBeScattered = true;

		// Token: 0x04000A59 RID: 2649
		public bool appearsRandomlyInCombatGroups = true;

		// Token: 0x04000A5A RID: 2650
		public bool aiAvoidCover;

		// Token: 0x04000A5B RID: 2651
		public FloatRange fleeHealthThresholdRange = new FloatRange(-0.4f, 0.4f);

		// Token: 0x04000A5C RID: 2652
		public float acceptArrestChanceFactor = 1f;

		// Token: 0x04000A5D RID: 2653
		public bool canUseAvoidGrid;

		// Token: 0x04000A5E RID: 2654
		public QualityCategory itemQuality = QualityCategory.Normal;

		// Token: 0x04000A5F RID: 2655
		public QualityCategory? forceWeaponQuality;

		// Token: 0x04000A60 RID: 2656
		public bool forceNormalGearQuality;

		// Token: 0x04000A61 RID: 2657
		public FloatRange gearHealthRange = FloatRange.One;

		// Token: 0x04000A62 RID: 2658
		public FloatRange weaponMoney = FloatRange.Zero;

		// Token: 0x04000A63 RID: 2659
		[NoTranslate]
		public List<string> weaponTags;

		// Token: 0x04000A64 RID: 2660
		public ThingDef weaponStuffOverride;

		// Token: 0x04000A65 RID: 2661
		public ThingStyleDef weaponStyleDef;

		// Token: 0x04000A66 RID: 2662
		public FloatRange apparelMoney = FloatRange.Zero;

		// Token: 0x04000A67 RID: 2663
		public List<ThingDef> apparelRequired;

		// Token: 0x04000A68 RID: 2664
		[NoTranslate]
		public List<string> apparelTags;

		// Token: 0x04000A69 RID: 2665
		[NoTranslate]
		public List<string> apparelDisallowTags;

		// Token: 0x04000A6A RID: 2666
		public float apparelAllowHeadgearChance = 1f;

		// Token: 0x04000A6B RID: 2667
		public bool ignoreApparelAllowChance;

		// Token: 0x04000A6C RID: 2668
		public bool apparelIgnoreSeasons;

		// Token: 0x04000A6D RID: 2669
		public bool apparelIgnorePollution;

		// Token: 0x04000A6E RID: 2670
		public bool ignoreFactionApparelStuffRequirements;

		// Token: 0x04000A6F RID: 2671
		public Color apparelColor = Color.white;

		// Token: 0x04000A70 RID: 2672
		public Color? skinColorOverride;

		// Token: 0x04000A71 RID: 2673
		public ColorDef favoriteColor;

		// Token: 0x04000A72 RID: 2674
		public bool ignoreIdeoApparelColors;

		// Token: 0x04000A73 RID: 2675
		public List<SpecificApparelRequirement> specificApparelRequirements;

		// Token: 0x04000A74 RID: 2676
		public List<ThingDef> techHediffsRequired;

		// Token: 0x04000A75 RID: 2677
		public FloatRange techHediffsMoney = FloatRange.Zero;

		// Token: 0x04000A76 RID: 2678
		[NoTranslate]
		public List<string> techHediffsTags;

		// Token: 0x04000A77 RID: 2679
		[NoTranslate]
		public List<string> techHediffsDisallowTags;

		// Token: 0x04000A78 RID: 2680
		public float techHediffsChance;

		// Token: 0x04000A79 RID: 2681
		public int techHediffsMaxAmount = 1;

		// Token: 0x04000A7A RID: 2682
		public float biocodeWeaponChance;

		// Token: 0x04000A7B RID: 2683
		public float humanPregnancyChance = 0.03f;

		// Token: 0x04000A7C RID: 2684
		public List<StartingHediff> startingHediffs;

		// Token: 0x04000A7D RID: 2685
		public float nakedChance;

		// Token: 0x04000A7E RID: 2686
		public List<MiscDamage> existingDamage = new List<MiscDamage>();

		// Token: 0x04000A7F RID: 2687
		public QualityCategory minApparelQuality;

		// Token: 0x04000A80 RID: 2688
		public QualityCategory maxApparelQuality = QualityCategory.Excellent;

		// Token: 0x04000A81 RID: 2689
		public List<ThingDefCountClass> fixedInventory = new List<ThingDefCountClass>();

		// Token: 0x04000A82 RID: 2690
		public PawnInventoryOption inventoryOptions;

		// Token: 0x04000A83 RID: 2691
		public float invNutrition;

		// Token: 0x04000A84 RID: 2692
		public ThingDef invFoodDef;

		// Token: 0x04000A85 RID: 2693
		public float chemicalAddictionChance;

		// Token: 0x04000A86 RID: 2694
		public float combatEnhancingDrugsChance;

		// Token: 0x04000A87 RID: 2695
		public IntRange combatEnhancingDrugsCount = IntRange.Zero;

		// Token: 0x04000A88 RID: 2696
		public List<ChemicalDef> forcedAddictions = new List<ChemicalDef>();

		// Token: 0x04000A89 RID: 2697
		public bool trader;

		// Token: 0x04000A8A RID: 2698
		public List<SkillRange> skills;

		// Token: 0x04000A8B RID: 2699
		public WorkTags requiredWorkTags;

		// Token: 0x04000A8C RID: 2700
		public WorkTags disabledWorkTags;

		// Token: 0x04000A8D RID: 2701
		public int extraSkillLevels;

		// Token: 0x04000A8E RID: 2702
		public int minTotalSkillLevels;

		// Token: 0x04000A8F RID: 2703
		public int minBestSkillLevel;

		// Token: 0x04000A90 RID: 2704
		[MustTranslate]
		public string labelMale;

		// Token: 0x04000A91 RID: 2705
		[MustTranslate]
		public string labelMalePlural;

		// Token: 0x04000A92 RID: 2706
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04000A93 RID: 2707
		[MustTranslate]
		public string labelFemalePlural;

		// Token: 0x04000A94 RID: 2708
		public IntRange wildGroupSize = IntRange.One;

		// Token: 0x04000A95 RID: 2709
		public float ecoSystemWeight = 1f;

		// Token: 0x04000A96 RID: 2710
		[NoTranslate]
		public string flyingAnimationFramePathPrefix;

		// Token: 0x04000A97 RID: 2711
		[NoTranslate]
		public string flyingAnimationFramePathPrefixFemale;

		// Token: 0x04000A98 RID: 2712
		public int flyingAnimationFrameCount;

		// Token: 0x04000A99 RID: 2713
		public int flyingAnimationTicksPerFrame = -1;

		// Token: 0x04000A9A RID: 2714
		public float flyingAnimationDrawSize = 1f;

		// Token: 0x04000A9B RID: 2715
		public bool flyingAnimationDrawSizeIsMultiplier;

		// Token: 0x04000A9C RID: 2716
		public bool flyingAnimationInheritColors;

		// Token: 0x04000A9D RID: 2717
		private const int MaxWeaponMoney = 999999;
	}
}
