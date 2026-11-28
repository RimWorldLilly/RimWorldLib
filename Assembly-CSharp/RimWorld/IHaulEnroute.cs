using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A7F RID: 6783
	public interface IHaulEnroute : ILoadReferenceable
	{
		// Token: 0x17001E57 RID: 7767
		// (get) Token: 0x0600AB31 RID: 43825
		Map Map { get; }

		// Token: 0x0600AB32 RID: 43826
		int SpaceRemainingFor(ThingDef stuff);
	}
}
