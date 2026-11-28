using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E51 RID: 3665
	public class ThoughtWorker_QuestPart : ThoughtWorker
	{
		// Token: 0x060061A6 RID: 24998 RVA: 0x00233924 File Offset: 0x00231B24
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			QuestPart_SituationalThought questPart_SituationalThought = this.FindQuestPart(p);
			if (questPart_SituationalThought == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(questPart_SituationalThought.stage);
		}

		// Token: 0x060061A7 RID: 24999 RVA: 0x00233950 File Offset: 0x00231B50
		public QuestPart_SituationalThought FindQuestPart(Pawn p)
		{
			List<QuestPart_SituationalThought> situationalThoughtQuestParts = Find.QuestManager.SituationalThoughtQuestParts;
			for (int i = 0; i < situationalThoughtQuestParts.Count; i++)
			{
				if (situationalThoughtQuestParts[i].quest.State == QuestState.Ongoing && situationalThoughtQuestParts[i].State == QuestPartState.Enabled && situationalThoughtQuestParts[i].def == this.def && situationalThoughtQuestParts[i].pawn == p)
				{
					return situationalThoughtQuestParts[i];
				}
			}
			return null;
		}
	}
}
