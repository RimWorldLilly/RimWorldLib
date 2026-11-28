using System;

namespace RimWorld
{
	// Token: 0x02001A5B RID: 6747
	public interface IOpenable
	{
		// Token: 0x17001DCF RID: 7631
		// (get) Token: 0x0600A971 RID: 43377
		bool CanOpen { get; }

		// Token: 0x17001DD0 RID: 7632
		// (get) Token: 0x0600A972 RID: 43378
		int OpenTicks { get; }

		// Token: 0x0600A973 RID: 43379
		void Open();
	}
}
