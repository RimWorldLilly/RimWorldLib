using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002189 RID: 8585
	public class QuestNode_End : QuestNode
	{
		// Token: 0x0600D66C RID: 54892 RVA: 0x004FA6D4 File Offset: 0x004F88D4
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D66D RID: 54893 RVA: 0x004FA6D8 File Offset: 0x004F88D8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int value = this.goodwillChangeAmount.GetValue(slate);
			Thing value2 = this.goodwillChangeFactionOf.GetValue(slate);
			if (value != 0 && value2 != null && value2.Faction != null)
			{
				QuestPart_FactionGoodwillChange questPart_FactionGoodwillChange = new QuestPart_FactionGoodwillChange();
				questPart_FactionGoodwillChange.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
				questPart_FactionGoodwillChange.faction = value2.Faction;
				questPart_FactionGoodwillChange.change = value;
				questPart_FactionGoodwillChange.historyEvent = this.goodwillChangeReason.GetValue(slate);
				slate.Set<string>("goodwillPenalty", Mathf.Abs(value).ToString(), false);
				QuestGen.quest.AddPart(questPart_FactionGoodwillChange);
			}
			QuestPart_QuestEnd questPart_QuestEnd = new QuestPart_QuestEnd();
			questPart_QuestEnd.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_QuestEnd.outcome = new QuestEndOutcome?(this.outcome.GetValue(slate));
			questPart_QuestEnd.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
			questPart_QuestEnd.sendLetter = this.sendStandardLetter.GetValue(slate).GetValueOrDefault();
			QuestGen.quest.AddPart(questPart_QuestEnd);
		}

		// Token: 0x040087BA RID: 34746
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040087BB RID: 34747
		public SlateRef<QuestEndOutcome> outcome;

		// Token: 0x040087BC RID: 34748
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x040087BD RID: 34749
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x040087BE RID: 34750
		public SlateRef<int> goodwillChangeAmount;

		// Token: 0x040087BF RID: 34751
		public SlateRef<Thing> goodwillChangeFactionOf;

		// Token: 0x040087C0 RID: 34752
		public SlateRef<HistoryEventDef> goodwillChangeReason;
	}
}
