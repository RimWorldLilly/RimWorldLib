using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023A5 RID: 9125
	public class Caravan_BabyTracker
	{
		// Token: 0x170027E5 RID: 10213
		// (get) Token: 0x0600E308 RID: 58120 RVA: 0x005472CD File Offset: 0x005454CD
		private float IdeoExposurePerCaravanIdeoMember
		{
			get
			{
				return 0.003f / (float)this.ideosForExposure.Count;
			}
		}

		// Token: 0x0600E309 RID: 58121 RVA: 0x005472E1 File Offset: 0x005454E1
		public Caravan_BabyTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600E30A RID: 58122 RVA: 0x00547308 File Offset: 0x00545508
		public void TickInterval(int delta)
		{
			if (!ModsConfig.IdeologyActive || !this.caravan.IsHashIntervalTick(30, delta))
			{
				return;
			}
			bool flag = false;
			foreach (Pawn pawn in this.babies)
			{
				if (pawn.Dead || !pawn.DevelopmentalStage.Baby())
				{
					flag = true;
				}
				else if (pawn.ideo != null)
				{
					float ideoExposurePerCaravanIdeoMember = this.IdeoExposurePerCaravanIdeoMember;
					foreach (Ideo ideo in this.ideosForExposure)
					{
						pawn.ideo.IncreaseIdeoExposureIfBaby(ideo, ideoExposurePerCaravanIdeoMember);
					}
				}
			}
			if (flag)
			{
				this.Recache();
			}
		}

		// Token: 0x0600E30B RID: 58123 RVA: 0x005473EC File Offset: 0x005455EC
		public void Recache()
		{
			this.RebuildBabyList();
			this.RebuildPawnIdeos();
		}

		// Token: 0x0600E30C RID: 58124 RVA: 0x005473FC File Offset: 0x005455FC
		private void RebuildBabyList()
		{
			this.babies.Clear();
			foreach (Pawn pawn in this.caravan.PawnsListForReading)
			{
				if (pawn.DevelopmentalStage.Baby())
				{
					this.babies.Add(pawn);
				}
			}
		}

		// Token: 0x0600E30D RID: 58125 RVA: 0x00547474 File Offset: 0x00545674
		private void RebuildPawnIdeos()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			this.ideosForExposure.Clear();
			foreach (Pawn pawn in this.caravan.PawnsListForReading)
			{
				if (pawn.Ideo != null)
				{
					this.ideosForExposure.Add(pawn.Ideo);
				}
			}
		}

		// Token: 0x040090E1 RID: 37089
		private readonly Caravan caravan;

		// Token: 0x040090E2 RID: 37090
		private readonly List<Pawn> babies = new List<Pawn>();

		// Token: 0x040090E3 RID: 37091
		private readonly List<Ideo> ideosForExposure = new List<Ideo>();

		// Token: 0x040090E4 RID: 37092
		private const int IdeoExposureIntervalTicks = 30;
	}
}
