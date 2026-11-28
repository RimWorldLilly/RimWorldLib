using System;

namespace Verse
{
	// Token: 0x02000338 RID: 824
	[Flags]
	public enum DdsPixelFormatFlags : uint
	{
		// Token: 0x040014DF RID: 5343
		None = 0U,
		// Token: 0x040014E0 RID: 5344
		AlphaPixels = 1U,
		// Token: 0x040014E1 RID: 5345
		Alpha = 2U,
		// Token: 0x040014E2 RID: 5346
		FourCC = 4U,
		// Token: 0x040014E3 RID: 5347
		RGB = 64U,
		// Token: 0x040014E4 RID: 5348
		YUV = 512U,
		// Token: 0x040014E5 RID: 5349
		Luminance = 131072U
	}
}
