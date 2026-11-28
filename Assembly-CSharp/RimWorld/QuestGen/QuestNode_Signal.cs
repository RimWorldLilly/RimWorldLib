using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002192 RID: 8594
	public class QuestNode_Signal : QuestNode
	{
		// Token: 0x0600D688 RID: 54920 RVA: 0x004FAE70 File Offset: 0x004F9070
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600D689 RID: 54921 RVA: 0x004FAE88 File Offset: 0x004F9088
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int num = ((this.outSignals.GetValue(slate) != null) ? this.outSignals.GetValue(slate).Count<string>() : 0) + ((this.node != null) ? 1 : 0);
			if (num == 0)
			{
				return;
			}
			if (num == 1)
			{
				QuestPart_Pass questPart_Pass = new QuestPart_Pass();
				questPart_Pass.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
				questPart_Pass.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
				if (this.node != null)
				{
					questPart_Pass.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
					QuestGenUtility.RunInnerNode(this.node, questPart_Pass.outSignal);
				}
				else
				{
					questPart_Pass.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignals.GetValue(slate).First<string>());
				}
				questPart_Pass.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
				QuestGen.quest.AddPart(questPart_Pass);
				return;
			}
			QuestPart_PassOutMany questPart_PassOutMany = new QuestPart_PassOutMany();
			questPart_PassOutMany.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
			questPart_PassOutMany.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			if (this.node != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_PassOutMany.outSignals.Add(text);
				QuestGenUtility.RunInnerNode(this.node, text);
			}
			foreach (string signal in this.outSignals.GetValue(slate))
			{
				questPart_PassOutMany.outSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			questPart_PassOutMany.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
			QuestGen.quest.AddPart(questPart_PassOutMany);
		}

		// Token: 0x040087D6 RID: 34774
		[NoTranslate]
		[TranslationHandle(Priority = 100)]
		public SlateRef<string> inSignal;

		// Token: 0x040087D7 RID: 34775
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x040087D8 RID: 34776
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x040087D9 RID: 34777
		public QuestNode node;

		// Token: 0x040087DA RID: 34778
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x040087DB RID: 34779
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
