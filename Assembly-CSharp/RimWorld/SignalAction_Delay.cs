using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B03 RID: 6915
	public class SignalAction_Delay : SignalAction
	{
		// Token: 0x17001FDF RID: 8159
		// (get) Token: 0x0600B0CF RID: 45263 RVA: 0x003E4E55 File Offset: 0x003E3055
		public bool Activated
		{
			get
			{
				return this.activated;
			}
		}

		// Token: 0x17001FE0 RID: 8160
		// (get) Token: 0x0600B0D0 RID: 45264 RVA: 0x003E4E5D File Offset: 0x003E305D
		public virtual Alert_ActionDelay Alert
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001FE1 RID: 8161
		// (get) Token: 0x0600B0D1 RID: 45265 RVA: 0x003E4E60 File Offset: 0x003E3060
		public virtual bool ShouldRemoveNow
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600B0D2 RID: 45266 RVA: 0x003E4E63 File Offset: 0x003E3063
		public override void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag == this.signalTag)
			{
				this.DoAction(signal.args);
			}
		}

		// Token: 0x0600B0D3 RID: 45267 RVA: 0x003E4E84 File Offset: 0x003E3084
		protected override void DoAction(SignalArgs args)
		{
			if (this.delayTicks <= 0)
			{
				this.CompleteInt();
				return;
			}
			this.activated = true;
		}

		// Token: 0x0600B0D4 RID: 45268 RVA: 0x003E4E9D File Offset: 0x003E309D
		protected virtual void Complete()
		{
			if (!this.completedSignalTag.NullOrEmpty())
			{
				Find.SignalManager.SendSignal(new Signal(this.completedSignalTag, false));
			}
		}

		// Token: 0x0600B0D5 RID: 45269 RVA: 0x003E4EC2 File Offset: 0x003E30C2
		private void CompleteInt()
		{
			this.Complete();
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600B0D6 RID: 45270 RVA: 0x003E4ED9 File Offset: 0x003E30D9
		protected override void Tick()
		{
			if (!this.activated)
			{
				return;
			}
			if (this.delayTicks <= 0)
			{
				this.CompleteInt();
				return;
			}
			if (this.ShouldRemoveNow)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			this.delayTicks--;
		}

		// Token: 0x0600B0D7 RID: 45271 RVA: 0x003E4F12 File Offset: 0x003E3112
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.activated, "activated", false, false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
			Scribe_Values.Look<string>(ref this.completedSignalTag, "completedSignalTag", null, false);
		}

		// Token: 0x0400660E RID: 26126
		public int delayTicks;

		// Token: 0x0400660F RID: 26127
		public string completedSignalTag;

		// Token: 0x04006610 RID: 26128
		private bool activated;
	}
}
