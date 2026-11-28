using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200222E RID: 8750
	public class QuestNode_ReplaceLostLeaderReferences : QuestNode
	{
		// Token: 0x0600D8BC RID: 55484 RVA: 0x005053A3 File Offset: 0x005035A3
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8BD RID: 55485 RVA: 0x005053A8 File Offset: 0x005035A8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ReplaceLostLeaderReferences questPart_ReplaceLostLeaderReferences = new QuestPart_ReplaceLostLeaderReferences();
			questPart_ReplaceLostLeaderReferences.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
			questPart_ReplaceLostLeaderReferences.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
			QuestGen.quest.AddPart(questPart_ReplaceLostLeaderReferences);
		}

		// Token: 0x04008A6B RID: 35435
		public SlateRef<string> inSignal;

		// Token: 0x04008A6C RID: 35436
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
