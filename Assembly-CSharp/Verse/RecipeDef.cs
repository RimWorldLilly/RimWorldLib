using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001AB RID: 427
	public class RecipeDef : Def
	{
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x0003E7DA File Offset: 0x0003C9DA
		public ThingDef UIIconThing
		{
			get
			{
				return this.uiIconThing ?? this.ProducedThingDef;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x0003E7EC File Offset: 0x0003C9EC
		public Texture2D UIIcon
		{
			get
			{
				if (!this.resolvedCachedIcon)
				{
					this.resolvedCachedIcon = true;
					if (!this.uiIconPath.NullOrEmpty())
					{
						this.cachedIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
					}
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x0003E822 File Offset: 0x0003CA22
		public RecipeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RecipeWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.recipe = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0003E854 File Offset: 0x0003CA54
		public RecipeWorkerCounter WorkerCounter
		{
			get
			{
				if (this.workerCounterInt == null)
				{
					this.workerCounterInt = (RecipeWorkerCounter)Activator.CreateInstance(this.workerCounterClass);
					this.workerCounterInt.recipe = this;
				}
				return this.workerCounterInt;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x0003E886 File Offset: 0x0003CA86
		public IngredientValueGetter IngredientValueGetter
		{
			get
			{
				if (this.ingredientValueGetterInt == null)
				{
					this.ingredientValueGetterInt = (IngredientValueGetter)Activator.CreateInstance(this.ingredientValueGetterClass);
				}
				return this.ingredientValueGetterInt;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x0003E8AC File Offset: 0x0003CAAC
		public bool AvailableNow
		{
			get
			{
				if (this.researchPrerequisite != null && !this.researchPrerequisite.IsFinished)
				{
					return false;
				}
				if (this.memePrerequisitesAny != null)
				{
					bool flag = false;
					foreach (MemeDef meme in this.memePrerequisitesAny)
					{
						if (Faction.OfPlayer.ideos.HasAnyIdeoWithMeme(meme))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				if (this.researchPrerequisites != null)
				{
					if (this.researchPrerequisites.Any((ResearchProjectDef r) => !r.IsFinished))
					{
						return false;
					}
				}
				if (this.factionPrerequisiteTags != null)
				{
					if (this.factionPrerequisiteTags.Any((string tag) => Faction.OfPlayer.def.recipePrerequisiteTags == null || !Faction.OfPlayer.def.recipePrerequisiteTags.Contains(tag)))
					{
						RecipeDef.<>c__DisplayClass91_0 CS$<>8__locals1;
						CS$<>8__locals1.unlockedByIdeo = false;
						this.<get_AvailableNow>g__Check|91_2(ref CS$<>8__locals1);
						if (!CS$<>8__locals1.unlockedByIdeo)
						{
							return false;
						}
					}
				}
				return !this.fromIdeoBuildingPreceptOnly || (ModsConfig.IdeologyActive && IdeoUtility.PlayerHasPreceptForBuilding(this.ProducedThingDef));
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x0003E9D8 File Offset: 0x0003CBD8
		public string MinSkillString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				if (this.skillRequirements != null)
				{
					for (int i = 0; i < this.skillRequirements.Count; i++)
					{
						SkillRequirement skillRequirement = this.skillRequirements[i];
						stringBuilder.AppendLine("   " + skillRequirement.skill.skillLabel.CapitalizeFirst() + ": " + skillRequirement.minLevel.ToString());
						flag = true;
					}
				}
				if (!flag)
				{
					stringBuilder.AppendLine("   (" + "NoneLower".Translate() + ")");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x0003EA7E File Offset: 0x0003CC7E
		public IEnumerable<ThingDef> AllRecipeUsers
		{
			get
			{
				RecipeDef.<get_AllRecipeUsers>d__95 <get_AllRecipeUsers>d__ = new RecipeDef.<get_AllRecipeUsers>d__95(-2);
				<get_AllRecipeUsers>d__.<>4__this = this;
				return <get_AllRecipeUsers>d__;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x0003EA8E File Offset: 0x0003CC8E
		public bool UsesUnfinishedThing
		{
			get
			{
				return this.unfinishedThingDef != null;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x0003EA9C File Offset: 0x0003CC9C
		public bool IsSurgery
		{
			get
			{
				if (this.isSurgeryCached == null)
				{
					this.isSurgeryCached = new bool?(false);
					using (IEnumerator<ThingDef> enumerator = this.AllRecipeUsers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.category == ThingCategory.Pawn)
							{
								this.isSurgeryCached = new bool?(true);
								break;
							}
						}
					}
				}
				return this.isSurgeryCached.Value;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x0003EB1C File Offset: 0x0003CD1C
		public ThingDef ProducedThingDef
		{
			get
			{
				if (this.specialProducts != null)
				{
					return null;
				}
				if (this.products == null || this.products.Count != 1)
				{
					return null;
				}
				return this.products[0].thingDef;
			}
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0003EB51 File Offset: 0x0003CD51
		public bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			return this.Worker.AvailableOnNow(thing, part);
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0003EB60 File Offset: 0x0003CD60
		public float WorkAmountTotal(Thing thing)
		{
			if (thing == null)
			{
				return this.WorkAmountForStuff(null);
			}
			if (this == RecipeDefOf.SmeltOrDestroyThing && thing.Smeltable)
			{
				return this.smeltingWorkAmount;
			}
			return this.WorkAmountForStuff(thing.Stuff);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0003EB90 File Offset: 0x0003CD90
		public float WorkAmountForStuff(ThingDef stuff)
		{
			if (this.workAmount >= 0f)
			{
				return this.workAmount;
			}
			return this.products[0].thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuff);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0003EBC2 File Offset: 0x0003CDC2
		public IEnumerable<ThingDef> PotentiallyMissingIngredients(Pawn billDoer, Map map)
		{
			RecipeDef.<PotentiallyMissingIngredients>d__105 <PotentiallyMissingIngredients>d__ = new RecipeDef.<PotentiallyMissingIngredients>d__105(-2);
			<PotentiallyMissingIngredients>d__.<>4__this = this;
			<PotentiallyMissingIngredients>d__.<>3__billDoer = billDoer;
			<PotentiallyMissingIngredients>d__.<>3__map = map;
			return <PotentiallyMissingIngredients>d__;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0003EBE0 File Offset: 0x0003CDE0
		public bool IsIngredient(ThingDef th)
		{
			for (int i = 0; i < this.ingredients.Count; i++)
			{
				if (this.ingredients[i].filter.Allows(th) && (this.ingredients[i].IsFixedIngredient || this.fixedIngredientFilter.Allows(th)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0003EC40 File Offset: 0x0003CE40
		public override IEnumerable<string> ConfigErrors()
		{
			RecipeDef.<ConfigErrors>d__107 <ConfigErrors>d__ = new RecipeDef.<ConfigErrors>d__107(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0003EC50 File Offset: 0x0003CE50
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			DeepProfiler.Start("Stat refs");
			try
			{
				if (this.workTableSpeedStat == null)
				{
					this.workTableSpeedStat = StatDefOf.WorkTableWorkSpeedFactor;
				}
				if (this.workTableEfficiencyStat == null)
				{
					this.workTableEfficiencyStat = StatDefOf.WorkTableEfficiencyFactor;
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ingredients reference resolve");
			try
			{
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					this.ingredients[i].ResolveReferences();
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("fixedIngredientFilter.ResolveReferences()");
			try
			{
				ThingFilter thingFilter = this.fixedIngredientFilter;
				if (thingFilter != null)
				{
					thingFilter.ResolveReferences();
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("defaultIngredientFilter setup");
			try
			{
				if (this.defaultIngredientFilter == null)
				{
					this.defaultIngredientFilter = new ThingFilter();
					if (this.fixedIngredientFilter != null)
					{
						this.defaultIngredientFilter.CopyAllowancesFrom(this.fixedIngredientFilter);
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("defaultIngredientFilter.ResolveReferences()");
			try
			{
				this.defaultIngredientFilter.ResolveReferences();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0003ED94 File Offset: 0x0003CF94
		public bool CompatibleWithHediff(HediffDef hediffDef)
		{
			if (this.incompatibleWithHediffTags.NullOrEmpty<string>() || hediffDef.tags.NullOrEmpty<string>())
			{
				return true;
			}
			for (int i = 0; i < this.incompatibleWithHediffTags.Count; i++)
			{
				for (int j = 0; j < hediffDef.tags.Count; j++)
				{
					if (this.incompatibleWithHediffTags[i].Equals(hediffDef.tags[j], StringComparison.InvariantCultureIgnoreCase))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x0003EE0C File Offset: 0x0003D00C
		public bool PawnSatisfiesSkillRequirements(Pawn pawn)
		{
			return this.FirstSkillRequirementPawnDoesntSatisfy(pawn) == null;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0003EE18 File Offset: 0x0003D018
		public SkillRequirement FirstSkillRequirementPawnDoesntSatisfy(Pawn pawn)
		{
			if (this.skillRequirements == null)
			{
				return null;
			}
			for (int i = 0; i < this.skillRequirements.Count; i++)
			{
				if (!this.skillRequirements[i].PawnSatisfies(pawn))
				{
					return this.skillRequirements[i];
				}
			}
			return null;
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x0003EE68 File Offset: 0x0003D068
		public List<ThingDef> GetPremultipliedSmallIngredients()
		{
			if (this.premultipliedSmallIngredients != null)
			{
				return this.premultipliedSmallIngredients;
			}
			this.premultipliedSmallIngredients = (from td in this.ingredients.SelectMany((IngredientCount ingredient) => ingredient.filter.AllowedThingDefs)
			where td.smallVolume
			select td).Distinct<ThingDef>().ToList<ThingDef>();
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					if (this.ingredients[i].filter.AllowedThingDefs.Any((ThingDef td) => !this.premultipliedSmallIngredients.Contains(td)))
					{
						foreach (ThingDef item in this.ingredients[i].filter.AllowedThingDefs)
						{
							flag |= this.premultipliedSmallIngredients.Remove(item);
						}
					}
				}
			}
			return this.premultipliedSmallIngredients;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x0003EF94 File Offset: 0x0003D194
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetIngredientsHyperlinks()
		{
			return Dialog_InfoCard.DefsToHyperlinks(from i in this.ingredients
			where i.IsFixedIngredient
			select i.FixedIngredient into i
			where i != null
			select i);
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x0003F018 File Offset: 0x0003D218
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetProductsHyperlinks()
		{
			return Dialog_InfoCard.DefsToHyperlinks(from i in this.products
			select i.thingDef);
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0003F049 File Offset: 0x0003D249
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			RecipeDef.<SpecialDisplayStats>d__115 <SpecialDisplayStats>d__ = new RecipeDef.<SpecialDisplayStats>d__115(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0003F158 File Offset: 0x0003D358
		[CompilerGenerated]
		private void <get_AvailableNow>g__Check|91_2(ref RecipeDef.<>c__DisplayClass91_0 A_1)
		{
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept_Role precept_Role in ideo.RolesListForReading)
				{
					if (precept_Role.apparelRequirements != null)
					{
						foreach (PreceptApparelRequirement preceptApparelRequirement in precept_Role.apparelRequirements)
						{
							ThingDef thingDef = preceptApparelRequirement.requirement.AllRequiredApparel(Gender.None).FirstOrDefault<ThingDef>();
							if (thingDef == null)
							{
								Log.Error("Apparel requirement for role " + precept_Role.Label + " is null");
							}
							using (List<ThingDefCountClass>.Enumerator enumerator4 = this.products.GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									if (enumerator4.Current.thingDef == thingDef)
									{
										A_1.unlockedByIdeo = true;
										return;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04000AEE RID: 2798
		public Type workerClass = typeof(RecipeWorker);

		// Token: 0x04000AEF RID: 2799
		public Type workerCounterClass = typeof(RecipeWorkerCounter);

		// Token: 0x04000AF0 RID: 2800
		[MustTranslate]
		public string jobString = "Doing an unknown recipe.";

		// Token: 0x04000AF1 RID: 2801
		public WorkTypeDef requiredGiverWorkType;

		// Token: 0x04000AF2 RID: 2802
		public float workAmount = -1f;

		// Token: 0x04000AF3 RID: 2803
		public StatDef workSpeedStat;

		// Token: 0x04000AF4 RID: 2804
		public StatDef efficiencyStat;

		// Token: 0x04000AF5 RID: 2805
		public StatDef workTableEfficiencyStat;

		// Token: 0x04000AF6 RID: 2806
		public StatDef workTableSpeedStat;

		// Token: 0x04000AF7 RID: 2807
		public List<IngredientCount> ingredients = new List<IngredientCount>();

		// Token: 0x04000AF8 RID: 2808
		public ThingFilter fixedIngredientFilter = new ThingFilter();

		// Token: 0x04000AF9 RID: 2809
		public ThingFilter defaultIngredientFilter;

		// Token: 0x04000AFA RID: 2810
		public bool allowMixingIngredients;

		// Token: 0x04000AFB RID: 2811
		public bool ignoreIngredientCountTakeEntireStacks;

		// Token: 0x04000AFC RID: 2812
		private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

		// Token: 0x04000AFD RID: 2813
		public List<SpecialThingFilterDef> forceHiddenSpecialFilters;

		// Token: 0x04000AFE RID: 2814
		public bool autoStripCorpses = true;

		// Token: 0x04000AFF RID: 2815
		public bool interruptIfIngredientIsRotting;

		// Token: 0x04000B00 RID: 2816
		public List<ThingDefCountClass> products = new List<ThingDefCountClass>();

		// Token: 0x04000B01 RID: 2817
		public List<SpecialProductType> specialProducts;

		// Token: 0x04000B02 RID: 2818
		public bool productHasIngredientStuff;

		// Token: 0x04000B03 RID: 2819
		public bool useIngredientsForColor = true;

		// Token: 0x04000B04 RID: 2820
		public int targetCountAdjustment = 1;

		// Token: 0x04000B05 RID: 2821
		public ThingDef unfinishedThingDef;

		// Token: 0x04000B06 RID: 2822
		public List<SkillRequirement> skillRequirements;

		// Token: 0x04000B07 RID: 2823
		public SkillDef workSkill;

		// Token: 0x04000B08 RID: 2824
		public float workSkillLearnFactor = 1f;

		// Token: 0x04000B09 RID: 2825
		public EffecterDef effectWorking;

		// Token: 0x04000B0A RID: 2826
		public SoundDef soundWorking;

		// Token: 0x04000B0B RID: 2827
		private ThingDef uiIconThing;

		// Token: 0x04000B0C RID: 2828
		[NoTranslate]
		private string uiIconPath;

		// Token: 0x04000B0D RID: 2829
		public List<ThingDef> recipeUsers;

		// Token: 0x04000B0E RID: 2830
		public List<BodyPartDef> appliedOnFixedBodyParts = new List<BodyPartDef>();

		// Token: 0x04000B0F RID: 2831
		public List<BodyPartGroupDef> appliedOnFixedBodyPartGroups = new List<BodyPartGroupDef>();

		// Token: 0x04000B10 RID: 2832
		public HediffDef addsHediff;

		// Token: 0x04000B11 RID: 2833
		public HediffDef removesHediff;

		// Token: 0x04000B12 RID: 2834
		public HediffDef addsHediffOnFailure;

		// Token: 0x04000B13 RID: 2835
		public HediffDef changesHediffLevel;

		// Token: 0x04000B14 RID: 2836
		public List<string> incompatibleWithHediffTags;

		// Token: 0x04000B15 RID: 2837
		public int hediffLevelOffset;

		// Token: 0x04000B16 RID: 2838
		public bool hideBodyPartNames;

		// Token: 0x04000B17 RID: 2839
		public bool isViolation;

		// Token: 0x04000B18 RID: 2840
		[MustTranslate]
		public string successfullyRemovedHediffMessage;

		// Token: 0x04000B19 RID: 2841
		public float surgerySuccessChanceFactor = 1f;

		// Token: 0x04000B1A RID: 2842
		public float deathOnFailedSurgeryChance;

		// Token: 0x04000B1B RID: 2843
		public bool targetsBodyPart = true;

		// Token: 0x04000B1C RID: 2844
		public bool anesthetize = true;

		// Token: 0x04000B1D RID: 2845
		public int minPartCount = -1;

		// Token: 0x04000B1E RID: 2846
		public bool surgeryIgnoreEnvironment;

		// Token: 0x04000B1F RID: 2847
		public SurgeryOutcomeEffectDef surgeryOutcomeEffect;

		// Token: 0x04000B20 RID: 2848
		public Gender? genderPrerequisite;

		// Token: 0x04000B21 RID: 2849
		public bool mustBeFertile;

		// Token: 0x04000B22 RID: 2850
		public bool allowedForQuestLodgers = true;

		// Token: 0x04000B23 RID: 2851
		public int minAllowedAge = -1;

		// Token: 0x04000B24 RID: 2852
		public DevelopmentalStage? developmentalStageFilter;

		// Token: 0x04000B25 RID: 2853
		public bool humanlikeOnly;

		// Token: 0x04000B26 RID: 2854
		public List<MutantDef> mutantPrerequisite;

		// Token: 0x04000B27 RID: 2855
		public List<MutantDef> mutantBlacklist;

		// Token: 0x04000B28 RID: 2856
		public float smeltingWorkAmount = -1f;

		// Token: 0x04000B29 RID: 2857
		public ResearchProjectDef researchPrerequisite;

		// Token: 0x04000B2A RID: 2858
		public List<MemeDef> memePrerequisitesAny;

		// Token: 0x04000B2B RID: 2859
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x04000B2C RID: 2860
		[NoTranslate]
		public List<string> factionPrerequisiteTags;

		// Token: 0x04000B2D RID: 2861
		public bool fromIdeoBuildingPreceptOnly;

		// Token: 0x04000B2E RID: 2862
		public ConceptDef conceptLearned;

		// Token: 0x04000B2F RID: 2863
		public bool dontShowIfAnyIngredientMissing;

		// Token: 0x04000B30 RID: 2864
		public int displayPriority = 99999;

		// Token: 0x04000B31 RID: 2865
		public int formingTicks;

		// Token: 0x04000B32 RID: 2866
		public bool mechanitorOnlyRecipe;

		// Token: 0x04000B33 RID: 2867
		public int gestationCycles;

		// Token: 0x04000B34 RID: 2868
		public bool mechResurrection;

		// Token: 0x04000B35 RID: 2869
		[Unsaved(false)]
		private RecipeWorker workerInt;

		// Token: 0x04000B36 RID: 2870
		[Unsaved(false)]
		private RecipeWorkerCounter workerCounterInt;

		// Token: 0x04000B37 RID: 2871
		[Unsaved(false)]
		private IngredientValueGetter ingredientValueGetterInt;

		// Token: 0x04000B38 RID: 2872
		[Unsaved(false)]
		private List<ThingDef> premultipliedSmallIngredients;

		// Token: 0x04000B39 RID: 2873
		[Unsaved(false)]
		public bool regenerateOnDifficultyChange;

		// Token: 0x04000B3A RID: 2874
		[Unsaved(false)]
		public int adjustedCount = 1;

		// Token: 0x04000B3B RID: 2875
		[Unsaved(false)]
		private Texture2D cachedIcon;

		// Token: 0x04000B3C RID: 2876
		[Unsaved(false)]
		private bool resolvedCachedIcon;

		// Token: 0x04000B3D RID: 2877
		[Unsaved(false)]
		private bool? isSurgeryCached;
	}
}
