using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002219 RID: 8729
	public class QuestNode_GuardianShipDelay : QuestNode
	{
		// Token: 0x0600D87B RID: 55419 RVA: 0x00503F74 File Offset: 0x00502174
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600D87C RID: 55420 RVA: 0x00503F8C File Offset: 0x0050218C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_GuardianShipDelay questPart_GuardianShipDelay = new QuestPart_GuardianShipDelay();
			questPart_GuardianShipDelay.pawn = this.pawn.GetValue(slate);
			questPart_GuardianShipDelay.delayTicks = this.delayTicks.GetValue(slate);
			questPart_GuardianShipDelay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_GuardianShipDelay.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_GuardianShipDelay.reactivatable = true;
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_GuardianShipDelay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_GuardianShipDelay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_GuardianShipDelay);
		}

		// Token: 0x04008A01 RID: 35329
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04008A02 RID: 35330
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04008A03 RID: 35331
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04008A04 RID: 35332
		public SlateRef<Pawn> pawn;

		// Token: 0x04008A05 RID: 35333
		public SlateRef<int> delayTicks;

		// Token: 0x04008A06 RID: 35334
		public QuestNode node;
	}
}
