using System;

namespace Verse
{
	// Token: 0x0200059E RID: 1438
	public interface ISuspendableThingHolder : IThingHolder
	{
		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06002B07 RID: 11015
		bool IsContentsSuspended { get; }
	}
}
