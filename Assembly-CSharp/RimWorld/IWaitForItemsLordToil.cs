using System;

namespace RimWorld
{
	// Token: 0x02000D02 RID: 3330
	public interface IWaitForItemsLordToil
	{
		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x06005DCF RID: 24015
		int CountRemaining { get; }

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x06005DD0 RID: 24016
		bool HasAllRequestedItems { get; }
	}
}
