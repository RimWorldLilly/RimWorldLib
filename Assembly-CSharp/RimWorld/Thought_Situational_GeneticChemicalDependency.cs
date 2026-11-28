using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E69 RID: 3689
	public class Thought_Situational_GeneticChemicalDependency : Thought_Situational
	{
		// Token: 0x060061F2 RID: 25074 RVA: 0x0023458C File Offset: 0x0023278C
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			foreach (Gene gene in this.pawn.genes.GenesListForReading)
			{
				Gene_ChemicalDependency gene_ChemicalDependency = gene as Gene_ChemicalDependency;
				if (gene_ChemicalDependency != null && gene_ChemicalDependency.def.chemical == this.def.chemicalDef && gene_ChemicalDependency.LinkedHediff != null)
				{
					return Thought_Situational_GeneticChemicalDependency.MoodOffsetCurve.Evaluate(gene_ChemicalDependency.LinkedHediff.Severity);
				}
			}
			return 0f;
		}

		// Token: 0x0400360C RID: 13836
		private static readonly SimpleCurve MoodOffsetCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, -4f),
				true
			},
			{
				new CurvePoint(6f, -20f),
				true
			}
		};
	}
}
