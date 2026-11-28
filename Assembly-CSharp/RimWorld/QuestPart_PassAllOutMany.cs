using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104E RID: 4174
	public class QuestPart_PassAllOutMany : QuestPart
	{
		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x0600691D RID: 26909 RVA: 0x00251DE2 File Offset: 0x0024FFE2
		private bool AllSignalsReceived
		{
			get
			{
				return PassAllQuestPartUtility.AllReceived(this.inSignals, this.signalsReceived);
			}
		}

		// Token: 0x0600691E RID: 26910 RVA: 0x00251DF8 File Offset: 0x0024FFF8
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
						for (int i = 0; i < this.outSignals.Count; i++)
						{
							Find.SignalManager.SendSignal(new Signal(this.outSignals[i], false));
						}
					}
				}
			}
		}

		// Token: 0x0600691F RID: 26911 RVA: 0x00251E88 File Offset: 0x00250088
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06006920 RID: 26912 RVA: 0x00251EE0 File Offset: 0x002500E0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int.ToString());
				this.outSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
		}

		// Token: 0x040042E8 RID: 17128
		public List<string> inSignals = new List<string>();

		// Token: 0x040042E9 RID: 17129
		public List<string> outSignals = new List<string>();

		// Token: 0x040042EA RID: 17130
		private List<bool> signalsReceived = new List<bool>();
	}
}
