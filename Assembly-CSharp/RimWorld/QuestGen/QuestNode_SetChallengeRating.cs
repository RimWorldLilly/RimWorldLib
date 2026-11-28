using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002236 RID: 8758
	public class QuestNode_SetChallengeRating : QuestNode
	{
		// Token: 0x0600D8D4 RID: 55508 RVA: 0x00505752 File Offset: 0x00503952
		protected override void RunInt()
		{
			QuestGen.quest.challengeRating = this.challengeRating.GetValue(QuestGen.slate);
		}

		// Token: 0x0600D8D5 RID: 55509 RVA: 0x0050576E File Offset: 0x0050396E
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04008A7B RID: 35451
		public SlateRef<int> challengeRating;
	}
}
