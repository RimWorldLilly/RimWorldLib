using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200223D RID: 8765
	public class QuestNode_ShuttleLeaveDelay : QuestNode
	{
		// Token: 0x0600D8EB RID: 55531 RVA: 0x00505A90 File Offset: 0x00503C90
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600D8EC RID: 55532 RVA: 0x00505AA8 File Offset: 0x00503CA8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ShuttleLeaveDelay questPart_ShuttleLeaveDelay = new QuestPart_ShuttleLeaveDelay();
			questPart_ShuttleLeaveDelay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ShuttleLeaveDelay.delayTicks = this.delayTicks.GetValue(slate);
			questPart_ShuttleLeaveDelay.shuttle = this.shuttle.GetValue(slate);
			questPart_ShuttleLeaveDelay.expiryInfoPart = "ShuttleDepartsIn".Translate();
			questPart_ShuttleLeaveDelay.expiryInfoPartTip = "ShuttleDepartsOn".Translate();
			if (this.inSignalsDisable.GetValue(slate) != null)
			{
				foreach (string signal in this.inSignalsDisable.GetValue(slate))
				{
					questPart_ShuttleLeaveDelay.inSignalsDisable.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
				}
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_ShuttleLeaveDelay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_ShuttleLeaveDelay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_ShuttleLeaveDelay);
		}

		// Token: 0x04008A8C RID: 35468
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04008A8D RID: 35469
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04008A8E RID: 35470
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignalsDisable;

		// Token: 0x04008A8F RID: 35471
		public SlateRef<int> delayTicks;

		// Token: 0x04008A90 RID: 35472
		public SlateRef<Thing> shuttle;

		// Token: 0x04008A91 RID: 35473
		public QuestNode node;
	}
}
