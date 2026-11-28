using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001056 RID: 4182
	public class QuestPart_PassWhileActive : QuestPartActivable
	{
		// Token: 0x06006940 RID: 26944 RVA: 0x00252731 File Offset: 0x00250931
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (signal.tag == this.inSignal)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, signal.args, false));
			}
		}

		// Token: 0x06006941 RID: 26945 RVA: 0x00252769 File Offset: 0x00250969
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
		}

		// Token: 0x040042FB RID: 17147
		public string inSignal;

		// Token: 0x040042FC RID: 17148
		public string outSignal;
	}
}
