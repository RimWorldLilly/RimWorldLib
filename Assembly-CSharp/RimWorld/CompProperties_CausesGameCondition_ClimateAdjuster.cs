using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC5 RID: 3781
	public class CompProperties_CausesGameCondition_ClimateAdjuster : CompProperties_CausesGameCondition
	{
		// Token: 0x06006363 RID: 25443 RVA: 0x0023B3C7 File Offset: 0x002395C7
		public CompProperties_CausesGameCondition_ClimateAdjuster()
		{
			this.compClass = typeof(CompCauseGameCondition_TemperatureOffset);
		}

		// Token: 0x04003799 RID: 14233
		public FloatRange temperatureOffsetRange = new FloatRange(-10f, 10f);
	}
}
