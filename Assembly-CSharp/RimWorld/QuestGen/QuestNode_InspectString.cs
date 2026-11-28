using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200221A RID: 8730
	public class QuestNode_InspectString : QuestNode
	{
		// Token: 0x0600D87E RID: 55422 RVA: 0x00504065 File Offset: 0x00502265
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D87F RID: 55423 RVA: 0x00504068 File Offset: 0x00502268
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.targets.GetValue(slate).EnumerableNullOrEmpty<ISelectable>())
			{
				return;
			}
			QuestPart_InspectString questPart_InspectString = new QuestPart_InspectString();
			questPart_InspectString.targets.AddRange(this.targets.GetValue(slate));
			questPart_InspectString.inspectString = this.inspectString.GetValue(slate);
			questPart_InspectString.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_InspectString);
		}

		// Token: 0x04008A07 RID: 35335
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04008A08 RID: 35336
		public SlateRef<IEnumerable<ISelectable>> targets;

		// Token: 0x04008A09 RID: 35337
		public SlateRef<string> inspectString;
	}
}
