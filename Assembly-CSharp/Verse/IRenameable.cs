using System;

namespace Verse
{
	// Token: 0x0200032B RID: 811
	public interface IRenameable
	{
		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060018D3 RID: 6355
		// (set) Token: 0x060018D4 RID: 6356
		string RenamableLabel { get; set; }

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060018D5 RID: 6357
		string BaseLabel { get; }

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060018D6 RID: 6358
		string InspectLabel { get; }
	}
}
