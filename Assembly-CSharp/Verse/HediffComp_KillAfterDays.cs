using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000450 RID: 1104
	public class HediffComp_KillAfterDays : HediffComp
	{
		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001F95 RID: 8085 RVA: 0x000BFCCD File Offset: 0x000BDECD
		public HediffCompProperties_KillAfterDays Props
		{
			get
			{
				return (HediffCompProperties_KillAfterDays)this.props;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001F96 RID: 8086 RVA: 0x000BFCDC File Offset: 0x000BDEDC
		public override string CompTipStringExtra
		{
			get
			{
				if (this.ticksLeft <= 0)
				{
					return null;
				}
				return "DeathIn".Translate(this.ticksLeft.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor)).Resolve().CapitalizeFirst();
			}
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x000BFD2A File Offset: 0x000BDF2A
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.ticksLeft = 60000 * this.Props.days;
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x000BFD44 File Offset: 0x000BDF44
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			this.ticksLeft -= delta;
			if (this.ticksLeft <= 0)
			{
				base.Pawn.Kill(null, this.parent);
			}
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x000BFD82 File Offset: 0x000BDF82
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x0400189D RID: 6301
		private int ticksLeft;
	}
}
