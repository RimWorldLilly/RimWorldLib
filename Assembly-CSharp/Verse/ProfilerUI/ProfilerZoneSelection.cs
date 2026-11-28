using System;

namespace Verse.ProfilerUI
{
	// Token: 0x0200078B RID: 1931
	public readonly struct ProfilerZoneSelection
	{
		// Token: 0x06003D08 RID: 15624 RVA: 0x00181ADE File Offset: 0x0017FCDE
		public ProfilerZoneSelection(int ordinal, int threadId, int pathId)
		{
			this.ordinal = ordinal;
			this.threadId = threadId;
			this.pathId = pathId;
		}

		// Token: 0x040027AE RID: 10158
		public readonly int ordinal;

		// Token: 0x040027AF RID: 10159
		public readonly int threadId;

		// Token: 0x040027B0 RID: 10160
		public readonly int pathId;
	}
}
