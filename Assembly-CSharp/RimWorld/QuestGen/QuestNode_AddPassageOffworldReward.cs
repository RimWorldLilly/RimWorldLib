using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021FD RID: 8701
	public class QuestNode_AddPassageOffworldReward : QuestNode
	{
		// Token: 0x0600D81D RID: 55325 RVA: 0x005027B8 File Offset: 0x005009B8
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D81E RID: 55326 RVA: 0x005027BC File Offset: 0x005009BC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Choice questPart_Choice = new QuestPart_Choice();
			questPart_Choice.inSignalChoiceUsed = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalChoiceUsed.GetValue(slate));
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_PassageOffworld());
			questPart_Choice.choices.Add(choice);
			QuestGen.quest.AddPart(questPart_Choice);
		}

		// Token: 0x0400897E RID: 35198
		[NoTranslate]
		public SlateRef<string> inSignalChoiceUsed;
	}
}
