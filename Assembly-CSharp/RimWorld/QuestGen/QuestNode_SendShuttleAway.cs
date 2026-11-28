using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002233 RID: 8755
	public class QuestNode_SendShuttleAway : QuestNode
	{
		// Token: 0x0600D8CB RID: 55499 RVA: 0x005055DE File Offset: 0x005037DE
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8CC RID: 55500 RVA: 0x005055E4 File Offset: 0x005037E4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.shuttle.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_SendShuttleAway questPart_SendShuttleAway = new QuestPart_SendShuttleAway();
			questPart_SendShuttleAway.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SendShuttleAway.shuttle = this.shuttle.GetValue(slate);
			questPart_SendShuttleAway.dropEverything = this.dropEverything.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SendShuttleAway);
		}

		// Token: 0x04008A74 RID: 35444
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008A75 RID: 35445
		public SlateRef<Thing> shuttle;

		// Token: 0x04008A76 RID: 35446
		public SlateRef<bool> dropEverything;
	}
}
