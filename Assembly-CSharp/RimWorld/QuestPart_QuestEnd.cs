using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001058 RID: 4184
	public class QuestPart_QuestEnd : QuestPart
	{
		// Token: 0x06006947 RID: 26951 RVA: 0x0025285C File Offset: 0x00250A5C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag != this.inSignal)
			{
				return;
			}
			QuestEndOutcome questEndOutcome;
			if (this.outcome != null)
			{
				questEndOutcome = this.outcome.Value;
			}
			else if (!signal.args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome))
			{
				questEndOutcome = QuestEndOutcome.Unknown;
			}
			this.quest.End(questEndOutcome, this.sendLetter, this.playSound);
		}

		// Token: 0x06006948 RID: 26952 RVA: 0x002528D0 File Offset: 0x00250AD0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<QuestEndOutcome?>(ref this.outcome, "outcome", null, false);
			Scribe_Values.Look<bool>(ref this.sendLetter, "sendLetter", false, false);
			Scribe_Values.Look<bool>(ref this.playSound, "playSound", false, false);
		}

		// Token: 0x06006949 RID: 26953 RVA: 0x00252934 File Offset: 0x00250B34
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x040042FE RID: 17150
		public string inSignal;

		// Token: 0x040042FF RID: 17151
		public QuestEndOutcome? outcome;

		// Token: 0x04004300 RID: 17152
		public bool sendLetter;

		// Token: 0x04004301 RID: 17153
		public bool playSound;
	}
}
