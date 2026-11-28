using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A22 RID: 6690
	public class CompInitiatable : ThingComp
	{
		// Token: 0x17001D74 RID: 7540
		// (get) Token: 0x0600A805 RID: 43013 RVA: 0x003BC11A File Offset: 0x003BA31A
		public bool Initiated
		{
			get
			{
				return this.Delay <= 0 || (this.parent.spawnedTick >= 0 && Find.TickManager.TicksGame >= this.parent.spawnedTick + this.Delay);
			}
		}

		// Token: 0x17001D75 RID: 7541
		// (get) Token: 0x0600A806 RID: 43014 RVA: 0x003BC158 File Offset: 0x003BA358
		private int Delay
		{
			get
			{
				if (this.initiationDelayTicksOverride <= 0)
				{
					return this.Props.initiationDelayTicks;
				}
				return this.initiationDelayTicksOverride;
			}
		}

		// Token: 0x17001D76 RID: 7542
		// (get) Token: 0x0600A807 RID: 43015 RVA: 0x003BC175 File Offset: 0x003BA375
		private CompProperties_Initiatable Props
		{
			get
			{
				return (CompProperties_Initiatable)this.props;
			}
		}

		// Token: 0x0600A808 RID: 43016 RVA: 0x003BC184 File Offset: 0x003BA384
		public override string CompInspectStringExtra()
		{
			if (!this.Initiated)
			{
				return "InitiatesIn".Translate() + ": " + (this.parent.spawnedTick + this.Delay - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false);
			}
			return base.CompInspectStringExtra();
		}

		// Token: 0x0600A809 RID: 43017 RVA: 0x003BC1E5 File Offset: 0x003BA3E5
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.initiationDelayTicksOverride, "initiationDelayTicksOverride", 0, false);
		}

		// Token: 0x040062C7 RID: 25287
		public int initiationDelayTicksOverride;
	}
}
