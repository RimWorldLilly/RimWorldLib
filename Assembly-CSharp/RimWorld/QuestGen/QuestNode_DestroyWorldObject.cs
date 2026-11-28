using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200220B RID: 8715
	public class QuestNode_DestroyWorldObject : QuestNode
	{
		// Token: 0x0600D848 RID: 55368 RVA: 0x00503037 File Offset: 0x00501237
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D849 RID: 55369 RVA: 0x0050303C File Offset: 0x0050123C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_DestroyWorldObject questPart_DestroyWorldObject = new QuestPart_DestroyWorldObject();
			questPart_DestroyWorldObject.worldObject = this.worldObject.GetValue(slate);
			questPart_DestroyWorldObject.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_DestroyWorldObject);
		}

		// Token: 0x040089AA RID: 35242
		public SlateRef<WorldObject> worldObject;

		// Token: 0x040089AB RID: 35243
		[NoTranslate]
		public SlateRef<string> inSignal;
	}
}
