using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200207A RID: 8314
	public static class GeneMaker
	{
		// Token: 0x0600D178 RID: 53624 RVA: 0x004D0222 File Offset: 0x004CE422
		public static Gene MakeGene(GeneDef def, Pawn pawn)
		{
			Gene gene = (Gene)Activator.CreateInstance(def.geneClass);
			gene.def = def;
			gene.pawn = pawn;
			gene.loadID = Find.UniqueIDsManager.GetNextGeneID();
			gene.PostMake();
			return gene;
		}
	}
}
