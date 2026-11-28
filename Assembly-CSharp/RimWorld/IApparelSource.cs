using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A7D RID: 6781
	public interface IApparelSource : IThingHolder
	{
		// Token: 0x17001E52 RID: 7762
		// (get) Token: 0x0600AB2A RID: 43818
		Map Map { get; }

		// Token: 0x17001E53 RID: 7763
		// (get) Token: 0x0600AB2B RID: 43819
		bool ApparelSourceEnabled { get; }

		// Token: 0x0600AB2C RID: 43820
		bool RemoveApparel(Apparel apparel);
	}
}
