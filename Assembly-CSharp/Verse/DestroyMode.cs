using System;

namespace Verse
{
	// Token: 0x02000599 RID: 1433
	public enum DestroyMode : byte
	{
		// Token: 0x04001D3A RID: 7482
		Vanish,
		// Token: 0x04001D3B RID: 7483
		WillReplace,
		// Token: 0x04001D3C RID: 7484
		KillFinalize,
		// Token: 0x04001D3D RID: 7485
		KillFinalizeLeavingsOnly,
		// Token: 0x04001D3E RID: 7486
		Deconstruct,
		// Token: 0x04001D3F RID: 7487
		FailConstruction,
		// Token: 0x04001D40 RID: 7488
		Cancel,
		// Token: 0x04001D41 RID: 7489
		Refund,
		// Token: 0x04001D42 RID: 7490
		QuestLogic
	}
}
