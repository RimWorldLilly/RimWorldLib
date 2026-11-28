using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002182 RID: 8578
	public class QuestNode_AllSignalsActivable : QuestNode
	{
		// Token: 0x0600D655 RID: 54869 RVA: 0x004F9D3C File Offset: 0x004F7F3C
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600D656 RID: 54870 RVA: 0x004F9D54 File Offset: 0x004F7F54
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (!((this.outSignals.GetValue(slate) != null && this.outSignals.GetValue(slate).Count<string>() != 0) + (this.node != null)))
			{
				return;
			}
			QuestPart_PassAllActivable questPart_PassAllActivable = new QuestPart_PassAllActivable();
			QuestGen.quest.AddPart(questPart_PassAllActivable);
			questPart_PassAllActivable.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_PassAllActivable.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			foreach (string signal in this.inSignals.GetValue(slate))
			{
				questPart_PassAllActivable.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PassAllActivable.OutSignalCompleted);
			}
			IEnumerable<string> value = this.outSignals.GetValue(slate);
			if (value != null)
			{
				foreach (string signal2 in value)
				{
					questPart_PassAllActivable.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal2));
				}
			}
			questPart_PassAllActivable.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
		}

		// Token: 0x04008794 RID: 34708
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04008795 RID: 34709
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04008796 RID: 34710
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x04008797 RID: 34711
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x04008798 RID: 34712
		public QuestNode node;

		// Token: 0x04008799 RID: 34713
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
