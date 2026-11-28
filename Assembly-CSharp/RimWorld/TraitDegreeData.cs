using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001006 RID: 4102
	public class TraitDegreeData : IRenderNodePropertiesParent
	{
		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x06006775 RID: 26485 RVA: 0x00249851 File Offset: 0x00247A51
		public string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null)
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x06006776 RID: 26486 RVA: 0x00249872 File Offset: 0x00247A72
		public TraitMentalStateGiver MentalStateGiver
		{
			get
			{
				if (this.mentalStateGiverInt == null)
				{
					this.mentalStateGiverInt = (TraitMentalStateGiver)Activator.CreateInstance(this.mentalStateGiverClass);
					this.mentalStateGiverInt.traitDegreeData = this;
				}
				return this.mentalStateGiverInt;
			}
		}

		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x06006777 RID: 26487 RVA: 0x002498A4 File Offset: 0x00247AA4
		public bool HasDefinedGraphicProperties
		{
			get
			{
				return !this.renderNodeProperties.NullOrEmpty<PawnRenderNodeProperties>();
			}
		}

		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x06006778 RID: 26488 RVA: 0x002498B4 File Offset: 0x00247AB4
		public List<PawnRenderNodeProperties> RenderNodeProperties
		{
			get
			{
				return this.renderNodeProperties;
			}
		}

		// Token: 0x06006779 RID: 26489 RVA: 0x002498BC File Offset: 0x00247ABC
		public string GetLabelFor(Pawn pawn)
		{
			return this.GetLabelFor((pawn != null) ? pawn.gender : Gender.None);
		}

		// Token: 0x0600677A RID: 26490 RVA: 0x002498D0 File Offset: 0x00247AD0
		public string GetLabelCapFor(Pawn pawn)
		{
			return this.GetLabelCapFor((pawn != null) ? pawn.gender : Gender.None);
		}

		// Token: 0x0600677B RID: 26491 RVA: 0x002498E4 File Offset: 0x00247AE4
		public string GetLabelFor(Gender gender)
		{
			if (gender != Gender.Male)
			{
				if (gender != Gender.Female)
				{
					return this.label;
				}
				if (!this.labelFemale.NullOrEmpty())
				{
					return this.labelFemale;
				}
				return this.label;
			}
			else
			{
				if (!this.labelMale.NullOrEmpty())
				{
					return this.labelMale;
				}
				return this.label;
			}
		}

		// Token: 0x0600677C RID: 26492 RVA: 0x00249938 File Offset: 0x00247B38
		public string GetLabelCapFor(Gender gender)
		{
			if (gender != Gender.Male)
			{
				if (gender != Gender.Female)
				{
					return this.LabelCap;
				}
				if (this.labelFemale.NullOrEmpty())
				{
					return this.LabelCap;
				}
				if (this.cachedLabelFemaleCap == null)
				{
					this.cachedLabelFemaleCap = this.labelFemale.CapitalizeFirst();
				}
				return this.cachedLabelFemaleCap;
			}
			else
			{
				if (this.labelMale.NullOrEmpty())
				{
					return this.LabelCap;
				}
				if (this.cachedLabelMaleCap == null)
				{
					this.cachedLabelMaleCap = this.labelMale.CapitalizeFirst();
				}
				return this.cachedLabelMaleCap;
			}
		}

		// Token: 0x0600677D RID: 26493 RVA: 0x002499C0 File Offset: 0x00247BC0
		public List<IssueDef> GetAffectedIssues(TraitDef def)
		{
			if (this.affectedIssuesCached == null)
			{
				this.affectedIssuesCached = new List<IssueDef>();
				List<PreceptDef> allDefsListForReading = DefDatabase<PreceptDef>.AllDefsListForReading;
				Predicate<TraitRequirement> <>9__0;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (!this.affectedIssuesCached.Contains(allDefsListForReading[i].issue))
					{
						List<TraitRequirement> traitsAffecting = allDefsListForReading[i].TraitsAffecting;
						Predicate<TraitRequirement> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((TraitRequirement x) => x.def == def && x.degree.GetValueOrDefault() == this.degree));
						}
						if (traitsAffecting.Any(predicate))
						{
							this.affectedIssuesCached.Add(allDefsListForReading[i].issue);
						}
					}
				}
			}
			return this.affectedIssuesCached;
		}

		// Token: 0x0600677E RID: 26494 RVA: 0x00249A78 File Offset: 0x00247C78
		public List<MemeDef> GetAffectedMemes(TraitDef def, bool agreeable)
		{
			if (agreeable)
			{
				if (this.agreeableMemesCached == null)
				{
					this.agreeableMemesCached = new List<MemeDef>();
					List<MemeDef> allDefsListForReading = DefDatabase<MemeDef>.AllDefsListForReading;
					Predicate<TraitRequirement> <>9__0;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (!allDefsListForReading[i].agreeableTraits.NullOrEmpty<TraitRequirement>())
						{
							List<TraitRequirement> agreeableTraits = allDefsListForReading[i].agreeableTraits;
							Predicate<TraitRequirement> predicate;
							if ((predicate = <>9__0) == null)
							{
								predicate = (<>9__0 = ((TraitRequirement x) => x.def == def && x.degree.GetValueOrDefault() == this.degree));
							}
							if (agreeableTraits.Any(predicate))
							{
								this.agreeableMemesCached.Add(allDefsListForReading[i]);
							}
						}
					}
				}
				return this.agreeableMemesCached;
			}
			if (this.disagreeableMemesCached == null)
			{
				this.disagreeableMemesCached = new List<MemeDef>();
				List<MemeDef> allDefsListForReading2 = DefDatabase<MemeDef>.AllDefsListForReading;
				Predicate<TraitRequirement> <>9__1;
				for (int j = 0; j < allDefsListForReading2.Count; j++)
				{
					if (!allDefsListForReading2[j].disagreeableTraits.NullOrEmpty<TraitRequirement>())
					{
						List<TraitRequirement> disagreeableTraits = allDefsListForReading2[j].disagreeableTraits;
						Predicate<TraitRequirement> predicate2;
						if ((predicate2 = <>9__1) == null)
						{
							predicate2 = (<>9__1 = ((TraitRequirement x) => x.def == def && x.degree.GetValueOrDefault() == this.degree));
						}
						if (disagreeableTraits.Any(predicate2))
						{
							this.disagreeableMemesCached.Add(allDefsListForReading2[j]);
						}
					}
				}
			}
			return this.disagreeableMemesCached;
		}

		// Token: 0x0600677F RID: 26495 RVA: 0x00249BC0 File Offset: 0x00247DC0
		public int AptitudeFor(SkillDef skill)
		{
			int num = 0;
			if (this.aptitudes.NullOrEmpty<Aptitude>())
			{
				return num;
			}
			for (int i = 0; i < this.aptitudes.Count; i++)
			{
				if (this.aptitudes[i].skill == skill)
				{
					num += this.aptitudes[i].level;
				}
			}
			return num;
		}

		// Token: 0x06006780 RID: 26496 RVA: 0x00249C1D File Offset: 0x00247E1D
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x06006781 RID: 26497 RVA: 0x00249C2C File Offset: 0x00247E2C
		public void ResolveReferences()
		{
			if (this.renderNodeProperties != null)
			{
				for (int i = 0; i < this.renderNodeProperties.Count; i++)
				{
					this.renderNodeProperties[i].ResolveReferencesRecursive();
				}
			}
		}

		// Token: 0x04004172 RID: 16754
		[MustTranslate]
		public string label;

		// Token: 0x04004173 RID: 16755
		[MustTranslate]
		public string labelMale;

		// Token: 0x04004174 RID: 16756
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04004175 RID: 16757
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x04004176 RID: 16758
		[MustTranslate]
		public string description;

		// Token: 0x04004177 RID: 16759
		public int degree;

		// Token: 0x04004178 RID: 16760
		public float commonality = 1f;

		// Token: 0x04004179 RID: 16761
		public List<StatModifier> statOffsets;

		// Token: 0x0400417A RID: 16762
		public List<StatModifier> statFactors;

		// Token: 0x0400417B RID: 16763
		public ThinkTreeDef thinkTree;

		// Token: 0x0400417C RID: 16764
		public MentalStateDef randomMentalState;

		// Token: 0x0400417D RID: 16765
		public SimpleCurve randomMentalStateMtbDaysMoodCurve;

		// Token: 0x0400417E RID: 16766
		public MentalStateDef forcedMentalState;

		// Token: 0x0400417F RID: 16767
		public float forcedMentalStateMtbDays = -1f;

		// Token: 0x04004180 RID: 16768
		public List<MentalStateDef> disallowedMentalStates;

		// Token: 0x04004181 RID: 16769
		public List<ThoughtDef> disallowedThoughts;

		// Token: 0x04004182 RID: 16770
		public List<TraitIngestionThoughtsOverride> disallowedThoughtsFromIngestion;

		// Token: 0x04004183 RID: 16771
		public List<TraitIngestionThoughtsOverride> extraThoughtsFromIngestion;

		// Token: 0x04004184 RID: 16772
		public List<InspirationDef> disallowedInspirations;

		// Token: 0x04004185 RID: 16773
		public List<InspirationDef> mentalBreakInspirationGainSet;

		// Token: 0x04004186 RID: 16774
		public string mentalBreakInspirationGainReasonText;

		// Token: 0x04004187 RID: 16775
		public List<MeditationFocusDef> allowedMeditationFocusTypes;

		// Token: 0x04004188 RID: 16776
		public List<MeditationFocusDef> disallowedMeditationFocusTypes;

		// Token: 0x04004189 RID: 16777
		public float mentalBreakInspirationGainChance;

		// Token: 0x0400418A RID: 16778
		public List<MentalBreakDef> theOnlyAllowedMentalBreaks;

		// Token: 0x0400418B RID: 16779
		public List<SkillGain> skillGains = new List<SkillGain>();

		// Token: 0x0400418C RID: 16780
		public float socialFightChanceFactor = 1f;

		// Token: 0x0400418D RID: 16781
		public float marketValueFactorOffset;

		// Token: 0x0400418E RID: 16782
		public float randomDiseaseMtbDays;

		// Token: 0x0400418F RID: 16783
		public float hungerRateFactor = 1f;

		// Token: 0x04004190 RID: 16784
		public float painOffset;

		// Token: 0x04004191 RID: 16785
		public float painFactor = 1f;

		// Token: 0x04004192 RID: 16786
		public Type mentalStateGiverClass = typeof(TraitMentalStateGiver);

		// Token: 0x04004193 RID: 16787
		public List<AbilityDef> abilities;

		// Token: 0x04004194 RID: 16788
		public List<IngestibleModifiers> ingestibleModifiers;

		// Token: 0x04004195 RID: 16789
		public List<Aptitude> aptitudes;

		// Token: 0x04004196 RID: 16790
		public List<NeedDef> enablesNeeds;

		// Token: 0x04004197 RID: 16791
		public List<NeedDef> disablesNeeds;

		// Token: 0x04004198 RID: 16792
		private List<PawnRenderNodeProperties> renderNodeProperties;

		// Token: 0x04004199 RID: 16793
		public List<PossessionThingDefCountClass> possessions = new List<PossessionThingDefCountClass>();

		// Token: 0x0400419A RID: 16794
		[Unsaved(false)]
		private TraitMentalStateGiver mentalStateGiverInt;

		// Token: 0x0400419B RID: 16795
		[Unsaved(false)]
		private string cachedLabelCap;

		// Token: 0x0400419C RID: 16796
		[Unsaved(false)]
		private string cachedLabelMaleCap;

		// Token: 0x0400419D RID: 16797
		[Unsaved(false)]
		private string cachedLabelFemaleCap;

		// Token: 0x0400419E RID: 16798
		[Unsaved(false)]
		private List<IssueDef> affectedIssuesCached;

		// Token: 0x0400419F RID: 16799
		[Unsaved(false)]
		private List<MemeDef> agreeableMemesCached;

		// Token: 0x040041A0 RID: 16800
		[Unsaved(false)]
		private List<MemeDef> disagreeableMemesCached;
	}
}
