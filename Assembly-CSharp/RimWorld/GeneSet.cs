using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001B1C RID: 6940
	public class GeneSet : IExposable
	{
		// Token: 0x17001FFE RID: 8190
		// (get) Token: 0x0600B196 RID: 45462 RVA: 0x003E937B File Offset: 0x003E757B
		public string Label
		{
			get
			{
				if (this.name.NullOrEmpty())
				{
					this.GenerateName();
				}
				return this.name;
			}
		}

		// Token: 0x17001FFF RID: 8191
		// (get) Token: 0x0600B197 RID: 45463 RVA: 0x003E9396 File Offset: 0x003E7596
		private string LabelShortAdj
		{
			get
			{
				if (!this.genes.NullOrEmpty<GeneDef>())
				{
					return this.genes[0].LabelShortAdj;
				}
				return "ERR";
			}
		}

		// Token: 0x17002000 RID: 8192
		// (get) Token: 0x0600B198 RID: 45464 RVA: 0x003E93BC File Offset: 0x003E75BC
		public List<GeneDef> GenesListForReading
		{
			get
			{
				return this.genes;
			}
		}

		// Token: 0x17002001 RID: 8193
		// (get) Token: 0x0600B199 RID: 45465 RVA: 0x003E93C4 File Offset: 0x003E75C4
		public bool Empty
		{
			get
			{
				return this.genes.Count == 0;
			}
		}

		// Token: 0x17002002 RID: 8194
		// (get) Token: 0x0600B19A RID: 45466 RVA: 0x003E93D4 File Offset: 0x003E75D4
		public int ComplexityTotal
		{
			get
			{
				this.RecacheOverridesIfNeeded();
				int num = 0;
				for (int i = 0; i < this.cachedNonOverriddenGenes.Count; i++)
				{
					num += this.cachedNonOverriddenGenes[i].biostatCpx;
				}
				return num;
			}
		}

		// Token: 0x17002003 RID: 8195
		// (get) Token: 0x0600B19B RID: 45467 RVA: 0x003E9414 File Offset: 0x003E7614
		public int MetabolismTotal
		{
			get
			{
				this.RecacheOverridesIfNeeded();
				int num = 0;
				for (int i = 0; i < this.cachedNonOverriddenGenes.Count; i++)
				{
					num += this.cachedNonOverriddenGenes[i].biostatMet;
				}
				return num;
			}
		}

		// Token: 0x17002004 RID: 8196
		// (get) Token: 0x0600B19C RID: 45468 RVA: 0x003E9454 File Offset: 0x003E7654
		public int ArchitesTotal
		{
			get
			{
				this.RecacheOverridesIfNeeded();
				int num = 0;
				for (int i = 0; i < this.cachedNonOverriddenGenes.Count; i++)
				{
					num += this.cachedNonOverriddenGenes[i].biostatArc;
				}
				return num;
			}
		}

		// Token: 0x17002005 RID: 8197
		// (get) Token: 0x0600B19D RID: 45469 RVA: 0x003E9494 File Offset: 0x003E7694
		private IEnumerable<DefHyperlink> GeneDefHyperlinks
		{
			get
			{
				GeneSet.<get_GeneDefHyperlinks>d__19 <get_GeneDefHyperlinks>d__ = new GeneSet.<get_GeneDefHyperlinks>d__19(-2);
				<get_GeneDefHyperlinks>d__.<>4__this = this;
				return <get_GeneDefHyperlinks>d__;
			}
		}

		// Token: 0x0600B19E RID: 45470 RVA: 0x003E94A4 File Offset: 0x003E76A4
		public void AddGene(GeneDef gene)
		{
			if (!this.genes.Contains(gene))
			{
				this.genes.Add(gene);
				this.DirtyCache();
			}
		}

		// Token: 0x0600B19F RID: 45471 RVA: 0x003E94C6 File Offset: 0x003E76C6
		public void SetNameDirect(string name)
		{
			this.name = name;
		}

		// Token: 0x0600B1A0 RID: 45472 RVA: 0x003E94CF File Offset: 0x003E76CF
		public bool IsOverridden(GeneDef geneDef)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (!this.genes.Contains(geneDef))
			{
				return false;
			}
			this.RecacheOverridesIfNeeded();
			return !this.cachedNonOverriddenGenes.Contains(geneDef);
		}

		// Token: 0x0600B1A1 RID: 45473 RVA: 0x003E9500 File Offset: 0x003E7700
		public void GenerateName()
		{
			if (ModsConfig.BiotechActive && this.genes.Any<GeneDef>())
			{
				this.name = GrammarResolver.Resolve("r_name", new GrammarRequest
				{
					Includes = 
					{
						RulePackDefOf.NamerGenepack
					},
					Rules = 
					{
						new Rule_String("geneWord", this.LabelShortAdj),
						new Rule_String("geneCountMinusOne", (this.genes.Count - 1).ToString())
					},
					Constants = 
					{
						{
							"geneCount",
							this.genes.Count.ToString()
						}
					}
				}, null, false, null, null, null, false);
				return;
			}
			this.name = "ERR";
		}

		// Token: 0x0600B1A2 RID: 45474 RVA: 0x003E95D0 File Offset: 0x003E77D0
		public bool CanAddGeneDuringGeneration(GeneDef gene)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (this.genes.Contains(gene))
			{
				return false;
			}
			if (this.genes.Count > 0 && !GeneTuning.BiostatRange.Includes(gene.biostatMet + this.MetabolismTotal))
			{
				return false;
			}
			if (!gene.canGenerateInGeneSet)
			{
				return false;
			}
			if (gene.prerequisite != null && !this.genes.Contains(gene.prerequisite))
			{
				return false;
			}
			for (int i = 0; i < this.genes.Count; i++)
			{
				if (gene.ConflictsWith(this.genes[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B1A3 RID: 45475 RVA: 0x003E9676 File Offset: 0x003E7876
		public void SortGenes()
		{
			this.genes.SortBy((GeneDef x) => -x.displayCategory.displayPriorityInGenepack);
		}

		// Token: 0x0600B1A4 RID: 45476 RVA: 0x003E96A4 File Offset: 0x003E78A4
		public override string ToString()
		{
			string str = "GeneSet: ";
			string str2;
			if (!this.genes.NullOrEmpty<GeneDef>())
			{
				str2 = (from x in this.genes
				select x.LabelCap.ToString()).ToCommaList(false, false);
			}
			else
			{
				str2 = "None";
			}
			return str + str2;
		}

		// Token: 0x0600B1A5 RID: 45477 RVA: 0x003E9700 File Offset: 0x003E7900
		public IEnumerable<StatDrawEntry> SpecialDisplayStats(Dialog_InfoCard.Hyperlink? inspectGenesHyperlink = null)
		{
			GeneSet.<SpecialDisplayStats>d__27 <SpecialDisplayStats>d__ = new GeneSet.<SpecialDisplayStats>d__27(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__inspectGenesHyperlink = inspectGenesHyperlink;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600B1A6 RID: 45478 RVA: 0x003E9717 File Offset: 0x003E7917
		public void Debug_RemoveGene(GeneDef gene)
		{
			if (this.genes.Remove(gene))
			{
				this.DirtyCache();
			}
		}

		// Token: 0x0600B1A7 RID: 45479 RVA: 0x003E972D File Offset: 0x003E792D
		private void DirtyCache()
		{
			this.name = null;
			this.cachedNonOverriddenGenes = null;
		}

		// Token: 0x0600B1A8 RID: 45480 RVA: 0x003E9740 File Offset: 0x003E7940
		private void RecacheOverridesIfNeeded()
		{
			if (this.cachedNonOverriddenGenes == null)
			{
				this.cachedNonOverriddenGenes = new List<GeneDef>();
				foreach (GeneDef geneDef in this.genes)
				{
					this.tmpGeneDefWithTypes.Add(new GeneDefWithType(geneDef, true));
				}
				this.cachedNonOverriddenGenes.AddRange(this.tmpGeneDefWithTypes.NonOverriddenGenes());
				this.tmpGeneDefWithTypes.Clear();
			}
		}

		// Token: 0x0600B1A9 RID: 45481 RVA: 0x003E97D4 File Offset: 0x003E79D4
		public GeneSet Copy()
		{
			GeneSet geneSet = new GeneSet();
			geneSet.name = this.name;
			geneSet.genes.AddRange(this.genes);
			geneSet.DirtyCache();
			return geneSet;
		}

		// Token: 0x0600B1AA RID: 45482 RVA: 0x003E9800 File Offset: 0x003E7A00
		public bool Matches(GeneSet other)
		{
			if (this.genes.Count != other.genes.Count)
			{
				return false;
			}
			for (int i = 0; i < this.genes.Count; i++)
			{
				if (!other.genes.Contains(this.genes[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B1AB RID: 45483 RVA: 0x003E985C File Offset: 0x003E7A5C
		public void ExposeData()
		{
			Scribe_Collections.Look<GeneDef>(ref this.genes, "genes", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.genes.RemoveAll((GeneDef x) => x == null);
			}
		}

		// Token: 0x0400669A RID: 26266
		private List<GeneDef> genes = new List<GeneDef>();

		// Token: 0x0400669B RID: 26267
		private string name;

		// Token: 0x0400669C RID: 26268
		[Unsaved(false)]
		private List<GeneDef> cachedNonOverriddenGenes;

		// Token: 0x0400669D RID: 26269
		public const string ErrorSlugline = "ERR";

		// Token: 0x0400669E RID: 26270
		private List<GeneDefWithType> tmpGeneDefWithTypes = new List<GeneDefWithType>();
	}
}
