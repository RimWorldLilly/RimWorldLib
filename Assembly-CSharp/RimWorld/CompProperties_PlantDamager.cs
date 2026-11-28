using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001930 RID: 6448
	public class CompProperties_PlantDamager : CompProperties
	{
		// Token: 0x0600A1CB RID: 41419 RVA: 0x0039D886 File Offset: 0x0039BA86
		public CompProperties_PlantDamager()
		{
			this.compClass = typeof(CompPlantDamager);
		}

		// Token: 0x04005F92 RID: 24466
		public int ticksBetweenDamage;

		// Token: 0x04005F93 RID: 24467
		public float radius;

		// Token: 0x04005F94 RID: 24468
		public bool ignoreAnima = true;

		// Token: 0x04005F95 RID: 24469
		public float damagePerCycle;

		// Token: 0x04005F96 RID: 24470
		public int cycleCountOnSpawn;
	}
}
