using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F59 RID: 3929
	public class FactionDef : Def
	{
		// Token: 0x17001143 RID: 4419
		// (get) Token: 0x06006508 RID: 25864 RVA: 0x002407A4 File Offset: 0x0023E9A4
		public List<RoyalTitleDef> RoyalTitlesAwardableInSeniorityOrderForReading
		{
			get
			{
				if (this.royalTitlesAwardableInSeniorityOrderForReading == null)
				{
					this.royalTitlesAwardableInSeniorityOrderForReading = new List<RoyalTitleDef>();
					if (this.royalTitleTags != null && ModLister.RoyaltyInstalled)
					{
						foreach (RoyalTitleDef royalTitleDef in DefDatabase<RoyalTitleDef>.AllDefsListForReading)
						{
							if (royalTitleDef.Awardable && royalTitleDef.tags.SharesElementWith(this.royalTitleTags))
							{
								this.royalTitlesAwardableInSeniorityOrderForReading.Add(royalTitleDef);
							}
						}
						this.royalTitlesAwardableInSeniorityOrderForReading.SortBy((RoyalTitleDef t) => t.seniority);
					}
				}
				return this.royalTitlesAwardableInSeniorityOrderForReading;
			}
		}

		// Token: 0x17001144 RID: 4420
		// (get) Token: 0x06006509 RID: 25865 RVA: 0x00240870 File Offset: 0x0023EA70
		public List<RoyalTitleDef> RoyalTitlesAllInSeniorityOrderForReading
		{
			get
			{
				if (this.royalTitlesAllInSeniorityOrderForReading == null)
				{
					this.royalTitlesAllInSeniorityOrderForReading = new List<RoyalTitleDef>();
					if (this.royalTitleTags != null && ModLister.RoyaltyInstalled)
					{
						foreach (RoyalTitleDef royalTitleDef in DefDatabase<RoyalTitleDef>.AllDefsListForReading)
						{
							if (royalTitleDef.tags.SharesElementWith(this.royalTitleTags))
							{
								this.royalTitlesAllInSeniorityOrderForReading.Add(royalTitleDef);
							}
						}
						this.royalTitlesAllInSeniorityOrderForReading.SortBy((RoyalTitleDef t) => t.seniority);
					}
				}
				return this.royalTitlesAllInSeniorityOrderForReading;
			}
		}

		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x0600650A RID: 25866 RVA: 0x00240930 File Offset: 0x0023EB30
		public RoyalTitleInheritanceWorker RoyalTitleInheritanceWorker
		{
			get
			{
				if (this.royalTitleInheritanceWorker == null && this.royalTitleInheritanceWorkerClass != null)
				{
					this.royalTitleInheritanceWorker = (RoyalTitleInheritanceWorker)Activator.CreateInstance(this.royalTitleInheritanceWorkerClass);
				}
				return this.royalTitleInheritanceWorker;
			}
		}

		// Token: 0x17001146 RID: 4422
		// (get) Token: 0x0600650B RID: 25867 RVA: 0x00240964 File Offset: 0x0023EB64
		public bool CanEverBeNonHostile
		{
			get
			{
				return !this.permanentEnemy;
			}
		}

		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x0600650C RID: 25868 RVA: 0x00240970 File Offset: 0x0023EB70
		public Texture2D SettlementTexture
		{
			get
			{
				if (this.settlementTexture == null)
				{
					if (!this.settlementTexturePath.NullOrEmpty())
					{
						this.settlementTexture = ContentFinder<Texture2D>.Get(this.settlementTexturePath, true);
					}
					else
					{
						this.settlementTexture = BaseContent.BadTex;
					}
				}
				return this.settlementTexture;
			}
		}

		// Token: 0x17001148 RID: 4424
		// (get) Token: 0x0600650D RID: 25869 RVA: 0x002409C0 File Offset: 0x0023EBC0
		public Texture2D FactionIcon
		{
			get
			{
				if (this.factionIcon == null)
				{
					if (!this.factionIconPath.NullOrEmpty())
					{
						this.factionIcon = ContentFinder<Texture2D>.Get(this.factionIconPath, true);
					}
					else
					{
						this.factionIcon = BaseContent.BadTex;
					}
				}
				return this.factionIcon;
			}
		}

		// Token: 0x17001149 RID: 4425
		// (get) Token: 0x0600650E RID: 25870 RVA: 0x00240A0D File Offset: 0x0023EC0D
		public Texture2D RoyalFavorIcon
		{
			get
			{
				if (this.royalFavorIcon == null && !this.royalFavorIconPath.NullOrEmpty())
				{
					this.royalFavorIcon = ContentFinder<Texture2D>.Get(this.royalFavorIconPath, true);
				}
				return this.royalFavorIcon;
			}
		}

		// Token: 0x1700114A RID: 4426
		// (get) Token: 0x0600650F RID: 25871 RVA: 0x00240A42 File Offset: 0x0023EC42
		public bool HasRoyalTitles
		{
			get
			{
				return this.RoyalTitlesAwardableInSeniorityOrderForReading.Count > 0;
			}
		}

		// Token: 0x1700114B RID: 4427
		// (get) Token: 0x06006510 RID: 25872 RVA: 0x00240A52 File Offset: 0x0023EC52
		public Color DefaultColor
		{
			get
			{
				if (this.colorSpectrum.NullOrEmpty<Color>())
				{
					return Color.white;
				}
				return this.colorSpectrum[0];
			}
		}

		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x06006511 RID: 25873 RVA: 0x00240A73 File Offset: 0x0023EC73
		public float BaselinerChance
		{
			get
			{
				if (this.xenotypeSet != null)
				{
					return this.xenotypeSet.BaselinerChance;
				}
				return 1f;
			}
		}

		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x06006512 RID: 25874 RVA: 0x00240A90 File Offset: 0x0023EC90
		public string Description
		{
			get
			{
				if (this.cachedDescription == null)
				{
					if (this.description.NullOrEmpty())
					{
						this.description = string.Empty;
					}
					else
					{
						this.cachedDescription = this.description;
					}
					if (ModsConfig.BiotechActive && this.humanlikeFaction)
					{
						List<XenotypeChance> list = new List<XenotypeChance>();
						this.cachedDescription = this.cachedDescription + "\n\n" + ("MemberXenotypeChances".Translate() + ":").AsTipTitle() + "\n";
						if (this.BaselinerChance > 0f)
						{
							list.Add(new XenotypeChance(XenotypeDefOf.Baseliner, this.BaselinerChance));
						}
						if (this.xenotypeSet != null)
						{
							for (int i = 0; i < this.xenotypeSet.Count; i++)
							{
								if (this.xenotypeSet[i].xenotype != XenotypeDefOf.Baseliner)
								{
									list.Add(this.xenotypeSet[i]);
								}
							}
						}
						if (list.Any<XenotypeChance>())
						{
							list.SortBy((XenotypeChance x) => -x.chance);
							this.cachedDescription += list.Select(delegate(XenotypeChance x)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
								defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(x.xenotype.LabelCap);
								defaultInterpolatedStringHandler.AppendLiteral(": ");
								defaultInterpolatedStringHandler.AppendFormatted(Mathf.Min(x.chance, 1f).ToStringPercent());
								return defaultInterpolatedStringHandler.ToStringAndClear();
							}).ToLineList("  - ", false);
						}
					}
				}
				return this.cachedDescription;
			}
		}

		// Token: 0x06006513 RID: 25875 RVA: 0x00240BFC File Offset: 0x0023EDFC
		public bool PermanentlyHostileTo(FactionDef otherFactionDef)
		{
			return this.permanentEnemy || (this.permanentEnemyToEveryoneExcept != null && !this.permanentEnemyToEveryoneExcept.Contains(otherFactionDef)) || (this.permanentEnemyToEveryoneExceptPlayer && !otherFactionDef.isPlayer);
		}

		// Token: 0x06006514 RID: 25876 RVA: 0x00240C34 File Offset: 0x0023EE34
		public float MinPointsToGeneratePawnGroup(PawnGroupKindDef groupKind, PawnGroupMakerParms parms = null)
		{
			if (this.pawnGroupMakers == null)
			{
				return 0f;
			}
			IEnumerable<PawnGroupMaker> source = from x in this.pawnGroupMakers
			where x.kindDef == groupKind
			select x;
			if (!source.Any<PawnGroupMaker>())
			{
				return 0f;
			}
			return source.Min((PawnGroupMaker pgm) => pgm.MinPointsToGenerateAnything(this, parms));
		}

		// Token: 0x06006515 RID: 25877 RVA: 0x00240CA2 File Offset: 0x0023EEA2
		public bool CanUseStuffForApparel(ThingDef stuffDef)
		{
			return this.apparelStuffFilter == null || this.apparelStuffFilter.Allows(stuffDef);
		}

		// Token: 0x06006516 RID: 25878 RVA: 0x00240CBA File Offset: 0x0023EEBA
		public float RaidCommonalityFromPoints(float points)
		{
			if (points < 0f || this.raidCommonalityFromPointsCurve == null)
			{
				return 1f;
			}
			return this.raidCommonalityFromPointsCurve.Evaluate(points);
		}

		// Token: 0x06006517 RID: 25879 RVA: 0x00240CDE File Offset: 0x0023EEDE
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.apparelStuffFilter != null)
			{
				this.apparelStuffFilter.ResolveReferences();
			}
		}

		// Token: 0x06006518 RID: 25880 RVA: 0x00240CFC File Offset: 0x0023EEFC
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
		}

		// Token: 0x06006519 RID: 25881 RVA: 0x00240D4E File Offset: 0x0023EF4E
		public override IEnumerable<string> ConfigErrors()
		{
			FactionDef.<ConfigErrors>d__150 <ConfigErrors>d__ = new FactionDef.<ConfigErrors>d__150(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600651A RID: 25882 RVA: 0x00240D5E File Offset: 0x0023EF5E
		public static FactionDef Named(string defName)
		{
			return DefDatabase<FactionDef>.GetNamed(defName, true);
		}

		// Token: 0x04003AF1 RID: 15089
		public bool isPlayer;

		// Token: 0x04003AF2 RID: 15090
		public RulePackDef factionNameMaker;

		// Token: 0x04003AF3 RID: 15091
		public RulePackDef settlementNameMaker;

		// Token: 0x04003AF4 RID: 15092
		public RulePackDef playerInitialSettlementNameMaker;

		// Token: 0x04003AF5 RID: 15093
		[MustTranslate]
		public string fixedName;

		// Token: 0x04003AF6 RID: 15094
		public bool humanlikeFaction = true;

		// Token: 0x04003AF7 RID: 15095
		public bool hidden;

		// Token: 0x04003AF8 RID: 15096
		public float listOrderPriority;

		// Token: 0x04003AF9 RID: 15097
		public List<PawnGroupMaker> pawnGroupMakers;

		// Token: 0x04003AFA RID: 15098
		public SimpleCurve raidCommonalityFromPointsCurve;

		// Token: 0x04003AFB RID: 15099
		public bool autoFlee = true;

		// Token: 0x04003AFC RID: 15100
		public FloatRange attackersDownPercentageRangeForAutoFlee = new FloatRange(0.4f, 0.7f);

		// Token: 0x04003AFD RID: 15101
		public bool canSiege;

		// Token: 0x04003AFE RID: 15102
		public bool canStageAttacks;

		// Token: 0x04003AFF RID: 15103
		public bool canUseAvoidGrid = true;

		// Token: 0x04003B00 RID: 15104
		public bool canPsychicRitualSiege;

		// Token: 0x04003B01 RID: 15105
		public float earliestRaidDays;

		// Token: 0x04003B02 RID: 15106
		public FloatRange allowedArrivalTemperatureRange = new FloatRange(-1000f, 1000f);

		// Token: 0x04003B03 RID: 15107
		public SimpleCurve minSettlementTemperatureChanceCurve;

		// Token: 0x04003B04 RID: 15108
		public PawnKindDef basicMemberKind;

		// Token: 0x04003B05 RID: 15109
		public List<ResearchProjectTagDef> startingResearchTags;

		// Token: 0x04003B06 RID: 15110
		public List<ResearchProjectTagDef> startingTechprintsResearchTags;

		// Token: 0x04003B07 RID: 15111
		[NoTranslate]
		public List<string> recipePrerequisiteTags;

		// Token: 0x04003B08 RID: 15112
		public bool rescueesCanJoin;

		// Token: 0x04003B09 RID: 15113
		[MustTranslate]
		public string pawnSingular = "member";

		// Token: 0x04003B0A RID: 15114
		[MustTranslate]
		public string pawnsPlural = "members";

		// Token: 0x04003B0B RID: 15115
		[MustTranslate]
		public string leaderTitle = "leader";

		// Token: 0x04003B0C RID: 15116
		[MustTranslate]
		public string leaderTitleFemale;

		// Token: 0x04003B0D RID: 15117
		[MustTranslate]
		public string royalFavorLabel;

		// Token: 0x04003B0E RID: 15118
		[NoTranslate]
		public string royalFavorIconPath;

		// Token: 0x04003B0F RID: 15119
		public List<PawnKindDef> fixedLeaderKinds;

		// Token: 0x04003B10 RID: 15120
		public bool leaderForceGenerateNewPawn;

		// Token: 0x04003B11 RID: 15121
		public float forageabilityFactor = 1f;

		// Token: 0x04003B12 RID: 15122
		public SimpleCurve maxPawnCostPerTotalPointsCurve;

		// Token: 0x04003B13 RID: 15123
		public List<string> royalTitleTags;

		// Token: 0x04003B14 RID: 15124
		[NoTranslate]
		public string categoryTag;

		// Token: 0x04003B15 RID: 15125
		public bool hostileToFactionlessHumanlikes;

		// Token: 0x04003B16 RID: 15126
		public ThingDef dropPodActive;

		// Token: 0x04003B17 RID: 15127
		public ThingDef dropPodIncoming;

		// Token: 0x04003B18 RID: 15128
		public bool raidsForbidden;

		// Token: 0x04003B19 RID: 15129
		public bool animalsFleeDanger = true;

		// Token: 0x04003B1A RID: 15130
		private PawnGroupKindDef defaultSettlementGroupKindDef;

		// Token: 0x04003B1B RID: 15131
		public bool canRequestTraders = true;

		// Token: 0x04003B1C RID: 15132
		public bool canRequestMilitaryAid = true;

		// Token: 0x04003B1D RID: 15133
		public bool canRequestOrbitalTrader;

		// Token: 0x04003B1E RID: 15134
		public bool canGenerateQuestSites = true;

		// Token: 0x04003B1F RID: 15135
		public bool hideGiftingInHostilityText;

		// Token: 0x04003B20 RID: 15136
		public List<PawnsArrivalModeDef> arrivalModeWhitelist = new List<PawnsArrivalModeDef>();

		// Token: 0x04003B21 RID: 15137
		public List<PawnsArrivalModeDef> arrivalModeBlacklist = new List<PawnsArrivalModeDef>();

		// Token: 0x04003B22 RID: 15138
		public List<PlanetLayerDef> layerWhitelist = new List<PlanetLayerDef>();

		// Token: 0x04003B23 RID: 15139
		public List<PlanetLayerDef> layerBlacklist = new List<PlanetLayerDef>();

		// Token: 0x04003B24 RID: 15140
		public List<PlanetLayerDef> arrivalLayerWhitelist = new List<PlanetLayerDef>();

		// Token: 0x04003B25 RID: 15141
		public List<PlanetLayerDef> arrivalLayerBlacklist = new List<PlanetLayerDef>();

		// Token: 0x04003B26 RID: 15142
		public List<PlanetLayerDef> neutralArrivalLayerWhitelist = new List<PlanetLayerDef>();

		// Token: 0x04003B27 RID: 15143
		public List<PlanetLayerDef> neutralArrivalLayerBlacklist = new List<PlanetLayerDef>();

		// Token: 0x04003B28 RID: 15144
		public List<PlanetLayerDef> raidArrivalLayerWhitelist = new List<PlanetLayerDef>();

		// Token: 0x04003B29 RID: 15145
		public List<PlanetLayerDef> raidArrivalLayerBlacklist = new List<PlanetLayerDef>();

		// Token: 0x04003B2A RID: 15146
		public int requiredCountAtGameStart;

		// Token: 0x04003B2B RID: 15147
		public float settlementGenerationWeight;

		// Token: 0x04003B2C RID: 15148
		public bool generateNewLeaderFromMapMembersOnly;

		// Token: 0x04003B2D RID: 15149
		public int maxConfigurableAtWorldCreation = -1;

		// Token: 0x04003B2E RID: 15150
		public int startingCountAtWorldCreation = 1;

		// Token: 0x04003B2F RID: 15151
		public int configurationListOrderPriority;

		// Token: 0x04003B30 RID: 15152
		public FactionDef replacesFaction;

		// Token: 0x04003B31 RID: 15153
		public bool displayInFactionSelection = true;

		// Token: 0x04003B32 RID: 15154
		public TechLevel techLevel;

		// Token: 0x04003B33 RID: 15155
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFilters;

		// Token: 0x04003B34 RID: 15156
		[NoTranslate]
		private List<string> backstoryCategories;

		// Token: 0x04003B35 RID: 15157
		public ThingFilter apparelStuffFilter;

		// Token: 0x04003B36 RID: 15158
		public ThingSetMakerDef settlementLootMaker;

		// Token: 0x04003B37 RID: 15159
		public ThingSetMakerDef raidLootMaker;

		// Token: 0x04003B38 RID: 15160
		public SimpleCurve raidLootValueFromPointsCurve;

		// Token: 0x04003B39 RID: 15161
		public List<TraderKindDef> caravanTraderKinds = new List<TraderKindDef>();

		// Token: 0x04003B3A RID: 15162
		public List<TraderKindDef> orbitalTraderKinds = new List<TraderKindDef>();

		// Token: 0x04003B3B RID: 15163
		public List<TraderKindDef> visitorTraderKinds = new List<TraderKindDef>();

		// Token: 0x04003B3C RID: 15164
		public List<TraderKindDef> baseTraderKinds = new List<TraderKindDef>();

		// Token: 0x04003B3D RID: 15165
		public XenotypeSet xenotypeSet;

		// Token: 0x04003B3E RID: 15166
		public FloatRange melaninRange = FloatRange.ZeroToOne;

		// Token: 0x04003B3F RID: 15167
		public List<RaidStrategyDef> disallowedRaidStrategies;

		// Token: 0x04003B40 RID: 15168
		public List<RaidAgeRestrictionDef> disallowedRaidAgeRestrictions;

		// Token: 0x04003B41 RID: 15169
		public bool mustStartOneEnemy;

		// Token: 0x04003B42 RID: 15170
		public bool naturalEnemy;

		// Token: 0x04003B43 RID: 15171
		public bool permanentEnemy;

		// Token: 0x04003B44 RID: 15172
		public bool permanentEnemyToEveryoneExceptPlayer;

		// Token: 0x04003B45 RID: 15173
		public List<FactionDef> permanentEnemyToEveryoneExcept;

		// Token: 0x04003B46 RID: 15174
		[NoTranslate]
		public string settlementTexturePath;

		// Token: 0x04003B47 RID: 15175
		[NoTranslate]
		public string factionIconPath;

		// Token: 0x04003B48 RID: 15176
		public List<Color> colorSpectrum;

		// Token: 0x04003B49 RID: 15177
		public List<PawnRelationDef> royalTitleInheritanceRelations;

		// Token: 0x04003B4A RID: 15178
		public Type royalTitleInheritanceWorkerClass;

		// Token: 0x04003B4B RID: 15179
		public List<RoyalImplantRule> royalImplantRules;

		// Token: 0x04003B4C RID: 15180
		public string renounceTitleMessage;

		// Token: 0x04003B4D RID: 15181
		public List<CultureDef> allowedCultures;

		// Token: 0x04003B4E RID: 15182
		public List<MemeDef> requiredMemes;

		// Token: 0x04003B4F RID: 15183
		public List<MemeDef> allowedMemes;

		// Token: 0x04003B50 RID: 15184
		public List<MemeDef> disallowedMemes;

		// Token: 0x04003B51 RID: 15185
		public List<PreceptDef> disallowedPrecepts;

		// Token: 0x04003B52 RID: 15186
		public List<MemeWeight> structureMemeWeights;

		// Token: 0x04003B53 RID: 15187
		public bool classicIdeo;

		// Token: 0x04003B54 RID: 15188
		public bool fixedIdeo;

		// Token: 0x04003B55 RID: 15189
		public string ideoName;

		// Token: 0x04003B56 RID: 15190
		public bool hiddenIdeo;

		// Token: 0x04003B57 RID: 15191
		[MustTranslate]
		public string ideoDescription;

		// Token: 0x04003B58 RID: 15192
		public List<StyleCategoryDef> styles;

		// Token: 0x04003B59 RID: 15193
		public List<DeityPreset> deityPresets;

		// Token: 0x04003B5A RID: 15194
		public List<MemeDef> forcedMemes;

		// Token: 0x04003B5B RID: 15195
		public bool requiredPreceptsOnly;

		// Token: 0x04003B5C RID: 15196
		[Obsolete("Will be removed in future version.")]
		public int maxCountAtGameStart;

		// Token: 0x04003B5D RID: 15197
		[Obsolete("Will be removed in future version.")]
		public bool canMakeRandomly;

		// Token: 0x04003B5E RID: 15198
		[MayTranslate]
		public string dialogFactionGreetingHostile;

		// Token: 0x04003B5F RID: 15199
		[MayTranslate]
		public string dialogFactionGreetingHostileAppreciative;

		// Token: 0x04003B60 RID: 15200
		[MayTranslate]
		public string dialogFactionGreetingWary;

		// Token: 0x04003B61 RID: 15201
		[MayTranslate]
		public string dialogFactionGreetingWarm;

		// Token: 0x04003B62 RID: 15202
		[MayTranslate]
		public string dialogMilitaryAidSent;

		// Token: 0x04003B63 RID: 15203
		[MustTranslate]
		public string messageDefendersAttacking;

		// Token: 0x04003B64 RID: 15204
		[Unsaved(false)]
		private Texture2D factionIcon;

		// Token: 0x04003B65 RID: 15205
		[Unsaved(false)]
		private Texture2D settlementTexture;

		// Token: 0x04003B66 RID: 15206
		[Unsaved(false)]
		private Texture2D royalFavorIcon;

		// Token: 0x04003B67 RID: 15207
		[Unsaved(false)]
		private string cachedDescription;

		// Token: 0x04003B68 RID: 15208
		[Unsaved(false)]
		private List<RoyalTitleDef> royalTitlesAwardableInSeniorityOrderForReading;

		// Token: 0x04003B69 RID: 15209
		[Unsaved(false)]
		private List<RoyalTitleDef> royalTitlesAllInSeniorityOrderForReading;

		// Token: 0x04003B6A RID: 15210
		[Unsaved(false)]
		private RoyalTitleInheritanceWorker royalTitleInheritanceWorker;
	}
}
