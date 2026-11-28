using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200112B RID: 4395
	public class SignalManager
	{
		// Token: 0x06006E29 RID: 28201 RVA: 0x00269F05 File Offset: 0x00268105
		public void RegisterReceiver(ISignalReceiver receiver)
		{
			if (receiver == null)
			{
				Log.Error("Tried to register a null receiver.");
				return;
			}
			if (this.receivers.Contains(receiver))
			{
				Log.Error("Tried to register the same receiver twice: " + receiver.ToStringSafe<ISignalReceiver>());
				return;
			}
			this.receivers.Add(receiver);
		}

		// Token: 0x06006E2A RID: 28202 RVA: 0x00269F45 File Offset: 0x00268145
		public void DeregisterReceiver(ISignalReceiver receiver)
		{
			this.receivers.Remove(receiver);
		}

		// Token: 0x06006E2B RID: 28203 RVA: 0x00269F54 File Offset: 0x00268154
		public void SendSignal(Signal signal)
		{
			if (this.signalsThisFrame >= 3000)
			{
				if (this.signalsThisFrame == 3000)
				{
					Log.Error("Reached max signals per frame (" + 3000.ToString() + "). Ignoring further signals.");
				}
				this.signalsThisFrame++;
				return;
			}
			this.signalsThisFrame++;
			if (DebugViewSettings.logSignals)
			{
				Log.Message("Signal: tag=" + signal.tag.ToStringSafe<string>() + " args=" + signal.args.Args.ToStringSafeEnumerable());
			}
			SignalManager.tmpReceivers.Clear();
			SignalManager.tmpReceivers.AddRange(this.receivers);
			for (int i = 0; i < SignalManager.tmpReceivers.Count; i++)
			{
				try
				{
					SignalManager.tmpReceivers[i].Notify_SignalReceived(signal);
				}
				catch (Exception ex)
				{
					string str = "Error while sending signal to ";
					string str2 = SignalManager.tmpReceivers[i].ToStringSafe<ISignalReceiver>();
					string str3 = ": ";
					Exception ex2 = ex;
					Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x06006E2C RID: 28204 RVA: 0x0026A078 File Offset: 0x00268278
		public void SignalManagerUpdate()
		{
			this.signalsThisFrame = 0;
		}

		// Token: 0x04004672 RID: 18034
		private int signalsThisFrame;

		// Token: 0x04004673 RID: 18035
		private const int MaxSignalsPerFrame = 3000;

		// Token: 0x04004674 RID: 18036
		public List<ISignalReceiver> receivers = new List<ISignalReceiver>();

		// Token: 0x04004675 RID: 18037
		private static List<ISignalReceiver> tmpReceivers = new List<ISignalReceiver>();
	}
}
