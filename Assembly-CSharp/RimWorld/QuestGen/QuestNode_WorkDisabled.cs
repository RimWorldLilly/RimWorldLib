using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002244 RID: 8772
	public class QuestNode_WorkDisabled : QuestNode
	{
		// Token: 0x0600D901 RID: 55553 RVA: 0x00506324 File Offset: 0x00504524
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_WorkDisabled questPart_WorkDisabled = new QuestPart_WorkDisabled();
			questPart_WorkDisabled.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_WorkDisabled.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_WorkDisabled.disabledWorkTags = this.disabledWorkTags.GetValue(slate);
			QuestGen.quest.AddPart(questPart_WorkDisabled);
		}

		// Token: 0x0600D902 RID: 55554 RVA: 0x005063AC File Offset: 0x005045AC
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04008AB0 RID: 35504
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04008AB1 RID: 35505
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04008AB2 RID: 35506
		public SlateRef<WorkTags> disabledWorkTags;
	}
}
