using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200222A RID: 8746
	public class QuestNode_RecordHistoryEvent : QuestNode
	{
		// Token: 0x0600D8B0 RID: 55472 RVA: 0x00505198 File Offset: 0x00503398
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8B1 RID: 55473 RVA: 0x0050519C File Offset: 0x0050339C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			HistoryEventDef value = this.historyDef.GetValue(slate);
			if (value == null)
			{
				return;
			}
			quest.AddPart(new QuestPart_RecordHistoryEvent
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false)),
				historyEvent = value
			});
		}

		// Token: 0x04008A60 RID: 35424
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008A61 RID: 35425
		public SlateRef<HistoryEventDef> historyDef;
	}
}
