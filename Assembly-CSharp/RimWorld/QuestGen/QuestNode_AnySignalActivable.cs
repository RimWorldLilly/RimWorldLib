using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002184 RID: 8580
	public class QuestNode_AnySignalActivable : QuestNode
	{
		// Token: 0x0600D65B RID: 54875 RVA: 0x004FA0F8 File Offset: 0x004F82F8
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600D65C RID: 54876 RVA: 0x004FA110 File Offset: 0x004F8310
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (!((this.outSignals.GetValue(slate) != null && this.outSignals.GetValue(slate).Count<string>() != 0) + (this.node != null)))
			{
				return;
			}
			QuestPart_PassAnyActivable questPart_PassAnyActivable = new QuestPart_PassAnyActivable();
			QuestGen.quest.AddPart(questPart_PassAnyActivable);
			questPart_PassAnyActivable.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_PassAnyActivable.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			foreach (string signal in this.inSignals.GetValue(slate))
			{
				questPart_PassAnyActivable.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PassAnyActivable.OutSignalCompleted);
			}
			IEnumerable<string> value = this.outSignals.GetValue(slate);
			if (value != null)
			{
				foreach (string signal2 in value)
				{
					questPart_PassAnyActivable.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal2));
				}
			}
			questPart_PassAnyActivable.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
		}

		// Token: 0x0400879F RID: 34719
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040087A0 RID: 34720
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x040087A1 RID: 34721
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x040087A2 RID: 34722
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x040087A3 RID: 34723
		public QuestNode node;

		// Token: 0x040087A4 RID: 34724
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
