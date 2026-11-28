using System;

namespace Verse
{
	// Token: 0x0200047E RID: 1150
	public class HediffCompProperties_SeverityFromHemogen : HediffCompProperties
	{
		// Token: 0x06002017 RID: 8215 RVA: 0x000C1794 File Offset: 0x000BF994
		public HediffCompProperties_SeverityFromHemogen()
		{
			this.compClass = typeof(HediffComp_SeverityFromHemogen);
		}

		// Token: 0x040018F1 RID: 6385
		public float severityPerHourEmpty;

		// Token: 0x040018F2 RID: 6386
		public float severityPerHourHemogen;
	}
}
