using System;

namespace Verse
{
	// Token: 0x020004AF RID: 1199
	public class Hediff_FrenzyField : Hediff
	{
		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06002138 RID: 8504 RVA: 0x000C5F42 File Offset: 0x000C4142
		public bool InRangeOfInducer
		{
			get
			{
				return GenTicks.TicksGame <= this.lastTickInRangeOfInducer + 60;
			}
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x000C5F57 File Offset: 0x000C4157
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Frenzy field"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x000C5F7E File Offset: 0x000C417E
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.InRangeOfInducer)
			{
				this.Severity += 0.000132f * (float)delta;
				return;
			}
			this.Severity -= 2.2400001E-05f * (float)delta;
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x0600213B RID: 8507 RVA: 0x000C5FBA File Offset: 0x000C41BA
		public override bool ShouldRemove
		{
			get
			{
				return !this.InRangeOfInducer && base.ShouldRemove;
			}
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x000C5FCC File Offset: 0x000C41CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastTickInRangeOfInducer, "lastTickInRangeOfInducer", 0, false);
		}

		// Token: 0x04001960 RID: 6496
		public int lastTickInRangeOfInducer;

		// Token: 0x04001961 RID: 6497
		private const float SeverityRaisePerTick = 0.000132f;

		// Token: 0x04001962 RID: 6498
		private const float SeverityFallPerTick = 2.2400001E-05f;

		// Token: 0x04001963 RID: 6499
		private const int InducerBufferTicks = 60;
	}
}
