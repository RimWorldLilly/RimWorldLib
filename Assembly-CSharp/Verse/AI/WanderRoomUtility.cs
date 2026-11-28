using System;

namespace Verse.AI
{
	// Token: 0x020008F9 RID: 2297
	public static class WanderRoomUtility
	{
		// Token: 0x06004645 RID: 17989 RVA: 0x001B8EB4 File Offset: 0x001B70B4
		public static bool IsValidWanderDest(Pawn pawn, IntVec3 loc, IntVec3 root)
		{
			Room room = root.GetRoom(pawn.Map);
			return room == null || room.IsDoorway || WanderUtility.InSameRoom(root, loc, pawn.Map);
		}
	}
}
