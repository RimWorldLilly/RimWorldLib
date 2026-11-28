using System;

namespace Verse
{
	// Token: 0x02000137 RID: 311
	public class CompProperties_HeatPusher : CompProperties
	{
		// Token: 0x060009F5 RID: 2549 RVA: 0x00037081 File Offset: 0x00035281
		public CompProperties_HeatPusher()
		{
			this.compClass = typeof(CompHeatPusher);
		}

		// Token: 0x0400059A RID: 1434
		public float heatPerSecond;

		// Token: 0x0400059B RID: 1435
		public float heatPushMaxTemperature = 99999f;

		// Token: 0x0400059C RID: 1436
		public float heatPushMinTemperature = -99999f;
	}
}
