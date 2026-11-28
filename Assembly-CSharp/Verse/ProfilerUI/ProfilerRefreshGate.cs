using System;

namespace Verse.ProfilerUI
{
	// Token: 0x0200079E RID: 1950
	internal struct ProfilerRefreshGate
	{
		// Token: 0x06003E35 RID: 15925 RVA: 0x0018BF37 File Offset: 0x0018A137
		public void Invalidate()
		{
			this.valid = false;
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x0018BF40 File Offset: 0x0018A140
		public bool ShouldRefresh(ProfilerSession session)
		{
			if (this.valid && this.lastRefreshTick == session.RefreshTick && this.lastRegistryVersion == ProfilerThreadRegistry.Version && this.lastSelectionVersion == ProfilerThreadRegistry.SelectionVersion)
			{
				return false;
			}
			this.valid = true;
			this.lastRefreshTick = session.RefreshTick;
			this.lastRegistryVersion = ProfilerThreadRegistry.Version;
			this.lastSelectionVersion = ProfilerThreadRegistry.SelectionVersion;
			return true;
		}

		// Token: 0x040028FC RID: 10492
		private bool valid;

		// Token: 0x040028FD RID: 10493
		private int lastRefreshTick;

		// Token: 0x040028FE RID: 10494
		private int lastRegistryVersion;

		// Token: 0x040028FF RID: 10495
		private int lastSelectionVersion;
	}
}
