using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002F1 RID: 753
	public static class ReachabilityUtility
	{
		// Token: 0x060016ED RID: 5869 RVA: 0x00089B2C File Offset: 0x00087D2C
		public static bool CanReach(this Pawn pawn, LocalTargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBashDoors = false, bool canBashFences = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReach(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBashDoors, false, canBashFences, true));
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x00089B6C File Offset: 0x00087D6C
		public static bool CanReach(this Pawn pawn, IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBashDoors = false, bool canBashFences = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReach(start, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBashDoors, false, canBashFences, true));
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x00089BA8 File Offset: 0x00087DA8
		public static bool CanReachNonLocal(this Pawn pawn, TargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBashDoors = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReachNonLocal(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBashDoors, false, false, true));
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x00089BE8 File Offset: 0x00087DE8
		public static bool CanReachMapEdge(this Pawn p)
		{
			return p.Spawned && p.Map.reachability.CanReachMapEdge(p.Position, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true));
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x00089C24 File Offset: 0x00087E24
		public static void ClearCache()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reachability.ClearCache();
			}
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00089C5C File Offset: 0x00087E5C
		public static void ClearCacheFor(Pawn p)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reachability.ClearCacheFor(p);
			}
		}
	}
}
