using System;

namespace Verse
{
	// Token: 0x02000492 RID: 1170
	public class HediffCompProperties_SeverityPerDay : HediffCompProperties
	{
		// Token: 0x06002078 RID: 8312 RVA: 0x000C2E36 File Offset: 0x000C1036
		public HediffCompProperties_SeverityPerDay()
		{
			this.compClass = typeof(HediffComp_SeverityPerDay);
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x000C2E64 File Offset: 0x000C1064
		public float CalculateSeverityPerDay()
		{
			float num = this.severityPerDay + this.severityPerDayRange.RandomInRange;
			if (Rand.Chance(this.reverseSeverityChangeChance))
			{
				num *= -1f;
			}
			return num;
		}

		// Token: 0x0400191E RID: 6430
		public float severityPerDay;

		// Token: 0x0400191F RID: 6431
		public bool showDaysToRecover;

		// Token: 0x04001920 RID: 6432
		public bool showHoursToRecover;

		// Token: 0x04001921 RID: 6433
		public float mechanitorFactor = 1f;

		// Token: 0x04001922 RID: 6434
		public float reverseSeverityChangeChance;

		// Token: 0x04001923 RID: 6435
		public FloatRange severityPerDayRange = FloatRange.Zero;

		// Token: 0x04001924 RID: 6436
		public float minAge;
	}
}
