using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B0 RID: 6576
	public class CompProperties_TemperatureRuinable : CompProperties
	{
		// Token: 0x0600A513 RID: 42259 RVA: 0x003AEE75 File Offset: 0x003AD075
		public CompProperties_TemperatureRuinable()
		{
			this.compClass = typeof(CompTemperatureRuinable);
		}

		// Token: 0x04006183 RID: 24963
		public float minSafeTemperature;

		// Token: 0x04006184 RID: 24964
		public float maxSafeTemperature = 100f;

		// Token: 0x04006185 RID: 24965
		public float progressPerDegreePerTick = 1E-05f;
	}
}
