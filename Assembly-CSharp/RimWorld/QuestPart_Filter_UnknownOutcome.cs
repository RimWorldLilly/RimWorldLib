using System;

namespace RimWorld
{
	// Token: 0x0200107A RID: 4218
	public class QuestPart_Filter_UnknownOutcome : QuestPart_Filter
	{
		// Token: 0x060069B1 RID: 27057 RVA: 0x00253D20 File Offset: 0x00251F20
		protected override bool Pass(SignalArgs args)
		{
			QuestEndOutcome questEndOutcome;
			return !args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome) || questEndOutcome == QuestEndOutcome.Unknown;
		}
	}
}
