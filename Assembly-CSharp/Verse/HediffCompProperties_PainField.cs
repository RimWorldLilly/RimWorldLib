using System;

namespace Verse
{
	// Token: 0x02000461 RID: 1121
	public class HediffCompProperties_PainField : HediffCompProperties
	{
		// Token: 0x06001FC8 RID: 8136 RVA: 0x000C07A0 File Offset: 0x000BE9A0
		public HediffCompProperties_PainField()
		{
			this.compClass = typeof(HediffComp_PainField);
		}

		// Token: 0x040018BD RID: 6333
		public float painDistance = 5.9f;

		// Token: 0x040018BE RID: 6334
		public float painInRange = 0.25f;

		// Token: 0x040018BF RID: 6335
		public SimpleCurve activityMultiplier;

		// Token: 0x040018C0 RID: 6336
		public float psychicSensitivityMultiplier = 1f;

		// Token: 0x040018C1 RID: 6337
		public float activatedMinimum = 0.2f;

		// Token: 0x040018C2 RID: 6338
		public bool disableWhenSuppressed = true;
	}
}
