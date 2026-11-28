using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200223E RID: 8766
	public class QuestNode_SituationalThought : QuestNode
	{
		// Token: 0x0600D8EE RID: 55534 RVA: 0x00505BF0 File Offset: 0x00503DF0
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8EF RID: 55535 RVA: 0x00505BF4 File Offset: 0x00503DF4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_SituationalThought questPart_SituationalThought = new QuestPart_SituationalThought();
			questPart_SituationalThought.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SituationalThought.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_SituationalThought.def = this.def.GetValue(slate);
			questPart_SituationalThought.pawn = this.pawn.GetValue(slate);
			questPart_SituationalThought.delayTicks = this.delayTicks.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SituationalThought);
		}

		// Token: 0x04008A92 RID: 35474
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04008A93 RID: 35475
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04008A94 RID: 35476
		public SlateRef<ThoughtDef> def;

		// Token: 0x04008A95 RID: 35477
		public SlateRef<Pawn> pawn;

		// Token: 0x04008A96 RID: 35478
		public SlateRef<int> delayTicks;
	}
}
