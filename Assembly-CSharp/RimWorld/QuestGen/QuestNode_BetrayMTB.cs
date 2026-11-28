using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002200 RID: 8704
	public class QuestNode_BetrayMTB : QuestNode
	{
		// Token: 0x0600D826 RID: 55334 RVA: 0x005028E5 File Offset: 0x00500AE5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D827 RID: 55335 RVA: 0x005028E8 File Offset: 0x00500AE8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate).EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_BetrayMTB questPart_BetrayMTB = new QuestPart_BetrayMTB();
			questPart_BetrayMTB.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_BetrayMTB.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_BetrayMTB.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignal.GetValue(slate)));
			questPart_BetrayMTB.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_BetrayMTB);
		}

		// Token: 0x04008983 RID: 35203
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04008984 RID: 35204
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04008985 RID: 35205
		[NoTranslate]
		public SlateRef<string> outSignal;

		// Token: 0x04008986 RID: 35206
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
