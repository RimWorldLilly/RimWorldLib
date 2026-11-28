using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104F RID: 4175
	public class QuestPart_PassAllSequence : QuestPart
	{
		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x06006922 RID: 26914 RVA: 0x00251F77 File Offset: 0x00250177
		private bool AllSignalsReceived
		{
			get
			{
				return this.ptr >= this.inSignals.Count - 1;
			}
		}

		// Token: 0x06006923 RID: 26915 RVA: 0x00251F94 File Offset: 0x00250194
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (!this.AllSignalsReceived && this.inSignals.IndexOf(signal.tag) == this.ptr + 1)
			{
				this.ptr++;
				if (this.AllSignalsReceived)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignal, false));
				}
			}
		}

		// Token: 0x06006924 RID: 26916 RVA: 0x00251FF0 File Offset: 0x002501F0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<int>(ref this.ptr, "ptr", 0, false);
		}

		// Token: 0x06006925 RID: 26917 RVA: 0x00252040 File Offset: 0x00250240
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

		// Token: 0x040042EB RID: 17131
		public List<string> inSignals = new List<string>();

		// Token: 0x040042EC RID: 17132
		public string outSignal;

		// Token: 0x040042ED RID: 17133
		private int ptr = -1;
	}
}
