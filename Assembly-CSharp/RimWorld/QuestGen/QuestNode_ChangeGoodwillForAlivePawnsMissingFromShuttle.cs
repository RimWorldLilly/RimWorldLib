using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002204 RID: 8708
	public class QuestNode_ChangeGoodwillForAlivePawnsMissingFromShuttle : QuestNode
	{
		// Token: 0x0600D832 RID: 55346 RVA: 0x00502B17 File Offset: 0x00500D17
		protected override bool TestRunInt(Slate slate)
		{
			return this.faction.GetValue(slate) != null;
		}

		// Token: 0x0600D833 RID: 55347 RVA: 0x00502B2C File Offset: 0x00500D2C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ChangeGoodwillForAlivePawnsMissingFromShuttle part = new QuestPart_ChangeGoodwillForAlivePawnsMissingFromShuttle
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				pawns = this.pawns.GetValue(slate),
				faction = this.faction.GetValue(slate),
				goodwillChange = this.goodwillChange.GetValue(slate),
				historyEvent = this.reason.GetValue(slate)
			};
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x04008991 RID: 35217
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008992 RID: 35218
		public SlateRef<List<Pawn>> pawns;

		// Token: 0x04008993 RID: 35219
		public SlateRef<Faction> faction;

		// Token: 0x04008994 RID: 35220
		public SlateRef<int> goodwillChange;

		// Token: 0x04008995 RID: 35221
		public SlateRef<HistoryEventDef> reason;
	}
}
