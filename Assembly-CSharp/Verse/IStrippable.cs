using System;

namespace Verse
{
	// Token: 0x020004DA RID: 1242
	public interface IStrippable
	{
		// Token: 0x06002454 RID: 9300
		bool AnythingToStrip();

		// Token: 0x06002455 RID: 9301
		void Strip(bool notifyFaction = true);
	}
}
