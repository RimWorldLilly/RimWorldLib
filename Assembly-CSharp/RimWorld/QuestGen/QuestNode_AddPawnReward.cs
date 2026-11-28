using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021FE RID: 8702
	public class QuestNode_AddPawnReward : QuestNode
	{
		// Token: 0x0600D820 RID: 55328 RVA: 0x00502821 File Offset: 0x00500A21
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D821 RID: 55329 RVA: 0x00502824 File Offset: 0x00500A24
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn value = this.pawn.GetValue(slate);
			if (value == null)
			{
				return;
			}
			QuestPart_Choice questPart_Choice = new QuestPart_Choice();
			questPart_Choice.inSignalChoiceUsed = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalChoiceUsed.GetValue(slate));
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_Pawn
			{
				pawn = value,
				detailsHidden = this.rewardDetailsHidden.GetValue(slate)
			});
			questPart_Choice.choices.Add(choice);
			QuestGen.quest.AddPart(questPart_Choice);
		}

		// Token: 0x0400897F RID: 35199
		public SlateRef<Pawn> pawn;

		// Token: 0x04008980 RID: 35200
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;

		// Token: 0x04008981 RID: 35201
		public SlateRef<bool> rewardDetailsHidden;
	}
}
