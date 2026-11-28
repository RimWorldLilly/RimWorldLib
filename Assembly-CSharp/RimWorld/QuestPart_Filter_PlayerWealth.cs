using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001077 RID: 4215
	public class QuestPart_Filter_PlayerWealth : QuestPart_Filter
	{
		// Token: 0x060069A9 RID: 27049 RVA: 0x00253C55 File Offset: 0x00251E55
		protected override bool Pass(SignalArgs args)
		{
			return WealthUtility.PlayerWealth >= this.minPlayerWealth;
		}

		// Token: 0x060069AA RID: 27050 RVA: 0x00253C67 File Offset: 0x00251E67
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.minPlayerWealth, "minPlayerWealth", 0f, false);
		}

		// Token: 0x0400432B RID: 17195
		public float minPlayerWealth;
	}
}
