using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000881 RID: 2177
	public abstract class MentalState_SlaughterThing : MentalState
	{
		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x060043E8 RID: 17384 RVA: 0x001AAEA1 File Offset: 0x001A90A1
		protected virtual int MinTicksBetweenSlaughter
		{
			get
			{
				return 3750;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x060043E9 RID: 17385 RVA: 0x001AAEA8 File Offset: 0x001A90A8
		protected virtual int MaxThingsSlaughtered
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x060043EA RID: 17386 RVA: 0x001AAEAB File Offset: 0x001A90AB
		public bool SlaughteredRecently
		{
			get
			{
				return this.lastSlaughterTicks >= 0 && Find.TickManager.TicksGame - this.lastSlaughterTicks < this.MinTicksBetweenSlaughter;
			}
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x060043EB RID: 17387 RVA: 0x001AAED1 File Offset: 0x001A90D1
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.lastSlaughterTicks >= 0;
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x060043EC RID: 17388
		protected abstract bool SlaughterTargetAvailable { get; }

		// Token: 0x060043ED RID: 17389 RVA: 0x001AAEDF File Offset: 0x001A90DF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSlaughterTicks, "lastSlaughterTicks", 0, false);
			Scribe_Values.Look<int>(ref this.thingsSlaughtered, "thingsSlaughtered", 0, false);
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x001AAF0C File Offset: 0x001A910C
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			if (this.pawn.IsHashIntervalTick(600, delta) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.Slaughter) && !this.SlaughterTargetAvailable)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x001AAF65 File Offset: 0x001A9165
		public override void Notify_SlaughteredTarget()
		{
			this.lastSlaughterTicks = Find.TickManager.TicksGame;
			this.thingsSlaughtered++;
			if (this.thingsSlaughtered >= this.MaxThingsSlaughtered)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x04002C2C RID: 11308
		protected int lastSlaughterTicks = -1;

		// Token: 0x04002C2D RID: 11309
		[LoadAlias("animalsSlaughtered")]
		protected int thingsSlaughtered;

		// Token: 0x04002C2E RID: 11310
		protected const int NoThingToSlaughterCheckInterval = 600;
	}
}
