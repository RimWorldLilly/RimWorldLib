using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E4 RID: 4580
	public class StorytellerCompProperties_RandomMain : StorytellerCompProperties
	{
		// Token: 0x0600720F RID: 29199 RVA: 0x0027EBD8 File Offset: 0x0027CDD8
		public StorytellerCompProperties_RandomMain()
		{
			this.compClass = typeof(StorytellerComp_RandomMain);
		}

		// Token: 0x0400481F RID: 18463
		public float mtbDays;

		// Token: 0x04004820 RID: 18464
		public List<IncidentCategoryEntry> categoryWeights = new List<IncidentCategoryEntry>();

		// Token: 0x04004821 RID: 18465
		public float maxThreatBigIntervalDays = 99999f;

		// Token: 0x04004822 RID: 18466
		public FloatRange randomPointsFactorRange = new FloatRange(0.5f, 1.5f);

		// Token: 0x04004823 RID: 18467
		public bool skipThreatBigIfRaidBeacon;

		// Token: 0x04004824 RID: 18468
		public float spaceMtbDayFactor = 2f;

		// Token: 0x04004825 RID: 18469
		public float spaceMinSpacingDays = 2f;
	}
}
