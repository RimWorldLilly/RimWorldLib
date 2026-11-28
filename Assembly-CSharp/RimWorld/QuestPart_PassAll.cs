using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104B RID: 4171
	public class QuestPart_PassAll : QuestPart
	{
		// Token: 0x1700120C RID: 4620
		// (get) Token: 0x0600690F RID: 26895 RVA: 0x002519D8 File Offset: 0x0024FBD8
		private bool AllSignalsReceived
		{
			get
			{
				return PassAllQuestPartUtility.AllReceived(this.inSignals, this.signalsReceived);
			}
		}

		// Token: 0x06006910 RID: 26896 RVA: 0x002519EC File Offset: 0x0024FBEC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (!this.AllSignalsReceived)
			{
				int num = this.inSignals.IndexOf(signal.tag);
				if (num >= 0)
				{
					while (this.signalsReceived.Count <= num)
					{
						this.signalsReceived.Add(false);
					}
					this.signalsReceived[num] = true;
					if (this.AllSignalsReceived)
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignal, false));
					}
				}
			}
		}

		// Token: 0x06006911 RID: 26897 RVA: 0x00251A60 File Offset: 0x0024FC60
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06006912 RID: 26898 RVA: 0x00251AB4 File Offset: 0x0024FCB4
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

		// Token: 0x040042E1 RID: 17121
		public List<string> inSignals = new List<string>();

		// Token: 0x040042E2 RID: 17122
		public string outSignal;

		// Token: 0x040042E3 RID: 17123
		private List<bool> signalsReceived = new List<bool>();
	}
}
