using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010DC RID: 4316
	public class QuestPart_RecordHistoryEvent : QuestPart
	{
		// Token: 0x06006BE7 RID: 27623 RVA: 0x0025DDDA File Offset: 0x0025BFDA
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(this.historyEvent), true);
			}
		}

		// Token: 0x06006BE8 RID: 27624 RVA: 0x0025DE0C File Offset: 0x0025C00C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<HistoryEventDef>(ref this.historyEvent, "historyEvent");
		}

		// Token: 0x040044A1 RID: 17569
		public string inSignal;

		// Token: 0x040044A2 RID: 17570
		public HistoryEventDef historyEvent;
	}
}
