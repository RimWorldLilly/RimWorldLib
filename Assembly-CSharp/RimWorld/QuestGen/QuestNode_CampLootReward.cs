using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002185 RID: 8581
	public class QuestNode_CampLootReward : QuestNode
	{
		// Token: 0x0600D65E RID: 54878 RVA: 0x004FA284 File Offset: 0x004F8484
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D65F RID: 54879 RVA: 0x004FA288 File Offset: 0x004F8488
		protected override void RunInt()
		{
			QuestPart_Choice questPart_Choice = QuestGen.quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_CampLoot());
			questPart_Choice.choices.Add(choice);
		}
	}
}
