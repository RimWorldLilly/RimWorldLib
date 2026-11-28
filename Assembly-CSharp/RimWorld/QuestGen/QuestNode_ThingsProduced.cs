using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002242 RID: 8770
	public class QuestNode_ThingsProduced : QuestNode
	{
		// Token: 0x0600D8FB RID: 55547 RVA: 0x00506144 File Offset: 0x00504344
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600D8FC RID: 55548 RVA: 0x0050615C File Offset: 0x0050435C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ThingsProduced questPart_ThingsProduced = new QuestPart_ThingsProduced();
			questPart_ThingsProduced.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ThingsProduced.def = this.def.GetValue(slate);
			questPart_ThingsProduced.stuff = this.stuff.GetValue(slate);
			questPart_ThingsProduced.count = this.count.GetValue(slate);
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_ThingsProduced);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_ThingsProduced.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_ThingsProduced);
		}

		// Token: 0x04008AA6 RID: 35494
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04008AA7 RID: 35495
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04008AA8 RID: 35496
		public SlateRef<ThingDef> def;

		// Token: 0x04008AA9 RID: 35497
		public SlateRef<ThingDef> stuff;

		// Token: 0x04008AAA RID: 35498
		public SlateRef<int> count;

		// Token: 0x04008AAB RID: 35499
		public QuestNode node;
	}
}
