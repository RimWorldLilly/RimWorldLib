using System;

namespace Verse
{
	// Token: 0x02000495 RID: 1173
	public class HediffCompProperties_SeverityPerSecond : HediffCompProperties
	{
		// Token: 0x06002085 RID: 8325 RVA: 0x000C3144 File Offset: 0x000C1344
		public HediffCompProperties_SeverityPerSecond()
		{
			this.compClass = typeof(HediffComp_SeverityPerSecond);
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x000C3167 File Offset: 0x000C1367
		public float CalculateSeverityPerSecond()
		{
			return this.severityPerSecond + this.severityPerSecondRange.RandomInRange;
		}

		// Token: 0x04001927 RID: 6439
		public float severityPerSecond;

		// Token: 0x04001928 RID: 6440
		public FloatRange severityPerSecondRange = FloatRange.Zero;
	}
}
