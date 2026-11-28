using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200220D RID: 8717
	public class QuestNode_DropMonumentMarkerCopy : QuestNode
	{
		// Token: 0x0600D84E RID: 55374 RVA: 0x00503139 File Offset: 0x00501339
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x0600D84F RID: 55375 RVA: 0x00503148 File Offset: 0x00501348
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_DropMonumentMarkerCopy questPart_DropMonumentMarkerCopy = new QuestPart_DropMonumentMarkerCopy();
			questPart_DropMonumentMarkerCopy.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_DropMonumentMarkerCopy.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_DropMonumentMarkerCopy.outSignalResult = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalResult.GetValue(slate));
			questPart_DropMonumentMarkerCopy.destroyOrPassToWorldOnCleanup = this.destroyOrPassToWorldOnCleanup.GetValue(slate);
			QuestGen.quest.AddPart(questPart_DropMonumentMarkerCopy);
		}

		// Token: 0x040089AF RID: 35247
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040089B0 RID: 35248
		[NoTranslate]
		public SlateRef<string> outSignalResult;

		// Token: 0x040089B1 RID: 35249
		public SlateRef<bool> destroyOrPassToWorldOnCleanup;
	}
}
