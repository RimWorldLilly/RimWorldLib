using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002228 RID: 8744
	public class QuestNode_PlantsHarvested : QuestNode
	{
		// Token: 0x0600D8A8 RID: 55464 RVA: 0x00504F90 File Offset: 0x00503190
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600D8A9 RID: 55465 RVA: 0x00504FA8 File Offset: 0x005031A8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_PlantsHarvested questPart_PlantsHarvested = new QuestPart_PlantsHarvested();
			questPart_PlantsHarvested.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PlantsHarvested.plant = this.plant.GetValue(slate);
			questPart_PlantsHarvested.count = this.count.GetValue(slate);
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PlantsHarvested);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_PlantsHarvested.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_PlantsHarvested);
		}

		// Token: 0x04008A58 RID: 35416
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04008A59 RID: 35417
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04008A5A RID: 35418
		public SlateRef<ThingDef> plant;

		// Token: 0x04008A5B RID: 35419
		public SlateRef<int> count;

		// Token: 0x04008A5C RID: 35420
		public QuestNode node;
	}
}
