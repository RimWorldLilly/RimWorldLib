using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200223C RID: 8764
	public class QuestNode_ShuttleDelay : QuestNode
	{
		// Token: 0x0600D8E8 RID: 55528 RVA: 0x0050597B File Offset: 0x00503B7B
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600D8E9 RID: 55529 RVA: 0x00505994 File Offset: 0x00503B94
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ShuttleDelay questPart_ShuttleDelay = new QuestPart_ShuttleDelay();
			questPart_ShuttleDelay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ShuttleDelay.delayTicks = this.delayTicks.GetValue(slate);
			if (this.lodgers.GetValue(slate) != null)
			{
				questPart_ShuttleDelay.lodgers.AddRange(this.lodgers.GetValue(slate));
			}
			questPart_ShuttleDelay.expiryInfoPart = "ShuttleArrivesIn".Translate();
			questPart_ShuttleDelay.expiryInfoPartTip = "ShuttleArrivesOn".Translate();
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_ShuttleDelay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_ShuttleDelay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_ShuttleDelay);
		}

		// Token: 0x04008A87 RID: 35463
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04008A88 RID: 35464
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04008A89 RID: 35465
		public SlateRef<int> delayTicks;

		// Token: 0x04008A8A RID: 35466
		public SlateRef<IEnumerable<Pawn>> lodgers;

		// Token: 0x04008A8B RID: 35467
		public QuestNode node;
	}
}
