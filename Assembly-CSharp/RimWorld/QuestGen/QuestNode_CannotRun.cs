using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002202 RID: 8706
	public class QuestNode_CannotRun : QuestNode
	{
		// Token: 0x0600D82C RID: 55340 RVA: 0x00502A1A File Offset: 0x00500C1A
		protected override void RunInt()
		{
		}

		// Token: 0x0600D82D RID: 55341 RVA: 0x00502A1C File Offset: 0x00500C1C
		protected override bool TestRunInt(Slate slate)
		{
			return false;
		}
	}
}
