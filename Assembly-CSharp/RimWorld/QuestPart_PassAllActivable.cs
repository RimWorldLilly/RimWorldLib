using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104D RID: 4173
	public class QuestPart_PassAllActivable : QuestPartActivable
	{
		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x06006915 RID: 26901 RVA: 0x00251B78 File Offset: 0x0024FD78
		private bool AllSignalsReceived
		{
			get
			{
				if (this.inSignals.Count != this.signalsReceived.Count)
				{
					return false;
				}
				for (int i = 0; i < this.signalsReceived.Count; i++)
				{
					if (!this.signalsReceived[i])
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x06006916 RID: 26902 RVA: 0x00251BC6 File Offset: 0x0024FDC6
		public int SignalsReceivedCount
		{
			get
			{
				return this.signalsReceived.Count((bool s) => s);
			}
		}

		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x06006917 RID: 26903 RVA: 0x00251BF4 File Offset: 0x0024FDF4
		public override string ExpiryInfoPart
		{
			get
			{
				if (this.expiryInfoPartKey.NullOrEmpty())
				{
					return null;
				}
				return this.expiryInfoPartKey.Translate() + " " + this.SignalsReceivedCount.ToString() + " / " + this.inSignals.Count.ToString();
			}
		}

		// Token: 0x06006918 RID: 26904 RVA: 0x00251C55 File Offset: 0x0024FE55
		protected override void Enable(SignalArgs receivedArgs)
		{
			this.signalsReceived.Clear();
			base.Enable(receivedArgs);
		}

		// Token: 0x06006919 RID: 26905 RVA: 0x00251C6C File Offset: 0x0024FE6C
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			int num = this.inSignals.IndexOf(signal.tag);
			if (num >= 0)
			{
				while (this.signalsReceived.Count <= num)
				{
					this.signalsReceived.Add(false);
				}
				this.signalsReceived[num] = true;
				if (!this.outSignalAny.NullOrEmpty())
				{
					SignalArgs args = signal.args;
					args.Add(this.SignalsReceivedCount.Named("COUNT"));
					Find.SignalManager.SendSignal(new Signal(this.outSignalAny, args, false));
				}
				if (this.AllSignalsReceived)
				{
					base.Complete();
				}
			}
		}

		// Token: 0x0600691A RID: 26906 RVA: 0x00251D14 File Offset: 0x0024FF14
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignalAny, "outSignalAny", null, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPartKey, "expiryInfoPartKey", null, false);
		}

		// Token: 0x0600691B RID: 26907 RVA: 0x00251D78 File Offset: 0x0024FF78
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
		}

		// Token: 0x040042E4 RID: 17124
		public List<string> inSignals = new List<string>();

		// Token: 0x040042E5 RID: 17125
		public string outSignalAny;

		// Token: 0x040042E6 RID: 17126
		public string expiryInfoPartKey;

		// Token: 0x040042E7 RID: 17127
		private List<bool> signalsReceived = new List<bool>();
	}
}
