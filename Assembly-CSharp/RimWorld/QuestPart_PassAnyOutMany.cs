using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001052 RID: 4178
	public class QuestPart_PassAnyOutMany : QuestPart
	{
		// Token: 0x0600692F RID: 26927 RVA: 0x00252248 File Offset: 0x00250448
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				for (int i = 0; i < this.outSignals.Count; i++)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignals[i], signal.args, false));
				}
			}
		}

		// Token: 0x06006930 RID: 26928 RVA: 0x002522A7 File Offset: 0x002504A7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06006931 RID: 26929 RVA: 0x002522DC File Offset: 0x002504DC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			this.outSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int.ToString());
				this.outSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
		}

		// Token: 0x040042F1 RID: 17137
		public List<string> inSignals = new List<string>();

		// Token: 0x040042F2 RID: 17138
		public List<string> outSignals = new List<string>();
	}
}
