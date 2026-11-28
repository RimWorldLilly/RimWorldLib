using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001929 RID: 6441
	public class CompProperties_OxygenPusher : CompProperties
	{
		// Token: 0x0600A1A3 RID: 41379 RVA: 0x0039C668 File Offset: 0x0039A868
		public CompProperties_OxygenPusher()
		{
			this.compClass = typeof(CompOxygenPusher);
		}

		// Token: 0x04005F7C RID: 24444
		public bool requiresPower = true;

		// Token: 0x04005F7D RID: 24445
		public float airPerSecondPerHundredCells = 0.1f;
	}
}
