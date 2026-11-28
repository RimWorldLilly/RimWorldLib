using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002183 RID: 8579
	public class QuestNode_AnySignal : QuestNode
	{
		// Token: 0x0600D658 RID: 54872 RVA: 0x004F9EC8 File Offset: 0x004F80C8
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600D659 RID: 54873 RVA: 0x004F9EE0 File Offset: 0x004F80E0
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
				QuestPart_PassAny questPart_PassAny = new QuestPart_PassAny();
				foreach (string signal in this.inSignals.GetValue(slate))
				{
					questPart_PassAny.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
				}
				if (this.node != null)
				{
					questPart_PassAny.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
					QuestGenUtility.RunInnerNode(this.node, questPart_PassAny.outSignal);
				}
				else
				{
					questPart_PassAny.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignals.GetValue(slate).First<string>());
				}
				questPart_PassAny.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
				QuestGen.quest.AddPart(questPart_PassAny);
				return;
			}
			QuestPart_PassAnyOutMany questPart_PassAnyOutMany = new QuestPart_PassAnyOutMany();
			foreach (string signal2 in this.inSignals.GetValue(slate))
			{
				questPart_PassAnyOutMany.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal2));
			}
			if (this.node != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_PassAnyOutMany.outSignals.Add(text);
				QuestGenUtility.RunInnerNode(this.node, text);
			}
			foreach (string signal3 in this.outSignals.GetValue(slate))
			{
				questPart_PassAnyOutMany.outSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal3));
			}
			questPart_PassAnyOutMany.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
			QuestGen.quest.AddPart(questPart_PassAnyOutMany);
		}

		// Token: 0x0400879A RID: 34714
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x0400879B RID: 34715
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x0400879C RID: 34716
		public QuestNode node;

		// Token: 0x0400879D RID: 34717
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x0400879E RID: 34718
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
