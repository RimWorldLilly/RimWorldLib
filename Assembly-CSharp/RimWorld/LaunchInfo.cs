using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AB6 RID: 6838
	public class LaunchInfo : IExposable
	{
		// Token: 0x0600AD53 RID: 44371 RVA: 0x003D1748 File Offset: 0x003CF948
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pilot, "pilot", false);
			Scribe_References.Look<Pawn>(ref this.copilot, "copilot", false);
			Scribe_Values.Look<float>(ref this.quality, "quality", 0f, false);
			Scribe_Values.Look<bool>(ref this.doNegativeOutcome, "doNegativeOutcome", false, false);
		}

		// Token: 0x04006478 RID: 25720
		public Pawn pilot;

		// Token: 0x04006479 RID: 25721
		public Pawn copilot;

		// Token: 0x0400647A RID: 25722
		public float quality;

		// Token: 0x0400647B RID: 25723
		public bool doNegativeOutcome;
	}
}
