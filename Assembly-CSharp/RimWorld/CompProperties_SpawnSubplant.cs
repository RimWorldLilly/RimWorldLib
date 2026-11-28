using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001991 RID: 6545
	public class CompProperties_SpawnSubplant : CompProperties
	{
		// Token: 0x0400611A RID: 24858
		public ThingDef subplant;

		// Token: 0x0400611B RID: 24859
		[NoTranslate]
		public string saveKeysPrefix;

		// Token: 0x0400611C RID: 24860
		public SoundDef spawnSound;

		// Token: 0x0400611D RID: 24861
		public float maxRadius;

		// Token: 0x0400611E RID: 24862
		public float subplantSpawnDays;

		// Token: 0x0400611F RID: 24863
		public float minGrowthForSpawn = 0.2f;

		// Token: 0x04006120 RID: 24864
		public FloatRange? initialGrowthRange;

		// Token: 0x04006121 RID: 24865
		public bool canSpawnOverPlayerSownPlants = true;

		// Token: 0x04006122 RID: 24866
		public List<ThingDef> plantsToNotOverwrite;

		// Token: 0x04006123 RID: 24867
		public SimpleCurve chanceOverDistance;

		// Token: 0x04006124 RID: 24868
		public ThingDef dontWipePlant;

		// Token: 0x04006125 RID: 24869
		public int maxPlants = -1;

		// Token: 0x04006126 RID: 24870
		public int checkRespawnIntervalHours = 18;
	}
}
