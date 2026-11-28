using System;

namespace RimWorld
{
	// Token: 0x02000EF5 RID: 3829
	[Flags]
	public enum FilthSourceFlags
	{
		// Token: 0x04003853 RID: 14419
		None = 0,
		// Token: 0x04003854 RID: 14420
		Terrain = 1,
		// Token: 0x04003855 RID: 14421
		Natural = 2,
		// Token: 0x04003856 RID: 14422
		Unnatural = 4,
		// Token: 0x04003857 RID: 14423
		Pawn = 8,
		// Token: 0x04003858 RID: 14424
		Any = 15
	}
}
