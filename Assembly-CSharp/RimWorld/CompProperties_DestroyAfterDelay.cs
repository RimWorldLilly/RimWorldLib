using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200185B RID: 6235
	public class CompProperties_DestroyAfterDelay : CompProperties
	{
		// Token: 0x06009C0E RID: 39950 RVA: 0x00383566 File Offset: 0x00381766
		public CompProperties_DestroyAfterDelay()
		{
			this.compClass = typeof(CompDestroyAfterDelay);
		}

		// Token: 0x04005CC2 RID: 23746
		public int delayTicks;

		// Token: 0x04005CC3 RID: 23747
		public DestroyMode destroyMode;

		// Token: 0x04005CC4 RID: 23748
		public bool displayCountdownOnLabel;

		// Token: 0x04005CC5 RID: 23749
		[MustTranslate]
		public string countdownLabel;
	}
}
