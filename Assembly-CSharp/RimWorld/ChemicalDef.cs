using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F48 RID: 3912
	public class ChemicalDef : Def
	{
		// Token: 0x060064D4 RID: 25812 RVA: 0x0023FE10 File Offset: 0x0023E010
		public override IEnumerable<string> ConfigErrors()
		{
			ChemicalDef.<ConfigErrors>d__11 <ConfigErrors>d__ = new ChemicalDef.<ConfigErrors>d__11(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003A60 RID: 14944
		public HediffDef addictionHediff;

		// Token: 0x04003A61 RID: 14945
		public HediffDef toleranceHediff;

		// Token: 0x04003A62 RID: 14946
		public bool canBinge = true;

		// Token: 0x04003A63 RID: 14947
		public bool canBeAddicted = true;

		// Token: 0x04003A64 RID: 14948
		public bool generateAddictionGenes = true;

		// Token: 0x04003A65 RID: 14949
		public float geneOverdoseChanceFactorResist = 1f;

		// Token: 0x04003A66 RID: 14950
		public float geneOverdoseChanceFactorImmune = 1f;

		// Token: 0x04003A67 RID: 14951
		public float geneToleranceBuildupFactorResist = 1f;

		// Token: 0x04003A68 RID: 14952
		public float geneToleranceBuildupFactorImmune = 1f;

		// Token: 0x04003A69 RID: 14953
		public float onGeneratedAddictedToleranceChance;

		// Token: 0x04003A6A RID: 14954
		public List<HediffGiver_Event> onGeneratedAddictedEvents;
	}
}
