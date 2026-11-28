using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001055 RID: 4181
	public class QuestPart_PassOutRandom : QuestPart
	{
		// Token: 0x0600693C RID: 26940 RVA: 0x00252640 File Offset: 0x00250840
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			string tag;
			if (signal.tag == this.inSignal && this.outSignals.TryRandomElement(out tag))
			{
				Find.SignalManager.SendSignal(new Signal(tag, signal.args, false));
			}
		}

		// Token: 0x0600693D RID: 26941 RVA: 0x0025268D File Offset: 0x0025088D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0600693E RID: 26942 RVA: 0x002526C0 File Offset: 0x002508C0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			for (int i = 0; i < 3; i++)
			{
				this.outSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
		}

		// Token: 0x040042F9 RID: 17145
		public string inSignal;

		// Token: 0x040042FA RID: 17146
		public List<string> outSignals = new List<string>();
	}
}
