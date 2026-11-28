using System;

namespace RimWorld
{
	// Token: 0x02001B76 RID: 7030
	public interface ITraderRestockingInfoProvider
	{
		// Token: 0x1700208F RID: 8335
		// (get) Token: 0x0600B47B RID: 46203
		bool EverVisited { get; }

		// Token: 0x17002090 RID: 8336
		// (get) Token: 0x0600B47C RID: 46204
		bool RestockedSinceLastVisit { get; }

		// Token: 0x17002091 RID: 8337
		// (get) Token: 0x0600B47D RID: 46205
		int NextRestockTick { get; }
	}
}
