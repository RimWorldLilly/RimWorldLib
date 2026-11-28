using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001852 RID: 6226
	public class CompCountdown : ThingComp
	{
		// Token: 0x17001A9C RID: 6812
		// (get) Token: 0x06009BDF RID: 39903 RVA: 0x00382A5A File Offset: 0x00380C5A
		public CompProperties_Countdown Props
		{
			get
			{
				return (CompProperties_Countdown)this.props;
			}
		}

		// Token: 0x06009BE0 RID: 39904 RVA: 0x00382A68 File Offset: 0x00380C68
		public override string CompInspectStringExtra()
		{
			int num = this.endTick - GenTicks.TicksGame;
			if (num <= 0)
			{
				return null;
			}
			return this.Props.label.Formatted(num.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor).Named("DURATION")).Resolve();
		}

		// Token: 0x06009BE1 RID: 39905 RVA: 0x00382ABF File Offset: 0x00380CBF
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.endTick, "endTick", 0, false);
		}

		// Token: 0x04005CAC RID: 23724
		public int endTick;
	}
}
