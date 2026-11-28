using System;

namespace RimWorld
{
	// Token: 0x02001078 RID: 4216
	public class QuestPart_Filter_Success : QuestPart_Filter
	{
		// Token: 0x060069AC RID: 27052 RVA: 0x00253C90 File Offset: 0x00251E90
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) && questEndOutcome == QuestEndOutcome.Success;
		}
	}
}
