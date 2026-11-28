using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200223B RID: 8763
	public class QuestNode_SetTicksUntilAcceptanceExpiry : QuestNode
	{
		// Token: 0x0600D8E5 RID: 55525 RVA: 0x0050593E File Offset: 0x00503B3E
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8E6 RID: 55526 RVA: 0x00505944 File Offset: 0x00503B44
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.acceptanceExpireTick = GenTicks.TicksGame + this.ticks.GetValue(slate);
		}

		// Token: 0x04008A86 RID: 35462
		public SlateRef<int> ticks;
	}
}
