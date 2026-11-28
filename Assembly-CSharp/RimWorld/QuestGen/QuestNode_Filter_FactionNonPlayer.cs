using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020022B6 RID: 8886
	public class QuestNode_Filter_FactionNonPlayer : QuestNode_Filter
	{
		// Token: 0x0600DB5F RID: 56159 RVA: 0x0051BBED File Offset: 0x00519DED
		protected override QuestPart_Filter MakeFilterQuestPart()
		{
			return new QuestPart_Filter_FactionNonPlayer();
		}
	}
}
