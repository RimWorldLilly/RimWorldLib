using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D9 RID: 4313
	public class QuestPart_PlayerWealth : QuestPartActivable
	{
		// Token: 0x06006BDE RID: 27614 RVA: 0x0025DC5E File Offset: 0x0025BE5E
		public override void QuestPartTick()
		{
			if (Find.TickManager.TicksGame % 60 == 0 && WealthUtility.PlayerWealth >= this.playerWealth)
			{
				base.Complete();
			}
		}

		// Token: 0x06006BDF RID: 27615 RVA: 0x0025DC82 File Offset: 0x0025BE82
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.playerWealth, "playerWealth", 0f, false);
		}

		// Token: 0x04004498 RID: 17560
		public float playerWealth = 100000f;

		// Token: 0x04004499 RID: 17561
		public const int CheckInterval = 60;
	}
}
