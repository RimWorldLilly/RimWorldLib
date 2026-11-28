using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A80 RID: 6784
	public interface IHaulSource : IStoreSettingsParent, IThingHolder
	{
		// Token: 0x17001E58 RID: 7768
		// (get) Token: 0x0600AB33 RID: 43827
		Map Map { get; }

		// Token: 0x17001E59 RID: 7769
		// (get) Token: 0x0600AB34 RID: 43828
		bool HaulSourceEnabled { get; }
	}
}
