using System;

namespace Verse.AI
{
	// Token: 0x02000907 RID: 2311
	[Flags]
	public enum TargetScanFlags
	{
		// Token: 0x04002DA3 RID: 11683
		None = 0,
		// Token: 0x04002DA4 RID: 11684
		NeedLOSToPawns = 1,
		// Token: 0x04002DA5 RID: 11685
		NeedLOSToNonPawns = 2,
		// Token: 0x04002DA6 RID: 11686
		NeedLOSToAll = 3,
		// Token: 0x04002DA7 RID: 11687
		NeedReachable = 4,
		// Token: 0x04002DA8 RID: 11688
		NeedReachableIfCantHitFromMyPos = 8,
		// Token: 0x04002DA9 RID: 11689
		NeedNonBurning = 16,
		// Token: 0x04002DAA RID: 11690
		NeedThreat = 32,
		// Token: 0x04002DAB RID: 11691
		NeedActiveThreat = 64,
		// Token: 0x04002DAC RID: 11692
		LOSBlockableByGas = 128,
		// Token: 0x04002DAD RID: 11693
		NeedAutoTargetable = 256,
		// Token: 0x04002DAE RID: 11694
		NeedNotUnderThickRoof = 512,
		// Token: 0x04002DAF RID: 11695
		IgnoreNonCombatants = 1024
	}
}
