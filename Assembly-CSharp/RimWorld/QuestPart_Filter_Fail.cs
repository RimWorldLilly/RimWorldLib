using System;

namespace RimWorld
{
	// Token: 0x02001074 RID: 4212
	public class QuestPart_Filter_Fail : QuestPart_Filter
	{
		// Token: 0x060069A2 RID: 27042 RVA: 0x00253BC4 File Offset: 0x00251DC4
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) && questEndOutcome == QuestEndOutcome.Fail;
		}
	}
}
