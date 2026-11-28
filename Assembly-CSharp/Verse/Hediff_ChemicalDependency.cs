using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020004A5 RID: 1189
	public class Hediff_ChemicalDependency : HediffWithComps
	{
		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060020E4 RID: 8420 RVA: 0x000C4806 File Offset: 0x000C2A06
		public override string LabelBase
		{
			get
			{
				return "ChemicalDependency".Translate(this.chemical.Named("CHEMICAL"));
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060020E5 RID: 8421 RVA: 0x000C4827 File Offset: 0x000C2A27
		public override bool ShouldRemove
		{
			get
			{
				return this.LinkedGene == null || !this.LinkedGene.Active;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060020E6 RID: 8422 RVA: 0x000C4841 File Offset: 0x000C2A41
		public bool ShouldSatify
		{
			get
			{
				return this.Severity >= this.def.stages[2].minSeverity - 0.1f;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x060020E7 RID: 8423 RVA: 0x000C486C File Offset: 0x000C2A6C
		public Gene_ChemicalDependency LinkedGene
		{
			get
			{
				if (this.cachedDependencyGene == null && this.pawn.genes != null)
				{
					List<Gene> genesListForReading = this.pawn.genes.GenesListForReading;
					for (int i = 0; i < genesListForReading.Count; i++)
					{
						Gene_ChemicalDependency gene_ChemicalDependency = genesListForReading[i] as Gene_ChemicalDependency;
						if (gene_ChemicalDependency != null && gene_ChemicalDependency.def.chemical == this.chemical)
						{
							this.cachedDependencyGene = gene_ChemicalDependency;
							break;
						}
					}
				}
				return this.cachedDependencyGene;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060020E8 RID: 8424 RVA: 0x000C48E2 File Offset: 0x000C2AE2
		// (set) Token: 0x060020E9 RID: 8425 RVA: 0x000C490B File Offset: 0x000C2B0B
		public override float Severity
		{
			get
			{
				if (this.LinkedGene == null || !this.LinkedGene.Active)
				{
					return this.def.initialSeverity;
				}
				return base.Severity;
			}
			set
			{
				base.Severity = value;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060020EA RID: 8426 RVA: 0x000C4914 File Offset: 0x000C2B14
		public override string TipStringExtra
		{
			get
			{
				string text = base.TipStringExtra;
				Gene_ChemicalDependency linkedGene = this.LinkedGene;
				if (linkedGene != null)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n\n";
					}
					text += "GeneDefChemicalNeedDurationDesc".Translate(this.chemical.label, this.pawn.Named("PAWN"), "PeriodDays".Translate(5f).Named("DEFICIENCYDURATION"), "PeriodDays".Translate(30f).Named("COMADURATION"), "PeriodDays".Translate(60f).Named("DEATHDURATION")).Resolve();
					text = text + "\n\n" + "LastIngestedDurationAgo".Translate(this.chemical.Named("CHEMICAL"), (Find.TickManager.TicksGame - linkedGene.lastIngestedTick).ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION")).Resolve();
				}
				return text;
			}
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000C4A40 File Offset: 0x000C2C40
		public override bool TryMergeWith(Hediff other)
		{
			Hediff_ChemicalDependency hediff_ChemicalDependency = other as Hediff_ChemicalDependency;
			return hediff_ChemicalDependency != null && hediff_ChemicalDependency.chemical == this.chemical && base.TryMergeWith(other);
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x000C4A70 File Offset: 0x000C2C70
		public override void CopyFrom(Hediff other)
		{
			base.CopyFrom(other);
			Hediff_ChemicalDependency hediff_ChemicalDependency = other as Hediff_ChemicalDependency;
			if (hediff_ChemicalDependency != null)
			{
				this.chemical = hediff_ChemicalDependency.chemical;
			}
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x000C4A9A File Offset: 0x000C2C9A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ChemicalDef>(ref this.chemical, "chemical");
		}

		// Token: 0x0400193B RID: 6459
		public ChemicalDef chemical;

		// Token: 0x0400193C RID: 6460
		[Unsaved(false)]
		private Gene_ChemicalDependency cachedDependencyGene;
	}
}
