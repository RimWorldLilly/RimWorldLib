using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001965 RID: 6501
	public class CompSelfhealHitpoints : ThingComp
	{
		// Token: 0x17001C42 RID: 7234
		// (get) Token: 0x0600A329 RID: 41769 RVA: 0x003A45B3 File Offset: 0x003A27B3
		public CompProperties_SelfhealHitpoints Props
		{
			get
			{
				return (CompProperties_SelfhealHitpoints)this.props;
			}
		}

		// Token: 0x0600A32A RID: 41770 RVA: 0x003A45C0 File Offset: 0x003A27C0
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassedSinceLastHeal, "ticksPassedSinceLastHeal", 0, false);
		}

		// Token: 0x0600A32B RID: 41771 RVA: 0x003A45D4 File Offset: 0x003A27D4
		public override void CompTickInterval(int delta)
		{
			this.TickInterval(delta);
		}

		// Token: 0x0600A32C RID: 41772 RVA: 0x003A45DD File Offset: 0x003A27DD
		public override void CompTickRare()
		{
			this.TickInterval(250);
		}

		// Token: 0x0600A32D RID: 41773 RVA: 0x003A45EA File Offset: 0x003A27EA
		public override void CompTickLong()
		{
			this.TickInterval(2000);
		}

		// Token: 0x0600A32E RID: 41774 RVA: 0x003A45F8 File Offset: 0x003A27F8
		private void TickInterval(int ticks)
		{
			this.ticksPassedSinceLastHeal += ticks;
			if (this.ticksPassedSinceLastHeal < this.Props.ticksPerHeal)
			{
				return;
			}
			this.ticksPassedSinceLastHeal = 0;
			if (this.parent.HitPoints < this.parent.MaxHitPoints)
			{
				ThingWithComps parent = this.parent;
				int hitPoints = parent.HitPoints;
				parent.HitPoints = hitPoints + 1;
			}
		}

		// Token: 0x04006062 RID: 24674
		public int ticksPassedSinceLastHeal;
	}
}
