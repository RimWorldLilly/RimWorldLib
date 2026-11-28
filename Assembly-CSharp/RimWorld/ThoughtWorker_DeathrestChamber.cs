using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E85 RID: 3717
	public class ThoughtWorker_DeathrestChamber : ThoughtWorker
	{
		// Token: 0x0600624D RID: 25165 RVA: 0x0023567C File Offset: 0x0023387C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Pawn_GeneTracker genes = p.genes;
			Gene_Deathrest gene_Deathrest = (genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null;
			if (gene_Deathrest == null || gene_Deathrest.chamberThoughtIndex < 0)
			{
				return false;
			}
			if (p.Deathresting)
			{
				return false;
			}
			int chamberThoughtIndex = gene_Deathrest.chamberThoughtIndex;
			if (chamberThoughtIndex == 2 || chamberThoughtIndex == 3)
			{
				return false;
			}
			return ThoughtState.ActiveAtStage(chamberThoughtIndex);
		}

		// Token: 0x04003621 RID: 13857
		private const int DullIndex = 2;

		// Token: 0x04003622 RID: 13858
		private const int MediocreIndex = 3;
	}
}
