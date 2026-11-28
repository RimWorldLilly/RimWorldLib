using System;

namespace Verse
{
	// Token: 0x02000304 RID: 772
	[Flags]
	public enum RegionType
	{
		// Token: 0x040013EC RID: 5100
		None = 0,
		// Token: 0x040013ED RID: 5101
		ImpassableFreeAirExchange = 1,
		// Token: 0x040013EE RID: 5102
		Normal = 2,
		// Token: 0x040013EF RID: 5103
		Portal = 4,
		// Token: 0x040013F0 RID: 5104
		Fence = 8,
		// Token: 0x040013F1 RID: 5105
		Set_Passable = 14,
		// Token: 0x040013F2 RID: 5106
		Set_Impassable = 1,
		// Token: 0x040013F3 RID: 5107
		Set_All = 15
	}
}
