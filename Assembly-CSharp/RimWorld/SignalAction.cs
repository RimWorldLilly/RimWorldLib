using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B00 RID: 6912
	public abstract class SignalAction : Thing
	{
		// Token: 0x0600B0C6 RID: 45254 RVA: 0x003E4995 File Offset: 0x003E2B95
		public override void Notify_SignalReceived(Signal signal)
		{
			base.Notify_SignalReceived(signal);
			if (signal.tag == this.signalTag)
			{
				this.DoAction(signal.args);
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600B0C7 RID: 45255
		protected abstract void DoAction(SignalArgs args);

		// Token: 0x0600B0C8 RID: 45256 RVA: 0x003E49CC File Offset: 0x003E2BCC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04006602 RID: 26114
		public string signalTag;
	}
}
