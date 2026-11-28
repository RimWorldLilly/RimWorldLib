using System;

namespace Verse
{
	// Token: 0x020004CB RID: 1227
	public class Hediff_SleepSuppression : Hediff
	{
		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06002232 RID: 8754 RVA: 0x000CB1A9 File Offset: 0x000C93A9
		public bool InRangeOfSuppressor
		{
			get
			{
				return GenTicks.TicksGame <= this.lastTickInRangeOfSuppressor + 60;
			}
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x000CB1BE File Offset: 0x000C93BE
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Sleep suppression"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x000CB1E5 File Offset: 0x000C93E5
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (!this.InRangeOfSuppressor)
			{
				this.Severity -= 0.000132f * (float)delta;
				return;
			}
			this.Severity += 6.4E-05f * (float)delta;
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06002235 RID: 8757 RVA: 0x000CB221 File Offset: 0x000C9421
		public override bool ShouldRemove
		{
			get
			{
				return !this.InRangeOfSuppressor && base.ShouldRemove;
			}
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x000CB233 File Offset: 0x000C9433
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastTickInRangeOfSuppressor, "lastTickInRangeOfSuppressor", 0, false);
		}

		// Token: 0x040019B9 RID: 6585
		public int lastTickInRangeOfSuppressor;

		// Token: 0x040019BA RID: 6586
		private const float FallRatePerHour = 0.33f;

		// Token: 0x040019BB RID: 6587
		private const float GainRatePerHour = 0.16f;

		// Token: 0x040019BC RID: 6588
		private const int SuppressorBufferTicks = 60;
	}
}
