using System;

namespace LudeonTK
{
	// Token: 0x02000032 RID: 50
	[Flags]
	public enum AllowedGameStates
	{
		// Token: 0x04000079 RID: 121
		Invalid = 0,
		// Token: 0x0400007A RID: 122
		Entry = 1,
		// Token: 0x0400007B RID: 123
		Playing = 2,
		// Token: 0x0400007C RID: 124
		WorldRenderedNow = 4,
		// Token: 0x0400007D RID: 125
		IsCurrentlyOnMap = 8,
		// Token: 0x0400007E RID: 126
		HasGameCondition = 16,
		// Token: 0x0400007F RID: 127
		PlayingOnMap = 10,
		// Token: 0x04000080 RID: 128
		PlayingOnWorld = 6
	}
}
