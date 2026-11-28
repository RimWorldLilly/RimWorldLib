using System;

namespace RimWorld
{
	// Token: 0x02000E22 RID: 3618
	public class ThoughtWorker_DeadMansApparel : ThoughtWorker_ApparelThought
	{
		// Token: 0x06006133 RID: 24883 RVA: 0x00232134 File Offset: 0x00230334
		protected override bool ApparelCounts(Apparel apparel)
		{
			return apparel.WornByCorpse;
		}
	}
}
