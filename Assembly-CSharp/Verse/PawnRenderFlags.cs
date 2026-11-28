using System;

namespace Verse
{
	// Token: 0x02000394 RID: 916
	[Flags]
	public enum PawnRenderFlags : uint
	{
		// Token: 0x040016E8 RID: 5864
		None = 0U,
		// Token: 0x040016E9 RID: 5865
		Portrait = 1U,
		// Token: 0x040016EA RID: 5866
		HeadStump = 2U,
		// Token: 0x040016EB RID: 5867
		Invisible = 4U,
		// Token: 0x040016EC RID: 5868
		DrawNow = 8U,
		// Token: 0x040016ED RID: 5869
		Cache = 16U,
		// Token: 0x040016EE RID: 5870
		Headgear = 32U,
		// Token: 0x040016EF RID: 5871
		Clothes = 64U,
		// Token: 0x040016F0 RID: 5872
		NeverAimWeapon = 128U,
		// Token: 0x040016F1 RID: 5873
		StylingStation = 256U,
		// Token: 0x040016F2 RID: 5874
		NoBody = 512U,
		// Token: 0x040016F3 RID: 5875
		Statue = 1024U
	}
}
