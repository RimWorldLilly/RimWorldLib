using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001927 RID: 6439
	public class CompProperties_OverseerSubject : CompProperties
	{
		// Token: 0x0600A18E RID: 41358 RVA: 0x0039BE3E File Offset: 0x0039A03E
		public CompProperties_OverseerSubject()
		{
			this.compClass = typeof(CompOverseerSubject);
		}

		// Token: 0x04005F74 RID: 24436
		public EffecterDef needsOverseerEffect;

		// Token: 0x04005F75 RID: 24437
		public int delayUntilFeralCheck = 60000;

		// Token: 0x04005F76 RID: 24438
		public int feralMtbDays = 10;

		// Token: 0x04005F77 RID: 24439
		public int feralCascadeRadialDistance = 25;
	}
}
