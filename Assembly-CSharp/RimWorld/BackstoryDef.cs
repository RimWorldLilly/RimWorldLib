using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F33 RID: 3891
	public class BackstoryDef : Def
	{
		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x06006482 RID: 25730 RVA: 0x0023E63B File Offset: 0x0023C83B
		public bool IsPlayerColonyChildBackstory
		{
			get
			{
				List<string> list = this.spawnCategories;
				return list != null && list.Contains("Child");
			}
		}

		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x06006483 RID: 25731 RVA: 0x0023E654 File Offset: 0x0023C854
		public List<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				if (this.cachedDisabledWorkTypes == null)
				{
					this.cachedDisabledWorkTypes = new List<WorkTypeDef>();
					List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (!this.AllowsWorkType(allDefsListForReading[i]))
						{
							this.cachedDisabledWorkTypes.Add(allDefsListForReading[i]);
						}
					}
				}
				return this.cachedDisabledWorkTypes;
			}
		}

		// Token: 0x17001125 RID: 4389
		// (get) Token: 0x06006484 RID: 25732 RVA: 0x0023E6B2 File Offset: 0x0023C8B2
		public IEnumerable<WorkGiverDef> DisabledWorkGivers
		{
			get
			{
				BackstoryDef.<get_DisabledWorkGivers>d__32 <get_DisabledWorkGivers>d__ = new BackstoryDef.<get_DisabledWorkGivers>d__32(-2);
				<get_DisabledWorkGivers>d__.<>4__this = this;
				return <get_DisabledWorkGivers>d__;
			}
		}

		// Token: 0x06006485 RID: 25733 RVA: 0x0023E6C4 File Offset: 0x0023C8C4
		public override void PostLoad()
		{
			base.PostLoad();
			this.untranslatedTitle = this.title;
			this.untranslatedTitleFemale = this.titleFemale;
			this.untranslatedTitleShort = this.titleShort;
			this.untranslatedTitleShortFemale = this.titleShortFemale;
			this.untranslatedDesc = this.description;
			if (this.description.NullOrEmpty() && !this.baseDesc.NullOrEmpty())
			{
				this.description = this.baseDesc;
			}
			this.description = this.description.TrimEnd();
			this.description = this.description.Replace("\r", "");
		}

		// Token: 0x06006486 RID: 25734 RVA: 0x0023E765 File Offset: 0x0023C965
		public string TitleFor(Gender g)
		{
			if (g != Gender.Female || this.titleFemale.NullOrEmpty())
			{
				return this.title;
			}
			return this.titleFemale;
		}

		// Token: 0x06006487 RID: 25735 RVA: 0x0023E785 File Offset: 0x0023C985
		public string TitleCapFor(Gender g)
		{
			return this.TitleFor(g).CapitalizeFirst();
		}

		// Token: 0x06006488 RID: 25736 RVA: 0x0023E793 File Offset: 0x0023C993
		public string TitleShortFor(Gender g)
		{
			if (g == Gender.Female && !this.titleShortFemale.NullOrEmpty())
			{
				return this.titleShortFemale;
			}
			if (!this.titleShort.NullOrEmpty())
			{
				return this.titleShort;
			}
			return this.TitleFor(g);
		}

		// Token: 0x06006489 RID: 25737 RVA: 0x0023E7C8 File Offset: 0x0023C9C8
		public BodyTypeDef BodyTypeFor(Gender g)
		{
			if (this.bodyTypeGlobal != null || g == Gender.None)
			{
				return this.bodyTypeGlobal;
			}
			if (g == Gender.Female)
			{
				return this.bodyTypeFemale;
			}
			return this.bodyTypeMale;
		}

		// Token: 0x0600648A RID: 25738 RVA: 0x0023E7F0 File Offset: 0x0023C9F0
		public TaggedString FullDescriptionFor(Pawn p)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.description.Formatted(p.Named("PAWN")).AdjustedFor(p, "PAWN", true).Resolve());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				SkillDef skillDef = allDefsListForReading[i];
				foreach (SkillGain skillGain in this.skillGains)
				{
					if (skillGain.skill == skillDef)
					{
						stringBuilder.AppendLine(skillDef.skillLabel.CapitalizeFirst() + ":   " + skillGain.amount.ToString("+##;-##"));
						break;
					}
				}
			}
			if (this.DisabledWorkTypes.Any<WorkTypeDef>() || this.DisabledWorkGivers.Any<WorkGiverDef>())
			{
				stringBuilder.AppendLine();
			}
			foreach (WorkTypeDef workTypeDef in this.DisabledWorkTypes)
			{
				stringBuilder.AppendLine(workTypeDef.gerundLabel.CapitalizeFirst() + " " + "DisabledLower".Translate());
			}
			foreach (WorkGiverDef workGiverDef in this.DisabledWorkGivers)
			{
				stringBuilder.AppendLine(workGiverDef.workType.gerundLabel.CapitalizeFirst() + ": " + workGiverDef.LabelCap + " " + "DisabledLower".Translate());
			}
			if (ModsConfig.RoyaltyActive)
			{
				this.unlockedMeditationTypesTemp.Clear();
				foreach (MeditationFocusDef meditationFocusDef in DefDatabase<MeditationFocusDef>.AllDefs)
				{
					for (int j = 0; j < meditationFocusDef.requiredBackstoriesAny.Count; j++)
					{
						BackstoryCategoryAndSlot backstoryCategoryAndSlot = meditationFocusDef.requiredBackstoriesAny[j];
						if (this.spawnCategories.Contains(backstoryCategoryAndSlot.categoryName) && backstoryCategoryAndSlot.slot == this.slot)
						{
							this.unlockedMeditationTypesTemp.Add(meditationFocusDef.LabelCap);
							break;
						}
					}
				}
				if (this.unlockedMeditationTypesTemp.Count > 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("MeditationFocusesUnlocked".Translate() + ": ");
					stringBuilder.AppendLine(this.unlockedMeditationTypesTemp.ToLineList("  - "));
				}
			}
			if (!this.modContentPack.IsOfficialMod)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Source".Translate() + ": " + this.modContentPack.Name);
			}
			string str = stringBuilder.ToString().TrimEndNewlines();
			return Find.ActiveLanguageWorker.PostProcessed(str);
		}

		// Token: 0x0600648B RID: 25739 RVA: 0x0023EB64 File Offset: 0x0023CD64
		public void SetTitle(string newTitle, string newTitleFemale)
		{
			this.title = newTitle;
			this.titleFemale = newTitleFemale;
		}

		// Token: 0x0600648C RID: 25740 RVA: 0x0023EB74 File Offset: 0x0023CD74
		public void SetTitleShort(string newTitleShort, string newTitleShortFemale)
		{
			this.titleShort = newTitleShort;
			this.titleShortFemale = newTitleShortFemale;
		}

		// Token: 0x0600648D RID: 25741 RVA: 0x0023EB84 File Offset: 0x0023CD84
		public bool DisallowsTrait(TraitDef def, int degree)
		{
			if (this.disallowedTraits == null)
			{
				return false;
			}
			for (int i = 0; i < this.disallowedTraits.Count; i++)
			{
				if (this.disallowedTraits[i].def == def && this.disallowedTraits[i].degree == degree)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600648E RID: 25742 RVA: 0x0023EBDC File Offset: 0x0023CDDC
		private bool AllowsWorkType(WorkTypeDef workType)
		{
			return (this.workDisables & workType.workTags) == WorkTags.None;
		}

		// Token: 0x0600648F RID: 25743 RVA: 0x0023EBEE File Offset: 0x0023CDEE
		private bool AllowsWorkGiver(WorkGiverDef workGiver)
		{
			return (this.workDisables & workGiver.workTags) == WorkTags.None;
		}

		// Token: 0x06006490 RID: 25744 RVA: 0x0023EC00 File Offset: 0x0023CE00
		public override IEnumerable<string> ConfigErrors()
		{
			BackstoryDef.<ConfigErrors>d__45 <ConfigErrors>d__ = new BackstoryDef.<ConfigErrors>d__45(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040039BB RID: 14779
		public const string PlayerColonistCategoryChild = "Child";

		// Token: 0x040039BC RID: 14780
		[MayTranslate]
		public string baseDesc;

		// Token: 0x040039BD RID: 14781
		[NoTranslate]
		public string identifier;

		// Token: 0x040039BE RID: 14782
		public BackstorySlot slot;

		// Token: 0x040039BF RID: 14783
		[MustTranslate]
		public string title;

		// Token: 0x040039C0 RID: 14784
		[MustTranslate]
		public string titleFemale;

		// Token: 0x040039C1 RID: 14785
		[MustTranslate]
		public string titleShort;

		// Token: 0x040039C2 RID: 14786
		[MustTranslate]
		public string titleShortFemale;

		// Token: 0x040039C3 RID: 14787
		public List<SkillGain> skillGains = new List<SkillGain>();

		// Token: 0x040039C4 RID: 14788
		public WorkTags workDisables;

		// Token: 0x040039C5 RID: 14789
		public WorkTags requiredWorkTags;

		// Token: 0x040039C6 RID: 14790
		[NoTranslate]
		public List<string> spawnCategories = new List<string>();

		// Token: 0x040039C7 RID: 14791
		public BodyTypeDef bodyTypeGlobal;

		// Token: 0x040039C8 RID: 14792
		public BodyTypeDef bodyTypeFemale;

		// Token: 0x040039C9 RID: 14793
		public BodyTypeDef bodyTypeMale;

		// Token: 0x040039CA RID: 14794
		public List<BackstoryTrait> forcedTraits;

		// Token: 0x040039CB RID: 14795
		public List<BackstoryTrait> disallowedTraits;

		// Token: 0x040039CC RID: 14796
		public RulePackDef nameMaker;

		// Token: 0x040039CD RID: 14797
		public List<PossessionThingDefCountClass> possessions = new List<PossessionThingDefCountClass>();

		// Token: 0x040039CE RID: 14798
		public bool shuffleable = true;

		// Token: 0x040039CF RID: 14799
		public bool requiresSpawnCategory;

		// Token: 0x040039D0 RID: 14800
		[Unsaved(false)]
		public string untranslatedTitle;

		// Token: 0x040039D1 RID: 14801
		[Unsaved(false)]
		public string untranslatedTitleFemale;

		// Token: 0x040039D2 RID: 14802
		[Unsaved(false)]
		public string untranslatedTitleShort;

		// Token: 0x040039D3 RID: 14803
		[Unsaved(false)]
		public string untranslatedTitleShortFemale;

		// Token: 0x040039D4 RID: 14804
		[Unsaved(false)]
		public string untranslatedDesc;

		// Token: 0x040039D5 RID: 14805
		private List<WorkTypeDef> cachedDisabledWorkTypes;

		// Token: 0x040039D6 RID: 14806
		private List<string> unlockedMeditationTypesTemp = new List<string>();
	}
}
