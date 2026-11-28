using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F2B RID: 3883
	public class AbilityDef : Def
	{
		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x0600645B RID: 25691 RVA: 0x0023DC8C File Offset: 0x0023BE8C
		public float EntropyGain
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_EntropyGain, 0f);
			}
		}

		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x0600645C RID: 25692 RVA: 0x0023DCA3 File Offset: 0x0023BEA3
		public float PsyfocusCost
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_PsyfocusCost, 0f);
			}
		}

		// Token: 0x17001118 RID: 4376
		// (get) Token: 0x0600645D RID: 25693 RVA: 0x0023DCBA File Offset: 0x0023BEBA
		public float EffectRadius
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_EffectRadius, 0f);
			}
		}

		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x0600645E RID: 25694 RVA: 0x0023DCD1 File Offset: 0x0023BED1
		public bool HasAreaOfEffect
		{
			get
			{
				return this.EffectRadius > float.Epsilon;
			}
		}

		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x0600645F RID: 25695 RVA: 0x0023DCE0 File Offset: 0x0023BEE0
		public float DetectionChance
		{
			get
			{
				if (this.detectionChanceOverride < 0f)
				{
					return this.GetStatValueAbstract(StatDefOf.Ability_DetectChancePerEntropy, null);
				}
				return this.detectionChanceOverride;
			}
		}

		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x06006460 RID: 25696 RVA: 0x0023DD02 File Offset: 0x0023BF02
		public bool IsPsycast
		{
			get
			{
				return typeof(Psycast).IsAssignableFrom(this.abilityClass);
			}
		}

		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x06006461 RID: 25697 RVA: 0x0023DD19 File Offset: 0x0023BF19
		public string PsyfocusCostPercent
		{
			get
			{
				if (this.psyfocusCostPercent.NullOrEmpty())
				{
					this.psyfocusCostPercent = this.PsyfocusCost.ToStringPercent();
				}
				return this.psyfocusCostPercent;
			}
		}

		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x06006462 RID: 25698 RVA: 0x0023DD3F File Offset: 0x0023BF3F
		public string PsyfocusCostPercentMax
		{
			get
			{
				if (this.psyfocusCostPercentMax.NullOrEmpty())
				{
					this.psyfocusCostPercentMax = this.PsyfocusCostRange.max.ToStringPercent();
				}
				return this.psyfocusCostPercentMax;
			}
		}

		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x06006463 RID: 25699 RVA: 0x0023DD6C File Offset: 0x0023BF6C
		public int RequiredPsyfocusBand
		{
			get
			{
				if (this.requiredPsyfocusBandCached == -1)
				{
					this.requiredPsyfocusBandCached = Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand.Count - 1;
					for (int i = 0; i < Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand.Count; i++)
					{
						int num = Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[i];
						if (this.level <= num)
						{
							this.requiredPsyfocusBandCached = i;
							break;
						}
					}
				}
				return this.requiredPsyfocusBandCached;
			}
		}

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x06006464 RID: 25700 RVA: 0x0023DDD0 File Offset: 0x0023BFD0
		public bool AnyCompOverridesPsyfocusCost
		{
			get
			{
				if (this.anyCompOverridesPsyfocusCost == null)
				{
					this.anyCompOverridesPsyfocusCost = new bool?(false);
					if (this.comps != null)
					{
						using (List<AbilityCompProperties>.Enumerator enumerator = this.comps.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.OverridesPsyfocusCost)
								{
									this.anyCompOverridesPsyfocusCost = new bool?(true);
									break;
								}
							}
						}
					}
				}
				return this.anyCompOverridesPsyfocusCost.Value;
			}
		}

		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x06006465 RID: 25701 RVA: 0x0023DE5C File Offset: 0x0023C05C
		public FloatRange PsyfocusCostRange
		{
			get
			{
				if (this.psyfocusCostRange.min < 0f)
				{
					if (!this.AnyCompOverridesPsyfocusCost)
					{
						this.psyfocusCostRange = new FloatRange(this.PsyfocusCost, this.PsyfocusCost);
					}
					else
					{
						foreach (AbilityCompProperties abilityCompProperties in this.comps)
						{
							if (abilityCompProperties.OverridesPsyfocusCost)
							{
								this.psyfocusCostRange = abilityCompProperties.PsyfocusCostRange;
								break;
							}
						}
					}
				}
				return this.psyfocusCostRange;
			}
		}

		// Token: 0x06006466 RID: 25702 RVA: 0x0023DEF8 File Offset: 0x0023C0F8
		public IEnumerable<string> StatSummary(Pawn forPawn = null)
		{
			AbilityDef.<StatSummary>d__86 <StatSummary>d__ = new AbilityDef.<StatSummary>d__86(-2);
			<StatSummary>d__.<>4__this = this;
			<StatSummary>d__.<>3__forPawn = forPawn;
			return <StatSummary>d__;
		}

		// Token: 0x06006467 RID: 25703 RVA: 0x0023DF0F File Offset: 0x0023C10F
		public float EffectDuration(Pawn forPawn = null)
		{
			return this.GetStatValueAbstract(StatDefOf.Ability_Duration, forPawn);
		}

		// Token: 0x06006468 RID: 25704 RVA: 0x0023DF1D File Offset: 0x0023C11D
		public override void PostLoad()
		{
			if (!string.IsNullOrEmpty(this.iconPath))
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				});
			}
		}

		// Token: 0x06006469 RID: 25705 RVA: 0x0023DF40 File Offset: 0x0023C140
		public string GetTooltip(Pawn pawn = null)
		{
			if (this.cachedTooltip == null || this.cachedTooltipPawn != pawn)
			{
				this.cachedTooltip = this.LabelCap.Colorize(ColoredText.TipSectionTitleColor) + ((this.level > 0) ? ("\n" + "Level".Translate().CapitalizeFirst() + " " + this.level.ToString()) : "") + "\n\n" + this.description;
				this.cachedTooltipPawn = pawn;
				string text = this.StatSummary(pawn).ToLineList(null, false);
				if (!text.NullOrEmpty())
				{
					this.cachedTooltip = this.cachedTooltip + "\n\n" + text;
				}
			}
			if (pawn != null && ModsConfig.RoyaltyActive && this.IsPsycast && this.level > 0)
			{
				Faction first = Faction.GetMinTitleForImplantAllFactions(HediffDefOf.PsychicAmplifier).First;
				if (first != null)
				{
					RoyalTitleDef minTitleForImplant = first.GetMinTitleForImplant(HediffDefOf.PsychicAmplifier, this.level);
					RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(first);
					if (minTitleForImplant != null && (currentTitle == null || currentTitle.seniority < minTitleForImplant.seniority) && this.DetectionChance > 0f)
					{
						return this.cachedTooltip + "\n\n" + "PsycastIsIllegal".Translate(pawn.Named("PAWN"), minTitleForImplant.GetLabelCapFor(pawn).Named("TITLE")).Colorize(ColoredText.WarningColor);
					}
				}
			}
			return this.cachedTooltip;
		}

		// Token: 0x0600646A RID: 25706 RVA: 0x0023E0DA File Offset: 0x0023C2DA
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			AbilityDef.<SpecialDisplayStats>d__90 <SpecialDisplayStats>d__ = new AbilityDef.<SpecialDisplayStats>d__90(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x17001121 RID: 4385
		// (get) Token: 0x0600646B RID: 25707 RVA: 0x0023E0F1 File Offset: 0x0023C2F1
		public Texture2D WarmupMoteSocialSymbol
		{
			get
			{
				if (!this.warmupMoteSocialSymbol.NullOrEmpty() && this.warmupMoteSocialSymbolCached == null)
				{
					this.warmupMoteSocialSymbolCached = ContentFinder<Texture2D>.Get(this.warmupMoteSocialSymbol, true);
				}
				return this.warmupMoteSocialSymbolCached;
			}
		}

		// Token: 0x0600646C RID: 25708 RVA: 0x0023E126 File Offset: 0x0023C326
		public override IEnumerable<string> ConfigErrors()
		{
			AbilityDef.<ConfigErrors>d__94 <ConfigErrors>d__ = new AbilityDef.<ConfigErrors>d__94(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003961 RID: 14689
		public Type abilityClass = typeof(Ability);

		// Token: 0x04003962 RID: 14690
		public Type gizmoClass = typeof(Command_Ability);

		// Token: 0x04003963 RID: 14691
		public List<AbilityCompProperties> comps = new List<AbilityCompProperties>();

		// Token: 0x04003964 RID: 14692
		public AbilityCategoryDef category;

		// Token: 0x04003965 RID: 14693
		public int displayOrder;

		// Token: 0x04003966 RID: 14694
		public List<StatModifier> statBases;

		// Token: 0x04003967 RID: 14695
		public VerbProperties verbProperties;

		// Token: 0x04003968 RID: 14696
		public KeyBindingDef hotKey;

		// Token: 0x04003969 RID: 14697
		public JobDef jobDef;

		// Token: 0x0400396A RID: 14698
		public ThingDef warmupMote;

		// Token: 0x0400396B RID: 14699
		public EffecterDef warmupEffecter;

		// Token: 0x0400396C RID: 14700
		public FleckDef emittedFleck;

		// Token: 0x0400396D RID: 14701
		public int emissionInterval;

		// Token: 0x0400396E RID: 14702
		public string warmupMoteSocialSymbol;

		// Token: 0x0400396F RID: 14703
		public SoundDef warmupStartSound;

		// Token: 0x04003970 RID: 14704
		public SoundDef warmupSound;

		// Token: 0x04003971 RID: 14705
		public SoundDef warmupPreEndSound;

		// Token: 0x04003972 RID: 14706
		public float warmupPreEndSoundSeconds;

		// Token: 0x04003973 RID: 14707
		public Vector3 moteDrawOffset;

		// Token: 0x04003974 RID: 14708
		public float moteOffsetAmountTowardsTarget;

		// Token: 0x04003975 RID: 14709
		public bool canUseAoeToGetTargets = true;

		// Token: 0x04003976 RID: 14710
		public bool useAverageTargetPositionForWarmupEffecter;

		// Token: 0x04003977 RID: 14711
		public bool targetRequired = true;

		// Token: 0x04003978 RID: 14712
		public bool targetWorldCell;

		// Token: 0x04003979 RID: 14713
		public bool showGizmoOnWorldView;

		// Token: 0x0400397A RID: 14714
		public bool aiCanUse;

		// Token: 0x0400397B RID: 14715
		public bool ai_SearchAOEForTargets;

		// Token: 0x0400397C RID: 14716
		public bool ai_IsOffensive = true;

		// Token: 0x0400397D RID: 14717
		public bool ai_IsIncendiary = true;

		// Token: 0x0400397E RID: 14718
		public bool groupAbility;

		// Token: 0x0400397F RID: 14719
		public int level;

		// Token: 0x04003980 RID: 14720
		public IntRange cooldownTicksRange;

		// Token: 0x04003981 RID: 14721
		public bool cooldownPerCharge;

		// Token: 0x04003982 RID: 14722
		public bool hasExternallyHandledCooldown;

		// Token: 0x04003983 RID: 14723
		public int charges = -1;

		// Token: 0x04003984 RID: 14724
		public AbilityGroupDef groupDef;

		// Token: 0x04003985 RID: 14725
		public bool overrideGroupCooldown;

		// Token: 0x04003986 RID: 14726
		public List<MemeDef> requiredMemes;

		// Token: 0x04003987 RID: 14727
		public bool sendLetterOnCooldownComplete;

		// Token: 0x04003988 RID: 14728
		public bool sendMessageOnCooldownComplete;

		// Token: 0x04003989 RID: 14729
		public bool displayGizmoWhileUndrafted;

		// Token: 0x0400398A RID: 14730
		public bool disableGizmoWhileUndrafted = true;

		// Token: 0x0400398B RID: 14731
		public bool writeCombatLog;

		// Token: 0x0400398C RID: 14732
		public bool stunTargetWhileCasting;

		// Token: 0x0400398D RID: 14733
		public bool showPsycastEffects = true;

		// Token: 0x0400398E RID: 14734
		public bool showCastingProgressBar;

		// Token: 0x0400398F RID: 14735
		public float detectionChanceOverride = -1f;

		// Token: 0x04003990 RID: 14736
		public float uiOrder;

		// Token: 0x04003991 RID: 14737
		public bool waitForJobEnd;

		// Token: 0x04003992 RID: 14738
		public bool showWhenDrafted = true;

		// Token: 0x04003993 RID: 14739
		public bool showOnCharacterCard = true;

		// Token: 0x04003994 RID: 14740
		public bool hostile = true;

		// Token: 0x04003995 RID: 14741
		public bool casterMustBeCapableOfViolence = true;

		// Token: 0x04003996 RID: 14742
		[MustTranslate]
		public string confirmationDialogText;

		// Token: 0x04003997 RID: 14743
		[NoTranslate]
		public string iconPath;

		// Token: 0x04003998 RID: 14744
		public Texture2D uiIcon = BaseContent.BadTex;

		// Token: 0x04003999 RID: 14745
		private string cachedTooltip;

		// Token: 0x0400399A RID: 14746
		private Pawn cachedTooltipPawn;

		// Token: 0x0400399B RID: 14747
		private List<string> cachedTargets;

		// Token: 0x0400399C RID: 14748
		private int requiredPsyfocusBandCached = -1;

		// Token: 0x0400399D RID: 14749
		private bool? anyCompOverridesPsyfocusCost;

		// Token: 0x0400399E RID: 14750
		private FloatRange psyfocusCostRange = new FloatRange(-1f, -1f);

		// Token: 0x0400399F RID: 14751
		private string psyfocusCostPercent;

		// Token: 0x040039A0 RID: 14752
		private string psyfocusCostPercentMax;

		// Token: 0x040039A1 RID: 14753
		private Texture2D warmupMoteSocialSymbolCached;
	}
}
