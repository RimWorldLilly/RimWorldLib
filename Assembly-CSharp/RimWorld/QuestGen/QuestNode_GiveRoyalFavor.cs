using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002216 RID: 8726
	public class QuestNode_GiveRoyalFavor : QuestNode
	{
		// Token: 0x0600D871 RID: 55409 RVA: 0x00503C97 File Offset: 0x00501E97
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D872 RID: 55410 RVA: 0x00503C9C File Offset: 0x00501E9C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_GiveRoyalFavor questPart_GiveRoyalFavor = new QuestPart_GiveRoyalFavor();
			questPart_GiveRoyalFavor.giveTo = this.giveTo.GetValue(slate);
			questPart_GiveRoyalFavor.giveToAccepter = this.giveToAccepter.GetValue(slate);
			questPart_GiveRoyalFavor.faction = (this.faction.GetValue(slate) ?? this.factionOf.GetValue(slate).Faction);
			questPart_GiveRoyalFavor.amount = this.amount.GetValue(slate);
			questPart_GiveRoyalFavor.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_GiveRoyalFavor);
			if (this.isSingleReward.GetValue(slate))
			{
				QuestPart_Choice questPart_Choice = new QuestPart_Choice();
				questPart_Choice.inSignalChoiceUsed = questPart_GiveRoyalFavor.inSignal;
				QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
				choice.questParts.Add(questPart_GiveRoyalFavor);
				choice.rewards.Add(new Reward_RoyalFavor
				{
					faction = questPart_GiveRoyalFavor.faction,
					amount = questPart_GiveRoyalFavor.amount
				});
				questPart_Choice.choices.Add(choice);
				QuestGen.quest.AddPart(questPart_Choice);
				this.PostProcessRewardChoice(questPart_Choice);
			}
		}

		// Token: 0x0600D873 RID: 55411 RVA: 0x00503DC1 File Offset: 0x00501FC1
		protected virtual void PostProcessRewardChoice(QuestPart_Choice rewardChoice)
		{
		}

		// Token: 0x040089F7 RID: 35319
		public SlateRef<Pawn> giveTo;

		// Token: 0x040089F8 RID: 35320
		public SlateRef<bool> giveToAccepter;

		// Token: 0x040089F9 RID: 35321
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040089FA RID: 35322
		public SlateRef<Faction> faction;

		// Token: 0x040089FB RID: 35323
		public SlateRef<Thing> factionOf;

		// Token: 0x040089FC RID: 35324
		public SlateRef<int> amount;

		// Token: 0x040089FD RID: 35325
		public SlateRef<bool> isSingleReward;
	}
}
