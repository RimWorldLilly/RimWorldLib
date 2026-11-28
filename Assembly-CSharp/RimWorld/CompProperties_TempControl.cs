using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE9 RID: 3817
	public class CompProperties_TempControl : CompProperties
	{
		// Token: 0x060063C7 RID: 25543 RVA: 0x0023C544 File Offset: 0x0023A744
		public CompProperties_TempControl()
		{
			this.compClass = typeof(CompTempControl);
		}

		// Token: 0x04003829 RID: 14377
		public float energyPerSecond = 12f;

		// Token: 0x0400382A RID: 14378
		public float defaultTargetTemperature = 21f;

		// Token: 0x0400382B RID: 14379
		public float minTargetTemperature = -50f;

		// Token: 0x0400382C RID: 14380
		public float maxTargetTemperature = 50f;

		// Token: 0x0400382D RID: 14381
		public float lowPowerConsumptionFactor = 0.1f;

		// Token: 0x0400382E RID: 14382
		[MustTranslate]
		public string inspectString;
	}
}
