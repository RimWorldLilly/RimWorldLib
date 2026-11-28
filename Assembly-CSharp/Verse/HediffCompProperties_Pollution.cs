using System;

namespace Verse
{
	// Token: 0x02000463 RID: 1123
	public class HediffCompProperties_Pollution : HediffCompProperties
	{
		// Token: 0x06001FCB RID: 8139 RVA: 0x000C080B File Offset: 0x000BEA0B
		public HediffCompProperties_Pollution()
		{
			this.compClass = typeof(HediffComp_Pollution);
		}

		// Token: 0x040018C3 RID: 6339
		public float pollutedSeverity;

		// Token: 0x040018C4 RID: 6340
		public float unpollutedSeverity;

		// Token: 0x040018C5 RID: 6341
		public int interval = 1;
	}
}
