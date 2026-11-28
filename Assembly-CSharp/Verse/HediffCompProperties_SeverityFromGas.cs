using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000479 RID: 1145
	public class HediffCompProperties_SeverityFromGas : HediffCompProperties
	{
		// Token: 0x06002008 RID: 8200 RVA: 0x000C14D9 File Offset: 0x000BF6D9
		public HediffCompProperties_SeverityFromGas()
		{
			this.compClass = typeof(HediffComp_SeverityFromGas);
		}

		// Token: 0x040018E9 RID: 6377
		public GasType gasType;

		// Token: 0x040018EA RID: 6378
		public float severityGasDensityFactor;

		// Token: 0x040018EB RID: 6379
		public int intervalTicks = 60;

		// Token: 0x040018EC RID: 6380
		public float severityNotExposed;

		// Token: 0x040018ED RID: 6381
		public StatDef exposureStatFactor;
	}
}
