using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002197 RID: 8599
	public class QuestNode_RuntimeLog : QuestNode
	{
		// Token: 0x0600D698 RID: 54936 RVA: 0x004FB3BB File Offset: 0x004F95BB
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D699 RID: 54937 RVA: 0x004FB3C0 File Offset: 0x004F95C0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Log questPart_Log = new QuestPart_Log();
			questPart_Log.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Log.message = this.message.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Log);
		}

		// Token: 0x040087E9 RID: 34793
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040087EA RID: 34794
		[NoTranslate]
		public SlateRef<string> message;
	}
}
