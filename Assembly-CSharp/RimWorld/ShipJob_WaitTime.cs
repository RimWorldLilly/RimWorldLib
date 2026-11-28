using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D36 RID: 3382
	public class ShipJob_WaitTime : ShipJob_Wait
	{
		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x06005ED7 RID: 24279 RVA: 0x0022AD8A File Offset: 0x00228F8A
		protected override bool ShouldEnd
		{
			get
			{
				return this.startTick >= 0 && Find.TickManager.TicksGame >= this.startTick + this.duration;
			}
		}

		// Token: 0x06005ED8 RID: 24280 RVA: 0x0022ADB3 File Offset: 0x00228FB3
		public override bool TryStart()
		{
			if (!this.transportShip.ShipExistsAndIsSpawned)
			{
				return false;
			}
			if (!base.TryStart())
			{
				return false;
			}
			if (this.startTick < 0)
			{
				this.startTick = Find.TickManager.TicksGame;
			}
			return true;
		}

		// Token: 0x06005ED9 RID: 24281 RVA: 0x0022ADE8 File Offset: 0x00228FE8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		// Token: 0x040035B4 RID: 13748
		public int duration;

		// Token: 0x040035B5 RID: 13749
		private int startTick = -1;
	}
}
