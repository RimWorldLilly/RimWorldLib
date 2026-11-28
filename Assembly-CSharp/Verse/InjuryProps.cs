using System;

namespace Verse
{
	// Token: 0x02000175 RID: 373
	public class InjuryProps
	{
		// Token: 0x040008D2 RID: 2258
		public float painPerSeverity = 1f;

		// Token: 0x040008D3 RID: 2259
		public float averagePainPerSeverityPermanent = 0.5f;

		// Token: 0x040008D4 RID: 2260
		public float bleedRate;

		// Token: 0x040008D5 RID: 2261
		public bool canMerge;

		// Token: 0x040008D6 RID: 2262
		public string destroyedLabel;

		// Token: 0x040008D7 RID: 2263
		public string destroyedOutLabel;

		// Token: 0x040008D8 RID: 2264
		public bool useRemovedLabel;

		// Token: 0x040008D9 RID: 2265
		public bool alwaysUseDestroyedLabel;
	}
}
