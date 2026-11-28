using System;

namespace RimWorld
{
	// Token: 0x020011DC RID: 4572
	public class StorytellerCompProperties_NoxiousHaze : StorytellerCompProperties
	{
		// Token: 0x060071F7 RID: 29175 RVA: 0x0027E7CC File Offset: 0x0027C9CC
		public StorytellerCompProperties_NoxiousHaze()
		{
			this.compClass = typeof(StorytellerComp_NoxiousHaze);
		}

		// Token: 0x0400480F RID: 18447
		public float lastFireMinMTBThreshold = 0.5f;

		// Token: 0x04004810 RID: 18448
		public float lastFireMaxMTBThreshold = 2f;
	}
}
