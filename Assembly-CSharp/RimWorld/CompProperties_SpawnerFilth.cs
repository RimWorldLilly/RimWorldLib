using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001980 RID: 6528
	public class CompProperties_SpawnerFilth : CompProperties
	{
		// Token: 0x0600A3F7 RID: 41975 RVA: 0x003A94BC File Offset: 0x003A76BC
		public CompProperties_SpawnerFilth()
		{
			this.compClass = typeof(CompSpawnerFilth);
		}

		// Token: 0x040060D9 RID: 24793
		public ThingDef filthDef;

		// Token: 0x040060DA RID: 24794
		public int spawnCountOnSpawn = 5;

		// Token: 0x040060DB RID: 24795
		public float spawnMtbHours = 12f;

		// Token: 0x040060DC RID: 24796
		public float spawnRadius = 3f;

		// Token: 0x040060DD RID: 24797
		public float spawnEveryDays = -1f;

		// Token: 0x040060DE RID: 24798
		public RotStage? requiredRotStage;
	}
}
