using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001981 RID: 6529
	public class CompProperties_SpawnFilthOnTakeDamage : CompProperties
	{
		// Token: 0x0600A3F8 RID: 41976 RVA: 0x003A94FC File Offset: 0x003A76FC
		public CompProperties_SpawnFilthOnTakeDamage()
		{
			this.compClass = typeof(CompSpawnerFilthOnTakeDamage);
		}

		// Token: 0x040060DF RID: 24799
		public ThingDef filthDef;

		// Token: 0x040060E0 RID: 24800
		public IntRange filthCountRange = IntRange.One;

		// Token: 0x040060E1 RID: 24801
		public float chance = 1f;
	}
}
