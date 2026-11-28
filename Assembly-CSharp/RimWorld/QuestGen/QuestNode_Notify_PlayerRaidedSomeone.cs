using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002224 RID: 8740
	public class QuestNode_Notify_PlayerRaidedSomeone : QuestNode
	{
		// Token: 0x0600D89C RID: 55452 RVA: 0x00504995 File Offset: 0x00502B95
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D89D RID: 55453 RVA: 0x00504998 File Offset: 0x00502B98
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Notify_PlayerRaidedSomeone questPart_Notify_PlayerRaidedSomeone = new QuestPart_Notify_PlayerRaidedSomeone();
			questPart_Notify_PlayerRaidedSomeone.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Notify_PlayerRaidedSomeone.getRaidersFromMap = this.getRaidersFromMap.GetValue(slate);
			questPart_Notify_PlayerRaidedSomeone.getRaidersFromMapParent = this.getRaidersFromMapParent.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Notify_PlayerRaidedSomeone);
		}

		// Token: 0x04008A3D RID: 35389
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008A3E RID: 35390
		public SlateRef<Map> getRaidersFromMap;

		// Token: 0x04008A3F RID: 35391
		public SlateRef<MapParent> getRaidersFromMapParent;
	}
}
