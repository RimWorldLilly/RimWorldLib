using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002207 RID: 8711
	public class QuestNode_ChangeNeed : QuestNode
	{
		// Token: 0x0600D83C RID: 55356 RVA: 0x00502E22 File Offset: 0x00501022
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D83D RID: 55357 RVA: 0x00502E28 File Offset: 0x00501028
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ChangeNeed questPart_ChangeNeed = new QuestPart_ChangeNeed();
			questPart_ChangeNeed.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ChangeNeed.pawn = this.pawn.GetValue(slate);
			questPart_ChangeNeed.need = this.need.GetValue(slate);
			questPart_ChangeNeed.offset = this.offset.GetValue(slate);
			QuestGen.quest.AddPart(questPart_ChangeNeed);
		}

		// Token: 0x040089A0 RID: 35232
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040089A1 RID: 35233
		public SlateRef<Pawn> pawn;

		// Token: 0x040089A2 RID: 35234
		public SlateRef<NeedDef> need;

		// Token: 0x040089A3 RID: 35235
		public SlateRef<float> offset;
	}
}
