using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FAC RID: 4012
	public class MemeDef : Def
	{
		// Token: 0x17001177 RID: 4471
		// (get) Token: 0x06006602 RID: 26114 RVA: 0x002447DB File Offset: 0x002429DB
		public Texture2D Icon
		{
			get
			{
				if (!ModLister.CheckIdeology("Memes"))
				{
					return BaseContent.BadTex;
				}
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x17001178 RID: 4472
		// (get) Token: 0x06006603 RID: 26115 RVA: 0x00244818 File Offset: 0x00242A18
		public List<BuildableDef> AllDesignatorBuildables
		{
			get
			{
				if (this.cachedAllDesignatorBuildables == null)
				{
					this.cachedAllDesignatorBuildables = new List<BuildableDef>();
					if (this.addDesignators != null)
					{
						foreach (BuildableDef item in this.addDesignators)
						{
							this.cachedAllDesignatorBuildables.Add(item);
						}
					}
					if (this.addDesignatorGroups != null)
					{
						foreach (DesignatorDropdownGroupDef designatorDropdownGroupDef in this.addDesignatorGroups)
						{
							this.cachedAllDesignatorBuildables.AddRange(designatorDropdownGroupDef.BuildablesWithoutDefaultDesignators());
						}
					}
				}
				return this.cachedAllDesignatorBuildables;
			}
		}

		// Token: 0x06006604 RID: 26116 RVA: 0x002448E8 File Offset: 0x00242AE8
		public List<string> UnlockedRoles(Ideo ideo)
		{
			if (this.unlockedRoles == null || ideo != this.unlockedRolesCachedFor)
			{
				this.unlockedRoles = new List<string>();
				foreach (PreceptDef preceptDef in DefDatabase<PreceptDef>.AllDefsListForReading)
				{
					if (typeof(Precept_Role).IsAssignableFrom(preceptDef.preceptClass) && !preceptDef.requiredMemes.NullOrEmpty<MemeDef>() && preceptDef.requiredMemes.Contains(this))
					{
						this.unlockedRoles.Add(preceptDef.LabelCap.Resolve());
					}
				}
				this.unlockedRolesCachedFor = ideo;
			}
			return this.unlockedRoles;
		}

		// Token: 0x06006605 RID: 26117 RVA: 0x002449AC File Offset: 0x00242BAC
		public List<string> UnlockedRituals()
		{
			if (this.unlockedRituals == null)
			{
				this.unlockedRituals = new List<string>();
				if (this.consumableBuildings != null)
				{
					using (List<RitualPatternDef>.Enumerator enumerator = DefDatabase<RitualPatternDef>.AllDefsListForReading.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							RitualPatternDef p = enumerator.Current;
							if (!p.ignoreConsumableBuildingRequirement && p.ritualObligationTargetFilter != null && !p.ritualObligationTargetFilter.thingDefs.NullOrEmpty<ThingDef>() && p.ritualObligationTargetFilter.thingDefs.Any((ThingDef td) => this.consumableBuildings.Contains(td)))
							{
								string text;
								if ((text = p.shortDescOverride.CapitalizeFirst()) == null)
								{
									PreceptDef preceptDef = DefDatabase<PreceptDef>.AllDefsListForReading.FirstOrDefault((PreceptDef pr) => pr.ritualPatternBase == p);
									TaggedString? taggedString = (preceptDef != null) ? new TaggedString?(preceptDef.LabelCap) : null;
									text = ((taggedString != null) ? taggedString.GetValueOrDefault() : null);
								}
								string text2 = text;
								if (text2 != null)
								{
									this.unlockedRituals.Add(text2);
								}
							}
						}
					}
				}
			}
			return this.unlockedRituals;
		}

		// Token: 0x06006606 RID: 26118 RVA: 0x00244AFC File Offset: 0x00242CFC
		public override IEnumerable<string> ConfigErrors()
		{
			MemeDef.<ConfigErrors>d__52 <ConfigErrors>d__ = new MemeDef.<ConfigErrors>d__52(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003D80 RID: 15744
		public string iconPath;

		// Token: 0x04003D81 RID: 15745
		public int renderOrder;

		// Token: 0x04003D82 RID: 15746
		public int impact = -999;

		// Token: 0x04003D83 RID: 15747
		public MemeCategory category;

		// Token: 0x04003D84 RID: 15748
		public MemeGroupDef groupDef;

		// Token: 0x04003D85 RID: 15749
		[NoTranslate]
		public List<string> exclusionTags = new List<string>();

		// Token: 0x04003D86 RID: 15750
		public List<List<PreceptDef>> requireOne;

		// Token: 0x04003D87 RID: 15751
		public PreceptsWithNoneChance selectOneOrNone;

		// Token: 0x04003D88 RID: 15752
		public List<RequiredRitualAndBuilding> requiredRituals;

		// Token: 0x04003D89 RID: 15753
		public IdeoWeaponClassPair preferredWeaponClasses;

		// Token: 0x04003D8A RID: 15754
		public float randomizationSelectionWeightFactor = 1f;

		// Token: 0x04003D8B RID: 15755
		public List<ThingDef> requireAnyRitualSeat;

		// Token: 0x04003D8C RID: 15756
		public IntRange deityCount = new IntRange(-1, -1);

		// Token: 0x04003D8D RID: 15757
		public bool allowDuringTutorial;

		// Token: 0x04003D8E RID: 15758
		public List<FactionDef> factionWhitelist;

		// Token: 0x04003D8F RID: 15759
		public bool hiddenInChooseMemes;

		// Token: 0x04003D90 RID: 15760
		public RulePack generalRules;

		// Token: 0x04003D91 RID: 15761
		public IdeoDescriptionMaker descriptionMaker;

		// Token: 0x04003D92 RID: 15762
		public RulePackDef deityNameMakerOverride;

		// Token: 0x04003D93 RID: 15763
		public RulePackDef deityTypeMakerOverride;

		// Token: 0x04003D94 RID: 15764
		public bool allowSymbolsFromDeity = true;

		// Token: 0x04003D95 RID: 15765
		public bool symbolPackOverride;

		// Token: 0x04003D96 RID: 15766
		[MustTranslate]
		public List<IdeoSymbolPack> symbolPacks;

		// Token: 0x04003D97 RID: 15767
		public List<DeityNameType> fixedDeityNameTypes;

		// Token: 0x04003D98 RID: 15768
		public List<ThingStyleCategoryWithPriority> thingStyleCategories;

		// Token: 0x04003D99 RID: 15769
		public IntRange ritualsToMake = IntRange.Zero;

		// Token: 0x04003D9A RID: 15770
		public List<string> replaceRitualsWithTags = new List<string>();

		// Token: 0x04003D9B RID: 15771
		public List<RitualPatternDef> replacementPatterns;

		// Token: 0x04003D9C RID: 15772
		public List<ThingDef> consumableBuildings;

		// Token: 0x04003D9D RID: 15773
		public int veneratedAnimalsCountOffset;

		// Token: 0x04003D9E RID: 15774
		public int veneratedAnimalsCountOverride = -1;

		// Token: 0x04003D9F RID: 15775
		public List<StyleItemTagWeighted> styleItemTags;

		// Token: 0x04003DA0 RID: 15776
		[MustTranslate]
		public string worshipRoomLabel;

		// Token: 0x04003DA1 RID: 15777
		public List<ResearchProjectDef> startingResearchProjects = new List<ResearchProjectDef>();

		// Token: 0x04003DA2 RID: 15778
		public List<BuildableDef> addDesignators;

		// Token: 0x04003DA3 RID: 15779
		public List<DesignatorDropdownGroupDef> addDesignatorGroups;

		// Token: 0x04003DA4 RID: 15780
		public List<PreceptApparelRequirement> apparelRequirements;

		// Token: 0x04003DA5 RID: 15781
		[XmlInheritanceAllowDuplicateNodes]
		public List<TraitRequirement> agreeableTraits;

		// Token: 0x04003DA6 RID: 15782
		[XmlInheritanceAllowDuplicateNodes]
		public List<TraitRequirement> disagreeableTraits;

		// Token: 0x04003DA7 RID: 15783
		public bool preventApparelRequirements;

		// Token: 0x04003DA8 RID: 15784
		public XenotypeSet xenotypeSet;

		// Token: 0x04003DA9 RID: 15785
		private Texture2D icon;

		// Token: 0x04003DAA RID: 15786
		private List<BuildableDef> cachedAllDesignatorBuildables;

		// Token: 0x04003DAB RID: 15787
		private List<string> unlockedRoles;

		// Token: 0x04003DAC RID: 15788
		private List<string> unlockedRituals;

		// Token: 0x04003DAD RID: 15789
		private Ideo unlockedRolesCachedFor;
	}
}
