using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020022B5 RID: 8885
	public class QuestNode_Filter_DecreeNotPossible : QuestNode_Filter
	{
		// Token: 0x0600DB5D RID: 56157 RVA: 0x0051BBDE File Offset: 0x00519DDE
		protected override QuestPart_Filter MakeFilterQuestPart()
		{
			return new QuestPart_Filter_DecreeNotPossible();
		}
	}
}
