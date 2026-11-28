using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002225 RID: 8741
	public class QuestNode_NoWorldObject : QuestNode
	{
		// Token: 0x0600D89F RID: 55455 RVA: 0x00504A14 File Offset: 0x00502C14
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600D8A0 RID: 55456 RVA: 0x00504A2C File Offset: 0x00502C2C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_NoWorldObject questPart_NoWorldObject = new QuestPart_NoWorldObject();
			questPart_NoWorldObject.worldObject = this.worldObject.GetValue(slate);
			questPart_NoWorldObject.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_NoWorldObject);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_NoWorldObject.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_NoWorldObject);
		}

		// Token: 0x04008A40 RID: 35392
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04008A41 RID: 35393
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04008A42 RID: 35394
		public SlateRef<WorldObject> worldObject;

		// Token: 0x04008A43 RID: 35395
		public QuestNode node;
	}
}
