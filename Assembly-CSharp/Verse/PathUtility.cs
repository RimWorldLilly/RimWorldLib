using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200036C RID: 876
	public static class PathUtility
	{
		// Token: 0x06001ADF RID: 6879 RVA: 0x000A5240 File Offset: 0x000A3440
		public static Area GetAllowedArea(Pawn pawn)
		{
			if (pawn != null && pawn.playerSettings != null && !pawn.Drafted && ForbidUtility.CaresAboutForbidden(pawn, true, false))
			{
				Area area = pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap;
				if (area != null && area.TrueCount <= 0)
				{
					area = null;
				}
				return area;
			}
			return null;
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x000A5287 File Offset: 0x000A3487
		public static bool IsDestroyable(Thing thing)
		{
			return thing.def.useHitPoints && thing.def.destroyable;
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x000A52A4 File Offset: 0x000A34A4
		public static ushort GetDoorCost(Building_Door door, TraverseParms traverseParms, Pawn pawn, PathFinderCostTuning? tuningParams)
		{
			PathFinderCostTuning pathFinderCostTuning = tuningParams ?? PathFinderCostTuning.DefaultTuning;
			switch (traverseParms.mode)
			{
			case TraverseMode.ByPawn:
				if (!traverseParms.canBashDoors && door.IsForbiddenToPass(pawn))
				{
					return ushort.MaxValue;
				}
				if (door.PawnCanOpen(pawn) && !door.FreePassage)
				{
					return (ushort)door.TicksToOpenNow;
				}
				if (door.CanPhysicallyPass(pawn))
				{
					return 0;
				}
				if (traverseParms.canBashDoors)
				{
					return 300;
				}
				return ushort.MaxValue;
			case TraverseMode.PassDoors:
				if (pawn != null && door.PawnCanOpen(pawn) && !door.IsForbiddenToPass(pawn) && !door.FreePassage)
				{
					return (ushort)door.TicksToOpenNow;
				}
				if ((pawn != null && door.CanPhysicallyPass(pawn)) || door.FreePassage)
				{
					return 0;
				}
				return 150;
			case TraverseMode.NoPassClosedDoors:
			case TraverseMode.NoPassClosedDoorsOrWater:
				if (door.FreePassage)
				{
					return 0;
				}
				return ushort.MaxValue;
			case TraverseMode.PassAllDestroyableThings:
			case TraverseMode.PassAllDestroyablePlayerOwnedThings:
			case TraverseMode.PassAllDestroyableThingsNotWater:
				if (pawn != null && door.PawnCanOpen(pawn) && !door.IsForbiddenToPass(pawn) && !door.FreePassage)
				{
					return (ushort)door.TicksToOpenNow;
				}
				if ((pawn != null && door.CanPhysicallyPass(pawn)) || door.FreePassage)
				{
					return 0;
				}
				if (traverseParms.mode == TraverseMode.PassAllDestroyablePlayerOwnedThings && door.Faction != null && !door.Faction.IsPlayer)
				{
					return ushort.MaxValue;
				}
				return (ushort)((float)pathFinderCostTuning.costBlockedDoor + (float)door.HitPoints * pathFinderCostTuning.costBlockedWallExtraPerHitPoint);
			default:
				return 0;
			}
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x000A5414 File Offset: 0x000A3614
		public static ushort GetBuildingCost(Building b, TraverseParms traverseParms, Pawn pawn, PathFinderCostTuning? tuningParms)
		{
			PathFinderCostTuning pathFinderCostTuning = tuningParms ?? PathFinderCostTuning.DefaultTuning;
			int costBlockedDoor = pathFinderCostTuning.costBlockedDoor;
			float costBlockedDoorPerHitPoint = pathFinderCostTuning.costBlockedDoorPerHitPoint;
			Building_Door building_Door = b as Building_Door;
			if (building_Door != null)
			{
				return PathUtility.GetDoorCost(building_Door, traverseParms, pawn, tuningParms);
			}
			if (b.def.IsFence && traverseParms.fenceBlocked)
			{
				switch (traverseParms.mode)
				{
				case TraverseMode.ByPawn:
					if (traverseParms.canBashFences)
					{
						return 300;
					}
					return ushort.MaxValue;
				case TraverseMode.PassDoors:
				case TraverseMode.NoPassClosedDoors:
				case TraverseMode.NoPassClosedDoorsOrWater:
					return 0;
				case TraverseMode.PassAllDestroyableThings:
				case TraverseMode.PassAllDestroyableThingsNotWater:
					return (ushort)(costBlockedDoor + (int)((ushort)((float)b.HitPoints * costBlockedDoorPerHitPoint)));
				case TraverseMode.PassAllDestroyablePlayerOwnedThings:
					if (!b.Faction.IsPlayer)
					{
						return ushort.MaxValue;
					}
					return (ushort)(costBlockedDoor + (int)((float)b.HitPoints * costBlockedDoorPerHitPoint));
				}
			}
			else if (pawn != null)
			{
				IPathFindCostProvider pathFindCostProvider = b as IPathFindCostProvider;
				if (pathFindCostProvider != null)
				{
					return pathFindCostProvider.PathFindCostFor(pawn);
				}
			}
			return 0;
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x000A54FB File Offset: 0x000A36FB
		public static bool BlocksDiagonalMovement(int x, int z, PathingContext pc, bool canBashFences)
		{
			return PathUtility.BlocksDiagonalMovement(pc.map.cellIndices.CellToIndex(x, z), pc, canBashFences);
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x000A5518 File Offset: 0x000A3718
		public static bool BlocksDiagonalMovement(int index, PathingContext pc, bool canBashFences)
		{
			if (!pc.pathGrid.WalkableFast(index))
			{
				return true;
			}
			Building building = pc.map.edificeGrid[index];
			return building != null && canBashFences && building.def.IsFence;
		}

		// Token: 0x0400161F RID: 5663
		private const int Cost_DoorToBash = 300;

		// Token: 0x04001620 RID: 5664
		private const int Cost_FenceToBash = 300;
	}
}
