using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018AD RID: 6317
	public class CompProperties_GrowsFleshmassTendrils : CompProperties
	{
		// Token: 0x06009E2F RID: 40495 RVA: 0x0038C733 File Offset: 0x0038A933
		public CompProperties_GrowsFleshmassTendrils()
		{
			this.compClass = typeof(CompGrowsFleshmassTendrils);
		}

		// Token: 0x04005D92 RID: 23954
		public int mtbGrowthCycleHours;

		// Token: 0x04005D93 RID: 23955
		public int minGrowthCycleHours;

		// Token: 0x04005D94 RID: 23956
		public int maxGrowthCycleHours;

		// Token: 0x04005D95 RID: 23957
		public SimpleCurve startingGrowthPointsByThreat;

		// Token: 0x04005D96 RID: 23958
		public SimpleCurve growthCyclePointsByThreat;

		// Token: 0x04005D97 RID: 23959
		public SimpleCurve fleshbeastPointsByThreat;

		// Token: 0x04005D98 RID: 23960
		public IntRange? fleshbeastBirthThresholdRange;

		// Token: 0x04005D99 RID: 23961
		public bool spawnFleshbeasts = true;
	}
}
