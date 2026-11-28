using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001049 RID: 4169
	public class QuestPart_Pass : QuestPartCanDisable
	{
		// Token: 0x06006907 RID: 26887 RVA: 0x00251840 File Offset: 0x0024FA40
		protected override void ProcessQuestSignal(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				SignalArgs args = new SignalArgs(signal.args);
				if (this.outSignalOutcomeArg != null)
				{
					args.Add(this.outSignalOutcomeArg.Value.Named("OUTCOME"));
				}
				Find.SignalManager.SendSignal(new Signal(this.outSignal, args, false));
			}
		}

		// Token: 0x06006908 RID: 26888 RVA: 0x002518B4 File Offset: 0x0024FAB4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<QuestEndOutcome?>(ref this.outSignalOutcomeArg, "outSignalOutcomeArg", null, false);
		}

		// Token: 0x06006909 RID: 26889 RVA: 0x00251908 File Offset: 0x0024FB08
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.outSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x040042DD RID: 17117
		public string inSignal;

		// Token: 0x040042DE RID: 17118
		public string outSignal;

		// Token: 0x040042DF RID: 17119
		public QuestEndOutcome? outSignalOutcomeArg;
	}
}
