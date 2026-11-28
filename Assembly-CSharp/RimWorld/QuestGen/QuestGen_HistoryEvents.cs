using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200215E RID: 8542
	public static class QuestGen_HistoryEvents
	{
		// Token: 0x0600D5A2 RID: 54690 RVA: 0x004F4724 File Offset: 0x004F2924
		public static QuestPart_RecordHistoryEvent RecordHistoryEvent(this Quest quest, HistoryEventDef def, string inSignal = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_RecordHistoryEvent questPart_RecordHistoryEvent = new QuestPart_RecordHistoryEvent();
			questPart_RecordHistoryEvent.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_RecordHistoryEvent.historyEvent = def;
			questPart_RecordHistoryEvent.signalListenMode = signalListenMode;
			quest.AddPart(questPart_RecordHistoryEvent);
			return questPart_RecordHistoryEvent;
		}
	}
}
