using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200218E RID: 8590
	public class QuestNode_SendSignals : QuestNode
	{
		// Token: 0x0600D67B RID: 54907 RVA: 0x004FAB21 File Offset: 0x004F8D21
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D67C RID: 54908 RVA: 0x004FAB24 File Offset: 0x004F8D24
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<string> enumerable = Enumerable.Empty<string>();
			if (this.outSignals.GetValue(slate) != null)
			{
				enumerable = enumerable.Concat(this.outSignals.GetValue(slate));
			}
			if (this.outSignalsFormattedCount.GetValue(slate) > 0)
			{
				for (int i = 0; i < this.outSignalsFormattedCount.GetValue(slate); i++)
				{
					enumerable = enumerable.Concat(Gen.YieldSingle<string>(this.outSignalsFormat.GetValue(slate).Formatted(i.Named("INDEX")).ToString()));
				}
			}
			if (enumerable.EnumerableNullOrEmpty<string>())
			{
				return;
			}
			if (enumerable.Count<string>() == 1)
			{
				QuestPart_Pass questPart_Pass = new QuestPart_Pass();
				questPart_Pass.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
				questPart_Pass.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(enumerable.First<string>());
				QuestGen.quest.AddPart(questPart_Pass);
				return;
			}
			QuestPart_PassOutMany questPart_PassOutMany = new QuestPart_PassOutMany();
			questPart_PassOutMany.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
			foreach (string signal in enumerable)
			{
				questPart_PassOutMany.outSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			QuestGen.quest.AddPart(questPart_PassOutMany);
		}

		// Token: 0x040087CC RID: 34764
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x040087CD RID: 34765
		[NoTranslate]
		public SlateRef<string> outSignalsFormat;

		// Token: 0x040087CE RID: 34766
		public SlateRef<int> outSignalsFormattedCount;
	}
}
