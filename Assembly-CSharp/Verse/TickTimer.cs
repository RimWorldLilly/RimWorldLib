using System;

namespace Verse
{
	// Token: 0x020004AA RID: 1194
	public class TickTimer : IExposable
	{
		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002120 RID: 8480 RVA: 0x000C5AE3 File Offset: 0x000C3CE3
		public bool Finished
		{
			get
			{
				return this.elapsed >= this.duration;
			}
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x000C5AF8 File Offset: 0x000C3CF8
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.elapsed, "elapsed", 0, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !this.Finished)
			{
				this.start = GenTicks.TicksGame;
			}
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000C5B44 File Offset: 0x000C3D44
		public void Start(int startTicks, int durationTicks, Action finishAction)
		{
			this.start = startTicks;
			this.duration = durationTicks;
			this.OnFinish = finishAction;
			this.elapsed = 0;
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x000C5B62 File Offset: 0x000C3D62
		public void TickIntervalDelta()
		{
			this.elapsed = GenTicks.TicksGame - this.start;
			if (this.Finished && this.OnFinish != null)
			{
				this.OnFinish();
				this.OnFinish = null;
			}
		}

		// Token: 0x04001955 RID: 6485
		private int start;

		// Token: 0x04001956 RID: 6486
		private int elapsed;

		// Token: 0x04001957 RID: 6487
		private int duration;

		// Token: 0x04001958 RID: 6488
		public Action OnFinish;
	}
}
