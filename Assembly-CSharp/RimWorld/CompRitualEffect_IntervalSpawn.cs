using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001738 RID: 5944
	public abstract class CompRitualEffect_IntervalSpawn : RitualVisualEffectComp
	{
		// Token: 0x17001958 RID: 6488
		// (get) Token: 0x06009427 RID: 37927 RVA: 0x0036030C File Offset: 0x0035E50C
		protected CompProperties_RitualEffectIntervalSpawn Props
		{
			get
			{
				return (CompProperties_RitualEffectIntervalSpawn)this.props;
			}
		}

		// Token: 0x06009428 RID: 37928 RVA: 0x0036031C File Offset: 0x0035E51C
		public override bool ShouldSpawnNow(LordJob_Ritual ritual)
		{
			return (this.Props.delay <= 0 || this.ticksPassed >= this.Props.delay) && (this.Props.maxBursts <= 0 || this.burstsDone < this.Props.maxBursts) && (this.lastSpawnTick == -1 || GenTicks.TicksGame - this.lastSpawnTick >= this.Props.spawnIntervalTicks);
		}

		// Token: 0x06009429 RID: 37929 RVA: 0x00360396 File Offset: 0x0035E596
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.ticksPassed += delta;
		}

		// Token: 0x0600942A RID: 37930 RVA: 0x003603AD File Offset: 0x0035E5AD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSpawnTick, "lastSpawnTick", -1, false);
			Scribe_Values.Look<int>(ref this.burstsDone, "burstsDone", 0, false);
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x0400597E RID: 22910
		public int lastSpawnTick = -1;

		// Token: 0x0400597F RID: 22911
		public int ticksPassed;

		// Token: 0x04005980 RID: 22912
		protected int burstsDone;
	}
}
