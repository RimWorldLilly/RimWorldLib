using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001050 RID: 4176
	public class QuestPart_PassAny : QuestPart
	{
		// Token: 0x06006927 RID: 26919 RVA: 0x002520C3 File Offset: 0x002502C3
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, signal.args, false));
			}
		}

		// Token: 0x06006928 RID: 26920 RVA: 0x002520FB File Offset: 0x002502FB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
		}

		// Token: 0x06006929 RID: 26921 RVA: 0x0025212C File Offset: 0x0025032C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
			this.outSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x040042EE RID: 17134
		public List<string> inSignals = new List<string>();

		// Token: 0x040042EF RID: 17135
		public string outSignal;
	}
}
