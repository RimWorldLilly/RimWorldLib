using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002234 RID: 8756
	public class QuestNode_SendShuttleAwayOnCleanup : QuestNode
	{
		// Token: 0x0600D8CE RID: 55502 RVA: 0x0050566F File Offset: 0x0050386F
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8CF RID: 55503 RVA: 0x00505674 File Offset: 0x00503874
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.shuttle.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_SendShuttleAwayOnCleanup questPart_SendShuttleAwayOnCleanup = new QuestPart_SendShuttleAwayOnCleanup();
			questPart_SendShuttleAwayOnCleanup.shuttle = this.shuttle.GetValue(slate);
			questPart_SendShuttleAwayOnCleanup.dropEverything = this.dropEverything.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SendShuttleAwayOnCleanup);
		}

		// Token: 0x04008A77 RID: 35447
		public SlateRef<Thing> shuttle;

		// Token: 0x04008A78 RID: 35448
		public SlateRef<bool> dropEverything;
	}
}
