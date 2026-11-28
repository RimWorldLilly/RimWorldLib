using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004C0 RID: 1216
	public class Hediff_PsychicBond : HediffWithTarget
	{
		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x060021E9 RID: 8681 RVA: 0x000C994F File Offset: 0x000C7B4F
		public override string LabelBase
		{
			get
			{
				string labelBase = base.LabelBase;
				string str = " (";
				Thing target = this.target;
				return labelBase + str + ((target != null) ? target.LabelShortCap : null) + ")";
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x060021EA RID: 8682 RVA: 0x000C9978 File Offset: 0x000C7B78
		public override bool ShouldRemove
		{
			get
			{
				return base.ShouldRemove || this.pawn.Dead;
			}
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x000C9990 File Offset: 0x000C7B90
		public override void PostRemoved()
		{
			Pawn_GeneTracker genes = this.pawn.genes;
			Gene_PsychicBonding gene_PsychicBonding = (genes != null) ? genes.GetFirstGeneOfType<Gene_PsychicBonding>() : null;
			if (gene_PsychicBonding != null)
			{
				gene_PsychicBonding.RemoveBond();
				return;
			}
			if (this.target != null)
			{
				Pawn pawn = this.target as Pawn;
				if (pawn != null)
				{
					Pawn_GeneTracker genes2 = pawn.genes;
					if (genes2 == null)
					{
						return;
					}
					Gene_PsychicBonding firstGeneOfType = genes2.GetFirstGeneOfType<Gene_PsychicBonding>();
					if (firstGeneOfType == null)
					{
						return;
					}
					firstGeneOfType.RemoveBond();
				}
			}
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x000C99F0 File Offset: 0x000C7BF0
		public override void PostTickInterval(int delta)
		{
			base.PostTickInterval(delta);
			if (this.pawn.IsHashIntervalTick(65, delta))
			{
				this.Severity = (ThoughtWorker_PsychicBondProximity.NearPsychicBondedPerson(this.pawn, this) ? 0.5f : 1.5f);
			}
		}

		// Token: 0x0400199F RID: 6559
		private const int HediffCheckInterval = 65;
	}
}
