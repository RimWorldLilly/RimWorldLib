using System;

namespace RimWorld
{
	// Token: 0x02001E4F RID: 7759
	[Flags]
	public enum SpectateRectSide
	{
		// Token: 0x040072A5 RID: 29349
		None = 0,
		// Token: 0x040072A6 RID: 29350
		Up = 1,
		// Token: 0x040072A7 RID: 29351
		Right = 2,
		// Token: 0x040072A8 RID: 29352
		Down = 4,
		// Token: 0x040072A9 RID: 29353
		Left = 8,
		// Token: 0x040072AA RID: 29354
		Vertical = 5,
		// Token: 0x040072AB RID: 29355
		Horizontal = 10,
		// Token: 0x040072AC RID: 29356
		All = 15
	}
}
