using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002211 RID: 8721
	public class QuestNode_FactionGoodwillForMoodChange : QuestNode
	{
		// Token: 0x0600D85A RID: 55386 RVA: 0x00503678 File Offset: 0x00501878
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D85B RID: 55387 RVA: 0x0050367C File Offset: 0x0050187C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) != null)
			{
				QuestPart_FactionGoodwillForMoodChange questPart_FactionGoodwillForMoodChange = new QuestPart_FactionGoodwillForMoodChange();
				questPart_FactionGoodwillForMoodChange.pawns.AddRange(this.pawns.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.outSignalSuccess = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalSuccess.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.outSignalFailed = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalFailed.GetValue(slate));
				questPart_FactionGoodwillForMoodChange.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
				QuestGen.quest.AddPart(questPart_FactionGoodwillForMoodChange);
			}
		}

		// Token: 0x040089CF RID: 35279
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040089D0 RID: 35280
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040089D1 RID: 35281
		[NoTranslate]
		public SlateRef<string> outSignalSuccess;

		// Token: 0x040089D2 RID: 35282
		[NoTranslate]
		public SlateRef<string> outSignalFailed;

		// Token: 0x040089D3 RID: 35283
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
