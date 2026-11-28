using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.ProfilerUI
{
	// Token: 0x02000794 RID: 1940
	public sealed class ProfilerPrefsData
	{
		// Token: 0x04002899 RID: 10393
		public bool startOnBoot;

		// Token: 0x0400289A RID: 10394
		public bool keepRunningWhenClosed;

		// Token: 0x0400289B RID: 10395
		public int historyCapacity = 2000;

		// Token: 0x0400289C RID: 10396
		public bool allocTracking;

		// Token: 0x0400289D RID: 10397
		public bool mainThreadOnly = true;

		// Token: 0x0400289E RID: 10398
		public bool ignoreThrottle = true;

		// Token: 0x0400289F RID: 10399
		public List<string> instrumentationTargets;

		// Token: 0x040028A0 RID: 10400
		public bool instrumentation;

		// Token: 0x040028A1 RID: 10401
		public bool autoMuteTrivial = true;

		// Token: 0x040028A2 RID: 10402
		public float budgetMs;

		// Token: 0x040028A3 RID: 10403
		public bool foldRecursion = true;

		// Token: 0x040028A4 RID: 10404
		public Rect windowRect;
	}
}
