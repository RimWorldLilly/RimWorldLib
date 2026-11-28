using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE9 RID: 4073
	public class StatDef : Def
	{
		// Token: 0x170011A3 RID: 4515
		// (get) Token: 0x060066F8 RID: 26360 RVA: 0x00247E38 File Offset: 0x00246038
		public StatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					if (this.parts != null)
					{
						for (int i = 0; i < this.parts.Count; i++)
						{
							this.parts[i].parentStat = this;
						}
					}
					this.workerInt = (StatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.InitSetStat(this);
				}
				return this.workerInt;
			}
		}

		// Token: 0x170011A4 RID: 4516
		// (get) Token: 0x060066F9 RID: 26361 RVA: 0x00247EA5 File Offset: 0x002460A5
		public ToStringStyle ToStringStyleUnfinalized
		{
			get
			{
				if (this.toStringStyleUnfinalized == null)
				{
					return this.toStringStyle;
				}
				return this.toStringStyleUnfinalized.Value;
			}
		}

		// Token: 0x170011A5 RID: 4517
		// (get) Token: 0x060066FA RID: 26362 RVA: 0x00247EC6 File Offset: 0x002460C6
		public string LabelForFullStatList
		{
			get
			{
				if (!this.labelForFullStatList.NullOrEmpty())
				{
					return this.labelForFullStatList;
				}
				return this.label;
			}
		}

		// Token: 0x170011A6 RID: 4518
		// (get) Token: 0x060066FB RID: 26363 RVA: 0x00247EE2 File Offset: 0x002460E2
		public string LabelForFullStatListCap
		{
			get
			{
				return this.LabelForFullStatList.CapitalizeFirst(this);
			}
		}

		// Token: 0x170011A7 RID: 4519
		// (get) Token: 0x060066FC RID: 26364 RVA: 0x00247EF0 File Offset: 0x002460F0
		public string OffsetLabel
		{
			get
			{
				return this.offsetLabel ?? this.label;
			}
		}

		// Token: 0x170011A8 RID: 4520
		// (get) Token: 0x060066FD RID: 26365 RVA: 0x00247F02 File Offset: 0x00246102
		public string OffsetLabelCap
		{
			get
			{
				return this.OffsetLabel.CapitalizeFirst();
			}
		}

		// Token: 0x060066FE RID: 26366 RVA: 0x00247F0F File Offset: 0x0024610F
		public override IEnumerable<string> ConfigErrors()
		{
			StatDef.<ConfigErrors>d__78 <ConfigErrors>d__ = new StatDef.<ConfigErrors>d__78(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x060066FF RID: 26367 RVA: 0x00247F1F File Offset: 0x0024611F
		public string ValueToString(float val, ToStringNumberSense numberSense = ToStringNumberSense.Absolute, bool finalized = true)
		{
			return this.Worker.ValueToString(val, finalized, numberSense);
		}

		// Token: 0x06006700 RID: 26368 RVA: 0x00247F2F File Offset: 0x0024612F
		public static StatDef Named(string defName)
		{
			return DefDatabase<StatDef>.GetNamed(defName, true);
		}

		// Token: 0x06006701 RID: 26369 RVA: 0x00247F38 File Offset: 0x00246138
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.parts != null)
			{
				List<StatPart> partsCopy = this.parts.ToList<StatPart>();
				this.parts.SortBy((StatPart x) => -x.priority, (StatPart x) => partsCopy.IndexOf(x));
			}
		}

		// Token: 0x06006702 RID: 26370 RVA: 0x00247FA0 File Offset: 0x002461A0
		public T GetStatPart<T>() where T : StatPart
		{
			return this.parts.OfType<T>().FirstOrDefault<T>();
		}

		// Token: 0x06006703 RID: 26371 RVA: 0x00247FB4 File Offset: 0x002461B4
		public bool CanShowWithLoadedMods()
		{
			if (!this.showIfModsLoaded.NullOrEmpty<string>())
			{
				for (int i = 0; i < this.showIfModsLoaded.Count; i++)
				{
					if (!ModsConfig.IsActive(this.showIfModsLoaded[i]))
					{
						return false;
					}
				}
			}
			if (!this.showIfModsLoadedAny.NullOrEmpty<string>())
			{
				bool result = false;
				for (int j = 0; j < this.showIfModsLoadedAny.Count; j++)
				{
					if (ModsConfig.IsActive(this.showIfModsLoadedAny[j]))
					{
						result = true;
						break;
					}
				}
				return result;
			}
			return true;
		}

		// Token: 0x06006704 RID: 26372 RVA: 0x00248038 File Offset: 0x00246238
		private static void PopulateMutableStats()
		{
			StatDef.mutableStats = new HashSet<StatDef>();
			foreach (TraitDef traitDef in DefDatabase<TraitDef>.AllDefsListForReading)
			{
				foreach (TraitDegreeData traitDegreeData in traitDef.degreeDatas)
				{
					StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(traitDegreeData.statOffsets);
					StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(traitDegreeData.statFactors);
				}
			}
			foreach (HediffDef hediffDef in DefDatabase<HediffDef>.AllDefsListForReading)
			{
				if (hediffDef.stages != null)
				{
					foreach (HediffStage hediffStage in hediffDef.stages)
					{
						StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(hediffStage.statOffsets);
						StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(hediffStage.statFactors);
						if (hediffStage.statOffsetEffectMultiplier != null)
						{
							StatDef.mutableStats.Add(hediffStage.statOffsetEffectMultiplier);
						}
						if (hediffStage.statFactorEffectMultiplier != null)
						{
							StatDef.mutableStats.Add(hediffStage.statFactorEffectMultiplier);
						}
						if (hediffStage.capacityFactorEffectMultiplier != null)
						{
							StatDef.mutableStats.Add(hediffStage.capacityFactorEffectMultiplier);
						}
					}
				}
			}
			foreach (PreceptDef preceptDef in DefDatabase<PreceptDef>.AllDefsListForReading)
			{
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(preceptDef.statOffsets);
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(preceptDef.statFactors);
				if (preceptDef.roleEffects != null)
				{
					foreach (RoleEffect roleEffect in preceptDef.roleEffects)
					{
						RoleEffect_PawnStatOffset roleEffect_PawnStatOffset = roleEffect as RoleEffect_PawnStatOffset;
						if (roleEffect_PawnStatOffset != null)
						{
							StatDef.mutableStats.Add(roleEffect_PawnStatOffset.statDef);
						}
						RoleEffect_PawnStatFactor roleEffect_PawnStatFactor = roleEffect as RoleEffect_PawnStatFactor;
						if (roleEffect_PawnStatFactor != null)
						{
							StatDef.mutableStats.Add(roleEffect_PawnStatFactor.statDef);
						}
					}
				}
				if (preceptDef.abilityStatFactors != null)
				{
					foreach (AbilityStatModifiers abilityStatModifiers in preceptDef.abilityStatFactors)
					{
						StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(abilityStatModifiers.modifiers);
					}
				}
			}
			foreach (GeneDef geneDef in DefDatabase<GeneDef>.AllDefsListForReading)
			{
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(geneDef.statOffsets);
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(geneDef.statFactors);
				if (geneDef.conditionalStatAffecters != null)
				{
					foreach (ConditionalStatAffecter conditionalStatAffecter in geneDef.conditionalStatAffecters)
					{
						StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(conditionalStatAffecter.statOffsets);
						StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(conditionalStatAffecter.statFactors);
					}
				}
			}
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
			{
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(thingDef.equippedStatOffsets);
				if (thingDef.HasAssignableCompFrom(typeof(CompFacility)))
				{
					CompProperties_Facility compProperties = thingDef.GetCompProperties<CompProperties_Facility>();
					if (((compProperties != null) ? compProperties.statOffsets : null) != null)
					{
						StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(compProperties.statOffsets);
					}
					CompProperties_FacilityQualityBased compProperties_FacilityQualityBased = compProperties as CompProperties_FacilityQualityBased;
					if (compProperties_FacilityQualityBased != null)
					{
						foreach (StatDef item in compProperties_FacilityQualityBased.statOffsetsPerQuality.Keys)
						{
							StatDef.mutableStats.Add(item);
						}
					}
				}
			}
			foreach (WeaponTraitDef weaponTraitDef in DefDatabase<WeaponTraitDef>.AllDefsListForReading)
			{
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(weaponTraitDef.equippedStatOffsets);
			}
			foreach (LifeStageDef lifeStageDef in DefDatabase<LifeStageDef>.AllDefsListForReading)
			{
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(lifeStageDef.statFactors);
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(lifeStageDef.statOffsets);
			}
			foreach (InspirationDef inspirationDef in DefDatabase<InspirationDef>.AllDefsListForReading)
			{
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(inspirationDef.statOffsets);
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(inspirationDef.statFactors);
			}
		}

		// Token: 0x06006705 RID: 26373 RVA: 0x00248568 File Offset: 0x00246768
		public bool IsImmutable()
		{
			return !(this.workerClass != typeof(StatWorker)) && this.skillNeedOffsets.NullOrEmpty<SkillNeed>() && this.skillNeedFactors.NullOrEmpty<SkillNeed>() && this.capacityOffsets.NullOrEmpty<PawnCapacityOffset>() && this.capacityFactors.NullOrEmpty<PawnCapacityFactor>() && this.statFactors.NullOrEmpty<StatDef>() && this.parts.NullOrEmpty<StatPart>() && this.postProcessStatFactors.NullOrEmpty<StatDef>() && !StatDef.mutableStats.Contains(this);
		}

		// Token: 0x06006706 RID: 26374 RVA: 0x00248604 File Offset: 0x00246804
		public static void SetImmutability()
		{
			StatDef.PopulateMutableStats();
			foreach (StatDef statDef in DefDatabase<StatDef>.AllDefsListForReading)
			{
				statDef.immutable = statDef.IsImmutable();
				statDef.Worker.SetCacheability(statDef.immutable);
			}
		}

		// Token: 0x06006707 RID: 26375 RVA: 0x00248674 File Offset: 0x00246874
		public static void ResetStaticData()
		{
			StatDef.mutableStats = null;
			foreach (StatDef statDef in DefDatabase<StatDef>.AllDefsListForReading)
			{
				statDef.immutable = false;
				statDef.Worker.DeleteStatCache();
			}
		}

		// Token: 0x0600670A RID: 26378 RVA: 0x002487C4 File Offset: 0x002469C4
		[CompilerGenerated]
		internal static void <PopulateMutableStats>g__AddStatsFromModifiers|84_0(List<StatModifier> mods)
		{
			if (mods != null)
			{
				StatDef.mutableStats.AddRange(from mod in mods
				select mod.stat);
			}
		}

		// Token: 0x0400405A RID: 16474
		private static HashSet<StatDef> mutableStats;

		// Token: 0x0400405B RID: 16475
		public StatCategoryDef category;

		// Token: 0x0400405C RID: 16476
		public Type workerClass = typeof(StatWorker);

		// Token: 0x0400405D RID: 16477
		public string labelForFullStatList;

		// Token: 0x0400405E RID: 16478
		public bool forInformationOnly;

		// Token: 0x0400405F RID: 16479
		[MustTranslate]
		private string offsetLabel;

		// Token: 0x04004060 RID: 16480
		public float hideAtValue = -2.1474836E+09f;

		// Token: 0x04004061 RID: 16481
		public bool alwaysHide;

		// Token: 0x04004062 RID: 16482
		public bool showNonAbstract = true;

		// Token: 0x04004063 RID: 16483
		public bool showIfUndefined = true;

		// Token: 0x04004064 RID: 16484
		public bool showOnPawns = true;

		// Token: 0x04004065 RID: 16485
		public bool showOnHumanlikes = true;

		// Token: 0x04004066 RID: 16486
		public bool showOnNonWildManHumanlikes = true;

		// Token: 0x04004067 RID: 16487
		public bool showOnAnimals = true;

		// Token: 0x04004068 RID: 16488
		public bool showOnMechanoids = true;

		// Token: 0x04004069 RID: 16489
		public bool showOnNonWorkTables = true;

		// Token: 0x0400406A RID: 16490
		public bool showOnEntities = true;

		// Token: 0x0400406B RID: 16491
		public bool showOnDrones = true;

		// Token: 0x0400406C RID: 16492
		public bool showOnNonPowerPlants = true;

		// Token: 0x0400406D RID: 16493
		public bool showOnDefaultValue = true;

		// Token: 0x0400406E RID: 16494
		public bool showOnUnhaulables = true;

		// Token: 0x0400406F RID: 16495
		public bool showOnUntradeables = true;

		// Token: 0x04004070 RID: 16496
		public List<string> showIfModsLoaded;

		// Token: 0x04004071 RID: 16497
		public List<string> showIfModsLoadedAny;

		// Token: 0x04004072 RID: 16498
		public List<HediffDef> showIfHediffsPresent;

		// Token: 0x04004073 RID: 16499
		public bool neverDisabled;

		// Token: 0x04004074 RID: 16500
		public bool showZeroBaseValue;

		// Token: 0x04004075 RID: 16501
		public bool showOnSlavesOnly;

		// Token: 0x04004076 RID: 16502
		public bool showOnPlayerMechanoids;

		// Token: 0x04004077 RID: 16503
		public DevelopmentalStage showDevelopmentalStageFilter = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04004078 RID: 16504
		public bool hideInClassicMode;

		// Token: 0x04004079 RID: 16505
		public List<PawnKindDef> showOnPawnKind;

		// Token: 0x0400407A RID: 16506
		public bool overridesHideStats;

		// Token: 0x0400407B RID: 16507
		public int displayPriorityInCategory;

		// Token: 0x0400407C RID: 16508
		public ToStringNumberSense toStringNumberSense = ToStringNumberSense.Absolute;

		// Token: 0x0400407D RID: 16509
		public ToStringStyle toStringStyle;

		// Token: 0x0400407E RID: 16510
		private ToStringStyle? toStringStyleUnfinalized;

		// Token: 0x0400407F RID: 16511
		[MustTranslate]
		public string formatString;

		// Token: 0x04004080 RID: 16512
		[MustTranslate]
		public string formatStringUnfinalized;

		// Token: 0x04004081 RID: 16513
		public bool finalizeEquippedStatOffset = true;

		// Token: 0x04004082 RID: 16514
		[MustTranslate]
		public string statFactorsExplanationHeader;

		// Token: 0x04004083 RID: 16515
		public float defaultBaseValue = 1f;

		// Token: 0x04004084 RID: 16516
		public List<SkillNeed> skillNeedOffsets;

		// Token: 0x04004085 RID: 16517
		public float noSkillOffset;

		// Token: 0x04004086 RID: 16518
		public List<PawnCapacityOffset> capacityOffsets;

		// Token: 0x04004087 RID: 16519
		public List<StatDef> statFactors;

		// Token: 0x04004088 RID: 16520
		public bool applyFactorsIfNegative = true;

		// Token: 0x04004089 RID: 16521
		public List<SkillNeed> skillNeedFactors;

		// Token: 0x0400408A RID: 16522
		public float noSkillFactor = 1f;

		// Token: 0x0400408B RID: 16523
		public List<PawnCapacityFactor> capacityFactors;

		// Token: 0x0400408C RID: 16524
		public SimpleCurve postProcessCurve;

		// Token: 0x0400408D RID: 16525
		public List<StatDef> postProcessStatFactors;

		// Token: 0x0400408E RID: 16526
		public float minValue = -9999999f;

		// Token: 0x0400408F RID: 16527
		public float maxValue = 9999999f;

		// Token: 0x04004090 RID: 16528
		public float valueIfMissing;

		// Token: 0x04004091 RID: 16529
		public bool roundValue;

		// Token: 0x04004092 RID: 16530
		public float roundToFiveOver = float.MaxValue;

		// Token: 0x04004093 RID: 16531
		public bool minifiedThingInherits;

		// Token: 0x04004094 RID: 16532
		public bool supressDisabledError;

		// Token: 0x04004095 RID: 16533
		public bool cacheable;

		// Token: 0x04004096 RID: 16534
		public bool displayMaxWhenAboveOrEqual;

		// Token: 0x04004097 RID: 16535
		public bool scenarioRandomizable;

		// Token: 0x04004098 RID: 16536
		public SkillDef disableIfSkillDisabled;

		// Token: 0x04004099 RID: 16537
		public List<StatPart> parts;

		// Token: 0x0400409A RID: 16538
		[Unsaved(false)]
		private StatWorker workerInt;

		// Token: 0x0400409B RID: 16539
		[Unsaved(false)]
		public bool immutable;
	}
}
