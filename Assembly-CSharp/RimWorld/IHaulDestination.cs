using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A7E RID: 6782
	public interface IHaulDestination : IStoreSettingsParent
	{
		// Token: 0x17001E54 RID: 7764
		// (get) Token: 0x0600AB2D RID: 43821
		IntVec3 Position { get; }

		// Token: 0x17001E55 RID: 7765
		// (get) Token: 0x0600AB2E RID: 43822
		Map Map { get; }

		// Token: 0x17001E56 RID: 7766
		// (get) Token: 0x0600AB2F RID: 43823
		bool HaulDestinationEnabled { get; }

		// Token: 0x0600AB30 RID: 43824
		bool Accepts(Thing t);
	}
}
