using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001598 RID: 5528
	public class Pawn_GeneTracker : IExposable
	{
		// Token: 0x1700173D RID: 5949
		// (get) Token: 0x060087DF RID: 34783 RVA: 0x00311F91 File Offset: 0x00310191
		public XenotypeDef Xenotype
		{
			get
			{
				return this.xenotype ?? XenotypeDefOf.Baseliner;
			}
		}

		// Token: 0x1700173E RID: 5950
		// (get) Token: 0x060087E0 RID: 34784 RVA: 0x00311FA2 File Offset: 0x003101A2
		public bool UniqueXenotype
		{
			get
			{
				return !this.xenotypeName.NullOrEmpty();
			}
		}

		// Token: 0x1700173F RID: 5951
		// (get) Token: 0x060087E1 RID: 34785 RVA: 0x00311FB2 File Offset: 0x003101B2
		public string XenotypeLabel
		{
			get
			{
				if (!this.UniqueXenotype)
				{
					return this.Xenotype.label;
				}
				return this.xenotypeName ?? "Unique".Translate();
			}
		}

		// Token: 0x17001740 RID: 5952
		// (get) Token: 0x060087E2 RID: 34786 RVA: 0x00311FE1 File Offset: 0x003101E1
		public string XenotypeLabelCap
		{
			get
			{
				return this.XenotypeLabel.CapitalizeFirst();
			}
		}

		// Token: 0x17001741 RID: 5953
		// (get) Token: 0x060087E3 RID: 34787 RVA: 0x00311FF0 File Offset: 0x003101F0
		public CustomXenotype CustomXenotype
		{
			get
			{
				if (this.Xenotype != XenotypeDefOf.Baseliner || this.hybrid || Current.ProgramState != ProgramState.Playing)
				{
					return null;
				}
				if (this.cachedHasCustomXenotype == null)
				{
					this.cachedHasCustomXenotype = new bool?(false);
					foreach (CustomXenotype custom in Current.Game.customXenotypeDatabase.customXenotypes)
					{
						if (GeneUtility.PawnIsCustomXenotype(this.pawn, custom))
						{
							this.cachedHasCustomXenotype = new bool?(true);
							this.cachedCustomXenotype = custom;
							break;
						}
					}
				}
				return this.cachedCustomXenotype;
			}
		}

		// Token: 0x17001742 RID: 5954
		// (get) Token: 0x060087E4 RID: 34788 RVA: 0x003120A8 File Offset: 0x003102A8
		public string XenotypeDescShort
		{
			get
			{
				if (this.UniqueXenotype)
				{
					return "UniqueXenotypeDesc".Translate();
				}
				if (!this.Xenotype.descriptionShort.NullOrEmpty())
				{
					return this.Xenotype.descriptionShort + "\n\n" + "MoreInfoInInfoScreen".Translate().Colorize(ColoredText.SubtleGrayColor);
				}
				return this.Xenotype.description;
			}
		}

		// Token: 0x17001743 RID: 5955
		// (get) Token: 0x060087E5 RID: 34789 RVA: 0x00312114 File Offset: 0x00310314
		public Texture2D XenotypeIcon
		{
			get
			{
				if (!ModsConfig.BiotechActive)
				{
					return null;
				}
				if (this.iconDef != null)
				{
					return this.iconDef.Icon;
				}
				if (this.UniqueXenotype)
				{
					return XenotypeIconDefOf.Basic.Icon;
				}
				return this.Xenotype.Icon;
			}
		}

		// Token: 0x17001744 RID: 5956
		// (get) Token: 0x060087E6 RID: 34790 RVA: 0x00312151 File Offset: 0x00310351
		public List<Gene> GenesListForReading
		{
			get
			{
				if (this.cachedGenes == null)
				{
					this.cachedGenes = new List<Gene>();
					this.cachedGenes.AddRange(this.xenogenes);
					this.cachedGenes.AddRange(this.endogenes);
				}
				return this.cachedGenes;
			}
		}

		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x060087E7 RID: 34791 RVA: 0x0031218E File Offset: 0x0031038E
		public List<Gene> Xenogenes
		{
			get
			{
				return this.xenogenes;
			}
		}

		// Token: 0x17001746 RID: 5958
		// (get) Token: 0x060087E8 RID: 34792 RVA: 0x00312196 File Offset: 0x00310396
		public List<Gene> Endogenes
		{
			get
			{
				return this.endogenes;
			}
		}

		// Token: 0x17001747 RID: 5959
		// (get) Token: 0x060087E9 RID: 34793 RVA: 0x003121A0 File Offset: 0x003103A0
		public float PainOffset
		{
			get
			{
				float num = 0f;
				if (!ModLister.BiotechInstalled)
				{
					return num;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active)
					{
						num += genesListForReading[i].def.painOffset;
					}
				}
				return num;
			}
		}

		// Token: 0x17001748 RID: 5960
		// (get) Token: 0x060087EA RID: 34794 RVA: 0x003121F8 File Offset: 0x003103F8
		public float PainFactor
		{
			get
			{
				float num = 1f;
				if (!ModLister.BiotechInstalled)
				{
					return num;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active)
					{
						num *= genesListForReading[i].def.painFactor;
					}
				}
				return num;
			}
		}

		// Token: 0x17001749 RID: 5961
		// (get) Token: 0x060087EB RID: 34795 RVA: 0x00312250 File Offset: 0x00310450
		public float SocialFightChanceFactor
		{
			get
			{
				float num = 1f;
				if (!ModLister.BiotechInstalled)
				{
					return num;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active)
					{
						num *= genesListForReading[i].def.socialFightChanceFactor;
					}
				}
				return num;
			}
		}

		// Token: 0x1700174A RID: 5962
		// (get) Token: 0x060087EC RID: 34796 RVA: 0x003122A8 File Offset: 0x003104A8
		public float AggroMentalBreakSelectionChanceFactor
		{
			get
			{
				float num = 1f;
				if (!ModLister.BiotechInstalled)
				{
					return num;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active)
					{
						num *= genesListForReading[i].def.aggroMentalBreakSelectionChanceFactor;
					}
				}
				return num;
			}
		}

		// Token: 0x1700174B RID: 5963
		// (get) Token: 0x060087ED RID: 34797 RVA: 0x00312300 File Offset: 0x00310500
		public bool AffectedByDarkness
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return true;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active && genesListForReading[i].def.ignoreDarkness)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x1700174C RID: 5964
		// (get) Token: 0x060087EE RID: 34798 RVA: 0x00312354 File Offset: 0x00310554
		public float BiologicalAgeTickFactor
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return 1f;
				}
				if (this.cachedGenesAffectAge != null && !this.cachedGenesAffectAge.Value)
				{
					return 1f;
				}
				bool value = this.cachedGenesAffectAge.GetValueOrDefault();
				if (this.cachedGenesAffectAge == null)
				{
					value = false;
					this.cachedGenesAffectAge = new bool?(value);
				}
				float num = 1f;
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active && genesListForReading[i].def.biologicalAgeTickFactorFromAgeCurve != null)
					{
						num *= genesListForReading[i].def.biologicalAgeTickFactorFromAgeCurve.Evaluate(this.pawn.ageTracker.AgeBiologicalYearsFloat);
						this.cachedGenesAffectAge = new bool?(true);
					}
				}
				return num;
			}
		}

		// Token: 0x1700174D RID: 5965
		// (get) Token: 0x060087EF RID: 34799 RVA: 0x0031242C File Offset: 0x0031062C
		public bool EnjoysSunlight
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return true;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].def.dislikesSunlight && genesListForReading[i].Active)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x1700174E RID: 5966
		// (get) Token: 0x060087F0 RID: 34800 RVA: 0x00312480 File Offset: 0x00310680
		public bool ShouldHaveCallTracker
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return false;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active && genesListForReading[i].def.soundCall != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700174F RID: 5967
		// (get) Token: 0x060087F1 RID: 34801 RVA: 0x003124D4 File Offset: 0x003106D4
		public bool DontMindRawFood
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return false;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active && genesListForReading[i].def.dontMindRawFood)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001750 RID: 5968
		// (get) Token: 0x060087F2 RID: 34802 RVA: 0x00312528 File Offset: 0x00310728
		public bool CanHaveBeard
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return this.pawn.gender != Gender.Female;
				}
				if (this.pawn.gender == Gender.Female)
				{
					List<Gene> genesListForReading = this.GenesListForReading;
					for (int i = 0; i < genesListForReading.Count; i++)
					{
						if (genesListForReading[i].Active && genesListForReading[i].def.womenCanHaveBeards)
						{
							return true;
						}
					}
					return false;
				}
				return true;
			}
		}

		// Token: 0x17001751 RID: 5969
		// (get) Token: 0x060087F3 RID: 34803 RVA: 0x0031259C File Offset: 0x0031079C
		public float PrisonBreakIntervalFactor
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return 1f;
				}
				float num = 1f;
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active)
					{
						num *= genesListForReading[i].def.prisonBreakMTBFactor;
					}
				}
				return num;
			}
		}

		// Token: 0x17001752 RID: 5970
		// (get) Token: 0x060087F4 RID: 34804 RVA: 0x003125F8 File Offset: 0x003107F8
		public WorkTags DisabledWorkTags
		{
			get
			{
				if (!ModLister.BiotechInstalled)
				{
					return WorkTags.None;
				}
				List<Gene> genesListForReading = this.GenesListForReading;
				WorkTags workTags = WorkTags.None;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active)
					{
						workTags |= genesListForReading[i].def.disabledWorkTags;
					}
				}
				return workTags;
			}
		}

		// Token: 0x17001753 RID: 5971
		// (get) Token: 0x060087F5 RID: 34805 RVA: 0x0031264C File Offset: 0x0031084C
		public bool TattoosVisible
		{
			get
			{
				if (this.cachedTattoosVisible != null)
				{
					return this.cachedTattoosVisible.Value;
				}
				this.cachedTattoosVisible = new bool?(true);
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active && !genesListForReading[i].def.tattoosVisible)
					{
						this.cachedTattoosVisible = new bool?(false);
						break;
					}
				}
				return this.cachedTattoosVisible.Value;
			}
		}

		// Token: 0x17001754 RID: 5972
		// (get) Token: 0x060087F6 RID: 34806 RVA: 0x003126D0 File Offset: 0x003108D0
		public int? WaterCellCost
		{
			get
			{
				if (this.hasCachedWaterCost)
				{
					return this.cachedWaterCellCost;
				}
				this.hasCachedWaterCost = true;
				List<Gene> genesListForReading = this.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active && genesListForReading[i].def.waterCellCost != null)
					{
						int? waterCellCost = genesListForReading[i].def.waterCellCost;
						if (this.cachedWaterCellCost == null || waterCellCost.Value < this.cachedWaterCellCost.Value)
						{
							this.cachedWaterCellCost = waterCellCost;
						}
					}
				}
				return this.cachedWaterCellCost;
			}
		}

		// Token: 0x060087F7 RID: 34807 RVA: 0x00312774 File Offset: 0x00310974
		public Pawn_GeneTracker()
		{
		}

		// Token: 0x060087F8 RID: 34808 RVA: 0x003127CC File Offset: 0x003109CC
		public Pawn_GeneTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060087F9 RID: 34809 RVA: 0x00312828 File Offset: 0x00310A28
		public void SetXenotype(XenotypeDef xenotype)
		{
			if (!ModLister.CheckBiotech("Xenotypes"))
			{
				return;
			}
			this.xenotype = xenotype;
			this.xenotypeName = null;
			this.iconDef = null;
			this.cachedHasCustomXenotype = null;
			this.cachedCustomXenotype = null;
			this.ClearXenogenes();
			for (int i = 0; i < xenotype.genes.Count; i++)
			{
				this.AddGene(xenotype.genes[i], !xenotype.inheritable);
			}
		}

		// Token: 0x060087FA RID: 34810 RVA: 0x003128A2 File Offset: 0x00310AA2
		public void SetXenotypeDirect(XenotypeDef xenotype)
		{
			if (!ModLister.CheckBiotech("Xenotypes"))
			{
				return;
			}
			this.xenotype = xenotype;
			this.xenotypeName = null;
			this.cachedHasCustomXenotype = null;
			this.cachedCustomXenotype = null;
		}

		// Token: 0x060087FB RID: 34811 RVA: 0x003128D4 File Offset: 0x00310AD4
		[Obsolete("Use HasActiveGene() instead")]
		public bool HasGene(GeneDef geneDef)
		{
			if (geneDef == null)
			{
				return false;
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].def == geneDef)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060087FC RID: 34812 RVA: 0x00312910 File Offset: 0x00310B10
		public bool HasActiveGene(GeneDef geneDef)
		{
			if (geneDef == null)
			{
				return false;
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].def == geneDef && genesListForReading[i].Active)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060087FD RID: 34813 RVA: 0x0031295A File Offset: 0x00310B5A
		public bool TryGetNeedEnablingGene(NeedDef def, out Gene gene)
		{
			if (this.cachedEnabledNeeds == null)
			{
				gene = null;
				return false;
			}
			return this.cachedEnabledNeeds.TryGetValue(def, out gene);
		}

		// Token: 0x060087FE RID: 34814 RVA: 0x00312976 File Offset: 0x00310B76
		public bool TryGetNeedDisablingGene(NeedDef def, out Gene gene)
		{
			if (this.cachedDisabledNeeds == null)
			{
				gene = null;
				return false;
			}
			return this.cachedDisabledNeeds.TryGetValue(def, out gene);
		}

		// Token: 0x060087FF RID: 34815 RVA: 0x00312992 File Offset: 0x00310B92
		public bool EnablesNeed(NeedDef def)
		{
			Dictionary<NeedDef, Gene> dictionary = this.cachedEnabledNeeds;
			return dictionary != null && dictionary.ContainsKey(def);
		}

		// Token: 0x06008800 RID: 34816 RVA: 0x003129A6 File Offset: 0x00310BA6
		public bool DisablesNeed(NeedDef def)
		{
			Dictionary<NeedDef, Gene> dictionary = this.cachedDisabledNeeds;
			return dictionary != null && dictionary.ContainsKey(def);
		}

		// Token: 0x06008801 RID: 34817 RVA: 0x003129BA File Offset: 0x00310BBA
		public bool IsXenogene(Gene gene)
		{
			return this.xenogenes.Contains(gene);
		}

		// Token: 0x06008802 RID: 34818 RVA: 0x003129C8 File Offset: 0x00310BC8
		public bool HasXenogene(GeneDef geneDef)
		{
			for (int i = 0; i < this.xenogenes.Count; i++)
			{
				if (this.xenogenes[i].def == geneDef)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008803 RID: 34819 RVA: 0x00312A04 File Offset: 0x00310C04
		public bool HasEndogene(GeneDef geneDef)
		{
			for (int i = 0; i < this.endogenes.Count; i++)
			{
				if (this.endogenes[i].def == geneDef)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008804 RID: 34820 RVA: 0x00312A40 File Offset: 0x00310C40
		public Gene GetGene(GeneDef geneDef)
		{
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].def == geneDef)
				{
					return genesListForReading[i];
				}
			}
			return null;
		}

		// Token: 0x06008805 RID: 34821 RVA: 0x00312A80 File Offset: 0x00310C80
		public T GetFirstGeneOfType<T>() where T : Gene
		{
			if (!ModLister.BiotechInstalled)
			{
				return default(T);
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active)
				{
					T t = genesListForReading[i] as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06008806 RID: 34822 RVA: 0x00312AEC File Offset: 0x00310CEC
		public GeneDef GetMelaninGene()
		{
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active && genesListForReading[i].def.endogeneCategory == EndogeneCategory.Melanin)
				{
					return genesListForReading[i].def;
				}
			}
			return null;
		}

		// Token: 0x06008807 RID: 34823 RVA: 0x00312B44 File Offset: 0x00310D44
		public GeneDef GetHairColorGene()
		{
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active && genesListForReading[i].def.endogeneCategory == EndogeneCategory.HairColor)
				{
					return genesListForReading[i].def;
				}
			}
			return null;
		}

		// Token: 0x06008808 RID: 34824 RVA: 0x00312B9C File Offset: 0x00310D9C
		public GeneDef GetFirstEndogeneByCategory(EndogeneCategory cat)
		{
			List<Gene> list = this.Endogenes;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Active && list[i].def.endogeneCategory == cat)
				{
					return list[i].def;
				}
			}
			return null;
		}

		// Token: 0x06008809 RID: 34825 RVA: 0x00312BF1 File Offset: 0x00310DF1
		public Gene AddGene(GeneDef geneDef, bool xenogene)
		{
			if (xenogene && !ModLister.BiotechInstalled)
			{
				return null;
			}
			if (!xenogene && this.HasEndogene(geneDef))
			{
				return null;
			}
			return this.AddGene(GeneMaker.MakeGene(geneDef, this.pawn), xenogene);
		}

		// Token: 0x0600880A RID: 34826 RVA: 0x00312C20 File Offset: 0x00310E20
		private Gene AddGene(Gene gene, bool addAsXenogene)
		{
			if (addAsXenogene && !ModLister.BiotechInstalled)
			{
				return null;
			}
			if (addAsXenogene)
			{
				this.xenogenes.Add(gene);
			}
			else
			{
				this.endogenes.Add(gene);
			}
			this.cachedGenes = null;
			if (ModLister.BiotechInstalled)
			{
				this.CheckForOverrides();
				if (!gene.def.abilities.NullOrEmpty<AbilityDef>() && this.pawn.abilities != null)
				{
					for (int i = 0; i < gene.def.abilities.Count; i++)
					{
						this.pawn.abilities.GainAbility(gene.def.abilities[i]);
					}
				}
				if (!gene.def.forcedTraits.NullOrEmpty<GeneticTraitData>() && this.pawn.story != null)
				{
					for (int j = 0; j < gene.def.forcedTraits.Count; j++)
					{
						Trait trait = new Trait(gene.def.forcedTraits[j].def, gene.def.forcedTraits[j].degree, false);
						trait.sourceGene = gene;
						this.pawn.story.traits.GainTrait(trait, true);
					}
				}
				if (gene.def.passionMod != null)
				{
					SkillRecord skill = this.pawn.skills.GetSkill(gene.def.passionMod.skill);
					gene.passionPreAdd = new Passion?(skill.passion);
					skill.passion = gene.def.passionMod.NewPassionFor(skill);
				}
				Pawn_StoryTracker story = this.pawn.story;
				if (story != null)
				{
					TraitSet traits = story.traits;
					if (traits != null)
					{
						traits.RecalculateSuppression();
					}
				}
				if (gene.def.disabledWorkTags.HasFlag(WorkTags.Violent))
				{
					Pawn_EquipmentTracker equipment = this.pawn.equipment;
					if (((equipment != null) ? equipment.Primary : null) != null)
					{
						if (this.pawn.PositionHeld.IsValid && this.pawn.MapHeld != null)
						{
							ThingWithComps thingWithComps;
							this.pawn.equipment.TryDropEquipment(this.pawn.equipment.Primary, out thingWithComps, this.pawn.PositionHeld, false);
						}
						else
						{
							this.pawn.equipment.DestroyEquipment(this.pawn.equipment.Primary);
						}
					}
				}
			}
			this.Notify_GenesChanged(gene.def);
			if (ModLister.BiotechInstalled)
			{
				gene.PostAdd();
			}
			return gene;
		}

		// Token: 0x0600880B RID: 34827 RVA: 0x00312E94 File Offset: 0x00311094
		public void RemoveGene(Gene gene)
		{
			if (this.xenogenes.Remove(gene) || this.endogenes.Remove(gene))
			{
				this.cachedGenes = null;
				if (ModLister.BiotechInstalled)
				{
					this.CheckForOverrides();
					if (this.pawn.abilities != null && !gene.def.abilities.NullOrEmpty<AbilityDef>())
					{
						for (int i = 0; i < gene.def.abilities.Count; i++)
						{
							this.pawn.abilities.RemoveAbility(gene.def.abilities[i]);
						}
					}
					if (!gene.def.forcedTraits.NullOrEmpty<GeneticTraitData>() || !gene.def.suppressedTraits.NullOrEmpty<GeneticTraitData>())
					{
						Pawn_StoryTracker story = this.pawn.story;
						if (story != null)
						{
							story.traits.Notify_GeneRemoved(gene);
						}
					}
					if (gene.def.passionMod != null)
					{
						SkillRecord skill = this.pawn.skills.GetSkill(gene.def.passionMod.skill);
						skill.passion = gene.NewPassionForOnRemoval(skill);
					}
				}
				this.Notify_GenesChanged(gene.def);
				if (ModLister.BiotechInstalled)
				{
					gene.PostRemove();
				}
			}
		}

		// Token: 0x0600880C RID: 34828 RVA: 0x00312FC8 File Offset: 0x003111C8
		private void CheckForOverrides()
		{
			List<Gene> genesListForReading = this.GenesListForReading;
			foreach (Gene gene in genesListForReading)
			{
				if (!gene.def.RandomChosen)
				{
					gene.OverrideBy(null);
				}
			}
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (!genesListForReading[i].def.RandomChosen)
				{
					for (int j = i + 1; j < genesListForReading.Count; j++)
					{
						if (!genesListForReading[j].def.RandomChosen && genesListForReading[i].def.ConflictsWith(genesListForReading[j].def))
						{
							if (genesListForReading[i].def.Overrides(genesListForReading[j].def, this.IsXenogene(genesListForReading[i]), this.IsXenogene(genesListForReading[j])))
							{
								genesListForReading[j].OverrideBy(genesListForReading[i]);
							}
							else
							{
								genesListForReading[i].OverrideBy(genesListForReading[j]);
							}
						}
					}
				}
			}
			Pawn_SkillTracker skills = this.pawn.skills;
			if (skills != null)
			{
				skills.DirtyAptitudes();
			}
			this.pawn.Notify_DisabledWorkTypesChanged();
		}

		// Token: 0x0600880D RID: 34829 RVA: 0x00313134 File Offset: 0x00311334
		public void ClearXenogenes()
		{
			for (int i = this.xenogenes.Count - 1; i >= 0; i--)
			{
				this.RemoveGene(this.xenogenes[i]);
			}
		}

		// Token: 0x0600880E RID: 34830 RVA: 0x0031316C File Offset: 0x0031136C
		public void GeneTrackerTick()
		{
			if (!ModLister.BiotechInstalled)
			{
				return;
			}
			for (int i = this.xenogenes.Count - 1; i >= 0; i--)
			{
				if (this.xenogenes[i].Active)
				{
					this.xenogenes[i].Tick();
				}
			}
			for (int j = this.endogenes.Count - 1; j >= 0; j--)
			{
				if (this.endogenes[j].Active)
				{
					this.endogenes[j].Tick();
				}
			}
		}

		// Token: 0x0600880F RID: 34831 RVA: 0x003131FC File Offset: 0x003113FC
		public void GeneTrackerTickInterval(int delta)
		{
			if (!ModLister.BiotechInstalled)
			{
				return;
			}
			for (int i = this.xenogenes.Count - 1; i >= 0; i--)
			{
				if (this.xenogenes[i].Active)
				{
					this.xenogenes[i].TickInterval(delta);
				}
			}
			for (int j = this.endogenes.Count - 1; j >= 0; j--)
			{
				if (this.endogenes[j].Active)
				{
					this.endogenes[j].TickInterval(delta);
				}
			}
			if (this.pawn.Spawned && this.Xenotype != XenotypeDefOf.Baseliner && this.pawn.IsHashIntervalTick(300, delta))
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.GenesAndXenotypes, OpportunityType.Important);
			}
		}

		// Token: 0x06008810 RID: 34832 RVA: 0x003132C4 File Offset: 0x003114C4
		public void Notify_IngestedThing(Thing thing, int numTaken)
		{
			if (!ModLister.BiotechInstalled)
			{
				return;
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active)
				{
					genesListForReading[i].Notify_IngestedThing(thing, numTaken);
				}
			}
		}

		// Token: 0x06008811 RID: 34833 RVA: 0x00313310 File Offset: 0x00311510
		public void Reset()
		{
			for (int i = 0; i < this.GenesListForReading.Count; i++)
			{
				this.GenesListForReading[i].Reset();
			}
		}

		// Token: 0x06008812 RID: 34834 RVA: 0x00313344 File Offset: 0x00311544
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_GeneTracker.<GetGizmos>d__95 <GetGizmos>d__ = new Pawn_GeneTracker.<GetGizmos>d__95(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06008813 RID: 34835 RVA: 0x00313354 File Offset: 0x00311554
		private void Notify_GenesChanged(GeneDef addedOrRemovedGene)
		{
			Pawn_GeneTracker.<>c__DisplayClass97_0 CS$<>8__locals1 = new Pawn_GeneTracker.<>c__DisplayClass97_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.addedOrRemovedGene = addedOrRemovedGene;
			bool flag = false;
			this.cachedGenes = null;
			CS$<>8__locals1.genes = this.GenesListForReading;
			if (ModLister.BiotechInstalled && CS$<>8__locals1.addedOrRemovedGene.skinIsHairColor)
			{
				Gene gene;
				if (CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene x) => x.def.skinIsHairColor, out gene))
				{
					this.pawn.story.skinColorOverride = new Color?(this.pawn.story.HairColor);
					this.OverrideAllConflicting(gene);
				}
				else
				{
					this.pawn.story.skinColorOverride = null;
				}
				this.EnsureCorrectSkinColorOverride();
				flag = true;
			}
			if (CS$<>8__locals1.addedOrRemovedGene.hairColorOverride != null)
			{
				Gene gene2;
				if (CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene g) => g.def.hairColorOverride != null, out gene2))
				{
					Color color = gene2.def.hairColorOverride.Value;
					if (gene2.def.randomBrightnessFactor != 0f)
					{
						color *= 1f + Rand.Range(-gene2.def.randomBrightnessFactor, gene2.def.randomBrightnessFactor);
					}
					this.pawn.story.HairColor = color.ClampToValueRange(GeneTuning.HairColorValueRange);
					this.OverrideAllConflicting(gene2);
					this.EnsureCorrectSkinColorOverride();
					flag = true;
				}
			}
			if (CS$<>8__locals1.addedOrRemovedGene.skinColorBase != null)
			{
				Gene gene3;
				if (CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene g) => g.def.skinColorBase != null, out gene3))
				{
					this.OverrideAllConflicting(gene3);
					this.pawn.story.SkinColorBase = gene3.def.skinColorBase.Value;
					flag = true;
				}
			}
			if (ModLister.BiotechInstalled)
			{
				if (CS$<>8__locals1.addedOrRemovedGene.skinColorOverride != null)
				{
					Gene gene4;
					if (CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene g) => g.def.skinColorOverride != null, out gene4))
					{
						Color color2 = gene4.def.skinColorOverride.Value;
						if (gene4.def.randomBrightnessFactor != 0f)
						{
							color2 *= 1f + Rand.Range(-gene4.def.randomBrightnessFactor, gene4.def.randomBrightnessFactor);
						}
						this.pawn.story.skinColorOverride = new Color?(color2.ClampToValueRange(GeneTuning.SkinColorValueRange));
						this.OverrideAllConflicting(gene4);
					}
					else
					{
						this.pawn.story.skinColorOverride = null;
					}
					this.EnsureCorrectSkinColorOverride();
					flag = true;
				}
				if (CS$<>8__locals1.addedOrRemovedGene.bodyType != null && !this.pawn.DevelopmentalStage.Juvenile())
				{
					Gene gene5;
					if (CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene g) => g.def.bodyType != null, out gene5))
					{
						this.OverrideAllConflicting(gene5);
						this.pawn.story.bodyType = gene5.def.bodyType.Value.ToBodyType(this.pawn);
					}
					else
					{
						this.pawn.story.bodyType = PawnGenerator.GetBodyTypeFor(this.pawn);
					}
					flag = true;
				}
				if (!CS$<>8__locals1.addedOrRemovedGene.forcedHeadTypes.NullOrEmpty<HeadTypeDef>())
				{
					Gene gene6;
					if (CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene g) => !g.def.forcedHeadTypes.NullOrEmpty<HeadTypeDef>(), out gene6))
					{
						this.OverrideAllConflicting(gene6);
						if (!this.pawn.story.TryGetRandomHeadFromSet(gene6.def.forcedHeadTypes))
						{
							this.pawn.story.TryGetRandomHeadFromSet(from x in DefDatabase<HeadTypeDef>.AllDefs
							where x.randomChosen
							select x);
						}
					}
					else
					{
						this.pawn.story.TryGetRandomHeadFromSet(from x in DefDatabase<HeadTypeDef>.AllDefs
						where x.randomChosen
						select x);
					}
					flag = true;
				}
				if ((CS$<>8__locals1.addedOrRemovedGene.forcedHair != null || CS$<>8__locals1.addedOrRemovedGene.hairTagFilter != null) && !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.pawn.story.hairDef, null))
				{
					this.pawn.story.hairDef = PawnStyleItemChooser.RandomHairFor(this.pawn);
					flag = true;
				}
				if (CS$<>8__locals1.addedOrRemovedGene.beardTagFilter != null && this.pawn.style != null && !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.pawn.style.beardDef, null))
				{
					this.pawn.style.beardDef = PawnStyleItemChooser.RandomBeardFor(this.pawn);
					flag = true;
				}
				if (CS$<>8__locals1.addedOrRemovedGene.fur != null)
				{
					Gene gene7;
					if (CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene g) => g.def.fur != null, out gene7))
					{
						this.pawn.story.furDef = gene7.def.fur;
						this.OverrideAllConflicting(gene7);
					}
					else
					{
						this.pawn.story.furDef = null;
					}
					flag = true;
				}
				Gene gene8;
				if (CS$<>8__locals1.addedOrRemovedGene.RandomChosen && CS$<>8__locals1.<Notify_GenesChanged>g__SelectGene|0((Gene g) => g.Active && g.def.ConflictsWith(CS$<>8__locals1.addedOrRemovedGene), out gene8))
				{
					this.OverrideAllConflicting(gene8);
					flag = true;
				}
				this.RecacheNeeds();
				if (CS$<>8__locals1.addedOrRemovedGene.soundCall != null)
				{
					PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
				}
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs != null)
				{
					needs.AddOrRemoveNeedsAsAppropriate();
				}
				this.pawn.health.hediffSet.DirtyCache();
				this.cachedDamageFactors.Clear();
				this.cachedAddictionChanceFactors.Clear();
				this.cachedGenesAffectAge = null;
				this.cachedHasCustomXenotype = null;
				this.cachedCustomXenotype = null;
				this.cachedTattoosVisible = null;
				this.cachedWaterCellCost = null;
				this.hasCachedWaterCost = false;
				Pawn_SkillTracker skills = this.pawn.skills;
				if (skills != null)
				{
					skills.DirtyAptitudes();
				}
				this.pawn.Notify_DisabledWorkTypesChanged();
			}
			if (flag)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}

		// Token: 0x06008814 RID: 34836 RVA: 0x003139C8 File Offset: 0x00311BC8
		private void EnsureCorrectSkinColorOverride()
		{
			if (!ModLister.BiotechInstalled)
			{
				return;
			}
			bool flag = false;
			Color? skinColorOverride = null;
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				Gene gene = genesListForReading[i];
				if (gene.Active)
				{
					if (gene.def.skinIsHairColor)
					{
						flag = true;
					}
					else if (gene.def.skinColorOverride != null)
					{
						skinColorOverride = gene.def.skinColorOverride;
					}
				}
			}
			if (flag)
			{
				this.pawn.story.skinColorOverride = new Color?(this.pawn.story.HairColor);
				return;
			}
			this.pawn.story.skinColorOverride = skinColorOverride;
		}

		// Token: 0x06008815 RID: 34837 RVA: 0x00313A80 File Offset: 0x00311C80
		private void OverrideAllConflicting(Gene gene)
		{
			if (!ModLister.BiotechInstalled || !gene.def.RandomChosen)
			{
				return;
			}
			gene.OverrideBy(null);
			foreach (Gene gene2 in this.GenesListForReading)
			{
				if (gene2 != gene && gene2.def.ConflictsWith(gene.def))
				{
					gene2.OverrideBy(gene);
				}
			}
		}

		// Token: 0x06008816 RID: 34838 RVA: 0x00313B08 File Offset: 0x00311D08
		public void Notify_NewColony()
		{
			foreach (Gene gene in this.GenesListForReading)
			{
				gene.Notify_NewColony();
			}
		}

		// Token: 0x06008817 RID: 34839 RVA: 0x00313B58 File Offset: 0x00311D58
		public void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = genesListForReading.Count - 1; i >= 0; i--)
			{
				genesListForReading[i].Notify_PawnDied(dinfo, culprit);
			}
		}

		// Token: 0x06008818 RID: 34840 RVA: 0x00313B90 File Offset: 0x00311D90
		public bool StyleItemAllowed(StyleItemDef styleItem)
		{
			if (!ModLister.BiotechInstalled)
			{
				return true;
			}
			bool flag = styleItem is HairDef;
			bool flag2 = styleItem is BeardDef;
			if (!flag && !flag2)
			{
				return true;
			}
			if (flag2 && !this.pawn.style.CanWantBeard)
			{
				return styleItem == BeardDefOf.NoBeard;
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active)
				{
					if (flag)
					{
						if (genesListForReading[i].def.forcedHair != null && genesListForReading[i].def.forcedHair != styleItem)
						{
							return false;
						}
						if (genesListForReading[i].def.hairTagFilter != null && !genesListForReading[i].def.hairTagFilter.Allows(styleItem.styleTags))
						{
							return false;
						}
					}
					else if (flag2 && genesListForReading[i].def.beardTagFilter != null && !genesListForReading[i].def.beardTagFilter.Allows(styleItem.styleTags))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06008819 RID: 34841 RVA: 0x00313CA8 File Offset: 0x00311EA8
		public float FactorForDamage(DamageInfo dinfo)
		{
			if (!ModLister.BiotechInstalled || dinfo.Def == null || this.GenesListForReading.NullOrEmpty<Gene>())
			{
				return 1f;
			}
			float result;
			if (this.cachedDamageFactors.TryGetValue(dinfo.Def, out result))
			{
				return result;
			}
			float num = 1f;
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				Gene gene = genesListForReading[i];
				if (gene.Active && !gene.def.damageFactors.NullOrEmpty<DamageFactor>())
				{
					for (int j = 0; j < gene.def.damageFactors.Count; j++)
					{
						if (gene.def.damageFactors[j].damageDef == dinfo.Def)
						{
							num *= gene.def.damageFactors[j].factor;
						}
					}
				}
			}
			this.cachedDamageFactors.Add(dinfo.Def, num);
			return num;
		}

		// Token: 0x0600881A RID: 34842 RVA: 0x00313DAC File Offset: 0x00311FAC
		public float AddictionChanceFactor(ChemicalDef chemical)
		{
			if (!ModLister.BiotechInstalled || this.GenesListForReading.NullOrEmpty<Gene>())
			{
				return 1f;
			}
			float result;
			if (this.cachedAddictionChanceFactors.TryGetValue(chemical, out result))
			{
				return result;
			}
			float num = 1f;
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active && genesListForReading[i].def.chemical == chemical)
				{
					num *= genesListForReading[i].def.addictionChanceFactor;
				}
			}
			this.cachedAddictionChanceFactors.Add(chemical, num);
			return num;
		}

		// Token: 0x0600881B RID: 34843 RVA: 0x00313E48 File Offset: 0x00312048
		public bool HediffGiversCanGive(HediffDef hediff)
		{
			if (!ModLister.BiotechInstalled)
			{
				return true;
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active && genesListForReading[i].def.hediffGiversCannotGive != null && genesListForReading[i].def.hediffGiversCannotGive.Contains(hediff))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600881C RID: 34844 RVA: 0x00313EB4 File Offset: 0x003120B4
		public SoundDef GetSoundOverrideFromGenes(Func<GeneDef, SoundDef> getter, SoundDef oldSound)
		{
			if (!ModLister.BiotechInstalled)
			{
				return oldSound;
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active)
				{
					SoundDef soundDef = getter(genesListForReading[i].def);
					if (soundDef != null)
					{
						return soundDef;
					}
				}
			}
			return oldSound;
		}

		// Token: 0x0600881D RID: 34845 RVA: 0x00313F0C File Offset: 0x0031210C
		public void InitializeGenesFromOldSave(float melanin)
		{
			if (this.GetMelaninGene() == null)
			{
				this.AddGene(PawnSkinColors.GetSkinColorGene(melanin), false);
			}
			if (this.GetHairColorGene() == null)
			{
				GeneDef geneDef = PawnHairColors.ClosestHairColorGene(this.pawn.story.HairColor, this.pawn.story.SkinColorBase);
				if (geneDef != null)
				{
					this.AddGene(geneDef, false);
				}
			}
		}

		// Token: 0x0600881E RID: 34846 RVA: 0x00313F69 File Offset: 0x00312169
		public IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Pawn_GeneTracker.<SpecialDisplayStats>d__108 <SpecialDisplayStats>d__ = new Pawn_GeneTracker.<SpecialDisplayStats>d__108(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600881F RID: 34847 RVA: 0x00313F7C File Offset: 0x0031217C
		public void ExposeData()
		{
			Scribe_Collections.Look<Gene>(ref this.xenogenes, "xenogenes", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Gene>(ref this.endogenes, "endogenes", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<XenotypeDef>(ref this.xenotype, "xenotype");
			Scribe_Values.Look<string>(ref this.xenotypeName, "xenotypeName", null, false);
			Scribe_Values.Look<bool>(ref this.hybrid, "hybrid", false, false);
			Scribe_Defs.Look<XenotypeIconDef>(ref this.iconDef, "iconDef");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.xenogenes != null)
				{
					if (this.xenogenes.RemoveAll((Gene x) => x == null || x.def == null) > 0)
					{
						goto IL_E2;
					}
				}
				if (this.endogenes == null)
				{
					goto IL_EC;
				}
				if (this.endogenes.RemoveAll((Gene x) => x == null || x.def == null) <= 0)
				{
					goto IL_EC;
				}
				IL_E2:
				Log.Error("Removed null gene(s)");
			}
			IL_EC:
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.xenotype == null && ModsConfig.BiotechActive)
				{
					this.xenotype = XenotypeDefOf.Baseliner;
				}
				if (this.xenogenes == null)
				{
					this.xenogenes = new List<Gene>();
				}
				if (this.endogenes == null)
				{
					this.endogenes = new List<Gene>();
				}
				this.RecacheNeeds();
			}
		}

		// Token: 0x06008820 RID: 34848 RVA: 0x003140C4 File Offset: 0x003122C4
		private void RecacheNeeds()
		{
			Dictionary<NeedDef, Gene> dictionary = this.cachedDisabledNeeds;
			if (dictionary != null)
			{
				dictionary.Clear();
			}
			Dictionary<NeedDef, Gene> dictionary2 = this.cachedEnabledNeeds;
			if (dictionary2 != null)
			{
				dictionary2.Clear();
			}
			List<Gene> genesListForReading = this.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				Gene gene = genesListForReading[i];
				if (gene.Active && !gene.def.enablesNeeds.NullOrEmpty<NeedDef>())
				{
					for (int j = 0; j < gene.def.enablesNeeds.Count; j++)
					{
						NeedDef key = gene.def.enablesNeeds[j];
						if (this.cachedEnabledNeeds == null)
						{
							this.cachedEnabledNeeds = new Dictionary<NeedDef, Gene>();
						}
						this.cachedEnabledNeeds[key] = gene;
					}
				}
				if (gene.Active && !gene.def.disablesNeeds.NullOrEmpty<NeedDef>())
				{
					for (int k = 0; k < gene.def.disablesNeeds.Count; k++)
					{
						NeedDef key2 = gene.def.disablesNeeds[k];
						if (this.cachedDisabledNeeds == null)
						{
							this.cachedDisabledNeeds = new Dictionary<NeedDef, Gene>();
						}
						this.cachedDisabledNeeds[key2] = gene;
					}
				}
			}
		}

		// Token: 0x06008821 RID: 34849 RVA: 0x003141F0 File Offset: 0x003123F0
		public void Debug_AddAllGenes(bool xenogene)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			this.ClearXenogenes();
			this.cachedGenes = null;
			foreach (GeneDef geneDef in DefDatabase<GeneDef>.AllDefs)
			{
				if (geneDef.endogeneCategory != EndogeneCategory.Melanin && geneDef.endogeneCategory != EndogeneCategory.HairColor)
				{
					Gene gene = GeneMaker.MakeGene(geneDef, this.pawn);
					if (xenogene)
					{
						this.xenogenes.Add(gene);
					}
					else
					{
						this.endogenes.Add(gene);
					}
					this.Notify_GenesChanged(geneDef);
					gene.PostAdd();
				}
			}
			this.CheckForOverrides();
		}

		// Token: 0x040053E0 RID: 21472
		public Pawn pawn;

		// Token: 0x040053E1 RID: 21473
		public string xenotypeName;

		// Token: 0x040053E2 RID: 21474
		public XenotypeIconDef iconDef;

		// Token: 0x040053E3 RID: 21475
		public bool hybrid;

		// Token: 0x040053E4 RID: 21476
		private XenotypeDef xenotype;

		// Token: 0x040053E5 RID: 21477
		private List<Gene> xenogenes = new List<Gene>();

		// Token: 0x040053E6 RID: 21478
		private List<Gene> endogenes = new List<Gene>();

		// Token: 0x040053E7 RID: 21479
		[Unsaved(false)]
		private Dictionary<DamageDef, float> cachedDamageFactors = new Dictionary<DamageDef, float>();

		// Token: 0x040053E8 RID: 21480
		[Unsaved(false)]
		private Dictionary<ChemicalDef, float> cachedAddictionChanceFactors = new Dictionary<ChemicalDef, float>();

		// Token: 0x040053E9 RID: 21481
		[Unsaved(false)]
		private Dictionary<NeedDef, Gene> cachedEnabledNeeds;

		// Token: 0x040053EA RID: 21482
		[Unsaved(false)]
		private Dictionary<NeedDef, Gene> cachedDisabledNeeds;

		// Token: 0x040053EB RID: 21483
		[Unsaved(false)]
		private List<Gene> cachedGenes;

		// Token: 0x040053EC RID: 21484
		[Unsaved(false)]
		private bool? cachedGenesAffectAge;

		// Token: 0x040053ED RID: 21485
		[Unsaved(false)]
		private CustomXenotype cachedCustomXenotype;

		// Token: 0x040053EE RID: 21486
		[Unsaved(false)]
		private bool? cachedHasCustomXenotype;

		// Token: 0x040053EF RID: 21487
		[Unsaved(false)]
		private bool? cachedTattoosVisible;

		// Token: 0x040053F0 RID: 21488
		[Unsaved(false)]
		private int? cachedWaterCellCost;

		// Token: 0x040053F1 RID: 21489
		[Unsaved(false)]
		private bool hasCachedWaterCost;

		// Token: 0x040053F2 RID: 21490
		private const int LearningOpportunityCheckInterval = 300;

		// Token: 0x040053F3 RID: 21491
		private List<GeneDefWithType> tmpGeneDefWithTypes = new List<GeneDefWithType>();

		// Token: 0x040053F4 RID: 21492
		private readonly List<Gene> tmpGenes = new List<Gene>();
	}
}
