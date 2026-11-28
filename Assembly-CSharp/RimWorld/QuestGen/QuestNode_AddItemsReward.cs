using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021FB RID: 8699
	public class QuestNode_AddItemsReward : QuestNode
	{
		// Token: 0x0600D817 RID: 55319 RVA: 0x005025B2 File Offset: 0x005007B2
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D818 RID: 55320 RVA: 0x005025B8 File Offset: 0x005007B8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			RewardsGeneratorParams value = this.parms.GetValue(QuestGen.slate);
			IEnumerable<Thing> value2 = this.items.GetValue(slate);
			if (value2.EnumerableNullOrEmpty<Thing>())
			{
				return;
			}
			QuestPart_Choice questPart_Choice = new QuestPart_Choice();
			questPart_Choice.inSignalChoiceUsed = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalChoiceUsed.GetValue(slate));
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			Reward_Items reward_Items = new Reward_Items();
			reward_Items.items.AddRange(value2);
			choice.rewards.Add(reward_Items);
			questPart_Choice.choices.Add(choice);
			QuestGen.quest.AddPart(questPart_Choice);
			if (this.generateQuestParts)
			{
				foreach (QuestPart part in reward_Items.GenerateQuestParts(0, value, null, null, null, null))
				{
					QuestGen.quest.AddPart(part);
				}
			}
		}

		// Token: 0x04008975 RID: 35189
		public SlateRef<IEnumerable<Thing>> items;

		// Token: 0x04008976 RID: 35190
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;

		// Token: 0x04008977 RID: 35191
		public SlateRef<RewardsGeneratorParams> parms;

		// Token: 0x04008978 RID: 35192
		public bool generateQuestParts = true;
	}
}
