using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200072F RID: 1839
	public class ThingFilter : IExposable
	{
		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06003A02 RID: 14850 RVA: 0x00171CBE File Offset: 0x0016FEBE
		public TreeNode_ThingCategory OverrideRootNode
		{
			get
			{
				if (this.overrideRootDef == null)
				{
					return null;
				}
				return this.overrideRootDef.treeNode;
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06003A03 RID: 14851 RVA: 0x00171CD5 File Offset: 0x0016FED5
		public TreeNode_ThingCategory RootNode
		{
			get
			{
				if (this.overrideRootDef == null)
				{
					return ThingCategoryNodeDatabase.RootNode;
				}
				return this.overrideRootDef.treeNode;
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06003A04 RID: 14852 RVA: 0x00171CF0 File Offset: 0x0016FEF0
		public bool OnlySpecialFilters
		{
			get
			{
				return this.onlySpecialFilters;
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x06003A05 RID: 14853 RVA: 0x00171CF8 File Offset: 0x0016FEF8
		public string Summary
		{
			get
			{
				if (!this.customSummary.NullOrEmpty())
				{
					return this.customSummary;
				}
				if (this.thingDefs != null && this.thingDefs.Count == 1 && this.categories.NullOrEmpty<string>() && this.tradeTagsToAllow.NullOrEmpty<string>() && this.tradeTagsToDisallow.NullOrEmpty<string>() && this.thingSetMakerTagsToAllow.NullOrEmpty<string>() && this.thingSetMakerTagsToDisallow.NullOrEmpty<string>() && this.disallowedCategories.NullOrEmpty<string>() && this.specialFiltersToAllow.NullOrEmpty<string>() && this.specialFiltersToDisallow.NullOrEmpty<string>() && this.stuffCategoriesToAllow.NullOrEmpty<StuffCategoryDef>() && this.allowAllWhoCanMake.NullOrEmpty<ThingDef>() && this.disallowWorsePreferability == FoodPreferability.Undefined && !this.disallowInedibleByHuman && !this.disallowMedicalDrugs && !this.disallowDoesntProduceMeat && !this.disallowNotEverStorable && this.allowWithComp == null && this.disallowWithComp == null && this.disallowCheaperThan == -3.4028235E+38f && this.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					return this.thingDefs[0].label;
				}
				if (this.thingDefs.NullOrEmpty<ThingDef>() && this.categories != null && this.categories.Count == 1 && this.tradeTagsToAllow.NullOrEmpty<string>() && this.tradeTagsToDisallow.NullOrEmpty<string>() && this.thingSetMakerTagsToAllow.NullOrEmpty<string>() && this.thingSetMakerTagsToDisallow.NullOrEmpty<string>() && this.disallowedCategories.NullOrEmpty<string>() && this.specialFiltersToAllow.NullOrEmpty<string>() && this.specialFiltersToDisallow.NullOrEmpty<string>() && this.stuffCategoriesToAllow.NullOrEmpty<StuffCategoryDef>() && this.allowAllWhoCanMake.NullOrEmpty<ThingDef>() && this.disallowWorsePreferability == FoodPreferability.Undefined && !this.disallowInedibleByHuman && !this.disallowMedicalDrugs && !this.disallowDoesntProduceMeat && !this.disallowNotEverStorable && this.allowWithComp == null && this.disallowWithComp == null && this.disallowCheaperThan == -3.4028235E+38f && this.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					return DefDatabase<ThingCategoryDef>.GetNamed(this.categories[0], true).label;
				}
				if (this.allowedDefs.Count == 1)
				{
					return this.allowedDefs.First<ThingDef>().label;
				}
				return "UsableIngredients".Translate();
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06003A06 RID: 14854 RVA: 0x00171F9C File Offset: 0x0017019C
		public ThingRequest BestThingRequest
		{
			get
			{
				if (this.allowedDefs.Count == 1)
				{
					return ThingRequest.ForDef(this.allowedDefs.First<ThingDef>());
				}
				bool flag = true;
				bool flag2 = true;
				foreach (ThingDef thingDef in this.allowedDefs)
				{
					if (!thingDef.EverHaulable)
					{
						flag = false;
					}
					if (thingDef.category != ThingCategory.Pawn)
					{
						flag2 = false;
					}
				}
				if (flag)
				{
					return ThingRequest.ForGroup(ThingRequestGroup.HaulableEver);
				}
				if (flag2)
				{
					return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
				}
				return ThingRequest.ForGroup(ThingRequestGroup.Everything);
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06003A07 RID: 14855 RVA: 0x0017203C File Offset: 0x0017023C
		public ThingDef AnyAllowedDef
		{
			get
			{
				return this.allowedDefs.FirstOrDefault<ThingDef>();
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06003A08 RID: 14856 RVA: 0x00172049 File Offset: 0x00170249
		public IEnumerable<ThingDef> AllowedThingDefs
		{
			get
			{
				return this.allowedDefs;
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06003A09 RID: 14857 RVA: 0x00172051 File Offset: 0x00170251
		private static IEnumerable<ThingDef> AllStorableThingDefs
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefs
				where def.EverStorable(true)
				select def;
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06003A0A RID: 14858 RVA: 0x0017207C File Offset: 0x0017027C
		public int AllowedDefCount
		{
			get
			{
				return this.allowedDefs.Count;
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x06003A0B RID: 14859 RVA: 0x00172089 File Offset: 0x00170289
		// (set) Token: 0x06003A0C RID: 14860 RVA: 0x00172091 File Offset: 0x00170291
		public FloatRange AllowedHitPointsPercents
		{
			get
			{
				return this.allowedHitPointsPercents;
			}
			set
			{
				if (this.allowedHitPointsPercents == value)
				{
					return;
				}
				this.allowedHitPointsPercents = value;
				if (this.settingsChangedCallback != null)
				{
					this.settingsChangedCallback();
				}
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06003A0D RID: 14861 RVA: 0x001720BC File Offset: 0x001702BC
		// (set) Token: 0x06003A0E RID: 14862 RVA: 0x001720C4 File Offset: 0x001702C4
		public FloatRange AllowedMentalBreakChance
		{
			get
			{
				return this.allowedMentalBreakChance;
			}
			set
			{
				if (this.allowedMentalBreakChance == value)
				{
					return;
				}
				this.allowedMentalBreakChance = value;
				if (this.settingsChangedCallback != null)
				{
					this.settingsChangedCallback();
				}
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06003A0F RID: 14863 RVA: 0x001720EF File Offset: 0x001702EF
		// (set) Token: 0x06003A10 RID: 14864 RVA: 0x001720F7 File Offset: 0x001702F7
		public QualityRange AllowedQualityLevels
		{
			get
			{
				return this.allowedQualities;
			}
			set
			{
				if (this.allowedQualities == value)
				{
					return;
				}
				this.allowedQualities = value;
				if (this.settingsChangedCallback != null)
				{
					this.settingsChangedCallback();
				}
			}
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06003A11 RID: 14865 RVA: 0x00172122 File Offset: 0x00170322
		// (set) Token: 0x06003A12 RID: 14866 RVA: 0x00172146 File Offset: 0x00170346
		public TreeNode_ThingCategory DisplayRootCategory
		{
			get
			{
				if (this.displayRootCategoryInt == null)
				{
					this.RecalculateDisplayRootCategory();
				}
				if (this.displayRootCategoryInt == null)
				{
					return ThingCategoryNodeDatabase.RootNode;
				}
				return this.displayRootCategoryInt;
			}
			set
			{
				if (value == this.displayRootCategoryInt)
				{
					return;
				}
				this.displayRootCategoryInt = value;
				this.RecalculateSpecialFilterConfigurability();
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06003A13 RID: 14867 RVA: 0x0017215F File Offset: 0x0017035F
		public bool CaresAboutHitPoints
		{
			get
			{
				return this.allowedHitPointsConfigurable && this.allowedHitPointsPercents != FloatRange.ZeroToOne;
			}
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x0017217C File Offset: 0x0017037C
		public ThingFilter()
		{
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x001721EC File Offset: 0x001703EC
		public ThingFilter(ThingCategoryDef overrideRootDef, bool onlySpecialFilters = false)
		{
			this.overrideRootDef = overrideRootDef;
			this.onlySpecialFilters = onlySpecialFilters;
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x00172268 File Offset: 0x00170468
		public ThingFilter(Action settingsChangedCallback)
		{
			this.settingsChangedCallback = settingsChangedCallback;
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x001722E0 File Offset: 0x001704E0
		public virtual void ExposeData()
		{
			Scribe_Collections.Look<SpecialThingFilterDef>(ref this.disallowedSpecialFilters, "disallowedSpecialFilters", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<ThingDef>(ref this.allowedDefs, "allowedDefs", LookMode.Undefined);
			Scribe_Values.Look<FloatRange>(ref this.allowedHitPointsPercents, "allowedHitPointsPercents", default(FloatRange), false);
			Scribe_Values.Look<FloatRange>(ref this.allowedMentalBreakChance, "allowedMentalBreakChance", default(FloatRange), false);
			Scribe_Values.Look<QualityRange>(ref this.allowedQualities, "allowedQualityLevels", default(QualityRange), false);
			Scribe_Values.Look<bool>(ref this.onlySpecialFilters, "onlySpecialFilters", false, false);
			Scribe_Defs.Look<ThingCategoryDef>(ref this.overrideRootDef, "overrideRootDef");
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x00172384 File Offset: 0x00170584
		public void ResolveReferences()
		{
			for (int i = 0; i < DefDatabase<SpecialThingFilterDef>.AllDefsListForReading.Count; i++)
			{
				SpecialThingFilterDef specialThingFilterDef = DefDatabase<SpecialThingFilterDef>.AllDefsListForReading[i];
				if (!specialThingFilterDef.allowedByDefault)
				{
					this.SetAllow(specialThingFilterDef, false);
				}
			}
			if (this.thingDefs != null)
			{
				for (int j = 0; j < this.thingDefs.Count; j++)
				{
					if (this.thingDefs[j] != null)
					{
						this.SetAllow(this.thingDefs[j], true);
					}
					else
					{
						string str = "ThingFilter could not find thing def named ";
						ThingDef thingDef = this.thingDefs[j];
						Log.Error(str + ((thingDef != null) ? thingDef.ToString() : null));
					}
				}
			}
			if (this.categories != null)
			{
				for (int k = 0; k < this.categories.Count; k++)
				{
					ThingCategoryDef named = DefDatabase<ThingCategoryDef>.GetNamed(this.categories[k], false);
					if (named != null)
					{
						this.SetAllow(named, true, null, null);
					}
				}
			}
			if (this.tradeTagsToAllow != null)
			{
				for (int l = 0; l < this.tradeTagsToAllow.Count; l++)
				{
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int m = 0; m < allDefsListForReading.Count; m++)
					{
						ThingDef thingDef2 = allDefsListForReading[m];
						if (thingDef2.tradeTags != null && thingDef2.tradeTags.Contains(this.tradeTagsToAllow[l]))
						{
							this.SetAllow(thingDef2, true);
						}
					}
				}
			}
			if (this.tradeTagsToDisallow != null)
			{
				for (int n = 0; n < this.tradeTagsToDisallow.Count; n++)
				{
					List<ThingDef> allDefsListForReading2 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num = 0; num < allDefsListForReading2.Count; num++)
					{
						ThingDef thingDef3 = allDefsListForReading2[num];
						if (thingDef3.tradeTags != null && thingDef3.tradeTags.Contains(this.tradeTagsToDisallow[n]))
						{
							this.SetAllow(thingDef3, false);
						}
					}
				}
			}
			if (this.thingSetMakerTagsToAllow != null)
			{
				for (int num2 = 0; num2 < this.thingSetMakerTagsToAllow.Count; num2++)
				{
					List<ThingDef> allDefsListForReading3 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num3 = 0; num3 < allDefsListForReading3.Count; num3++)
					{
						ThingDef thingDef4 = allDefsListForReading3[num3];
						if (thingDef4.thingSetMakerTags != null && thingDef4.thingSetMakerTags.Contains(this.thingSetMakerTagsToAllow[num2]))
						{
							this.SetAllow(thingDef4, true);
						}
					}
				}
			}
			if (this.thingSetMakerTagsToDisallow != null)
			{
				for (int num4 = 0; num4 < this.thingSetMakerTagsToDisallow.Count; num4++)
				{
					List<ThingDef> allDefsListForReading4 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num5 = 0; num5 < allDefsListForReading4.Count; num5++)
					{
						ThingDef thingDef5 = allDefsListForReading4[num5];
						if (thingDef5.thingSetMakerTags != null && thingDef5.thingSetMakerTags.Contains(this.thingSetMakerTagsToDisallow[num4]))
						{
							this.SetAllow(thingDef5, false);
						}
					}
				}
			}
			if (this.disallowedCategories != null)
			{
				for (int num6 = 0; num6 < this.disallowedCategories.Count; num6++)
				{
					ThingCategoryDef named2 = DefDatabase<ThingCategoryDef>.GetNamed(this.disallowedCategories[num6], false);
					if (named2 != null)
					{
						this.SetAllow(named2, false, null, null);
					}
				}
			}
			if (this.specialFiltersToAllow != null)
			{
				for (int num7 = 0; num7 < this.specialFiltersToAllow.Count; num7++)
				{
					this.SetAllow(SpecialThingFilterDef.Named(this.specialFiltersToAllow[num7]), true);
				}
			}
			if (this.specialFiltersToDisallow != null)
			{
				for (int num8 = 0; num8 < this.specialFiltersToDisallow.Count; num8++)
				{
					this.SetAllow(SpecialThingFilterDef.Named(this.specialFiltersToDisallow[num8]), false);
				}
			}
			if (this.stuffCategoriesToAllow != null)
			{
				for (int num9 = 0; num9 < this.stuffCategoriesToAllow.Count; num9++)
				{
					this.SetAllow(this.stuffCategoriesToAllow[num9], true);
				}
			}
			if (this.allowAllWhoCanMake != null)
			{
				for (int num10 = 0; num10 < this.allowAllWhoCanMake.Count; num10++)
				{
					this.SetAllowAllWhoCanMake(this.allowAllWhoCanMake[num10]);
				}
			}
			if (this.disallowWorsePreferability != FoodPreferability.Undefined)
			{
				List<ThingDef> allDefsListForReading5 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num11 = 0; num11 < allDefsListForReading5.Count; num11++)
				{
					ThingDef thingDef6 = allDefsListForReading5[num11];
					if (thingDef6.IsIngestible && thingDef6.ingestible.preferability != FoodPreferability.Undefined && thingDef6.ingestible.preferability < this.disallowWorsePreferability)
					{
						this.SetAllow(thingDef6, false);
					}
				}
			}
			if (this.disallowInedibleByHuman)
			{
				List<ThingDef> allDefsListForReading6 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num12 = 0; num12 < allDefsListForReading6.Count; num12++)
				{
					ThingDef thingDef7 = allDefsListForReading6[num12];
					if (thingDef7.IsIngestible && !ThingDefOf.Human.race.CanEverEat(thingDef7))
					{
						this.SetAllow(thingDef7, false);
					}
				}
			}
			if (this.disallowMedicalDrugs)
			{
				List<ThingDef> allDefsListForReading7 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num13 = 0; num13 < allDefsListForReading7.Count; num13++)
				{
					ThingDef thingDef8 = allDefsListForReading7[num13];
					if (!thingDef8.IsIngestible || (thingDef8.IsDrug && thingDef8.ingestible.drugCategory == DrugCategory.Medical))
					{
						this.SetAllow(thingDef8, false);
					}
				}
			}
			if (this.disallowDoesntProduceMeat)
			{
				List<ThingDef> allDefsListForReading8 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num14 = 0; num14 < allDefsListForReading8.Count; num14++)
				{
					ThingDef thingDef9 = allDefsListForReading8[num14];
					if (thingDef9.race != null && !thingDef9.race.hasMeat)
					{
						this.SetAllow(thingDef9, false);
					}
				}
			}
			if (this.disallowNotEverStorable)
			{
				foreach (ThingDef thingDef10 in DefDatabase<ThingDef>.AllDefsListForReading)
				{
					if (!thingDef10.EverStorable(true))
					{
						this.SetAllow(thingDef10, false);
					}
				}
			}
			if (this.allowWithComp != null)
			{
				List<ThingDef> allDefsListForReading9 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num15 = 0; num15 < allDefsListForReading9.Count; num15++)
				{
					ThingDef thingDef11 = allDefsListForReading9[num15];
					if (thingDef11.HasComp(this.allowWithComp))
					{
						this.SetAllow(thingDef11, true);
					}
				}
			}
			if (this.disallowWithComp != null)
			{
				List<ThingDef> allDefsListForReading10 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num16 = 0; num16 < allDefsListForReading10.Count; num16++)
				{
					ThingDef thingDef12 = allDefsListForReading10[num16];
					if (thingDef12.HasComp(this.disallowWithComp))
					{
						this.SetAllow(thingDef12, false);
					}
				}
			}
			if (this.disallowCheaperThan != -3.4028235E+38f)
			{
				List<ThingDef> list = new List<ThingDef>();
				foreach (ThingDef thingDef13 in this.allowedDefs)
				{
					if (thingDef13.BaseMarketValue < this.disallowCheaperThan)
					{
						list.Add(thingDef13);
					}
				}
				for (int num17 = 0; num17 < list.Count; num17++)
				{
					this.SetAllow(list[num17], false);
				}
			}
			if (this.disallowedThingDefs != null)
			{
				for (int num18 = 0; num18 < this.disallowedThingDefs.Count; num18++)
				{
					if (this.disallowedThingDefs[num18] != null)
					{
						this.SetAllow(this.disallowedThingDefs[num18], false);
					}
					else
					{
						string str2 = "ThingFilter could not find excepted thing def named ";
						ThingDef thingDef14 = this.disallowedThingDefs[num18];
						Log.Error(str2 + ((thingDef14 != null) ? thingDef14.ToString() : null));
					}
				}
			}
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x00172AC8 File Offset: 0x00170CC8
		public void RecalculateDisplayRootCategory()
		{
			if (ThingCategoryNodeDatabase.allThingCategoryNodes == null)
			{
				this.DisplayRootCategory = ThingCategoryNodeDatabase.RootNode;
				return;
			}
			int lastFoundCategory = -1;
			object lockObject = new object();
			GenThreading.ParallelFor(0, ThingCategoryNodeDatabase.allThingCategoryNodes.Count, delegate(int index)
			{
				TreeNode_ThingCategory treeNode_ThingCategory = ThingCategoryNodeDatabase.allThingCategoryNodes[index];
				bool flag = false;
				bool flag2 = false;
				foreach (ThingDef thingDef in this.allowedDefs)
				{
					if (treeNode_ThingCategory.catDef.ContainedInThisOrDescendant(thingDef))
					{
						flag2 = true;
					}
					else
					{
						flag = true;
					}
				}
				if (!flag && flag2)
				{
					object lockObject = lockObject;
					lock (lockObject)
					{
						if (index > lastFoundCategory)
						{
							lastFoundCategory = index;
						}
					}
				}
			}, -1);
			if (lastFoundCategory == -1)
			{
				this.DisplayRootCategory = ThingCategoryNodeDatabase.RootNode;
				return;
			}
			this.DisplayRootCategory = ThingCategoryNodeDatabase.allThingCategoryNodes[lastFoundCategory];
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x00172B50 File Offset: 0x00170D50
		private void RecalculateSpecialFilterConfigurability()
		{
			if (this.DisplayRootCategory == null)
			{
				this.allowedHitPointsConfigurable = true;
				this.allowedQualitiesConfigurable = true;
				return;
			}
			this.allowedHitPointsConfigurable = false;
			this.allowedQualitiesConfigurable = false;
			foreach (ThingDef thingDef in this.DisplayRootCategory.catDef.DescendantThingDefs)
			{
				if (thingDef.useHitPoints)
				{
					this.allowedHitPointsConfigurable = true;
				}
				if (thingDef.HasComp(typeof(CompQuality)))
				{
					this.allowedQualitiesConfigurable = true;
				}
				if (this.allowedHitPointsConfigurable && this.allowedQualitiesConfigurable)
				{
					break;
				}
			}
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x00172C00 File Offset: 0x00170E00
		public bool IsAlwaysDisallowedDueToSpecialFilters(ThingDef def)
		{
			for (int i = 0; i < this.disallowedSpecialFilters.Count; i++)
			{
				if (this.disallowedSpecialFilters[i].Worker.AlwaysMatches(def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x00172C40 File Offset: 0x00170E40
		public virtual void CopyAllowancesFrom(ThingFilter other)
		{
			this.allowedDefs.Clear();
			this.allowedDefs.AddRange(other.allowedDefs);
			this.disallowedSpecialFilters = other.disallowedSpecialFilters.ListFullCopyOrNull<SpecialThingFilterDef>();
			this.allowedHitPointsPercents = other.allowedHitPointsPercents;
			this.allowedMentalBreakChance = other.allowedMentalBreakChance;
			this.allowedHitPointsConfigurable = other.allowedHitPointsConfigurable;
			this.allowedQualities = other.allowedQualities;
			this.allowedQualitiesConfigurable = other.allowedQualitiesConfigurable;
			this.thingDefs = other.thingDefs.ListFullCopyOrNull<ThingDef>();
			this.categories = other.categories.ListFullCopyOrNull<string>();
			this.tradeTagsToAllow = other.tradeTagsToAllow.ListFullCopyOrNull<string>();
			this.tradeTagsToDisallow = other.tradeTagsToDisallow.ListFullCopyOrNull<string>();
			this.thingSetMakerTagsToAllow = other.thingSetMakerTagsToAllow.ListFullCopyOrNull<string>();
			this.thingSetMakerTagsToDisallow = other.thingSetMakerTagsToDisallow.ListFullCopyOrNull<string>();
			this.disallowedCategories = other.disallowedCategories.ListFullCopyOrNull<string>();
			this.specialFiltersToAllow = other.specialFiltersToAllow.ListFullCopyOrNull<string>();
			this.specialFiltersToDisallow = other.specialFiltersToDisallow.ListFullCopyOrNull<string>();
			this.stuffCategoriesToAllow = other.stuffCategoriesToAllow.ListFullCopyOrNull<StuffCategoryDef>();
			this.allowAllWhoCanMake = other.allowAllWhoCanMake.ListFullCopyOrNull<ThingDef>();
			this.disallowWorsePreferability = other.disallowWorsePreferability;
			this.disallowInedibleByHuman = other.disallowInedibleByHuman;
			this.disallowMedicalDrugs = other.disallowMedicalDrugs;
			this.disallowNotEverStorable = other.disallowNotEverStorable;
			this.allowWithComp = other.allowWithComp;
			this.disallowWithComp = other.disallowWithComp;
			this.disallowCheaperThan = other.disallowCheaperThan;
			this.disallowedThingDefs = other.disallowedThingDefs.ListFullCopyOrNull<ThingDef>();
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x00172DEC File Offset: 0x00170FEC
		public void SetAllow(ThingDef thingDef, bool allow)
		{
			if (allow == this.Allows(thingDef))
			{
				return;
			}
			if (allow)
			{
				this.allowedDefs.Add(thingDef);
			}
			else
			{
				this.allowedDefs.Remove(thingDef);
			}
			foreach (ThingDef thingDef2 in thingDef.virtualDefs)
			{
				this.SetAllow(thingDef2, allow);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x00172E84 File Offset: 0x00171084
		public void SetAllow(SpecialThingFilterDef sfDef, bool allow)
		{
			if (allow == this.Allows(sfDef))
			{
				return;
			}
			if (allow)
			{
				if (this.disallowedSpecialFilters.Contains(sfDef))
				{
					this.disallowedSpecialFilters.Remove(sfDef);
				}
			}
			else if (!this.disallowedSpecialFilters.Contains(sfDef))
			{
				this.disallowedSpecialFilters.Add(sfDef);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x00172EF0 File Offset: 0x001710F0
		public void SetAllow(ThingCategoryDef categoryDef, bool allow, IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			if (!ThingCategoryNodeDatabase.initialized)
			{
				Log.Error("SetAllow categories won't work before ThingCategoryDatabase is initialized.");
			}
			foreach (ThingDef thingDef in categoryDef.DescendantThingDefs)
			{
				if (exceptedDefs == null || !exceptedDefs.Contains(thingDef))
				{
					this.SetAllow(thingDef, allow);
				}
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in categoryDef.DescendantSpecialThingFilterDefs)
			{
				if (specialThingFilterDef.configurable && (exceptedFilters == null || !exceptedFilters.Contains(specialThingFilterDef)))
				{
					this.SetAllow(specialThingFilterDef, allow);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06003A20 RID: 14880 RVA: 0x00172FC8 File Offset: 0x001711C8
		public void SetAllow(StuffCategoryDef cat, bool allow)
		{
			for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
			{
				ThingDef thingDef = DefDatabase<ThingDef>.AllDefsListForReading[i];
				if (thingDef.IsStuff && thingDef.stuffProps.categories.Contains(cat))
				{
					this.SetAllow(thingDef, allow);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06003A21 RID: 14881 RVA: 0x00173034 File Offset: 0x00171234
		public void SetAllowAllWhoCanMake(ThingDef thing)
		{
			for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
			{
				ThingDef thingDef = DefDatabase<ThingDef>.AllDefsListForReading[i];
				if (thingDef.IsStuff && thingDef.stuffProps.CanMake(thing))
				{
					this.SetAllow(thingDef, true);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x0017309C File Offset: 0x0017129C
		public void SetFromPreset(StorageSettingsPreset preset)
		{
			if (preset == StorageSettingsPreset.DefaultStockpile)
			{
				this.SetAllow(ThingCategoryDefOf.Foods, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Manufactured, true, null, null);
				this.SetAllow(ThingCategoryDefOf.ResourcesRaw, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Items, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Buildings, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Weapons, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
				this.SetAllow(ThingCategoryDefOf.BodyParts, true, null, null);
				if (ModsConfig.BiotechActive)
				{
					this.SetAllow(ThingDefOf.Wastepack, false);
				}
			}
			if (preset == StorageSettingsPreset.DumpingStockpile)
			{
				this.SetAllow(ThingCategoryDefOf.Corpses, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Chunks, true, null, null);
				if (ModsConfig.BiotechActive)
				{
					this.SetAllow(ThingDefOf.Wastepack, true);
				}
			}
			if (preset == StorageSettingsPreset.CorpseStockpile)
			{
				this.SetAllow(ThingCategoryDefOf.Corpses, true, null, null);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x00173194 File Offset: 0x00171394
		public void SetDisallowAll(IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			this.allowedDefs.RemoveWhere((ThingDef d) => exceptedDefs == null || !exceptedDefs.Contains(d));
			this.disallowedSpecialFilters.RemoveAll((SpecialThingFilterDef sf) => sf.configurable && (exceptedFilters == null || !exceptedFilters.Contains(sf)));
			if (this.onlySpecialFilters)
			{
				this.DisableSpecialFilters(this.RootNode);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x00173214 File Offset: 0x00171414
		private void DisableSpecialFilters(TreeNode_ThingCategory category)
		{
			foreach (TreeNode_ThingCategory treeNode_ThingCategory in category.ChildCategoryNodesAndThis)
			{
				foreach (SpecialThingFilterDef item in treeNode_ThingCategory.catDef.childSpecialFilters)
				{
					this.disallowedSpecialFilters.Add(item);
				}
			}
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x001732A4 File Offset: 0x001714A4
		public void SetAllowAll(ThingFilter parentFilter, bool includeNonStorable = false)
		{
			this.allowedDefs.Clear();
			if (parentFilter != null)
			{
				using (HashSet<ThingDef>.Enumerator enumerator = parentFilter.allowedDefs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef item = enumerator.Current;
						this.allowedDefs.Add(item);
					}
					goto IL_B9;
				}
			}
			if (includeNonStorable)
			{
				using (IEnumerator<ThingDef> enumerator2 = DefDatabase<ThingDef>.AllDefs.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ThingDef item2 = enumerator2.Current;
						this.allowedDefs.Add(item2);
					}
					goto IL_B9;
				}
			}
			foreach (ThingDef item3 in ThingFilter.AllStorableThingDefs)
			{
				this.allowedDefs.Add(item3);
			}
			IL_B9:
			this.disallowedSpecialFilters.RemoveAll((SpecialThingFilterDef sf) => sf.configurable);
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x001733D8 File Offset: 0x001715D8
		public virtual bool Allows(Thing t)
		{
			t = t.GetInnerIfMinified();
			if (!this.Allows(t.def) && !this.onlySpecialFilters)
			{
				return false;
			}
			if (t.def.useHitPoints && this.CaresAboutHitPoints)
			{
				float num = (float)t.HitPoints / (float)t.MaxHitPoints;
				num = GenMath.RoundedHundredth(num);
				if (!this.allowedHitPointsPercents.IncludesEpsilon(Mathf.Clamp01(num)))
				{
					return false;
				}
			}
			if (ModsConfig.AnomalyActive)
			{
				Book book = t as Book;
				if (book != null && !this.allowedMentalBreakChance.IncludesEpsilon(Mathf.Clamp01(book.MentalBreakChancePerHour)))
				{
					return false;
				}
			}
			QualityCategory p;
			if (this.allowedQualities != QualityRange.All && t.def.FollowQualityThingFilter() && t.TryGetQuality(out p) && !this.allowedQualities.Includes(p))
			{
				return false;
			}
			for (int i = 0; i < this.disallowedSpecialFilters.Count; i++)
			{
				if (this.disallowedSpecialFilters[i].Worker.Matches(t) && (t.def.IsWithinCategory(this.disallowedSpecialFilters[i].parentCategory) || this.onlySpecialFilters))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x00173502 File Offset: 0x00171702
		public bool Allows(ThingDef def)
		{
			return this.allowedDefs.Contains(def);
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x00173510 File Offset: 0x00171710
		public bool Allows(SpecialThingFilterDef sf)
		{
			return !this.disallowedSpecialFilters.Contains(sf);
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x00173521 File Offset: 0x00171721
		public ThingRequest GetThingRequest()
		{
			if (this.AllowedThingDefs.Any((ThingDef def) => !def.alwaysHaulable))
			{
				return ThingRequest.ForGroup(ThingRequestGroup.HaulableEver);
			}
			return ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways);
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x0017355C File Offset: 0x0017175C
		public override string ToString()
		{
			return this.Summary;
		}

		// Token: 0x06003A2B RID: 14891 RVA: 0x00173564 File Offset: 0x00171764
		public static ThingFilter CreateOnlyEverStorableThingFilter()
		{
			ThingFilter thingFilter = new ThingFilter();
			thingFilter.categories = new List<string>();
			thingFilter.categories.Add(ThingCategoryDefOf.Root.defName);
			thingFilter.disallowNotEverStorable = true;
			thingFilter.ResolveReferences();
			thingFilter.hiddenSpecialFilters = new List<SpecialThingFilterDef>
			{
				SpecialThingFilterDefOf.AllowLargeCorpses
			};
			return thingFilter;
		}

		// Token: 0x04002650 RID: 9808
		[Unsaved(false)]
		private Action settingsChangedCallback;

		// Token: 0x04002651 RID: 9809
		[Unsaved(false)]
		private TreeNode_ThingCategory displayRootCategoryInt;

		// Token: 0x04002652 RID: 9810
		[Unsaved(false)]
		private HashSet<ThingDef> allowedDefs = new HashSet<ThingDef>();

		// Token: 0x04002653 RID: 9811
		[Unsaved(false)]
		private List<SpecialThingFilterDef> disallowedSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x04002654 RID: 9812
		[Unsaved(false)]
		public List<SpecialThingFilterDef> hiddenSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x04002655 RID: 9813
		private ThingCategoryDef overrideRootDef;

		// Token: 0x04002656 RID: 9814
		private bool onlySpecialFilters;

		// Token: 0x04002657 RID: 9815
		private FloatRange allowedHitPointsPercents = FloatRange.ZeroToOne;

		// Token: 0x04002658 RID: 9816
		private FloatRange allowedMentalBreakChance = FloatRange.ZeroToOne;

		// Token: 0x04002659 RID: 9817
		public bool allowedHitPointsConfigurable = true;

		// Token: 0x0400265A RID: 9818
		private QualityRange allowedQualities = QualityRange.All;

		// Token: 0x0400265B RID: 9819
		public bool allowedQualitiesConfigurable = true;

		// Token: 0x0400265C RID: 9820
		[MustTranslate]
		public string customSummary;

		// Token: 0x0400265D RID: 9821
		private List<ThingDef> thingDefs;

		// Token: 0x0400265E RID: 9822
		[NoTranslate]
		private List<string> categories;

		// Token: 0x0400265F RID: 9823
		[NoTranslate]
		private List<string> tradeTagsToAllow;

		// Token: 0x04002660 RID: 9824
		[NoTranslate]
		private List<string> tradeTagsToDisallow;

		// Token: 0x04002661 RID: 9825
		[NoTranslate]
		private List<string> thingSetMakerTagsToAllow;

		// Token: 0x04002662 RID: 9826
		[NoTranslate]
		private List<string> thingSetMakerTagsToDisallow;

		// Token: 0x04002663 RID: 9827
		[NoTranslate]
		private List<string> disallowedCategories;

		// Token: 0x04002664 RID: 9828
		[NoTranslate]
		private List<string> specialFiltersToAllow;

		// Token: 0x04002665 RID: 9829
		[NoTranslate]
		private List<string> specialFiltersToDisallow;

		// Token: 0x04002666 RID: 9830
		private List<StuffCategoryDef> stuffCategoriesToAllow;

		// Token: 0x04002667 RID: 9831
		private List<ThingDef> allowAllWhoCanMake;

		// Token: 0x04002668 RID: 9832
		private FoodPreferability disallowWorsePreferability;

		// Token: 0x04002669 RID: 9833
		private bool disallowInedibleByHuman;

		// Token: 0x0400266A RID: 9834
		private bool disallowDoesntProduceMeat;

		// Token: 0x0400266B RID: 9835
		private bool disallowMedicalDrugs;

		// Token: 0x0400266C RID: 9836
		private bool disallowNotEverStorable;

		// Token: 0x0400266D RID: 9837
		private Type allowWithComp;

		// Token: 0x0400266E RID: 9838
		private Type disallowWithComp;

		// Token: 0x0400266F RID: 9839
		private float disallowCheaperThan = float.MinValue;

		// Token: 0x04002670 RID: 9840
		private List<ThingDef> disallowedThingDefs;
	}
}
