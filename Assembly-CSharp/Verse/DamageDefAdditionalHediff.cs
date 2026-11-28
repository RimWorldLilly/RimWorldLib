using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200012E RID: 302
	public class DamageDefAdditionalHediff
	{
		// Token: 0x04000587 RID: 1415
		public HediffDef hediff;

		// Token: 0x04000588 RID: 1416
		public float severityPerDamageDealt = 0.1f;

		// Token: 0x04000589 RID: 1417
		public float severityFixed;

		// Token: 0x0400058A RID: 1418
		public StatDef victimSeverityScaling;

		// Token: 0x0400058B RID: 1419
		public bool inverseStatScaling;

		// Token: 0x0400058C RID: 1420
		public bool victimSeverityScalingByInvBodySize;
	}
}
