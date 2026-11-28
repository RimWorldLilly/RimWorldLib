using System;

namespace RimWorld
{
	// Token: 0x02001059 RID: 4185
	public class QuestPart_QuestEndParent : QuestPart_QuestEnd
	{
		// Token: 0x0600694B RID: 26955 RVA: 0x0025296C File Offset: 0x00250B6C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag != this.inSignal)
			{
				return;
			}
			QuestEndOutcome outcome;
			if (this.outcome != null)
			{
				outcome = this.outcome.Value;
			}
			else if (!signal.args.TryGetArg<QuestEndOutcome>("OUTCOME", out outcome))
			{
				outcome = QuestEndOutcome.Unknown;
			}
			this.quest.parent.End(outcome, this.sendLetter, this.playSound);
		}
	}
}
