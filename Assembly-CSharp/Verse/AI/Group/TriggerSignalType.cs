using System;

namespace Verse.AI.Group
{
	// Token: 0x02000966 RID: 2406
	public enum TriggerSignalType : byte
	{
		// Token: 0x04002F11 RID: 12049
		Undefined,
		// Token: 0x04002F12 RID: 12050
		Tick,
		// Token: 0x04002F13 RID: 12051
		Memo,
		// Token: 0x04002F14 RID: 12052
		PawnDamaged,
		// Token: 0x04002F15 RID: 12053
		PawnArrestAttempted,
		// Token: 0x04002F16 RID: 12054
		PawnLost,
		// Token: 0x04002F17 RID: 12055
		BuildingDamaged,
		// Token: 0x04002F18 RID: 12056
		BuildingLost,
		// Token: 0x04002F19 RID: 12057
		FactionRelationsChanged,
		// Token: 0x04002F1A RID: 12058
		DormancyWakeup,
		// Token: 0x04002F1B RID: 12059
		Clamor,
		// Token: 0x04002F1C RID: 12060
		MechClusterDefeated,
		// Token: 0x04002F1D RID: 12061
		Signal,
		// Token: 0x04002F1E RID: 12062
		CorpseLost,
		// Token: 0x04002F1F RID: 12063
		AcquiredTarget
	}
}
