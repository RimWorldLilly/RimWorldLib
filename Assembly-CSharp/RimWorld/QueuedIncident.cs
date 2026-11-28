using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200115C RID: 4444
	public class QueuedIncident : IExposable
	{
		// Token: 0x17001323 RID: 4899
		// (get) Token: 0x06006F9B RID: 28571 RVA: 0x00270892 File Offset: 0x0026EA92
		public int FireTick
		{
			get
			{
				return this.fireTick;
			}
		}

		// Token: 0x17001324 RID: 4900
		// (get) Token: 0x06006F9C RID: 28572 RVA: 0x0027089A File Offset: 0x0026EA9A
		public FiringIncident FiringIncident
		{
			get
			{
				return this.firingInc;
			}
		}

		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x06006F9D RID: 28573 RVA: 0x002708A2 File Offset: 0x0026EAA2
		public int RetryDurationTicks
		{
			get
			{
				return this.retryDurationTicks;
			}
		}

		// Token: 0x17001326 RID: 4902
		// (get) Token: 0x06006F9E RID: 28574 RVA: 0x002708AA File Offset: 0x0026EAAA
		public bool TriedToFire
		{
			get
			{
				return this.triedToFire;
			}
		}

		// Token: 0x06006F9F RID: 28575 RVA: 0x002708B2 File Offset: 0x0026EAB2
		public QueuedIncident()
		{
		}

		// Token: 0x06006FA0 RID: 28576 RVA: 0x002708C1 File Offset: 0x0026EAC1
		public QueuedIncident(FiringIncident firingInc, int fireTick, int retryDurationTicks = 0)
		{
			this.firingInc = firingInc;
			this.fireTick = fireTick;
			this.retryDurationTicks = retryDurationTicks;
		}

		// Token: 0x06006FA1 RID: 28577 RVA: 0x002708E8 File Offset: 0x0026EAE8
		public void ExposeData()
		{
			Scribe_Deep.Look<FiringIncident>(ref this.firingInc, "firingInc", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.fireTick, "fireTick", 0, false);
			Scribe_Values.Look<int>(ref this.retryDurationTicks, "retryDurationTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.triedToFire, "triedToFire", false, false);
		}

		// Token: 0x06006FA2 RID: 28578 RVA: 0x00270940 File Offset: 0x0026EB40
		public void Notify_TriedToFire()
		{
			this.triedToFire = true;
		}

		// Token: 0x06006FA3 RID: 28579 RVA: 0x00270949 File Offset: 0x0026EB49
		public override string ToString()
		{
			return this.fireTick.ToString() + "->" + this.firingInc.ToString();
		}

		// Token: 0x04004770 RID: 18288
		private FiringIncident firingInc;

		// Token: 0x04004771 RID: 18289
		private int fireTick = -1;

		// Token: 0x04004772 RID: 18290
		private int retryDurationTicks;

		// Token: 0x04004773 RID: 18291
		private bool triedToFire;

		// Token: 0x04004774 RID: 18292
		public const int RetryIntervalTicks = 833;
	}
}
