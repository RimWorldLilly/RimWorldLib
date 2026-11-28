using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022B2 RID: 8882
	public class QuestNode_AnyPawnAlive : QuestNode
	{
		// Token: 0x0600DB54 RID: 56148 RVA: 0x0051BA1D File Offset: 0x00519C1D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600DB55 RID: 56149 RVA: 0x0051BA20 File Offset: 0x00519C20
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Filter_AnyPawnAlive questPart_Filter_AnyPawnAlive = new QuestPart_Filter_AnyPawnAlive
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				pawns = this.pawns.GetValue(slate)
			};
			if (this.node != null)
			{
				questPart_Filter_AnyPawnAlive.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				QuestGenUtility.RunInnerNode(this.node, questPart_Filter_AnyPawnAlive.outSignal);
			}
			QuestGen.quest.AddPart(questPart_Filter_AnyPawnAlive);
		}

		// Token: 0x04008C5E RID: 35934
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008C5F RID: 35935
		public SlateRef<List<Pawn>> pawns;

		// Token: 0x04008C60 RID: 35936
		public QuestNode node;

		// Token: 0x04008C61 RID: 35937
		private const string OuterNodeCompletedSignal = "OuterNodeCompleted";
	}
}
