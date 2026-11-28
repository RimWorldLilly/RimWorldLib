using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004C1 RID: 1217
	public class Hediff_PsychicBondTorn : HediffWithTarget
	{
		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x060021EE RID: 8686 RVA: 0x000C9A31 File Offset: 0x000C7C31
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

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060021EF RID: 8687 RVA: 0x000C9A5A File Offset: 0x000C7C5A
		public override bool Visible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x000C9A5D File Offset: 0x000C7C5D
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.creationTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x000C9A80 File Offset: 0x000C7C80
		public override void Notify_Resurrected()
		{
			bool flag = false;
			Pawn pawn = this.target as Pawn;
			if (pawn != null && !pawn.Dead && !pawn.Destroyed && this.creationTick >= 0 && Find.TickManager.TicksGame - this.creationTick <= ThoughtDefOf.PsychicBondTorn.DurationTicks)
			{
				Pawn_GeneTracker genes = this.pawn.genes;
				Gene_PsychicBonding gene_PsychicBonding = (genes != null) ? genes.GetFirstGeneOfType<Gene_PsychicBonding>() : null;
				if (gene_PsychicBonding != null)
				{
					gene_PsychicBonding.BondTo(pawn);
				}
				else
				{
					Pawn_GeneTracker genes2 = pawn.genes;
					if (genes2 != null)
					{
						Gene_PsychicBonding firstGeneOfType = genes2.GetFirstGeneOfType<Gene_PsychicBonding>();
						if (firstGeneOfType != null)
						{
							firstGeneOfType.BondTo(this.pawn);
						}
					}
				}
				flag = true;
			}
			if (!flag)
			{
				this.pawn.health.RemoveHediff(this);
			}
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x000C9B30 File Offset: 0x000C7D30
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.creationTick, "creationTick", -1, false);
		}

		// Token: 0x040019A0 RID: 6560
		private int creationTick = -1;
	}
}
