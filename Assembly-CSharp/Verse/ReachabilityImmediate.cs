using System;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002F2 RID: 754
	public static class ReachabilityImmediate
	{
		// Token: 0x060016F3 RID: 5875 RVA: 0x00089C94 File Offset: 0x00087E94
		public static bool CanReachImmediate(IntVec3 start, LocalTargetInfo target, Map map, PathEndMode peMode, Pawn pawn)
		{
			if (!target.IsValid)
			{
				return false;
			}
			target = (LocalTargetInfo)GenPath.ResolvePathMode(pawn, target.ToTargetInfo(map), ref peMode);
			if (target.HasThing)
			{
				Thing thing = target.Thing;
				if (thing.Spawned)
				{
					if (thing.Map != map)
					{
						return false;
					}
				}
				else
				{
					if (pawn != null)
					{
						if (pawn.carryTracker.innerContainer.Contains(thing))
						{
							return true;
						}
						if (pawn.inventory.innerContainer.Contains(thing))
						{
							return true;
						}
						if (pawn.apparel != null && pawn.apparel.Contains(thing))
						{
							return true;
						}
						if (pawn.equipment != null && pawn.equipment.Contains(thing))
						{
							return true;
						}
					}
					if (!thing.SpawnedOrAnyParentSpawned)
					{
						return false;
					}
					if (thing.MapHeld != map)
					{
						return false;
					}
				}
			}
			if (!target.HasThing || (target.Thing.def.size.x == 1 && target.Thing.def.size.z == 1))
			{
				if (start == target.Cell)
				{
					return true;
				}
			}
			else if (start.IsInside(target.Thing))
			{
				return true;
			}
			return peMode == PathEndMode.Touch && TouchPathEndModeUtility.IsAdjacentOrInsideAndAllowedToTouch(start, target, map.pathing.For(pawn));
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x00089DDE File Offset: 0x00087FDE
		public static bool CanReachImmediate(this Pawn pawn, LocalTargetInfo target, PathEndMode peMode)
		{
			return pawn.Spawned && ReachabilityImmediate.CanReachImmediate(pawn.Position, target, pawn.Map, peMode, pawn);
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x00089DFE File Offset: 0x00087FFE
		public static bool CanReachImmediateNonLocal(this Pawn pawn, TargetInfo target, PathEndMode peMode)
		{
			return pawn.Spawned && (target.Map == null || target.Map == pawn.Map) && pawn.CanReachImmediate((LocalTargetInfo)target, peMode);
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00089E34 File Offset: 0x00088034
		public static bool CanReachImmediate(IntVec3 start, CellRect rect, Map map, PathEndMode peMode, Pawn pawn)
		{
			IntVec3 c = rect.ClosestCellTo(start);
			return ReachabilityImmediate.CanReachImmediate(start, c, map, peMode, pawn);
		}
	}
}
