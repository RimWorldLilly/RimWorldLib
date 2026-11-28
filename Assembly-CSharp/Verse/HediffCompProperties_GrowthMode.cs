using System;

namespace Verse
{
	// Token: 0x02000445 RID: 1093
	public class HediffCompProperties_GrowthMode : HediffCompProperties
	{
		// Token: 0x06001F60 RID: 8032 RVA: 0x000BEC98 File Offset: 0x000BCE98
		public HediffCompProperties_GrowthMode()
		{
			this.compClass = typeof(HediffComp_GrowthMode);
		}

		// Token: 0x0400187B RID: 6267
		public float severityPerDayGrowing;

		// Token: 0x0400187C RID: 6268
		public float severityPerDayRemission;

		// Token: 0x0400187D RID: 6269
		public FloatRange severityPerDayGrowingRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x0400187E RID: 6270
		public FloatRange severityPerDayRemissionRandomFactor = new FloatRange(1f, 1f);
	}
}
