using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002223 RID: 8739
	public class QuestNode_MoodBelow : QuestNode
	{
		// Token: 0x0600D899 RID: 55449 RVA: 0x005048A4 File Offset: 0x00502AA4
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600D89A RID: 55450 RVA: 0x005048BC File Offset: 0x00502ABC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_MoodBelow questPart_MoodBelow = new QuestPart_MoodBelow();
			questPart_MoodBelow.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_MoodBelow.threshold = this.threshold.GetValue(slate);
			questPart_MoodBelow.minTicksBelowThreshold = 40000;
			questPart_MoodBelow.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_MoodBelow);
			}
			if (!this.outSignal.GetValue(slate).NullOrEmpty())
			{
				questPart_MoodBelow.outSignalsCompleted.Add(this.outSignal.GetValue(slate));
			}
			QuestGen.quest.AddPart(questPart_MoodBelow);
		}

		// Token: 0x04008A37 RID: 35383
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04008A38 RID: 35384
		[NoTranslate]
		public SlateRef<string> outSignal;

		// Token: 0x04008A39 RID: 35385
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04008A3A RID: 35386
		public SlateRef<float> threshold;

		// Token: 0x04008A3B RID: 35387
		public QuestNode node;

		// Token: 0x04008A3C RID: 35388
		private const int MinTicksBelowMinMood = 40000;
	}
}
