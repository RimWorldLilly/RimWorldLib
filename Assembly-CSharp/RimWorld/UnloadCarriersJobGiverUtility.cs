using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AF4 RID: 2804
	public static class UnloadCarriersJobGiverUtility
	{
		// Token: 0x06005395 RID: 21397 RVA: 0x001F1BAC File Offset: 0x001EFDAC
		public static bool HasJobOnThing(Pawn pawn, Thing t, bool forced)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && pawn2 != pawn && !pawn2.IsFreeColonist && pawn2.inventory.UnloadEverything && (pawn2.Faction == pawn.Faction || pawn2.HostFaction == pawn.Faction) && !t.IsForbidden(pawn) && !t.IsBurning() && pawn.CanReserve(t, 1, -1, null, forced);
		}
	}
}
