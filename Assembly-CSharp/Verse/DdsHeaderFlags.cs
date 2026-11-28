using System;

namespace Verse
{
	// Token: 0x02000336 RID: 822
	[Flags]
	public enum DdsHeaderFlags : uint
	{
		// Token: 0x040014CD RID: 5325
		Caps = 1U,
		// Token: 0x040014CE RID: 5326
		Height = 2U,
		// Token: 0x040014CF RID: 5327
		Width = 4U,
		// Token: 0x040014D0 RID: 5328
		PitchOrLinearSize = 8U,
		// Token: 0x040014D1 RID: 5329
		PixelFormat = 4096U,
		// Token: 0x040014D2 RID: 5330
		MipMapCount = 131072U,
		// Token: 0x040014D3 RID: 5331
		LinearSize = 524288U,
		// Token: 0x040014D4 RID: 5332
		Depth = 8388608U
	}
}
