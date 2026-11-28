using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022AB RID: 8875
	public class QuestNode_Root_VoidMonolith : QuestNode
	{
		// Token: 0x0600DB1E RID: 56094 RVA: 0x00519C08 File Offset: 0x00517E08
		protected override bool TestRunInt(Slate slate)
		{
			return ModsConfig.AnomalyActive && Find.Anomaly.GenerateMonolith && QuestGen_Get.GetMap(false, null, false) != null;
		}

		// Token: 0x0600DB1F RID: 56095 RVA: 0x00519C44 File Offset: 0x00517E44
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			Building_VoidMonolith monolith = slate.Get<Building_VoidMonolith>("monolith", null, false);
			string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("monolithMap");
			QuestUtility.AddQuestTag(ref map.Parent.questTags, questTagToAdd);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("monolithMap.Destroyed");
			quest.AddPart(new QuestPart_MonolithPart(monolith));
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, true, false);
			QuestPart_Hyperlinks part = new QuestPart_Hyperlinks();
			quest.AddPart(part);
		}
	}
}
