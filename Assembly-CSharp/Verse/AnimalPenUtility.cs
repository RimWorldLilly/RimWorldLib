using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000234 RID: 564
	public static class AnimalPenUtility
	{
		// Token: 0x0600106B RID: 4203 RVA: 0x0005E260 File Offset: 0x0005C460
		public static ThingFilter GetFixedAnimalFilter()
		{
			if (AnimalPenUtility.fixedFilter == null)
			{
				AnimalPenUtility.fixedFilter = new ThingFilter();
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where(new Func<ThingDef, bool>(AnimalPenUtility.IsRopeManagedAnimalDef)))
				{
					AnimalPenUtility.fixedFilter.SetAllow(thingDef, true);
				}
			}
			return AnimalPenUtility.fixedFilter;
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x0005E2D8 File Offset: 0x0005C4D8
		public static bool IsRopeManagedAnimalDef(ThingDef td)
		{
			return td.race != null && td.race.Roamer && td.IsWithinCategory(ThingCategoryDefOf.Animals);
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0005E2FC File Offset: 0x0005C4FC
		private static bool ShouldBePennedByDefault(ThingDef td)
		{
			return AnimalPenUtility.IsRopeManagedAnimalDef(td) && td.race.FenceBlocked;
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0005E313 File Offset: 0x0005C513
		public static bool NeedsToBeManagedByRope(Pawn pawn)
		{
			return AnimalPenUtility.IsRopeManagedAnimalDef(pawn.def) && pawn.Spawned && pawn.Map.IsPlayerHome && pawn.Roamer;
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0005E340 File Offset: 0x0005C540
		public static ThingFilter GetDefaultAnimalFilter()
		{
			if (AnimalPenUtility.defaultFilter == null)
			{
				AnimalPenUtility.defaultFilter = new ThingFilter();
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where(new Func<ThingDef, bool>(AnimalPenUtility.ShouldBePennedByDefault)))
				{
					AnimalPenUtility.defaultFilter.SetAllow(thingDef, true);
				}
			}
			return AnimalPenUtility.defaultFilter;
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x0005E3B8 File Offset: 0x0005C5B8
		public static void ResetStaticData()
		{
			AnimalPenUtility.defaultFilter = null;
			AnimalPenUtility.fixedFilter = null;
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x0005E3C8 File Offset: 0x0005C5C8
		public static CompAnimalPenMarker GetCurrentPenOf(Pawn animal, bool allowUnenclosedPens)
		{
			if (!animal.Roamer)
			{
				return null;
			}
			Map map = animal.Map;
			if (!map.listerBuildings.allBuildingsAnimalPenMarkers.Any<Building>())
			{
				return null;
			}
			List<District> list = AnimalPenUtility.tmpConnectedDistrictsCalc.CalculateConnectedDistricts(animal.Position, map);
			if (!list.Any<District>())
			{
				return null;
			}
			CompAnimalPenMarker compAnimalPenMarker = null;
			foreach (Building building in map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				if (list.Contains(building.Position.GetDistrict(map, RegionType.Set_Passable)))
				{
					CompAnimalPenMarker compAnimalPenMarker2 = building.TryGetComp<CompAnimalPenMarker>();
					if (AnimalPenUtility.CanUseAndReach(animal, compAnimalPenMarker2, allowUnenclosedPens, null) && (compAnimalPenMarker == null || (compAnimalPenMarker2.PenState.Enclosed && compAnimalPenMarker.PenState.Unenclosed) || map.cellIndices.CellToIndex(compAnimalPenMarker2.parent.Position) < map.cellIndices.CellToIndex(compAnimalPenMarker.parent.Position)))
					{
						compAnimalPenMarker = compAnimalPenMarker2;
					}
				}
			}
			AnimalPenUtility.tmpConnectedDistrictsCalc.Reset();
			return compAnimalPenMarker;
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x0005E4EC File Offset: 0x0005C6EC
		public static bool AnySuitablePens(Pawn animal, bool allowUnenclosedPens)
		{
			foreach (Building thing in animal.Map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker penMarker = thing.TryGetComp<CompAnimalPenMarker>();
				if (AnimalPenUtility.CanUseAndReach(animal, penMarker, allowUnenclosedPens, null))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x0005E55C File Offset: 0x0005C75C
		public static bool AnySuitableHitch(Pawn animal)
		{
			foreach (Building t in animal.Map.listerBuildings.allBuildingsHitchingPosts)
			{
				if (animal.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x0005E5CC File Offset: 0x0005C7CC
		public static CompAnimalPenMarker ClosestSuitablePen(Pawn animal, bool allowUnenclosedPens)
		{
			Map map = animal.Map;
			CompAnimalPenMarker compAnimalPenMarker = null;
			float num = 0f;
			foreach (Building thing in map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker compAnimalPenMarker2 = thing.TryGetComp<CompAnimalPenMarker>();
				if (AnimalPenUtility.CanUseAndReach(animal, compAnimalPenMarker2, allowUnenclosedPens, null))
				{
					int num2 = animal.Position.DistanceToSquared(compAnimalPenMarker2.parent.Position);
					if (compAnimalPenMarker == null || (float)num2 < num)
					{
						compAnimalPenMarker = compAnimalPenMarker2;
						num = (float)num2;
					}
				}
			}
			return compAnimalPenMarker;
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0005E664 File Offset: 0x0005C864
		private static bool CanUseAndReach(Pawn animal, CompAnimalPenMarker penMarker, bool allowUnenclosedPens, Pawn roper = null)
		{
			bool flag = false;
			return AnimalPenUtility.CheckUseAndReach(animal, penMarker, allowUnenclosedPens, roper, ref flag, ref flag, ref flag);
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x0005E684 File Offset: 0x0005C884
		private static bool CheckUseAndReach(Pawn animal, CompAnimalPenMarker penMarker, bool allowUnenclosedPens, Pawn roper, ref bool foundEnclosed, ref bool foundUsable, ref bool foundReachable)
		{
			if (!allowUnenclosedPens && penMarker.PenState.Unenclosed)
			{
				return false;
			}
			foundEnclosed = true;
			if (!penMarker.AcceptsToPen(animal))
			{
				return false;
			}
			if (roper == null && penMarker.parent.IsForbidden(Faction.OfPlayer))
			{
				return false;
			}
			if (roper != null && penMarker.parent.IsForbidden(roper))
			{
				return false;
			}
			foundUsable = true;
			bool flag;
			if (roper == null)
			{
				TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false).WithFenceblockedOf(animal);
				flag = animal.Map.reachability.CanReach(animal.Position, penMarker.parent, PathEndMode.Touch, traverseParams);
			}
			else
			{
				TraverseParms traverseParams2 = TraverseParms.For(roper, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true).WithFenceblockedOf(animal);
				flag = animal.Map.reachability.CanReach(animal.Position, penMarker.parent, PathEndMode.Touch, traverseParams2);
			}
			if (!flag)
			{
				return false;
			}
			foundReachable = true;
			return true;
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x0005E764 File Offset: 0x0005C964
		public static CompAnimalPenMarker GetPenAnimalShouldBeTakenTo(Pawn roper, Pawn animal, out string jobFailReason, bool forced = false, bool canInteractWhileSleeping = true, bool allowUnenclosedPens = false, bool ignoreSkillRequirements = true, RopingPriority mode = RopingPriority.Closest, AnimalPenBalanceCalculator balanceCalculator = null)
		{
			jobFailReason = null;
			if (allowUnenclosedPens && mode == RopingPriority.Balanced)
			{
				Log.Warning("Cannot allow unenclosed pens in balanced mode");
				return null;
			}
			if (animal == roper)
			{
				return null;
			}
			if (animal == null || !AnimalPenUtility.NeedsToBeManagedByRope(animal))
			{
				return null;
			}
			if (animal.Faction != roper.Faction)
			{
				return null;
			}
			if (!forced && animal.roping.IsRopedByPawn && animal.roping.RopedByPawn != roper)
			{
				return null;
			}
			if (AnimalPenUtility.RopeAttachmentInteractionCell(roper, animal) == IntVec3.Invalid)
			{
				jobFailReason = "CantRopeAnimalCantTouch".Translate();
				return null;
			}
			if (!forced && !roper.CanReserve(animal, 1, -1, null, false))
			{
				return null;
			}
			CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(animal, allowUnenclosedPens);
			if (mode == RopingPriority.Closest && currentPenOf != null && currentPenOf.PenState.Enclosed)
			{
				return null;
			}
			if (!WorkGiver_InteractAnimal.CanInteractWithAnimal(roper, animal, out jobFailReason, forced, canInteractWhileSleeping, ignoreSkillRequirements, true))
			{
				return null;
			}
			Map map = animal.Map;
			AnimalPenBalanceCalculator animalPenBalanceCalculator = balanceCalculator ?? new AnimalPenBalanceCalculator(map, true);
			CompAnimalPenMarker compAnimalPenMarker = null;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			foreach (Building thing in map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker compAnimalPenMarker2 = thing.TryGetComp<CompAnimalPenMarker>();
				flag2 = true;
				if (AnimalPenUtility.CheckUseAndReach(animal, compAnimalPenMarker2, allowUnenclosedPens, roper, ref flag3, ref flag4, ref flag5))
				{
					if (mode == RopingPriority.Closest)
					{
						if (compAnimalPenMarker == null || (compAnimalPenMarker2.PenState.Enclosed && compAnimalPenMarker.PenState.Unenclosed) || AnimalPenUtility.PenIsCloser(compAnimalPenMarker2, compAnimalPenMarker, animal))
						{
							compAnimalPenMarker = compAnimalPenMarker2;
						}
					}
					else if (mode == RopingPriority.Balanced)
					{
						if (currentPenOf != null && !animalPenBalanceCalculator.IsBetterPen(compAnimalPenMarker2, currentPenOf, true, animal))
						{
							flag = true;
						}
						else if (compAnimalPenMarker == null || animalPenBalanceCalculator.IsBetterPen(compAnimalPenMarker2, compAnimalPenMarker, false, animal))
						{
							compAnimalPenMarker = compAnimalPenMarker2;
							flag = false;
						}
					}
				}
			}
			if (currentPenOf != null && compAnimalPenMarker == currentPenOf)
			{
				return null;
			}
			if (compAnimalPenMarker == null)
			{
				if (flag)
				{
					jobFailReason = "CantRopeAnimalAlreadyInBestPen".Translate();
				}
				else if (!flag2)
				{
					jobFailReason = "CantRopeAnimalNoUsableReachablePens".Translate();
				}
				else if (!flag3)
				{
					jobFailReason = "CantRopeAnimalNoEnclosedPens".Translate();
				}
				else if (!flag4)
				{
					jobFailReason = "CantRopeAnimalNoAllowedPens".Translate();
				}
				else if (!flag5)
				{
					jobFailReason = "CantRopeAnimalNoReachablePens".Translate();
				}
				else
				{
					jobFailReason = "CantRopeAnimalNoUsableReachablePens".Translate();
				}
				return null;
			}
			return compAnimalPenMarker;
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0005E9C0 File Offset: 0x0005CBC0
		public static Building GetHitchingPostAnimalShouldBeTakenTo(Pawn roper, Pawn animal, out string jobFailReason, bool forced = false)
		{
			jobFailReason = null;
			if (animal == roper)
			{
				return null;
			}
			if (animal == null || !animal.Roamer || !AnimalPenUtility.IsRopeManagedAnimalDef(animal.def))
			{
				return null;
			}
			if (animal.Faction != roper.Faction)
			{
				return null;
			}
			if (!forced && animal.roping.IsRopedByPawn && animal.roping.RopedByPawn != roper)
			{
				return null;
			}
			if (AnimalPenUtility.RopeAttachmentInteractionCell(roper, animal) == IntVec3.Invalid)
			{
				jobFailReason = "CantRopeAnimalCantTouch".Translate();
				return null;
			}
			if (!forced && !roper.CanReserve(animal, 1, -1, null, false))
			{
				return null;
			}
			if (animal.roping.IsRopedToHitchingPost || AnimalPenUtility.GetCurrentPenOf(animal, false) != null)
			{
				return null;
			}
			if (!WorkGiver_InteractAnimal.CanInteractWithAnimal(roper, animal, out jobFailReason, forced, true, true, true))
			{
				return null;
			}
			Building building = null;
			foreach (Building building2 in animal.Map.listerBuildings.allBuildingsHitchingPosts)
			{
				if (!building2.IsForbidden(roper) && roper.CanReach(building2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && animal.Map.reachability.CanReach(animal.Position, building2, PathEndMode.Touch, TraverseParms.For(roper, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true).WithFenceblockedOf(animal)) && (building == null || (animal.Position - building2.Position).LengthManhattan < (animal.Position - building.Position).LengthManhattan))
				{
					building = building2;
				}
			}
			return building;
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0005EB68 File Offset: 0x0005CD68
		private static bool PenIsCloser(CompAnimalPenMarker markerA, CompAnimalPenMarker markerB, Pawn animal)
		{
			return animal.Position.DistanceToSquared(markerA.parent.Position) < animal.Position.DistanceToSquared(markerB.parent.Position);
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0005EB98 File Offset: 0x0005CD98
		public static IntVec3 RopeAttachmentInteractionCell(Pawn roper, Pawn ropee)
		{
			if (!roper.Spawned || !ropee.Spawned)
			{
				return IntVec3.Invalid;
			}
			if (AnimalPenUtility.IsGoodRopeAttachmentInteractionCell(roper, ropee, roper.Position))
			{
				return roper.Position;
			}
			Map map = ropee.Map;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = ropee.Position + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(map) && AnimalPenUtility.MutuallyWalkable(roper, ropee, intVec))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x0005EC18 File Offset: 0x0005CE18
		public static bool IsGoodRopeAttachmentInteractionCell(Pawn roper, Pawn ropee, IntVec3 cell)
		{
			return ropee.Position.AdjacentToCardinal(cell) && AnimalPenUtility.MutuallyWalkable(roper, ropee, cell);
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x0005EC40 File Offset: 0x0005CE40
		private static bool MutuallyWalkable(Pawn roper, Pawn ropee, IntVec3 c)
		{
			Map map = ropee.Map;
			return c.WalkableBy(map, ropee) && c.WalkableBy(map, roper);
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0005EC68 File Offset: 0x0005CE68
		public static IntVec3 FindPlaceInPenToStand(CompAnimalPenMarker marker, Pawn pawn)
		{
			AnimalPenUtility.<>c__DisplayClass21_0 CS$<>8__locals1 = new AnimalPenUtility.<>c__DisplayClass21_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.marker = marker;
			CS$<>8__locals1.map = CS$<>8__locals1.pawn.Map;
			CS$<>8__locals1.result = IntVec3.Invalid;
			RegionTraverser.BreadthFirstTraverse(CS$<>8__locals1.marker.parent.Position, CS$<>8__locals1.map, (Region from, Region to) => CS$<>8__locals1.marker.PenState.ContainsConnectedRegion(to), new RegionProcessor(CS$<>8__locals1.<FindPlaceInPenToStand>g__RegionProcessor|1), 999999, RegionType.Set_Passable);
			return CS$<>8__locals1.result;
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0005ECE8 File Offset: 0x0005CEE8
		public static bool IsUnnecessarilyRoped(Pawn animal)
		{
			if (!AnimalPenUtility.NeedsToBeManagedByRope(animal))
			{
				return false;
			}
			if (animal.roping.IsRopedByPawn || animal.roping.IsRopedToHitchingPost || !animal.roping.IsRopedToSpot)
			{
				return false;
			}
			Lord lord = animal.GetLord();
			bool? flag;
			if (lord == null)
			{
				flag = null;
			}
			else
			{
				LordJob lordJob = lord.LordJob;
				flag = ((lordJob != null) ? new bool?(lordJob.ManagesRopableAnimals) : null);
			}
			bool? flag2 = flag;
			return !flag2.GetValueOrDefault();
		}

		// Token: 0x04000F52 RID: 3922
		private static ThingFilter fixedFilter;

		// Token: 0x04000F53 RID: 3923
		private static ThingFilter defaultFilter;

		// Token: 0x04000F54 RID: 3924
		private static readonly AnimalPenConnectedDistrictsCalculator tmpConnectedDistrictsCalc = new AnimalPenConnectedDistrictsCalculator();
	}
}
