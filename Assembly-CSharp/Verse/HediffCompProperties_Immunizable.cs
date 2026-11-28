using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200048D RID: 1165
	public class HediffCompProperties_Immunizable : HediffCompProperties
	{
		// Token: 0x0600205A RID: 8282 RVA: 0x000C2690 File Offset: 0x000C0890
		public HediffCompProperties_Immunizable()
		{
			this.compClass = typeof(HediffComp_Immunizable);
		}

		// Token: 0x04001912 RID: 6418
		public float immunityPerDayNotSick;

		// Token: 0x04001913 RID: 6419
		public float immunityPerDaySick;

		// Token: 0x04001914 RID: 6420
		public float severityPerDayNotImmune;

		// Token: 0x04001915 RID: 6421
		public float severityPerDayImmune;

		// Token: 0x04001916 RID: 6422
		public FloatRange severityPerDayNotImmuneRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x04001917 RID: 6423
		public List<HediffDefFactor> severityFactorsFromHediffs = new List<HediffDefFactor>();

		// Token: 0x04001918 RID: 6424
		public bool hidden;
	}
}
