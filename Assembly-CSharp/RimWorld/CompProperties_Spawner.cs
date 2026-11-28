using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200197D RID: 6525
	public class CompProperties_Spawner : CompProperties
	{
		// Token: 0x0600A3DD RID: 41949 RVA: 0x003A8D08 File Offset: 0x003A6F08
		public CompProperties_Spawner()
		{
			this.compClass = typeof(CompSpawner);
		}

		// Token: 0x040060CD RID: 24781
		public ThingDef thingToSpawn;

		// Token: 0x040060CE RID: 24782
		public int spawnCount = 1;

		// Token: 0x040060CF RID: 24783
		public IntRange spawnIntervalRange = new IntRange(100, 100);

		// Token: 0x040060D0 RID: 24784
		public int spawnMaxAdjacent = -1;

		// Token: 0x040060D1 RID: 24785
		public bool spawnForbidden;

		// Token: 0x040060D2 RID: 24786
		public bool requiresPower;

		// Token: 0x040060D3 RID: 24787
		public bool writeTimeLeftToSpawn;

		// Token: 0x040060D4 RID: 24788
		public bool showMessageIfOwned;

		// Token: 0x040060D5 RID: 24789
		[NoTranslate]
		public string saveKeysPrefix;

		// Token: 0x040060D6 RID: 24790
		public bool inheritFaction;
	}
}
