using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC0 RID: 3776
	public class CompProperties_AnimalInsanityPulser : CompProperties
	{
		// Token: 0x0600635E RID: 25438 RVA: 0x0023B30A File Offset: 0x0023950A
		public CompProperties_AnimalInsanityPulser()
		{
			this.compClass = typeof(CompAnimalInsanityPulser);
		}

		// Token: 0x0400378B RID: 14219
		public IntRange pulseInterval = new IntRange(60000, 150000);

		// Token: 0x0400378C RID: 14220
		public int radius = 25;
	}
}
