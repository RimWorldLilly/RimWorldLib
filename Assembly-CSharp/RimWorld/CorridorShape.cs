using System;

namespace RimWorld
{
	// Token: 0x02001264 RID: 4708
	[Flags]
	public enum CorridorShape : byte
	{
		// Token: 0x04004943 RID: 18755
		Straight = 1,
		// Token: 0x04004944 RID: 18756
		Cross = 2,
		// Token: 0x04004945 RID: 18757
		T = 4,
		// Token: 0x04004946 RID: 18758
		H = 8,
		// Token: 0x04004947 RID: 18759
		AsymmetricCross = 16,
		// Token: 0x04004948 RID: 18760
		All = 31
	}
}
