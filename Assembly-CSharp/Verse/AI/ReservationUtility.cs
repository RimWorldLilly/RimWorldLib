using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008B6 RID: 2230
	public static class ReservationUtility
	{
		// Token: 0x06004579 RID: 17785 RVA: 0x001B6AF4 File Offset: 0x001B4CF4
		public static bool CanReserveSittableOrSpot(this Pawn pawn, IntVec3 exactSittingPos, bool ignoreOtherReservations = false)
		{
			return pawn.CanReserveSittableOrSpot(exactSittingPos, null, ignoreOtherReservations);
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x001B6B00 File Offset: 0x001B4D00
		public static bool CanReserveSittableOrSpot(this Pawn pawn, IntVec3 exactSittingPos, Thing ignoreThing, bool ignoreOtherReservations = false)
		{
			Building edifice = exactSittingPos.GetEdifice(pawn.Map);
			if (exactSittingPos.Impassable(pawn.Map) || exactSittingPos.IsForbidden(pawn))
			{
				return false;
			}
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = exactSittingPos + GenAdj.CardinalDirections[i];
				if (c.InBounds(pawn.Map))
				{
					Building edifice2 = c.GetEdifice(pawn.Map);
					Pawn pawn2;
					if (edifice2 != null && edifice2 != ignoreThing && edifice2.def.hasInteractionCell && edifice2.InteractionCell == exactSittingPos && pawn.Map.reservationManager.TryGetReserver(edifice2, pawn.Faction, out pawn2) && pawn2.Spawned && pawn2 != pawn)
					{
						return false;
					}
				}
			}
			if (edifice == null || edifice.def.building.multiSittable)
			{
				return pawn.CanReserve(exactSittingPos, 1, -1, null, ignoreOtherReservations);
			}
			return (!edifice.def.building.isSittable || !edifice.def.hasInteractionCell || !(exactSittingPos != edifice.InteractionCell)) && pawn.CanReserve(edifice, 1, -1, null, ignoreOtherReservations);
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x001B6C2C File Offset: 0x001B4E2C
		public static bool ReserveSittableOrSpot(this Pawn pawn, IntVec3 exactSittingPos, Job job, bool errorOnFailed = true)
		{
			Building edifice = exactSittingPos.GetEdifice(pawn.Map);
			if (exactSittingPos.Impassable(pawn.Map))
			{
				Log.Error("Tried reserving impassable sittable or spot.");
				return false;
			}
			if (edifice == null || edifice.def.building.multiSittable)
			{
				return pawn.Reserve(exactSittingPos, job, 1, -1, null, errorOnFailed, false);
			}
			return (edifice == null || !edifice.def.building.isSittable || !edifice.def.hasInteractionCell || !(exactSittingPos != edifice.InteractionCell)) && pawn.Reserve(edifice, job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x001B6CCC File Offset: 0x001B4ECC
		public static bool CanReserve(this Pawn p, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			return p.Spawned && p.Map.reservationManager.CanReserve(p, target, maxPawns, stackCount, layer, ignoreOtherReservations);
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x001B6CF0 File Offset: 0x001B4EF0
		public static bool CanReserveAndReach(this Pawn p, LocalTargetInfo target, PathEndMode peMode, Danger maxDanger, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			return p.Spawned && p.CanReach(target, peMode, maxDanger, false, false, TraverseMode.ByPawn) && p.Map.reservationManager.CanReserve(p, target, maxPawns, stackCount, layer, ignoreOtherReservations);
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x001B6D28 File Offset: 0x001B4F28
		public static bool TryFindReserveAndReachableOfDef(this Pawn p, ThingDef thingDef, out Thing thing, PathEndMode peMode = PathEndMode.Touch, Danger maxDanger = Danger.None, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			thing = null;
			if (!p.Spawned)
			{
				return false;
			}
			thing = GenClosest.ClosestThingReachable(p.PositionHeld, p.MapHeld, ThingRequest.ForDef(thingDef), PathEndMode.OnCell, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing t) => !t.IsForbidden(p) && p.CanReserveAndReach(t, peMode, maxDanger, maxPawns, stackCount, layer, ignoreOtherReservations), null, 0, -1, false, RegionType.Set_Passable, false, false);
			return thing != null;
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x001B6DD6 File Offset: 0x001B4FD6
		public static bool HasReserved(this Pawn pawn, ThingDef thingDef)
		{
			return pawn.Spawned && pawn.MapHeld.reservationManager.HasReservedOfDef(pawn, thingDef);
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x001B6DF4 File Offset: 0x001B4FF4
		public static bool ExistsUnreservedAmountOfDef(Map map, ThingDef thingDef, Faction faction, int amount, Predicate<Thing> validator = null)
		{
			int num = 0;
			foreach (Thing thing in map.listerThings.ThingsOfDef(thingDef))
			{
				if (!thing.IsForbidden(faction) && !map.reservationManager.IsReservedByAnyoneOf(thing, faction) && (validator == null || validator(thing)))
				{
					num += thing.stackCount;
					if (num >= amount)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004581 RID: 17793 RVA: 0x001B6E88 File Offset: 0x001B5088
		public static bool CanReserveAndReachableOfDef(this Pawn p, ThingDef thingDef, PathEndMode peMode = PathEndMode.Touch, Danger maxDanger = Danger.None, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			Thing thing;
			return p.TryFindReserveAndReachableOfDef(thingDef, out thing, peMode, maxDanger, maxPawns, stackCount, layer, ignoreOtherReservations);
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x001B6EA8 File Offset: 0x001B50A8
		public static bool Reserve(this Pawn p, LocalTargetInfo target, Job job, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool errorOnFailed = true, bool ignoreOtherReservations = false)
		{
			return p.Spawned && p.Map.reservationManager.Reserve(p, job, target, maxPawns, stackCount, layer, errorOnFailed, ignoreOtherReservations, true);
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x001B6EDC File Offset: 0x001B50DC
		public static void ReserveAsManyAsPossible(this Pawn p, List<LocalTargetInfo> target, Job job, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null)
		{
			if (!p.Spawned)
			{
				return;
			}
			for (int i = 0; i < target.Count; i++)
			{
				p.Map.reservationManager.Reserve(p, job, target[i], maxPawns, stackCount, layer, false, false, false);
			}
		}

		// Token: 0x06004584 RID: 17796 RVA: 0x001B6F25 File Offset: 0x001B5125
		public static bool HasReserved(this Pawn p, LocalTargetInfo target, Job job = null)
		{
			return p.Spawned && p.Map.reservationManager.ReservedBy(target, p, job);
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x001B6F44 File Offset: 0x001B5144
		public static bool HasReserved<TDriver>(this Pawn p, LocalTargetInfo target, LocalTargetInfo? targetAIsNot = null, LocalTargetInfo? targetBIsNot = null, LocalTargetInfo? targetCIsNot = null)
		{
			return p.Spawned && p.Map.reservationManager.ReservedBy<TDriver>(target, p, targetAIsNot, targetBIsNot, targetCIsNot);
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x001B6F66 File Offset: 0x001B5166
		public static bool CanReserveNew(this Pawn p, LocalTargetInfo target)
		{
			return target.IsValid && !p.HasReserved(target, null) && p.CanReserve(target, 1, -1, null, false);
		}
	}
}
