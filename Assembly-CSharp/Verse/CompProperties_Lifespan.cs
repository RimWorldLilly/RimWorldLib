using System;

namespace Verse
{
	// Token: 0x02000138 RID: 312
	public class CompProperties_Lifespan : CompProperties
	{
		// Token: 0x060009F6 RID: 2550 RVA: 0x000370AF File Offset: 0x000352AF
		public CompProperties_Lifespan()
		{
			this.compClass = typeof(CompLifespan);
		}

		// Token: 0x0400059D RID: 1437
		public int lifespanTicks = 100;

		// Token: 0x0400059E RID: 1438
		public EffecterDef expireEffect;

		// Token: 0x0400059F RID: 1439
		public ThingDef plantDefToSpawn;
	}
}
