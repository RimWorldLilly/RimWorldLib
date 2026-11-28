using System;

namespace Verse
{
	// Token: 0x02000476 RID: 1142
	public class HediffComp_SelfHeal : HediffComp
	{
		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001FFF RID: 8191 RVA: 0x000C140F File Offset: 0x000BF60F
		public HediffCompProperties_SelfHeal Props
		{
			get
			{
				return (HediffCompProperties_SelfHeal)this.props;
			}
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x000C141C File Offset: 0x000BF61C
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceHeal, "ticksSinceHeal", 0, false);
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x000C1430 File Offset: 0x000BF630
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksSinceHeal++;
			if (this.ticksSinceHeal > this.Props.healIntervalTicksStanding)
			{
				severityAdjustment -= this.Props.healAmount;
				this.ticksSinceHeal = 0;
			}
		}

		// Token: 0x040018E8 RID: 6376
		public int ticksSinceHeal;
	}
}
