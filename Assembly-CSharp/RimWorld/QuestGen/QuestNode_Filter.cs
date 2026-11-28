using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022B3 RID: 8883
	public abstract class QuestNode_Filter : QuestNode
	{
		// Token: 0x0600DB57 RID: 56151 RVA: 0x0051BAB4 File Offset: 0x00519CB4
		protected override bool TestRunInt(Slate slate)
		{
			return (this.node == null || this.node.TestRun(slate)) && (this.elseNode == null || this.elseNode.TestRun(slate));
		}

		// Token: 0x0600DB58 RID: 56152
		protected abstract QuestPart_Filter MakeFilterQuestPart();

		// Token: 0x0600DB59 RID: 56153 RVA: 0x0051BAE8 File Offset: 0x00519CE8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Filter questPart_Filter = this.MakeFilterQuestPart();
			questPart_Filter.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				questPart_Filter.outSignal = QuestGen.GenerateNewSignal("FilterPass", true);
				QuestGenUtility.RunInnerNode(this.node, questPart_Filter.outSignal);
			}
			if (this.elseNode != null)
			{
				questPart_Filter.outSignalElse = QuestGen.GenerateNewSignal("FilterNoPass", true);
				QuestGenUtility.RunInnerNode(this.elseNode, questPart_Filter.outSignalElse);
			}
			QuestGen.quest.AddPart(questPart_Filter);
		}

		// Token: 0x04008C62 RID: 35938
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008C63 RID: 35939
		public QuestNode node;

		// Token: 0x04008C64 RID: 35940
		public QuestNode elseNode;

		// Token: 0x04008C65 RID: 35941
		private const string FilterPassSignal = "FilterPass";

		// Token: 0x04008C66 RID: 35942
		private const string FilterNoPassSignal = "FilterNoPass";
	}
}
