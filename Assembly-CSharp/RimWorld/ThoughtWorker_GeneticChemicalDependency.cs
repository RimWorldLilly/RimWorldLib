using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E68 RID: 3688
	public class ThoughtWorker_GeneticChemicalDependency : ThoughtWorker
	{
		// Token: 0x060061EE RID: 25070 RVA: 0x00234483 File Offset: 0x00232683
		public override string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(this.def.chemicalDef.Named("CHEMICAL"));
		}

		// Token: 0x060061EF RID: 25071 RVA: 0x002344A5 File Offset: 0x002326A5
		public override string PostProcessDescription(Pawn p, string description)
		{
			return base.PostProcessDescription(p, description.Formatted(this.def.chemicalDef.Named("CHEMICAL")));
		}

		// Token: 0x060061F0 RID: 25072 RVA: 0x002344D0 File Offset: 0x002326D0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive || p.genes == null)
			{
				return ThoughtState.Inactive;
			}
			foreach (Gene gene in p.genes.GenesListForReading)
			{
				Gene_ChemicalDependency gene_ChemicalDependency = gene as Gene_ChemicalDependency;
				if (gene_ChemicalDependency != null && gene_ChemicalDependency.def.chemical == this.def.chemicalDef && gene_ChemicalDependency.LinkedHediff != null)
				{
					return gene_ChemicalDependency.LinkedHediff.Severity >= 1f;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
