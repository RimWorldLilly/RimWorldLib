using System;

namespace Verse
{
	// Token: 0x02000102 RID: 258
	internal class ExtendedShakeRequest : IExposable
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x00033C7B File Offset: 0x00031E7B
		public float Mag
		{
			get
			{
				return this.mag;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x00033C83 File Offset: 0x00031E83
		public int Duration
		{
			get
			{
				return this.duration;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x00033C8B File Offset: 0x00031E8B
		public int StartTick
		{
			get
			{
				return this.startTick;
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00033C93 File Offset: 0x00031E93
		public ExtendedShakeRequest()
		{
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00033C9B File Offset: 0x00031E9B
		public ExtendedShakeRequest(float mag, int duration)
		{
			this.mag = mag;
			this.duration = duration;
			this.startTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00033CC1 File Offset: 0x00031EC1
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.mag, "mag", 0f, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
		}

		// Token: 0x04000431 RID: 1073
		private float mag;

		// Token: 0x04000432 RID: 1074
		private int duration;

		// Token: 0x04000433 RID: 1075
		private int startTick;
	}
}
