using System;

namespace Verse
{
	// Token: 0x020000EE RID: 238
	public enum TraverseMode : byte
	{
		// Token: 0x040003D5 RID: 981
		ByPawn,
		// Token: 0x040003D6 RID: 982
		PassDoors,
		// Token: 0x040003D7 RID: 983
		NoPassClosedDoors,
		// Token: 0x040003D8 RID: 984
		PassAllDestroyableThings,
		// Token: 0x040003D9 RID: 985
		PassAllDestroyablePlayerOwnedThings,
		// Token: 0x040003DA RID: 986
		NoPassClosedDoorsOrWater,
		// Token: 0x040003DB RID: 987
		PassAllDestroyableThingsNotWater
	}
}
