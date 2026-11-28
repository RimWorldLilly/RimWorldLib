using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001613 RID: 5651
	public class PredatorThreat : IExposable
	{
		// Token: 0x17001864 RID: 6244
		// (get) Token: 0x06008CCC RID: 36044 RVA: 0x0033280E File Offset: 0x00330A0E
		public bool Expired
		{
			get
			{
				return !this.predator.Spawned || Find.TickManager.TicksGame >= this.lastAttackTicks + 600;
			}
		}

		// Token: 0x06008CCD RID: 36045 RVA: 0x0033283A File Offset: 0x00330A3A
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.predator, "predator", false);
			Scribe_Values.Look<int>(ref this.lastAttackTicks, "lastAttackTicks", 0, false);
		}

		// Token: 0x04005644 RID: 22084
		public Pawn predator;

		// Token: 0x04005645 RID: 22085
		public int lastAttackTicks;

		// Token: 0x04005646 RID: 22086
		private const int ExpireAfterTicks = 600;
	}
}
