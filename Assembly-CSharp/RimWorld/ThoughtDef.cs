using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001002 RID: 4098
	public class ThoughtDef : Def
	{
		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x06006756 RID: 26454 RVA: 0x002492D0 File Offset: 0x002474D0
		public string Label
		{
			get
			{
				if (!this.label.NullOrEmpty())
				{
					return this.label;
				}
				if (!this.stages.NullOrEmpty<ThoughtStage>())
				{
					if (!this.stages[0].label.NullOrEmpty())
					{
						return this.stages[0].label;
					}
					if (!this.stages[0].labelSocial.NullOrEmpty())
					{
						return this.stages[0].labelSocial;
					}
				}
				Log.Error("Cannot get good label for ThoughtDef " + this.defName);
				return this.defName;
			}
		}

		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x06006757 RID: 26455 RVA: 0x0024936D File Offset: 0x0024756D
		public override TaggedString LabelCap
		{
			get
			{
				if (this.Label.NullOrEmpty())
				{
					return null;
				}
				if (this.cachedLabelCap.NullOrEmpty())
				{
					this.cachedLabelCap = this.Label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x06006758 RID: 26456 RVA: 0x002493AC File Offset: 0x002475AC
		public int DurationTicks
		{
			get
			{
				return (int)(this.durationDays * 60000f);
			}
		}

		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x06006759 RID: 26457 RVA: 0x002493BC File Offset: 0x002475BC
		public bool IsMemory
		{
			get
			{
				if (this.isMemoryCached == BoolUnknown.Unknown)
				{
					this.isMemoryCached = ((this.durationDays > 0f || typeof(Thought_Memory).IsAssignableFrom(this.thoughtClass)) ? BoolUnknown.True : BoolUnknown.False);
				}
				return this.isMemoryCached == BoolUnknown.True;
			}
		}

		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x0600675A RID: 26458 RVA: 0x00249409 File Offset: 0x00247609
		public bool IsSituational
		{
			get
			{
				return this.Worker != null;
			}
		}

		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x0600675B RID: 26459 RVA: 0x00249414 File Offset: 0x00247614
		public bool IsSocial
		{
			get
			{
				return typeof(ISocialThought).IsAssignableFrom(this.ThoughtClass);
			}
		}

		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x0600675C RID: 26460 RVA: 0x0024942B File Offset: 0x0024762B
		public bool RequiresSpecificTraitsDegree
		{
			get
			{
				return this.requiredTraitsDegree != int.MinValue;
			}
		}

		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x0600675D RID: 26461 RVA: 0x0024943D File Offset: 0x0024763D
		public ThoughtWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (ThoughtWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x0600675E RID: 26462 RVA: 0x0024947D File Offset: 0x0024767D
		public Type ThoughtClass
		{
			get
			{
				if (this.thoughtClass != null)
				{
					return this.thoughtClass;
				}
				if (this.IsMemory)
				{
					return typeof(Thought_Memory);
				}
				return typeof(Thought_Situational);
			}
		}

		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x0600675F RID: 26463 RVA: 0x002494B1 File Offset: 0x002476B1
		public Texture2D Icon
		{
			get
			{
				if (this.iconInt == null)
				{
					if (this.icon == null)
					{
						return null;
					}
					this.iconInt = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconInt;
			}
		}

		// Token: 0x06006760 RID: 26464 RVA: 0x002494E3 File Offset: 0x002476E3
		public override IEnumerable<string> ConfigErrors()
		{
			ThoughtDef.<ConfigErrors>d__69 <ConfigErrors>d__ = new ThoughtDef.<ConfigErrors>d__69(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06006761 RID: 26465 RVA: 0x002494F3 File Offset: 0x002476F3
		public static ThoughtDef Named(string defName)
		{
			return DefDatabase<ThoughtDef>.GetNamed(defName, true);
		}

		// Token: 0x04004125 RID: 16677
		public Type thoughtClass;

		// Token: 0x04004126 RID: 16678
		public Type workerClass;

		// Token: 0x04004127 RID: 16679
		public List<ThoughtStage> stages = new List<ThoughtStage>();

		// Token: 0x04004128 RID: 16680
		public int stackLimit = 1;

		// Token: 0x04004129 RID: 16681
		public float stackedEffectMultiplier = 0.75f;

		// Token: 0x0400412A RID: 16682
		public bool stagesStack;

		// Token: 0x0400412B RID: 16683
		public float durationDays;

		// Token: 0x0400412C RID: 16684
		public bool invert;

		// Token: 0x0400412D RID: 16685
		public bool validWhileDespawned;

		// Token: 0x0400412E RID: 16686
		public ThoughtDef nextThought;

		// Token: 0x0400412F RID: 16687
		public ThoughtDef producesMemoryThought;

		// Token: 0x04004130 RID: 16688
		public List<TraitDef> nullifyingTraits;

		// Token: 0x04004131 RID: 16689
		public List<TraitDef> neverNullifyIfAnyTrait;

		// Token: 0x04004132 RID: 16690
		[XmlInheritanceAllowDuplicateNodes]
		public List<TraitRequirement> nullifyingTraitDegrees;

		// Token: 0x04004133 RID: 16691
		public List<TaleDef> nullifyingOwnTales;

		// Token: 0x04004134 RID: 16692
		public List<PreceptDef> nullifyingPrecepts;

		// Token: 0x04004135 RID: 16693
		public List<HediffDef> nullifyingHediffs;

		// Token: 0x04004136 RID: 16694
		public List<GeneDef> nullifyingGenes;

		// Token: 0x04004137 RID: 16695
		public List<TraitDef> requiredTraits;

		// Token: 0x04004138 RID: 16696
		public List<GeneDef> requiredGenes;

		// Token: 0x04004139 RID: 16697
		public int requiredTraitsDegree = int.MinValue;

		// Token: 0x0400413A RID: 16698
		public List<HediffDef> requiredHediffs;

		// Token: 0x0400413B RID: 16699
		public StatDef effectMultiplyingStat;

		// Token: 0x0400413C RID: 16700
		public SimpleCurve effectMultiplyingStatCurve;

		// Token: 0x0400413D RID: 16701
		public HediffDef hediff;

		// Token: 0x0400413E RID: 16702
		public MentalStateDef mentalState;

		// Token: 0x0400413F RID: 16703
		public GameConditionDef gameCondition;

		// Token: 0x04004140 RID: 16704
		public bool nullifiedIfNotColonist;

		// Token: 0x04004141 RID: 16705
		public ThoughtDef thoughtToMake;

		// Token: 0x04004142 RID: 16706
		[NoTranslate]
		private string icon;

		// Token: 0x04004143 RID: 16707
		public bool showBubble;

		// Token: 0x04004144 RID: 16708
		public ExpectationDef minExpectation;

		// Token: 0x04004145 RID: 16709
		public ExpectationDef minExpectationForNegativeThought;

		// Token: 0x04004146 RID: 16710
		public List<ThoughtDef> replaceThoughts;

		// Token: 0x04004147 RID: 16711
		public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04004148 RID: 16712
		public ChemicalDef chemicalDef;

		// Token: 0x04004149 RID: 16713
		public bool lerpMoodToZero;

		// Token: 0x0400414A RID: 16714
		public int stackLimitForSameOtherPawn = -1;

		// Token: 0x0400414B RID: 16715
		public float lerpOpinionToZeroAfterDurationPct = 0.7f;

		// Token: 0x0400414C RID: 16716
		public DevelopmentalStage socialTargetDevelopmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x0400414D RID: 16717
		public bool ignoreSubhumans = true;

		// Token: 0x0400414E RID: 16718
		public bool doNotApplyToQuestLodgers;

		// Token: 0x0400414F RID: 16719
		public float maxCumulatedOpinionOffset = float.MaxValue;

		// Token: 0x04004150 RID: 16720
		public TaleDef taleDef;

		// Token: 0x04004151 RID: 16721
		public Gender gender;

		// Token: 0x04004152 RID: 16722
		[MustTranslate]
		public string labelInBracketsExtraForHediff;

		// Token: 0x04004153 RID: 16723
		[Unsaved(false)]
		private ThoughtWorker workerInt;

		// Token: 0x04004154 RID: 16724
		[Unsaved(false)]
		private BoolUnknown isMemoryCached = BoolUnknown.Unknown;

		// Token: 0x04004155 RID: 16725
		private Texture2D iconInt;
	}
}
