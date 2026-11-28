using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001E4E RID: 7758
	public static class RCellFinder
	{
		// Token: 0x0600C923 RID: 51491 RVA: 0x0049E54E File Offset: 0x0049C74E
		private static bool IsGoodDestination(IntVec3 c, Map map, bool careAboutDanger)
		{
			return c.Standable(map) && (!careAboutDanger || !c.GetTerrain(map).dangerous);
		}

		// Token: 0x0600C924 RID: 51492 RVA: 0x0049E570 File Offset: 0x0049C770
		private static bool IsGoodDestinationFor(IntVec3 c, Pawn pawn, bool careAboutDanger)
		{
			Map map = pawn.Map;
			if (!RCellFinder.IsGoodDestination(c, map, careAboutDanger))
			{
				return false;
			}
			if (!c.WalkableBy(map, pawn))
			{
				return false;
			}
			if (!c.Standable(map))
			{
				Building_Door door = c.GetDoor(map);
				if (door == null || !door.CanPhysicallyPass(pawn))
				{
					return false;
				}
			}
			return !c.IsForbidden(pawn) && (!careAboutDanger || c.GetDangerFor(pawn, map) != Danger.Deadly) && (!careAboutDanger || !PawnUtility.KnownDangerAt(c, pawn.Map, pawn)) && (!careAboutDanger || !c.VacuumConcernTo(pawn));
		}

		// Token: 0x0600C925 RID: 51493 RVA: 0x0049E5FC File Offset: 0x0049C7FC
		public static IntVec3 BestOrderedGotoDestNear(IntVec3 root, Pawn searcher, Predicate<IntVec3> cellValidator = null, bool reachable = true)
		{
			RCellFinder.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.searcher = searcher;
			CS$<>8__locals1.cellValidator = cellValidator;
			CS$<>8__locals1.reachable = reachable;
			CS$<>8__locals1.map = CS$<>8__locals1.searcher.Map;
			if (RCellFinder.<BestOrderedGotoDestNear>g__IsGoodDest|2_0(root, ref CS$<>8__locals1))
			{
				return root;
			}
			int num = 1;
			IntVec3 result = default(IntVec3);
			float num2 = -1000f;
			bool flag = false;
			int num3 = GenRadial.NumCellsInRadius(30f);
			do
			{
				IntVec3 intVec = root + GenRadial.RadialPattern[num];
				if (RCellFinder.<BestOrderedGotoDestNear>g__IsGoodDest|2_0(intVec, ref CS$<>8__locals1))
				{
					float num4 = CoverUtility.TotalSurroundingCoverScore(intVec, CS$<>8__locals1.map);
					if (num4 > num2)
					{
						num2 = num4;
						result = intVec;
						flag = true;
					}
				}
				if (num >= 8 && flag)
				{
					return result;
				}
				num++;
			}
			while (num < num3);
			return CS$<>8__locals1.searcher.Position;
		}

		// Token: 0x0600C926 RID: 51494 RVA: 0x0049E6BC File Offset: 0x0049C8BC
		public static bool TryFindBestExitSpot(Pawn pawn, out IntVec3 spot, TraverseMode mode = TraverseMode.ByPawn, bool canBash = true)
		{
			if ((mode == TraverseMode.PassAllDestroyableThings || mode == TraverseMode.PassAllDestroyableThingsNotWater || mode == TraverseMode.PassAllDestroyablePlayerOwnedThings) && !pawn.Map.reachability.CanReachMapEdge(pawn.Position, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, canBash, false, false, true)))
			{
				return RCellFinder.TryFindRandomPawnEntryCell(out spot, pawn.Map, 0f, true, (IntVec3 x) => pawn.CanReach(x, PathEndMode.OnCell, Danger.Deadly, false, false, mode));
			}
			int num = 0;
			int num2 = 0;
			IntVec3 intVec2;
			for (;;)
			{
				num2++;
				if (num2 > 30)
				{
					break;
				}
				IntVec3 intVec;
				bool flag = CellFinder.TryFindRandomCellNear(pawn.Position, pawn.Map, num, null, out intVec, -1);
				num += 4;
				if (flag)
				{
					int num3 = intVec.x;
					intVec2 = new IntVec3(0, 0, intVec.z);
					if (pawn.Map.Size.z - intVec.z < num3)
					{
						num3 = pawn.Map.Size.z - intVec.z;
						intVec2 = new IntVec3(intVec.x, 0, pawn.Map.Size.z - 1);
					}
					if (pawn.Map.Size.x - intVec.x < num3)
					{
						num3 = pawn.Map.Size.x - intVec.x;
						intVec2 = new IntVec3(pawn.Map.Size.x - 1, 0, intVec.z);
					}
					if (intVec.z < num3)
					{
						intVec2 = new IntVec3(intVec.x, 0, 0);
					}
					if (RCellFinder.IsGoodDestinationFor(intVec2, pawn, true) && pawn.CanReach(intVec2, PathEndMode.OnCell, Danger.Deadly, canBash, false, mode))
					{
						goto Block_10;
					}
				}
			}
			spot = pawn.Position;
			return false;
			Block_10:
			spot = intVec2;
			return true;
		}

		// Token: 0x0600C927 RID: 51495 RVA: 0x0049E8C8 File Offset: 0x0049CAC8
		public static bool TryFindRandomExitSpot(Pawn pawn, out IntVec3 spot, TraverseMode mode = TraverseMode.ByPawn)
		{
			Danger maxDanger = Danger.Some;
			int num = 0;
			IntVec3 intVec;
			for (;;)
			{
				num++;
				if (num > 40)
				{
					break;
				}
				if (num > 15)
				{
					maxDanger = Danger.Deadly;
				}
				intVec = CellFinder.RandomCell(pawn.Map);
				int num2 = Rand.RangeInclusive(0, 3);
				if (num2 == 0)
				{
					intVec.x = 0;
				}
				if (num2 == 1)
				{
					intVec.x = pawn.Map.Size.x - 1;
				}
				if (num2 == 2)
				{
					intVec.z = 0;
				}
				if (num2 == 3)
				{
					intVec.z = pawn.Map.Size.z - 1;
				}
				if (RCellFinder.IsGoodDestinationFor(intVec, pawn, true) && pawn.CanReach(intVec, PathEndMode.OnCell, maxDanger, false, false, mode))
				{
					goto Block_8;
				}
			}
			spot = pawn.Position;
			return false;
			Block_8:
			spot = intVec;
			return true;
		}

		// Token: 0x0600C928 RID: 51496 RVA: 0x0049E988 File Offset: 0x0049CB88
		public static bool TryFindExitSpotNear(Pawn pawn, IntVec3 near, float radius, out IntVec3 spot, TraverseMode mode = TraverseMode.ByPawn)
		{
			return ((mode == TraverseMode.PassAllDestroyableThings || mode == TraverseMode.PassAllDestroyableThingsNotWater || mode == TraverseMode.PassAllDestroyablePlayerOwnedThings) && CellFinder.TryFindRandomEdgeCellNearWith(near, radius, pawn.Map, (IntVec3 x) => pawn.CanReach(x, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn), out spot)) || CellFinder.TryFindRandomEdgeCellNearWith(near, radius, pawn.Map, (IntVec3 x) => pawn.CanReach(x, PathEndMode.OnCell, Danger.Deadly, false, false, mode), out spot);
		}

		// Token: 0x0600C929 RID: 51497 RVA: 0x0049EA08 File Offset: 0x0049CC08
		public static bool TryFindExitPortal(Pawn pawn, out Thing portal)
		{
			portal = null;
			if (!pawn.Map.IsPocketMap)
			{
				return false;
			}
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.MapPortal);
			if (list.NullOrEmpty<Thing>())
			{
				return false;
			}
			portal = list.MinBy((Thing x) => x.Position.DistanceToSquared(pawn.Position));
			return true;
		}

		// Token: 0x0600C92A RID: 51498 RVA: 0x0049EA70 File Offset: 0x0049CC70
		public static IntVec3 RandomWanderDestFor(Pawn pawn, IntVec3 root, float radius, Func<Pawn, IntVec3, IntVec3, bool> validator, Danger maxDanger, bool canBashDoors = false)
		{
			if (radius > 12f)
			{
				Log.Warning(string.Concat(new string[]
				{
					"wanderRadius of ",
					radius.ToString(),
					" is greater than Region.GridSize of ",
					12.ToString(),
					" and will break."
				}));
			}
			bool flag = false;
			if (root.GetRegion(pawn.Map, RegionType.Set_Passable) != null)
			{
				int maxRegions = Mathf.Max((int)radius / 3, 13);
				bool careAboutSunlight = pawn.genes != null && !pawn.genes.EnjoysSunlight;
				float num = pawn.RaceProps.Humanlike ? 1f : 0.5f;
				bool careAboutPollution = ModsConfig.BiotechActive && pawn.GetStatValue(StatDefOf.ToxicEnvironmentResistance, true, -1) < num;
				bool careAboutRotStink = pawn.GetStatValue(StatDefOf.ToxicEnvironmentResistance, true, -1) < num;
				CellFinder.AllRegionsNear(RCellFinder.regions, root.GetRegion(pawn.Map, RegionType.Set_Passable), maxRegions, TraverseParms.For(pawn, maxDanger, TraverseMode.ByPawn, canBashDoors, false, false, true), (Region reg) => reg.extentsClose.ClosestDistSquaredTo(root) <= radius * radius, null, RegionType.Set_Passable);
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(root, 0.6f, "root", 50);
				}
				if (RCellFinder.regions.Count > 0)
				{
					for (int i = 0; i < 35; i++)
					{
						IntVec3 intVec = IntVec3.Invalid;
						for (int j = 0; j < 5; j++)
						{
							IntVec3 randomCell = RCellFinder.regions.RandomElementByWeight((Region reg) => (float)reg.CellCount).RandomCell;
							if ((float)randomCell.DistanceToSquared(root) <= radius * radius)
							{
								intVec = randomCell;
								break;
							}
						}
						if (!intVec.IsValid)
						{
							if (flag)
							{
								pawn.Map.debugDrawer.FlashCell(intVec, 0.32f, "distance", 50);
							}
						}
						else
						{
							if (RCellFinder.CanWanderToCell(intVec, pawn, root, validator, i, maxDanger, careAboutSunlight, careAboutPollution, careAboutRotStink, canBashDoors))
							{
								if (flag)
								{
									pawn.Map.debugDrawer.FlashCell(intVec, 0.9f, "go!", 50);
								}
								RCellFinder.regions.Clear();
								return intVec;
							}
							if (flag)
							{
								pawn.Map.debugDrawer.FlashCell(intVec, 0.6f, "validation", 50);
							}
						}
					}
				}
				RCellFinder.regions.Clear();
			}
			IntVec3 position;
			if (!CellFinder.TryFindRandomCellNear(root, pawn.Map, Mathf.FloorToInt(radius), (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.None, false, false, TraverseMode.ByPawn) && !c.IsForbidden(pawn) && (validator == null || validator(pawn, c, root)), out position, -1) && !CellFinder.TryFindRandomCellNear(root, pawn.Map, Mathf.FloorToInt(radius), (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.None, false, false, TraverseMode.ByPawn) && !c.IsForbidden(pawn), out position, -1) && !CellFinder.TryFindRandomCellNear(root, pawn.Map, Mathf.FloorToInt(radius), (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn), out position, -1) && !CellFinder.TryFindRandomCellNear(root, pawn.Map, 20, (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.None, false, false, TraverseMode.ByPawn) && !c.IsForbidden(pawn), out position, -1) && !CellFinder.TryFindRandomCellNear(root, pawn.Map, 30, (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn), out position, -1) && !CellFinder.TryFindRandomCellNear(pawn.Position, pawn.Map, 5, (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn), out position, -1))
			{
				position = pawn.Position;
			}
			if (flag)
			{
				pawn.Map.debugDrawer.FlashCell(position, 0.4f, "fallback", 50);
			}
			return position;
		}

		// Token: 0x0600C92B RID: 51499 RVA: 0x0049EEB4 File Offset: 0x0049D0B4
		private static bool CanWanderToCell(IntVec3 c, Pawn pawn, IntVec3 root, Func<Pawn, IntVec3, IntVec3, bool> validator = null, int tryIndex = 0, Danger maxDanger = Danger.Some, bool careAboutSunlight = false, bool careAboutPollution = true, bool careAboutRotStink = true, bool canBashDoors = false)
		{
			bool flag = false;
			if (!c.WalkableBy(pawn.Map, pawn))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0f, "walk", 50);
				}
				return false;
			}
			if (c.IsForbidden(pawn))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.25f, "forbid", 50);
				}
				return false;
			}
			if (tryIndex < 10 && !c.Standable(pawn.Map))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.25f, "stand", 50);
				}
				return false;
			}
			if (!pawn.CanReach(c, PathEndMode.OnCell, maxDanger, canBashDoors, false, TraverseMode.ByPawn))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.6f, "reach", 50);
				}
				return false;
			}
			if (PawnUtility.KnownDangerAt(c, pawn.Map, pawn))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.1f, "trap", 50);
				}
				return false;
			}
			if (careAboutSunlight && tryIndex < 20 && c.InSunlight(pawn.Map))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.3f, "sun", 50);
				}
				return false;
			}
			if (careAboutPollution && tryIndex < 20 && c.IsPolluted(pawn.Map))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.32f, "pol", 50);
				}
				return false;
			}
			if (careAboutRotStink && tryIndex < 20 && c.AnyGas(pawn.Map, GasType.RotStink))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.2f, "rot", 50);
				}
				return false;
			}
			if (tryIndex < 10)
			{
				TerrainDef terrain = c.GetTerrain(pawn.Map);
				if (terrain.avoidWander && (!terrain.IsWater || !pawn.RaceProps.waterSeeker))
				{
					if (flag)
					{
						pawn.Map.debugDrawer.FlashCell(c, 0.39f, "terr", 50);
					}
					return false;
				}
				if ((Pawn_PathFollower.GetPawnCellBaseCostOverride(pawn, c) ?? pawn.Map.pathing.For(pawn).pathGrid.Cost(c)) > 20)
				{
					if (flag)
					{
						pawn.Map.debugDrawer.FlashCell(c, 0.4f, "pcost", 50);
					}
					return false;
				}
				if (c.GetDangerFor(pawn, pawn.Map) > Danger.None)
				{
					if (flag)
					{
						pawn.Map.debugDrawer.FlashCell(c, 0.4f, "danger", 50);
					}
					return false;
				}
			}
			else if (tryIndex < 15 && c.GetDangerFor(pawn, pawn.Map) == Danger.Deadly)
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.4f, "deadly", 50);
				}
				return false;
			}
			if (!pawn.Map.pawnDestinationReservationManager.CanReserve(c, pawn, false))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.75f, "resvd", 50);
				}
				return false;
			}
			if (validator != null && !validator(pawn, c, root))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.15f, "valid", 50);
				}
				return false;
			}
			if (c.GetDoor(pawn.Map) != null)
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.32f, "door", 50);
				}
				return false;
			}
			if (c.ContainsStaticFire(pawn.Map))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.9f, "fire", 50);
				}
				return false;
			}
			if (c.GetTerrain(pawn.Map).dangerous)
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.9f, "dangerous terrain", 50);
				}
				return false;
			}
			if (pawn.RaceProps.waterSeeker && pawn.Map.terrainGrid.AnyWaterCells && pawn.Faction != Faction.OfPlayer && !pawn.Map.terrainGrid.WaterAt(c))
			{
				CellRect cellRect = CellRect.CenteredOn(c, 8).ClipInsideMap(pawn.Map);
				for (int i = 0; i < 10; i++)
				{
					if (pawn.Map.terrainGrid.WaterAt(cellRect.RandomCell))
					{
						return true;
					}
				}
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.67f, "no-h2o", 50);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600C92C RID: 51500 RVA: 0x0049F330 File Offset: 0x0049D530
		public static bool TryFindGoodAdjacentSpotToTouch(Pawn toucher, Thing touchee, out IntVec3 result)
		{
			IntVec3 intVec = IntVec3.Invalid;
			int num = int.MaxValue;
			foreach (IntVec3 intVec2 in GenAdj.CellsAdjacent8Way(touchee))
			{
				if (RCellFinder.IsGoodDestinationFor(intVec2, toucher, true) && toucher.CanReach(intVec2, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn) && ReachabilityImmediate.CanReachImmediate(intVec2, touchee, toucher.Map, PathEndMode.Touch, toucher))
				{
					if (toucher.Position == intVec2)
					{
						intVec = intVec2;
						break;
					}
					int num2 = toucher.Position.DistanceToSquared(intVec2);
					if (num2 < num || (intVec.GetTerrain(toucher.Map).avoidWander && !intVec2.GetTerrain(toucher.Map).avoidWander) || (intVec.GetFirstThing(toucher.Map) != null && intVec2.GetFirstThing(toucher.Map) == null))
					{
						num = num2;
						intVec = intVec2;
					}
				}
			}
			if (intVec.IsValid)
			{
				result = intVec;
				return true;
			}
			foreach (IntVec3 intVec3 in GenAdj.CellsAdjacent8Way(touchee).InRandomOrder(null))
			{
				if (intVec3.WalkableBy(toucher.Map, toucher) && toucher.CanReach(intVec3, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					result = intVec3;
					return true;
				}
			}
			result = touchee.Position;
			return false;
		}

		// Token: 0x0600C92D RID: 51501 RVA: 0x0049F4BC File Offset: 0x0049D6BC
		public static bool TryFindRandomPawnEntryCell(out IntVec3 result, Map map, float roadChance, bool allowFogged = false, Predicate<IntVec3> extraValidator = null)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => RCellFinder.IsGoodDestination(c, map, true) && (map.TileInfo.AllowRoofedEdgeWalkIn || !map.roofGrid.Roofed(c)) && map.reachability.CanReachColony(c) && c.GetDistrict(map, RegionType.Set_Passable).TouchesMapEdge && (allowFogged || !c.Fogged(map)) && (extraValidator == null || extraValidator(c)), map, roadChance, out result);
		}

		// Token: 0x0600C92E RID: 51502 RVA: 0x0049F500 File Offset: 0x0049D700
		public static bool TryFindClosestEdgeCellTo(IntVec3 root, Map map, out IntVec3 result)
		{
			result = IntVec3.Invalid;
			Region region = root.GetRegion(map, RegionType.Set_Passable);
			if (region == null)
			{
				return false;
			}
			TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false);
			Region exitRegion = null;
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.Allows(traverseParms, false), delegate(Region r)
			{
				if (!r.touchesMapEdge)
				{
					return false;
				}
				exitRegion = r;
				return true;
			}, 999999, RegionType.Set_Passable);
			return exitRegion != null && CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => object.Equals(c.GetRegion(map, RegionType.Set_Passable), exitRegion), map, 0f, out result);
		}

		// Token: 0x0600C92F RID: 51503 RVA: 0x0049F5A0 File Offset: 0x0049D7A0
		public static bool TryFindPrisonerReleaseCell(Pawn prisoner, Pawn warden, out IntVec3 result)
		{
			RCellFinder.<>c__DisplayClass13_0 CS$<>8__locals1 = new RCellFinder.<>c__DisplayClass13_0();
			if (prisoner.Map != warden.Map)
			{
				result = IntVec3.Invalid;
				return false;
			}
			Region region = prisoner.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				result = default(IntVec3);
				return false;
			}
			CS$<>8__locals1.traverseParms = TraverseParms.For(warden, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			CS$<>8__locals1.needMapEdge = (prisoner.Faction != warden.Faction);
			CS$<>8__locals1.foundResult = IntVec3.Invalid;
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.Allows(CS$<>8__locals1.traverseParms, false), new RegionProcessor(CS$<>8__locals1.<TryFindPrisonerReleaseCell>g__RegionProcessor|0), 999, RegionType.Set_Passable);
			if (CS$<>8__locals1.foundResult.IsValid)
			{
				result = CS$<>8__locals1.foundResult;
				return true;
			}
			result = default(IntVec3);
			return false;
		}

		// Token: 0x0600C930 RID: 51504 RVA: 0x0049F660 File Offset: 0x0049D860
		public static IntVec3 RandomAnimalSpawnCell_MapGen(Map map)
		{
			int numStand = 0;
			int numDistrict = 0;
			int numTouch = 0;
			IntVec3 intVec;
			if (!CellFinderLoose.TryGetRandomCellWith(delegate(IntVec3 c)
			{
				if (!c.Standable(map))
				{
					int num2 = numStand;
					numStand = num2 + 1;
					return false;
				}
				if (c.GetTerrain(map).avoidWander)
				{
					return false;
				}
				if (c.GetTerrain(map).dangerous)
				{
					return false;
				}
				District district = c.GetDistrict(map, RegionType.Set_Passable);
				if (district == null)
				{
					int num2 = numDistrict;
					numDistrict = num2 + 1;
					return false;
				}
				if (!district.TouchesMapEdge)
				{
					int num2 = numTouch;
					numTouch = num2 + 1;
					return false;
				}
				return true;
			}, map, 1000, out intVec))
			{
				intVec = CellFinder.RandomCell(map);
				string[] array = new string[10];
				array[0] = "RandomAnimalSpawnCell_MapGen failed: numStand=";
				array[1] = numStand.ToString();
				array[2] = ", numDistrict=";
				array[3] = numDistrict.ToString();
				array[4] = ", numTouch=";
				array[5] = numTouch.ToString();
				array[6] = ". PlayerStartSpot=";
				array[7] = MapGenerator.PlayerStartSpot.ToString();
				array[8] = ". Returning ";
				int num = 9;
				IntVec3 intVec2 = intVec;
				array[num] = intVec2.ToString();
				Log.Warning(string.Concat(array));
			}
			return intVec;
		}

		// Token: 0x0600C931 RID: 51505 RVA: 0x0049F74C File Offset: 0x0049D94C
		public static bool TryFindSkygazeCell(IntVec3 root, Pawn searcher, out IntVec3 result)
		{
			RCellFinder.<>c__DisplayClass15_0 CS$<>8__locals1 = new RCellFinder.<>c__DisplayClass15_0();
			CS$<>8__locals1.searcher = searcher;
			Predicate<Region> validator = delegate(Region r)
			{
				IntVec3 intVec;
				return r.Room.PsychologicallyOutdoors && !r.IsForbiddenEntirely(CS$<>8__locals1.searcher) && r.TryFindRandomCellInRegionUnforbidden(CS$<>8__locals1.searcher, new Predicate<IntVec3>(base.<TryFindSkygazeCell>g__Validator|0), out intVec);
			};
			TraverseParms traverseParms = TraverseParms.For(CS$<>8__locals1.searcher, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			Region root2;
			if (!CellFinder.TryFindClosestRegionWith(root.GetRegion(CS$<>8__locals1.searcher.Map, RegionType.Set_Passable), traverseParms, validator, 45, out root2, RegionType.Set_Passable))
			{
				result = root;
				return false;
			}
			return CellFinder.RandomRegionNear(root2, 14, traverseParms, validator, CS$<>8__locals1.searcher, RegionType.Set_Passable).TryFindRandomCellInRegionUnforbidden(CS$<>8__locals1.searcher, new Predicate<IntVec3>(CS$<>8__locals1.<TryFindSkygazeCell>g__Validator|0), out result);
		}

		// Token: 0x0600C932 RID: 51506 RVA: 0x0049F7DC File Offset: 0x0049D9DC
		public static bool TryFindTravelDestFrom(IntVec3 root, Map map, out IntVec3 travelDest)
		{
			travelDest = root;
			bool flag = false;
			Predicate<IntVec3> cellValidator = (IntVec3 c) => RCellFinder.IsGoodDestination(c, map, true) && map.reachability.CanReach(root, c, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.None) && (map.TileInfo.AllowRoofedEdgeWalkIn || !map.roofGrid.Roofed(c));
			if (root.x == 0)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.x == map.Size.x - 1 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			else if (root.x == map.Size.x - 1)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.x == 0 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			else if (root.z == 0)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.z == map.Size.z - 1 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			else if (root.z == map.Size.z - 1)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.z == 0 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			if (!flag)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => (c - root).LengthHorizontalSquared > 10000 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			if (!flag)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => (c - root).LengthHorizontalSquared > 2500 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			return flag;
		}

		// Token: 0x0600C933 RID: 51507 RVA: 0x0049F939 File Offset: 0x0049DB39
		public static bool TryFindRandomSpotJustOutsideColony(IntVec3 originCell, Map map, out IntVec3 result)
		{
			return RCellFinder.TryFindRandomSpotJustOutsideColony(originCell, map, null, out result, null);
		}

		// Token: 0x0600C934 RID: 51508 RVA: 0x0049F945 File Offset: 0x0049DB45
		public static bool TryFindRandomSpotJustOutsideColony(Pawn searcher, out IntVec3 result)
		{
			return RCellFinder.TryFindRandomSpotJustOutsideColony(searcher.Position, searcher.Map, searcher, out result, null);
		}

		// Token: 0x0600C935 RID: 51509 RVA: 0x0049F95C File Offset: 0x0049DB5C
		public static bool TryFindRandomSpotJustOutsideColony(IntVec3 root, Map map, Pawn searcher, out IntVec3 result, Predicate<IntVec3> extraValidator = null)
		{
			RCellFinder.<>c__DisplayClass20_0 CS$<>8__locals1 = new RCellFinder.<>c__DisplayClass20_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.root = root;
			CS$<>8__locals1.searcher = searcher;
			CS$<>8__locals1.extraValidator = extraValidator;
			IEnumerable<Building> source = from b in CS$<>8__locals1.map.listerBuildings.allBuildingsColonist
			where b.def.building.ai_chillDestination
			select b;
			int num = 0;
			Building building;
			while (num < 120 && source.TryRandomElement(out building))
			{
				CS$<>8__locals1.desperate = (num > 60);
				CS$<>8__locals1.walkRadius = 6 - num / 20;
				CS$<>8__locals1.walkRadiusMaxImpassable = 6 - num / 20;
				CS$<>8__locals1.minColonyBuildingsLOS = 5 - num / 30;
				if (CellFinder.TryFindRandomCellNear(building.Position, CS$<>8__locals1.map, 10, new Predicate<IntVec3>(CS$<>8__locals1.<TryFindRandomSpotJustOutsideColony>g__FinalValidator|0), out result, 50))
				{
					return true;
				}
				num++;
			}
			List<Building> allBuildingsColonist = CS$<>8__locals1.map.listerBuildings.allBuildingsColonist;
			int num2 = 0;
			Building building2;
			while (num2 < 120 && allBuildingsColonist.TryRandomElement(out building2))
			{
				CS$<>8__locals1.desperate = (num2 > 60);
				CS$<>8__locals1.walkRadius = 6 - num2 / 20;
				CS$<>8__locals1.walkRadiusMaxImpassable = 6 - num2 / 20;
				CS$<>8__locals1.minColonyBuildingsLOS = 4 - num2 / 30;
				if (CellFinder.TryFindRandomCellNear(building2.Position, CS$<>8__locals1.map, 15, new Predicate<IntVec3>(CS$<>8__locals1.<TryFindRandomSpotJustOutsideColony>g__FinalValidator|0), out result, 50))
				{
					return true;
				}
				num2++;
			}
			int num3 = 0;
			Pawn pawn;
			while (num3 < 50 && CS$<>8__locals1.map.mapPawns.FreeColonistsAndPrisonersSpawned.TryRandomElement(out pawn))
			{
				CS$<>8__locals1.desperate = (num3 > 25);
				CS$<>8__locals1.walkRadius = 3;
				CS$<>8__locals1.walkRadiusMaxImpassable = 6;
				CS$<>8__locals1.minColonyBuildingsLOS = 0;
				if (CellFinder.TryFindRandomCellNear(pawn.Position, CS$<>8__locals1.map, 15, new Predicate<IntVec3>(CS$<>8__locals1.<TryFindRandomSpotJustOutsideColony>g__FinalValidator|0), out result, 50))
				{
					return true;
				}
				num3++;
			}
			CS$<>8__locals1.desperate = true;
			CS$<>8__locals1.walkRadius = 3;
			CS$<>8__locals1.walkRadiusMaxImpassable = 6;
			CS$<>8__locals1.minColonyBuildingsLOS = 0;
			return CellFinderLoose.TryGetRandomCellWith(new Predicate<IntVec3>(CS$<>8__locals1.<TryFindRandomSpotJustOutsideColony>g__FinalValidator|0), CS$<>8__locals1.map, 1000, out result);
		}

		// Token: 0x0600C936 RID: 51510 RVA: 0x0049FB68 File Offset: 0x0049DD68
		public static bool TryFindRandomCellInRegionUnforbidden(this Region reg, Pawn pawn, Predicate<IntVec3> validator, out IntVec3 result)
		{
			if (reg == null)
			{
				throw new ArgumentNullException("reg");
			}
			if (reg.IsForbiddenEntirely(pawn))
			{
				result = IntVec3.Invalid;
				return false;
			}
			return reg.TryFindRandomCellInRegion((IntVec3 c) => !c.IsForbidden(pawn) && (validator == null || validator(c)), out result);
		}

		// Token: 0x0600C937 RID: 51511 RVA: 0x0049FBC8 File Offset: 0x0049DDC8
		public static bool TryFindDirectFleeDestination(IntVec3 root, float dist, Pawn pawn, out IntVec3 result)
		{
			for (int i = 0; i < 30; i++)
			{
				result = root + IntVec3.FromVector3(Vector3Utility.HorizontalVectorFromAngle((float)Rand.Range(0, 360)) * dist);
				if (result.WalkableBy(pawn.Map, pawn) && result.DistanceToSquared(pawn.Position) < result.DistanceToSquared(root) && GenSight.LineOfSight(root, result, pawn.Map, true, null, 0, 0))
				{
					return true;
				}
			}
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			for (int j = 0; j < 30; j++)
			{
				IntVec3 randomCell = CellFinder.RandomRegionNear(region, 15, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), null, null, RegionType.Set_Passable).RandomCell;
				if (randomCell.WalkableBy(pawn.Map, pawn) && (float)(root - randomCell).LengthHorizontalSquared > dist * dist)
				{
					using (PawnPath pawnPath = pawn.Map.pathFinder.FindPathNow(pawn.Position, randomCell, pawn, null, PathEndMode.OnCell))
					{
						if (PawnPathUtility.TryFindCellAtIndex(pawnPath, (int)dist + 3, out result))
						{
							return true;
						}
					}
				}
			}
			result = pawn.Position;
			return false;
		}

		// Token: 0x0600C938 RID: 51512 RVA: 0x0049FD24 File Offset: 0x0049DF24
		public static bool TryFindRandomCellOutsideColonyNearTheCenterOfTheMap(IntVec3 pos, Map map, float minDistToColony, out IntVec3 result)
		{
			int num = 30;
			CellRect cellRect = CellRect.CenteredOn(map.Center, num);
			cellRect.ClipInsideMap(map);
			List<IntVec3> list = new List<IntVec3>();
			if (minDistToColony > 0f)
			{
				foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
				{
					list.Add(pawn.Position);
				}
				foreach (Building building in map.listerBuildings.allBuildingsColonist)
				{
					list.Add(building.Position);
				}
			}
			float num2 = minDistToColony * minDistToColony;
			int num3 = 0;
			IntVec3 randomCell;
			for (;;)
			{
				num3++;
				if (num3 > 50)
				{
					if (num > map.Size.x)
					{
						goto IL_16D;
					}
					num = (int)((float)num * 1.5f);
					cellRect = CellRect.CenteredOn(map.Center, num);
					cellRect.ClipInsideMap(map);
					num3 = 0;
				}
				randomCell = cellRect.RandomCell;
				if (RCellFinder.IsGoodDestination(randomCell, map, true) && map.reachability.CanReach(randomCell, pos, PathEndMode.ClosestTouch, TraverseMode.NoPassClosedDoors, Danger.Deadly))
				{
					bool flag = false;
					for (int i = 0; i < list.Count; i++)
					{
						if ((float)(list[i] - randomCell).LengthHorizontalSquared < num2)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
				}
			}
			result = randomCell;
			return true;
			IL_16D:
			result = pos;
			return false;
		}

		// Token: 0x0600C939 RID: 51513 RVA: 0x0049FEC4 File Offset: 0x0049E0C4
		public static bool TryFindRandomCellNearTheCenterOfTheMapWith(Predicate<IntVec3> validator, Map map, out IntVec3 result)
		{
			int startingSearchRadius = Mathf.Clamp(Mathf.Max(map.Size.x, map.Size.z) / 20, 3, 25);
			return RCellFinder.TryFindRandomCellNearWith(map.Center, validator, map, out result, startingSearchRadius, int.MaxValue);
		}

		// Token: 0x0600C93A RID: 51514 RVA: 0x0049FF0C File Offset: 0x0049E10C
		public static bool TryFindRandomClearCellsNear(IntVec3 near, int amount, Map map, out List<IntVec3> cells, int startingSearchRadius = 5, int maxSearchRadius = 2147483647)
		{
			return RCellFinder.TryFindRandomCellsNear(near, amount, map, (IntVec3 pos) => pos.GetFirstBuilding(map) == null && pos.Walkable(map), out cells, startingSearchRadius, maxSearchRadius);
		}

		// Token: 0x0600C93B RID: 51515 RVA: 0x0049FF44 File Offset: 0x0049E144
		public static bool TryFindRandomCellsNear(IntVec3 near, int amount, Map map, Predicate<IntVec3> validator, out List<IntVec3> cells, int startingSearchRadius = 5, int maxSearchRadius = 2147483647)
		{
			cells = new List<IntVec3>();
			for (int i = 0; i < amount; i++)
			{
				IntVec3 item;
				if (RCellFinder.TryFindRandomCellNearWith(near, validator, map, out item, startingSearchRadius, maxSearchRadius))
				{
					cells.Add(item);
				}
			}
			return cells.Count == amount;
		}

		// Token: 0x0600C93C RID: 51516 RVA: 0x0049FF8C File Offset: 0x0049E18C
		public static bool TryFindRandomCellNearWith(IntVec3 near, Predicate<IntVec3> validator, Map map, out IntVec3 result, int startingSearchRadius = 5, int maxSearchRadius = 2147483647)
		{
			int num = startingSearchRadius;
			CellRect cellRect = CellRect.CenteredOn(near, num);
			cellRect.ClipInsideMap(map);
			int num2 = 0;
			IntVec3 randomCell;
			do
			{
				num2++;
				if (num2 > 30)
				{
					if (num >= maxSearchRadius || (num > map.Size.x * 2 && num > map.Size.z * 2))
					{
						goto IL_82;
					}
					num = Mathf.Min((int)((float)num * 1.5f), maxSearchRadius);
					cellRect = CellRect.CenteredOn(near, num);
					cellRect.ClipInsideMap(map);
					num2 = 0;
				}
				randomCell = cellRect.RandomCell;
			}
			while (!validator(randomCell));
			result = randomCell;
			return true;
			IL_82:
			result = near;
			return false;
		}

		// Token: 0x0600C93D RID: 51517 RVA: 0x004A0024 File Offset: 0x0049E224
		public static IntVec3 SpotToChewStandingNear(Pawn pawn, Thing ingestible, Predicate<IntVec3> extraValidator = null)
		{
			RCellFinder.<>c__DisplayClass28_0 CS$<>8__locals1 = new RCellFinder.<>c__DisplayClass28_0();
			CS$<>8__locals1.ingestible = ingestible;
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.extraValidator = extraValidator;
			return RCellFinder.SpotToStandDuringJob(CS$<>8__locals1.pawn, new Predicate<IntVec3>(CS$<>8__locals1.<SpotToChewStandingNear>g__Validator|0), null);
		}

		// Token: 0x0600C93E RID: 51518 RVA: 0x004A0064 File Offset: 0x0049E264
		public static IntVec3 SpotToStandDuringJobInRegion(Region region, Pawn pawn, float maxDistance, bool desperate = false, bool ignoreDanger = false, Predicate<IntVec3> extraValidator = null)
		{
			RCellFinder.<>c__DisplayClass29_0 CS$<>8__locals1 = new RCellFinder.<>c__DisplayClass29_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.maxDistance = maxDistance;
			CS$<>8__locals1.desperate = desperate;
			CS$<>8__locals1.ignoreDanger = ignoreDanger;
			CS$<>8__locals1.extraValidator = extraValidator;
			IntVec3 result;
			if (!region.TryFindRandomCellInRegionUnforbidden(CS$<>8__locals1.pawn, new Predicate<IntVec3>(CS$<>8__locals1.<SpotToStandDuringJobInRegion>g__Validator|0), out result))
			{
				return IntVec3.Invalid;
			}
			return result;
		}

		// Token: 0x0600C93F RID: 51519 RVA: 0x004A00C0 File Offset: 0x0049E2C0
		public static IntVec3 SpotToStandDuringJob(Pawn pawn, Predicate<IntVec3> extraValidator = null, Region targetRegion = null)
		{
			bool desperate = false;
			bool ignoreDanger = false;
			float maxDistance = 4f;
			int maxRegions = 1;
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			for (int i = 0; i < 30; i++)
			{
				if (i == 1)
				{
					desperate = true;
				}
				else if (i == 2)
				{
					desperate = false;
					maxRegions = 4;
				}
				else if (i == 6)
				{
					desperate = true;
				}
				else if (i == 10)
				{
					desperate = false;
					maxDistance = 8f;
					maxRegions = 12;
				}
				else if (i == 15)
				{
					desperate = true;
				}
				else if (i == 20)
				{
					maxDistance = 15f;
					maxRegions = 16;
				}
				else if (i == 26)
				{
					maxDistance = 5f;
					maxRegions = 4;
					ignoreDanger = true;
				}
				else if (i == 29)
				{
					maxDistance = 15f;
					maxRegions = 16;
				}
				IntVec3 intVec = RCellFinder.SpotToStandDuringJobInRegion(targetRegion ?? CellFinder.RandomRegionNear(region, maxRegions, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), null, pawn, RegionType.Set_Passable), pawn, maxDistance, desperate, ignoreDanger, extraValidator);
				if (intVec.IsValid)
				{
					if (DebugViewSettings.drawDestSearch)
					{
						pawn.Map.debugDrawer.FlashCell(intVec, 0.5f, "go!", 50);
					}
					return intVec;
				}
				if (DebugViewSettings.drawDestSearch)
				{
					pawn.Map.debugDrawer.FlashCell(intVec, 0f, i.ToString(), 50);
				}
			}
			IntVec3 randomCell = region.RandomCell;
			if (extraValidator != null && !extraValidator(randomCell))
			{
				return IntVec3.Invalid;
			}
			return region.RandomCell;
		}

		// Token: 0x0600C940 RID: 51520 RVA: 0x004A020C File Offset: 0x0049E40C
		public static bool TryFindMarriageSite(Pawn firstFiance, Pawn secondFiance, out IntVec3 result)
		{
			if (!firstFiance.CanReach(secondFiance, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				result = IntVec3.Invalid;
				return false;
			}
			Map map = firstFiance.Map;
			if ((from x in map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.MarriageSpot)
			where MarriageSpotUtility.IsValidMarriageSpotFor(x.Position, firstFiance, secondFiance, null)
			select x.Position).TryRandomElement(out result))
			{
				return true;
			}
			Predicate<IntVec3> noMarriageSpotValidator = delegate(IntVec3 cell)
			{
				IntVec3 c = cell + LordToil_MarriageCeremony.OtherFianceNoMarriageSpotCellOffset;
				if (!c.InBounds(map))
				{
					return false;
				}
				if (c.IsForbidden(firstFiance) || c.IsForbidden(secondFiance))
				{
					return false;
				}
				if (!c.Standable(map))
				{
					return false;
				}
				Room room = cell.GetRoom(map);
				return room == null || room.IsHuge || room.PsychologicallyOutdoors || room.CellCount >= 10;
			};
			foreach (CompGatherSpot compGatherSpot in map.gatherSpotLister.activeSpots.InRandomOrder(null))
			{
				for (int i = 0; i < 10; i++)
				{
					IntVec3 intVec = CellFinder.RandomClosewalkCellNear(compGatherSpot.parent.Position, compGatherSpot.parent.Map, 4, null);
					if (MarriageSpotUtility.IsValidMarriageSpotFor(intVec, firstFiance, secondFiance, null) && noMarriageSpotValidator(intVec))
					{
						result = intVec;
						return true;
					}
				}
			}
			if (CellFinder.TryFindRandomCellNear(firstFiance.Position, firstFiance.Map, 25, (IntVec3 cell) => MarriageSpotUtility.IsValidMarriageSpotFor(cell, firstFiance, secondFiance, null) && noMarriageSpotValidator(cell), out result, -1))
			{
				return true;
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600C941 RID: 51521 RVA: 0x004A03B4 File Offset: 0x0049E5B4
		public static bool TryFindGatheringSpot(Pawn organizer, GatheringDef gatheringDef, bool ignoreRequiredColonistCount, out IntVec3 result)
		{
			bool enjoyableOutside = JoyUtility.EnjoyableOutsideNow(organizer, null);
			Map map = organizer.Map;
			Predicate<IntVec3> baseValidator = (IntVec3 cell) => GatheringsUtility.ValidateGatheringSpot(cell, gatheringDef, organizer, enjoyableOutside, ignoreRequiredColonistCount);
			List<ThingDef> gatherSpotDefs = gatheringDef.gatherSpotDefs;
			try
			{
				foreach (ThingDef def in gatherSpotDefs)
				{
					foreach (Building item in map.listerBuildings.AllBuildingsColonistOfDef(def))
					{
						RCellFinder.tmpSpotThings.Add(item);
					}
				}
				if ((from x in RCellFinder.tmpSpotThings
				where baseValidator(x.Position)
				select x.Position).TryRandomElement(out result))
				{
					return true;
				}
			}
			finally
			{
				RCellFinder.tmpSpotThings.Clear();
			}
			Predicate<IntVec3> noPartySpotValidator = delegate(IntVec3 cell)
			{
				Room room = cell.GetRoom(map);
				return room == null || room.IsHuge || room.PsychologicallyOutdoors || room.CellCount >= 10;
			};
			foreach (CompGatherSpot compGatherSpot in map.gatherSpotLister.activeSpots.InRandomOrder(null))
			{
				for (int i = 0; i < 10; i++)
				{
					IntVec3 intVec = CellFinder.RandomClosewalkCellNear(compGatherSpot.parent.Position, compGatherSpot.parent.Map, 4, null);
					if (baseValidator(intVec) && noPartySpotValidator(intVec))
					{
						result = intVec;
						return true;
					}
				}
			}
			if (CellFinder.TryFindRandomCellNear(organizer.Position, organizer.Map, 25, (IntVec3 cell) => baseValidator(cell) && noPartySpotValidator(cell), out result, -1))
			{
				return true;
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600C942 RID: 51522 RVA: 0x004A0608 File Offset: 0x0049E808
		public static IntVec3 FindSiegePositionFrom(IntVec3 entrySpot, Map map, bool allowRoofed = false, bool errorOnFail = true, Func<IntVec3, bool> validator = null, bool requireBuildableTerrain = true)
		{
			if (!entrySpot.IsValid)
			{
				IntVec3 intVec;
				if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map), map, CellFinder.EdgeRoadChance_Ignore, out intVec))
				{
					intVec = CellFinder.RandomCell(map);
				}
				string str = "Tried to find a siege position from an invalid cell. Using ";
				IntVec3 intVec2 = intVec;
				Log.Error(str + intVec2.ToString());
				return intVec;
			}
			IntVec3 result;
			for (int i = 70; i >= 20; i -= 10)
			{
				if (RCellFinder.TryFindSiegePosition(entrySpot, (float)i, map, allowRoofed, out result, validator, requireBuildableTerrain))
				{
					return result;
				}
			}
			if (RCellFinder.TryFindSiegePosition(entrySpot, 100f, map, allowRoofed, out result, validator, requireBuildableTerrain))
			{
				return result;
			}
			if (RCellFinder.TryFindSiegePosition(entrySpot, 10f, map, allowRoofed, out result, validator, requireBuildableTerrain))
			{
				return result;
			}
			if (errorOnFail)
			{
				string str2 = "Could not find siege spot from ";
				IntVec3 intVec2 = entrySpot;
				string str3 = intVec2.ToString();
				string str4 = ", using ";
				intVec2 = entrySpot;
				Log.Error(str2 + str3 + str4 + intVec2.ToString());
			}
			return entrySpot;
		}

		// Token: 0x0600C943 RID: 51523 RVA: 0x004A0714 File Offset: 0x0049E914
		private static bool TryFindSiegePosition(IntVec3 entrySpot, float minDistToColony, Map map, bool allowRoofed, out IntVec3 result, Func<IntVec3, bool> validator = null, bool requireBuildableTerrain = true)
		{
			CellRect cellRect = CellRect.CenteredOn(entrySpot, 60);
			cellRect.ClipInsideMap(map);
			cellRect = cellRect.ContractedBy(14);
			List<IntVec3> list = new List<IntVec3>();
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				list.Add(pawn.Position);
			}
			foreach (Building building in map.listerBuildings.allBuildingsColonistCombatTargets)
			{
				list.Add(building.Position);
			}
			float num = minDistToColony * minDistToColony;
			int num2 = 0;
			IntVec3 randomCell;
			for (;;)
			{
				num2++;
				if (num2 > 200)
				{
					goto IL_1F9;
				}
				randomCell = cellRect.RandomCell;
				if (RCellFinder.IsGoodDestination(randomCell, map, true) && (!requireBuildableTerrain || (randomCell.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy) && randomCell.SupportsStructureType(map, TerrainAffordanceDefOf.Light))) && map.reachability.CanReach(randomCell, entrySpot, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Some) && map.reachability.CanReachColony(randomCell) && (validator == null || validator(randomCell)))
				{
					bool flag = false;
					for (int i = 0; i < list.Count; i++)
					{
						if ((float)(list[i] - randomCell).LengthHorizontalSquared < num)
						{
							flag = true;
							break;
						}
					}
					if (!flag && (allowRoofed || !randomCell.Roofed(map)))
					{
						if (!requireBuildableTerrain)
						{
							break;
						}
						int num3 = 0;
						foreach (IntVec3 intVec in CellRect.CenteredOn(randomCell, 10).ClipInsideMap(map))
						{
							if (randomCell.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy) && randomCell.SupportsStructureType(map, TerrainAffordanceDefOf.Light))
							{
								num3++;
							}
						}
						if (num3 >= 35)
						{
							break;
						}
					}
				}
			}
			result = randomCell;
			return true;
			IL_1F9:
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600C944 RID: 51524 RVA: 0x004A0950 File Offset: 0x0049EB50
		public static bool TryFindEdgeCellWithPathToPositionAvoidingColony(IntVec3 target, float minDistToColony, float minDistanceToTarget, Map map, out IntVec3 result)
		{
			bool flag = false;
			RCellFinder.tmpSpotsToAvoid.Clear();
			foreach (Building building in map.listerBuildings.allBuildingsColonist)
			{
				RCellFinder.tmpSpotsToAvoid.Add(building.Position);
			}
			foreach (Pawn pawn in map.mapPawns.FreeColonistsAndPrisonersSpawned)
			{
				RCellFinder.tmpSpotsToAvoid.Add(pawn.Position);
			}
			if (flag)
			{
				for (int i = 0; i < RCellFinder.tmpSpotsToAvoid.Count; i++)
				{
					map.debugDrawer.FlashCell(RCellFinder.tmpSpotsToAvoid[i], 1f, null, 50);
				}
			}
			float num = minDistToColony * minDistToColony;
			int num2 = 0;
			IntVec3 intVec;
			for (;;)
			{
				num2++;
				if (num2 > 200)
				{
					goto IL_1DB;
				}
				intVec = CellFinder.RandomEdgeCell(map);
				if ((map.TileInfo.AllowRoofedEdgeWalkIn || !intVec.Roofed(map)) && intVec.Standable(map) && map.reachability.CanReach(intVec, target, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Some) && !target.InHorDistOf(intVec, minDistanceToTarget))
				{
					bool flag2 = false;
					foreach (IntVec3 b in GenSight.PointsOnLineOfSight(intVec, target))
					{
						for (int j = 0; j < RCellFinder.tmpSpotsToAvoid.Count; j++)
						{
							if ((float)(RCellFinder.tmpSpotsToAvoid[j] - b).LengthHorizontalSquared < num)
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							break;
						}
					}
					if (flag)
					{
						map.debugDrawer.FlashLine(intVec, target, 50, flag2 ? SimpleColor.Red : SimpleColor.Green);
					}
					if (!flag2)
					{
						break;
					}
				}
			}
			result = intVec;
			return true;
			IL_1DB:
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600C945 RID: 51525 RVA: 0x004A0B70 File Offset: 0x0049ED70
		public static void FindBestAngleAvoidingSpots(IntVec3 position, List<IntVec3> spotsToAvoid, out float bestAngle, out float bestPerimeter)
		{
			if (RCellFinder.tmpSpotsToAvoid.Count == 0)
			{
				bestAngle = (float)Rand.Range(0, 360);
				bestPerimeter = 360f;
				return;
			}
			if (RCellFinder.tmpSpotsToAvoid.Count == 1)
			{
				float angleFlat = (RCellFinder.tmpSpotsToAvoid[0] - position).AngleFlat;
				bestAngle = angleFlat + 180f;
				bestPerimeter = 360f;
				return;
			}
			RCellFinder.tmpSpotsToAvoid.SortBy((IntVec3 s) => (s - position).AngleFlat);
			RCellFinder.tmpSpotsToAvoid.Add(RCellFinder.tmpSpotsToAvoid.First<IntVec3>());
			float num = 0f;
			float num2 = 0f;
			for (int i = 1; i < RCellFinder.tmpSpotsToAvoid.Count; i++)
			{
				IntVec3 intVec = RCellFinder.tmpSpotsToAvoid[i - 1] - position;
				IntVec3 intVec2 = RCellFinder.tmpSpotsToAvoid[i] - position;
				float angleFlat2 = intVec.AngleFlat;
				float angleFlat3 = intVec2.AngleFlat;
				float num3 = Mathf.Abs(angleFlat3 - angleFlat2);
				float num4 = (angleFlat3 < angleFlat2) ? (360f - num3) : num3;
				if (num4 > num2)
				{
					num2 = num4;
					num = intVec.AngleFlat;
				}
			}
			bestAngle = num + num2 / 2f;
			bestPerimeter = num2;
		}

		// Token: 0x0600C946 RID: 51526 RVA: 0x004A0CC0 File Offset: 0x0049EEC0
		public static bool TryFindRandomSpotNearAvoidingHostilePawns(Thing thing, Map map, Func<IntVec3, bool> predicate, out IntVec3 result, float maxSearchDistance = 100f, float minDistance = 10f, float maxDistance = 50f, bool avoidColony = true)
		{
			IntVec3 thingPosition = thing.Position;
			bool drawDebug = false;
			RCellFinder.tmpSpotsToAvoid.Clear();
			IReadOnlyList<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].HostileTo(thing) && allPawnsSpawned[i].Position.InHorDistOf(thingPosition, maxSearchDistance))
				{
					RCellFinder.tmpSpotsToAvoid.Add(allPawnsSpawned[i].Position);
				}
			}
			if (avoidColony)
			{
				foreach (Building building in map.listerBuildings.allBuildingsColonist)
				{
					if (building.Position.InHorDistOf(thingPosition, maxSearchDistance))
					{
						RCellFinder.tmpSpotsToAvoid.Add(building.Position);
					}
				}
			}
			if (drawDebug)
			{
				for (int j = 0; j < RCellFinder.tmpSpotsToAvoid.Count; j++)
				{
					map.debugDrawer.FlashCell(RCellFinder.tmpSpotsToAvoid[j], 0.2f, null, 50);
				}
			}
			float num;
			float num2;
			RCellFinder.FindBestAngleAvoidingSpots(thingPosition, RCellFinder.tmpSpotsToAvoid, out num, out num2);
			RCellFinder.tmpSpotsToAvoid.Clear();
			Vector3 v = IntVec3.North.ToVector3();
			if (drawDebug)
			{
				Vector3 vect = v.RotatedBy(num) * maxDistance;
				Vector3 vect2 = v.RotatedBy(num + num2) * maxDistance;
				map.debugDrawer.FlashLine(thingPosition, thingPosition + vect.ToIntVec3(), 50, SimpleColor.Red);
				map.debugDrawer.FlashLine(thingPosition, thingPosition + vect2.ToIntVec3(), 50, SimpleColor.Red);
			}
			Func<Vector3, IntVec3> func = delegate(Vector3 direction)
			{
				IntVec3 intVec3 = thingPosition + (direction * minDistance).ToIntVec3();
				IntVec3 intVec4 = intVec3 + (direction * (maxDistance - minDistance)).ToIntVec3();
				if (drawDebug)
				{
					map.debugDrawer.FlashLine(intVec3, intVec4, 50, SimpleColor.White);
				}
				foreach (IntVec3 intVec5 in GenSight.PointsOnLineOfSight(intVec3, intVec4).InRandomOrder(null))
				{
					if (predicate(intVec5))
					{
						return intVec5;
					}
				}
				return IntVec3.Invalid;
			};
			float num3 = num2 / 4f;
			for (float num4 = 0f; num4 < num3; num4 += 5f)
			{
				Vector3 arg = v.RotatedBy(num + num4);
				IntVec3 intVec = func(arg);
				if (intVec.IsValid)
				{
					result = intVec;
					return true;
				}
				if (!Mathf.Approximately(num4, 0f))
				{
					Vector3 arg2 = v.RotatedBy(num - num4);
					IntVec3 intVec2 = func(arg2);
					if (intVec2.IsValid)
					{
						result = intVec2;
						return true;
					}
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600C947 RID: 51527 RVA: 0x004A0F98 File Offset: 0x0049F198
		public static bool TryFindEdgeCellFromPositionAvoidingColony(IntVec3 position, Map map, Predicate<IntVec3> predicate, out IntVec3 result)
		{
			bool flag = false;
			RCellFinder.tmpSpotsToAvoid.Clear();
			List<Pawn> freeColonistsAndPrisonersSpawned = map.mapPawns.FreeColonistsAndPrisonersSpawned;
			for (int i = 0; i < freeColonistsAndPrisonersSpawned.Count; i++)
			{
				RCellFinder.tmpSpotsToAvoid.Add(freeColonistsAndPrisonersSpawned[i].Position);
			}
			foreach (Building building in map.listerBuildings.allBuildingsColonist)
			{
				RCellFinder.tmpSpotsToAvoid.Add(building.Position);
			}
			if (flag)
			{
				for (int j = 0; j < RCellFinder.tmpSpotsToAvoid.Count; j++)
				{
					map.debugDrawer.FlashCell(RCellFinder.tmpSpotsToAvoid[j], 0.2f, null, 50);
				}
			}
			float num;
			float num2;
			RCellFinder.FindBestAngleAvoidingSpots(position, RCellFinder.tmpSpotsToAvoid, out num, out num2);
			RCellFinder.tmpSpotsToAvoid.Clear();
			Vector3 v = IntVec3.North.ToVector3();
			if (flag)
			{
				Vector3 vect = v.RotatedBy(num) * map.Size.LengthHorizontal;
				Vector3 vect2 = v.RotatedBy(num + num2) * map.Size.LengthHorizontal;
				map.debugDrawer.FlashLine(position, position + vect.ToIntVec3(), 50, SimpleColor.Red);
				map.debugDrawer.FlashLine(position, position + vect2.ToIntVec3(), 50, SimpleColor.Red);
			}
			CellRect cellRect = CellRect.WholeMap(map);
			Vector3 normalized = v.RotatedBy(num).normalized;
			Vector3 vector = position.ToVector3();
			IntVec3 currentIntPosition = vector.ToIntVec3();
			while (!cellRect.IsOnEdge(currentIntPosition))
			{
				vector += normalized;
				currentIntPosition = vector.ToIntVec3();
				if (!cellRect.Contains(currentIntPosition))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to find map edge cell from position ");
					defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(position);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					result = IntVec3.Invalid;
					return false;
				}
			}
			RCellFinder.tmpEdgeCells.Clear();
			foreach (IntVec3 item in cellRect.EdgeCells)
			{
				RCellFinder.tmpEdgeCells.Add(item);
			}
			RCellFinder.tmpEdgeCells.SortBy((IntVec3 p) => p.DistanceToSquared(currentIntPosition));
			for (int k = 0; k < RCellFinder.tmpEdgeCells.Count; k++)
			{
				if (predicate(RCellFinder.tmpEdgeCells[k]))
				{
					result = RCellFinder.tmpEdgeCells[k];
					RCellFinder.tmpEdgeCells.Clear();
					return true;
				}
			}
			RCellFinder.tmpEdgeCells.Clear();
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600C948 RID: 51528 RVA: 0x004A1290 File Offset: 0x0049F490
		public static bool TryFindEdgeCellFromThingAvoidingColony(Thing thing, Map map, Func<IntVec3, IntVec3, bool> predicate, out IntVec3 result)
		{
			result = IntVec3.Invalid;
			if (thing.def.passability == Traversability.Impassable)
			{
				using (IEnumerator<IntVec3> enumerator = thing.OccupiedRect().ExpandedBy(1).EdgeCells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 cell = enumerator.Current;
						if (cell.InBounds(map) && RCellFinder.TryFindEdgeCellFromPositionAvoidingColony(cell, map, (IntVec3 p) => predicate(cell, p), out result))
						{
							return true;
						}
					}
					return false;
				}
			}
			if (RCellFinder.TryFindEdgeCellFromPositionAvoidingColony(thing.Position, map, (IntVec3 p) => predicate(thing.Position, p), out result))
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600C949 RID: 51529 RVA: 0x004A1384 File Offset: 0x0049F584
		public static bool TryFindAllowedUnroofedSpotOutsideColony(IntVec3 root, Pawn searcher, out IntVec3 result, int minDistanceFromColonyThing = 35, int maxDistanceFromColonyThing = 2147483647, int maxRegions = 45, Predicate<Region> extraRegionValidator = null)
		{
			RCellFinder.tmpNearbyColonyThings.Clear();
			IntVec3 foundCell = IntVec3.Invalid;
			TraverseParms traverseParms = TraverseParms.For(searcher, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			RegionTraverser.BreadthFirstTraverse(root, searcher.Map, (Region from, Region r) => r.Allows(traverseParms, false), delegate(Region r)
			{
				if (extraRegionValidator != null && !extraRegionValidator(r))
				{
					return false;
				}
				if (r.IsForbiddenEntirely(searcher))
				{
					return false;
				}
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].Faction == Faction.OfPlayer)
					{
						RCellFinder.tmpNearbyColonyThings.Add(list[i]);
					}
				}
				List<Thing> list2 = r.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
				for (int j = 0; j < list2.Count; j++)
				{
					if (list2[j].Faction == Faction.OfPlayer && !RCellFinder.tmpNearbyColonyThings.Contains(list2[j]))
					{
						RCellFinder.tmpNearbyColonyThings.Add(list2[j]);
					}
				}
				IntVec3 foundCell;
				if (r.TryFindRandomCellInRegion(new Predicate<IntVec3>(base.<TryFindAllowedUnroofedSpotOutsideColony>g__CellValidator|0), out foundCell))
				{
					foundCell = foundCell;
					return true;
				}
				return false;
			}, maxRegions, RegionType.Set_Passable);
			RCellFinder.tmpNearbyColonyThings.Clear();
			result = foundCell;
			return result.IsValid;
		}

		// Token: 0x0600C94A RID: 51530 RVA: 0x004A142C File Offset: 0x0049F62C
		public static bool TryFindRandomMechSelfShutdownSpot(IntVec3 root, Pawn pawn, Map map, out IntVec3 result, bool allowForbidden = false)
		{
			if (RCellFinder.CanSelfShutdown(pawn.Position, pawn, map, allowForbidden))
			{
				result = pawn.Position;
				return true;
			}
			return RCellFinder.TryFindRandomCellNearWith(root, (IntVec3 c) => RCellFinder.CanSelfShutdown(c, pawn, map, false), map, out result, 5, 20);
		}

		// Token: 0x0600C94B RID: 51531 RVA: 0x004A149C File Offset: 0x0049F69C
		private static bool CanSelfShutdown(IntVec3 c, Pawn pawn, Map map, bool allowForbidden = false)
		{
			if (!c.InBounds(map))
			{
				return false;
			}
			if (!pawn.CanReserve(c, 1, -1, null, false))
			{
				return false;
			}
			if (!pawn.CanReach(c, PathEndMode.OnCell, Danger.Some, false, false, TraverseMode.ByPawn))
			{
				return false;
			}
			if (!c.Standable(map))
			{
				return false;
			}
			if (c.GetTerrain(map).dangerous)
			{
				return false;
			}
			if (!allowForbidden && c.IsForbidden(pawn))
			{
				return false;
			}
			if (c.GetFirstBuilding(map) != null)
			{
				return false;
			}
			Room room = c.GetRoom(map);
			if (room != null && room.IsPrisonCell)
			{
				return false;
			}
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 c2 = c + GenAdj.CardinalDirections[i];
				if (c2.InBounds(map))
				{
					List<Thing> thingList = c2.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j].def.hasInteractionCell && thingList[j].InteractionCell == c)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600C94C RID: 51532 RVA: 0x004A159C File Offset: 0x0049F79C
		public static bool TryFindNearbyMechSelfShutdownSpot(IntVec3 root, Pawn pawn, Map map, out IntVec3 result, bool allowForbidden = false)
		{
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(root, GenRadial.MaxRadialPatternRadius - 1f, true))
			{
				if (RCellFinder.CanSelfShutdown(intVec, pawn, map, allowForbidden))
				{
					result = intVec;
					return true;
				}
			}
			return RCellFinder.TryFindRandomMechSelfShutdownSpot(root, pawn, map, out result, allowForbidden);
		}

		// Token: 0x0600C94D RID: 51533 RVA: 0x004A1614 File Offset: 0x0049F814
		public static bool TryFindNearbyDarkCellFor(Pawn pawn, out IntVec3 result, int maxRegionsTraversed = 15)
		{
			IntVec3 resultLocal = IntVec3.Invalid;
			RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(TraverseMode.NoPassClosedDoors, true), delegate(Region r)
			{
				if (r.type == RegionType.Portal)
				{
					return false;
				}
				int num = Mathf.Min(100, r.CellCount);
				for (int i = 0; i < num; i++)
				{
					IntVec3 randomCell = r.RandomCell;
					if (pawn.Map.glowGrid.GroundGlowAt(randomCell, false, false) <= 0f)
					{
						resultLocal = randomCell;
						return true;
					}
				}
				return false;
			}, maxRegionsTraversed, RegionType.Set_Passable);
			result = resultLocal;
			return resultLocal.IsValid;
		}

		// Token: 0x0600C94E RID: 51534 RVA: 0x004A169C File Offset: 0x0049F89C
		public static bool TryFindNearbyEmptyCell(Pawn pawn, out IntVec3 cell, Predicate<IntVec3, Map> validator = null, int maxRegions = 20)
		{
			cell = IntVec3.Invalid;
			IntVec3 local = IntVec3.Invalid;
			RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), true), delegate(Region r)
			{
				if (r.type == RegionType.Portal)
				{
					return false;
				}
				int num = Mathf.Min(100, r.CellCount);
				for (int i = 0; i < num; i++)
				{
					IntVec3 randomCell = r.RandomCell;
					if (RCellFinder.IsGoodDestinationFor(randomCell, pawn, true) && (validator == null || validator(randomCell, pawn.MapHeld)) && randomCell.GetFirstBuilding(pawn.MapHeld) == null && randomCell.Walkable(pawn.MapHeld))
					{
						local = randomCell;
						return true;
					}
				}
				return false;
			}, maxRegions, RegionType.Set_Passable);
			cell = local;
			return cell.IsValid;
		}

		// Token: 0x0600C950 RID: 51536 RVA: 0x004A175C File Offset: 0x0049F95C
		[CompilerGenerated]
		internal static bool <BestOrderedGotoDestNear>g__IsGoodDest|2_0(IntVec3 c, ref RCellFinder.<>c__DisplayClass2_0 A_1)
		{
			if (!RCellFinder.IsGoodDestinationFor(c, A_1.searcher, false))
			{
				return false;
			}
			if (A_1.cellValidator != null && !A_1.cellValidator(c))
			{
				return false;
			}
			if (!A_1.map.pawnDestinationReservationManager.CanReserve(c, A_1.searcher, true))
			{
				return false;
			}
			if (A_1.reachable && !A_1.searcher.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(A_1.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Pawn pawn = thingList[i] as Pawn;
				if (pawn != null && pawn != A_1.searcher && pawn.RaceProps.Humanlike && ((A_1.searcher.Faction == Faction.OfPlayer && pawn.Faction == A_1.searcher.Faction) || (A_1.searcher.Faction != Faction.OfPlayer && pawn.Faction != Faction.OfPlayer)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400729E RID: 29342
		private static readonly List<Region> regions = new List<Region>();

		// Token: 0x0400729F RID: 29343
		private static HashSet<Thing> tmpBuildings = new HashSet<Thing>();

		// Token: 0x040072A0 RID: 29344
		private static List<Thing> tmpSpotThings = new List<Thing>();

		// Token: 0x040072A1 RID: 29345
		private static List<IntVec3> tmpSpotsToAvoid = new List<IntVec3>();

		// Token: 0x040072A2 RID: 29346
		private static List<IntVec3> tmpEdgeCells = new List<IntVec3>();

		// Token: 0x040072A3 RID: 29347
		private static List<Thing> tmpNearbyColonyThings = new List<Thing>();
	}
}
