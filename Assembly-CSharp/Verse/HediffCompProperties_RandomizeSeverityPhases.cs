using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000490 RID: 1168
	public class HediffCompProperties_RandomizeSeverityPhases : HediffCompProperties_Randomizer
	{
		// Token: 0x0600206D RID: 8301 RVA: 0x000C2B47 File Offset: 0x000C0D47
		public HediffCompProperties_RandomizeSeverityPhases()
		{
			this.compClass = typeof(HediffComp_RandomizeSeverityPhases);
		}

		// Token: 0x0400191B RID: 6427
		public List<HediffCompProperties_RandomizeSeverityPhases.Phase> phases;

		// Token: 0x0400191C RID: 6428
		[MustTranslate]
		public string notifyMessage;

		// Token: 0x020026EC RID: 9964
		public class Phase
		{
			// Token: 0x04009F8B RID: 40843
			public HediffCompProperties comp;

			// Token: 0x04009F8C RID: 40844
			[MustTranslate]
			public string labelPrefix;

			// Token: 0x04009F8D RID: 40845
			[MustTranslate]
			public string descriptionExtra;

			// Token: 0x04009F8E RID: 40846
			public float severityPerDay;
		}
	}
}
