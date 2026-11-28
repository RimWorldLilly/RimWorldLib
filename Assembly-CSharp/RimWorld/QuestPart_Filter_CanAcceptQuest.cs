using System;

namespace RimWorld
{
	// Token: 0x02001071 RID: 4209
	public class QuestPart_Filter_CanAcceptQuest : QuestPart_Filter
	{
		// Token: 0x0600699B RID: 27035 RVA: 0x00253AEE File Offset: 0x00251CEE
		protected override bool Pass(SignalArgs args)
		{
			return QuestUtility.CanAcceptQuest(this.quest);
		}
	}
}
