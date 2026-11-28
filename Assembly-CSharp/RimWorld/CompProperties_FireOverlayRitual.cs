using System;

namespace RimWorld
{
	// Token: 0x02000ED9 RID: 3801
	public class CompProperties_FireOverlayRitual : CompProperties_FireOverlay
	{
		// Token: 0x0600639E RID: 25502 RVA: 0x0023BF6B File Offset: 0x0023A16B
		public CompProperties_FireOverlayRitual()
		{
			this.compClass = typeof(CompRitualFireOverlay);
		}

		// Token: 0x040037F3 RID: 14323
		public float minRitualProgress;
	}
}
