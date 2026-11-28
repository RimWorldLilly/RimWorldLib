using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F28 RID: 7976
	public static class InventoryCalculatorsUtility
	{
		// Token: 0x0600CB89 RID: 52105 RVA: 0x004AFFA4 File Offset: 0x004AE1A4
		public static bool ShouldIgnoreInventoryOf(Pawn pawn, IgnorePawnsInventoryMode ignoreMode)
		{
			switch (ignoreMode)
			{
			case IgnorePawnsInventoryMode.Ignore:
				return true;
			case IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload:
				return pawn.Spawned && pawn.inventory.UnloadEverything;
			case IgnorePawnsInventoryMode.IgnoreIfAssignedToUnloadOrPlayerPawn:
				return (pawn.Spawned && pawn.inventory.UnloadEverything) || Dialog_FormCaravan.CanListInventorySeparately(pawn);
			case IgnorePawnsInventoryMode.DontIgnore:
				return false;
			default:
				throw new NotImplementedException("IgnorePawnsInventoryMode");
			}
		}
	}
}
