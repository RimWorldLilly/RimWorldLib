using System;

namespace RimWorld
{
	// Token: 0x020011EE RID: 4590
	public class StorytellerCompProperties_SingleOnceFixed : StorytellerCompProperties
	{
		// Token: 0x06007228 RID: 29224 RVA: 0x0027EEBD File Offset: 0x0027D0BD
		public StorytellerCompProperties_SingleOnceFixed()
		{
			this.compClass = typeof(StorytellerComp_SingleOnceFixed);
		}

		// Token: 0x0400482D RID: 18477
		public IncidentDef incident;

		// Token: 0x0400482E RID: 18478
		public int fireAfterDaysPassed;

		// Token: 0x0400482F RID: 18479
		public RoyalTitleDef skipIfColonistHasMinTitle;

		// Token: 0x04004830 RID: 18480
		public bool skipIfOnExtremeBiome;

		// Token: 0x04004831 RID: 18481
		public int minColonistCount;
	}
}
