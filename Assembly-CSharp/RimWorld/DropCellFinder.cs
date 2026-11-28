using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001E48 RID: 7752
	public static class DropCellFinder
	{
		// Token: 0x0600C8F7 RID: 51447 RVA: 0x0049C0DC File Offset: 0x0049A2DC
		public static IntVec3 RandomDropSpot(Map map, bool standableOnly = true)
		{
			IntVec3 result = CellFinderLoose.RandomCellWith((IntVec3 c) => (!standableOnly || c.Standable(map)) && !c.GetTerrain(map).IsWater && !c.Roofed(map) && !c.Fogged(map), map, 1000);
			if (result.IsValid)
			{
				return result;
			}
			return CellFinderLoose.RandomCellWith((IntVec3 c) => (!standableOnly || c.Standable(map)) && !c.GetTerrain(map).IsWater && !c.Fogged(map), map, 1000);
		}

		// Token: 0x0600C8F8 RID: 51448 RVA: 0x0049C144 File Offset: 0x0049A344
		public static IntVec3 TradeDropSpot(Map map)
		{
			IEnumerable<Building> collection = from b in map.listerBuildings.allBuildingsColonist
			where b.def.IsCommsConsole
			select b;
			IEnumerable<Building> enumerable = from b in map.listerBuildings.allBuildingsColonist
			where b.def.IsOrbitalTradeBeacon
			select b;
			Building building = enumerable.FirstOrDefault((Building b) => !map.roofGrid.Roofed(b.Position) && DropCellFinder.AnyAdjacentGoodDropSpot(b.Position, map, false, false));
			IntVec3 position;
			if (building != null)
			{
				position = building.Position;
				IntVec3 result;
				if (!DropCellFinder.TryFindDropSpotNear(position, map, out result, false, false, true, null, true))
				{
					string str = "Could find no good TradeDropSpot near dropCenter ";
					IntVec3 intVec = position;
					Log.Error(str + intVec.ToString() + ". Using a random standable unfogged cell.");
					result = CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(map) && !c.Fogged(map), map, 1000);
				}
				return result;
			}
			List<Building> list = new List<Building>();
			list.AddRange(enumerable);
			list.AddRange(collection);
			list.RemoveAll(delegate(Building b)
			{
				CompPowerTrader compPowerTrader = b.TryGetComp<CompPowerTrader>();
				return compPowerTrader != null && !compPowerTrader.PowerOn;
			});
			Predicate<IntVec3> validator = (IntVec3 c) => DropCellFinder.IsGoodDropSpot(c, map, false, false, true);
			if (!list.Any<Building>())
			{
				list.AddRange(map.listerBuildings.allBuildingsColonist);
				list.Shuffle<Building>();
				if (!list.Any<Building>())
				{
					return CellFinderLoose.RandomCellWith(validator, map, 1000);
				}
			}
			int num = 8;
			for (;;)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (CellFinder.TryFindRandomCellNear(list[i].Position, map, num, validator, out position, -1))
					{
						return position;
					}
				}
				num = Mathf.RoundToInt((float)num * 1.1f);
				if (num > map.Size.x)
				{
					goto Block_9;
				}
			}
			return position;
			Block_9:
			Log.Error("Failed to generate trade drop center. Giving random.");
			return CellFinderLoose.RandomCellWith(validator, map, 1000);
		}

		// Token: 0x0600C8F9 RID: 51449 RVA: 0x0049C360 File Offset: 0x0049A560
		public static IntVec3 TryFindSafeLandingSpotCloseToColony(Map map, IntVec2 size, Faction faction = null, int borderWidth = 2)
		{
			DropCellFinder.<>c__DisplayClass3_0 CS$<>8__locals1 = new DropCellFinder.<>c__DisplayClass3_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.size = size;
			CS$<>8__locals1.faction = faction;
			DropCellFinder.<>c__DisplayClass3_0 CS$<>8__locals2 = CS$<>8__locals1;
			CS$<>8__locals2.size.x = CS$<>8__locals2.size.x + borderWidth;
			DropCellFinder.<>c__DisplayClass3_0 CS$<>8__locals3 = CS$<>8__locals1;
			CS$<>8__locals3.size.z = CS$<>8__locals3.size.z + borderWidth;
			DropCellFinder.tmpColonyBuildings.Clear();
			DropCellFinder.tmpColonyBuildings.AddRange(CS$<>8__locals1.map.listerBuildings.allBuildingsColonist);
			if (!DropCellFinder.tmpColonyBuildings.Any<Building>())
			{
				return CellFinderLoose.RandomCellWith(new Predicate<IntVec3>(CS$<>8__locals1.<TryFindSafeLandingSpotCloseToColony>g__SpotValidator|0), CS$<>8__locals1.map, 1000);
			}
			DropCellFinder.tmpColonyBuildings.Shuffle<Building>();
			for (int i = 0; i < DropCellFinder.tmpColonyBuildings.Count; i++)
			{
				IntVec3 intVec;
				if (DropCellFinder.TryFindDropSpotNear(DropCellFinder.tmpColonyBuildings[i].Position, CS$<>8__locals1.map, out intVec, false, false, false, new IntVec2?(CS$<>8__locals1.size), true) && DropCellFinder.SkyfallerCanLandAt(intVec, CS$<>8__locals1.map, CS$<>8__locals1.size, CS$<>8__locals1.faction))
				{
					DropCellFinder.tmpColonyBuildings.Clear();
					return intVec;
				}
			}
			DropCellFinder.tmpColonyBuildings.Clear();
			return CellFinderLoose.RandomCellWith(new Predicate<IntVec3>(CS$<>8__locals1.<TryFindSafeLandingSpotCloseToColony>g__SpotValidator|0), CS$<>8__locals1.map, 1000);
		}

		// Token: 0x0600C8FA RID: 51450 RVA: 0x0049C490 File Offset: 0x0049A690
		public static bool SkyfallerCanLandAt(IntVec3 c, Map map, IntVec2 size, Faction faction = null)
		{
			CellRect cellRect = GenAdj.OccupiedRect(c, Rot4.North, size);
			if (!cellRect.InBounds(map))
			{
				return false;
			}
			if (!DropCellFinder.IsSafeDropSpot(c, map, faction, new IntVec2?(size), 5, 35, 15, null))
			{
				return false;
			}
			foreach (IntVec3 c2 in cellRect)
			{
				List<Thing> thingList = c2.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing is IActiveTransporter || thing is Skyfaller)
					{
						return false;
					}
					if (thing.def.preventSkyfallersLandingOn)
					{
						return false;
					}
					ThingCategory category = thing.def.category;
					if (category == ThingCategory.Item || category == ThingCategory.Building)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600C8FB RID: 51451 RVA: 0x0049C584 File Offset: 0x0049A784
		public static IntVec3 GetBestShuttleLandingSpot(Map map, Faction factionForFindingSpot)
		{
			Thing thing;
			return DropCellFinder.GetBestShuttleLandingSpot(map, factionForFindingSpot, out thing);
		}

		// Token: 0x0600C8FC RID: 51452 RVA: 0x0049C59C File Offset: 0x0049A79C
		public static IntVec3 GetBestShuttleLandingSpot(Map map, Faction factionForFindingSpot, out Thing firstBlockingThing)
		{
			IntVec3 result;
			if (!DropCellFinder.TryFindShipLandingArea(map, out result, out firstBlockingThing))
			{
				result = DropCellFinder.TryFindSafeLandingSpotCloseToColony(map, ThingDefOf.Shuttle.Size, factionForFindingSpot, 2);
			}
			if (!result.IsValid && !DropCellFinder.FindSafeLandingSpot(out result, factionForFindingSpot, map, 35, 15, 25, new IntVec2?(ThingDefOf.Shuttle.Size + new IntVec2(2, 2)), null))
			{
				IntVec3 intVec = DropCellFinder.RandomDropSpot(map, true);
				if (!intVec.IsValid)
				{
					intVec = DropCellFinder.RandomDropSpot(map, false);
				}
				if (!DropCellFinder.TryFindDropSpotNear(intVec, map, out result, false, false, false, new IntVec2?(ThingDefOf.Shuttle.Size + new IntVec2(2, 2)), true))
				{
					result = intVec;
				}
			}
			return result;
		}

		// Token: 0x0600C8FD RID: 51453 RVA: 0x0049C64C File Offset: 0x0049A84C
		public static bool TryFindShipLandingArea(Map map, out IntVec3 result, out Thing firstBlockingThing)
		{
			DropCellFinder.tmpShipLandingAreas.Clear();
			List<ShipLandingArea> landingZones = ShipLandingBeaconUtility.GetLandingZones(map);
			if (landingZones.Any<ShipLandingArea>())
			{
				for (int i = 0; i < landingZones.Count; i++)
				{
					if (landingZones[i].Clear)
					{
						DropCellFinder.tmpShipLandingAreas.Add(landingZones[i]);
					}
				}
				if (DropCellFinder.tmpShipLandingAreas.Any<ShipLandingArea>())
				{
					result = DropCellFinder.tmpShipLandingAreas.RandomElement<ShipLandingArea>().CenterCell;
					firstBlockingThing = null;
					DropCellFinder.tmpShipLandingAreas.Clear();
					return true;
				}
				firstBlockingThing = landingZones[0].FirstBlockingThing;
			}
			else
			{
				firstBlockingThing = null;
			}
			result = IntVec3.Invalid;
			DropCellFinder.tmpShipLandingAreas.Clear();
			return false;
		}

		// Token: 0x0600C8FE RID: 51454 RVA: 0x0049C6FC File Offset: 0x0049A8FC
		public static bool TryFindDropSpotNear(IntVec3 center, Map map, out IntVec3 result, bool allowFogged, bool canRoofPunch, bool allowIndoors = true, IntVec2? size = null, bool mustBeReachableFromCenter = true)
		{
			return DropCellFinder.TryFindDropSpotNear(center, map, out result, allowFogged, canRoofPunch, 16, allowIndoors, size, mustBeReachableFromCenter);
		}

		// Token: 0x0600C8FF RID: 51455 RVA: 0x0049C71C File Offset: 0x0049A91C
		public static bool TryFindDropSpotNear(IntVec3 center, Map map, out IntVec3 result, bool allowFogged, bool canRoofPunch, int maxRadius, bool allowIndoors = true, IntVec2? size = null, bool mustBeReachableFromCenter = true)
		{
			DropCellFinder.<>c__DisplayClass10_0 CS$<>8__locals1 = new DropCellFinder.<>c__DisplayClass10_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.size = size;
			CS$<>8__locals1.allowFogged = allowFogged;
			CS$<>8__locals1.canRoofPunch = canRoofPunch;
			CS$<>8__locals1.allowIndoors = allowIndoors;
			CS$<>8__locals1.mustBeReachableFromCenter = mustBeReachableFromCenter;
			CS$<>8__locals1.center = center;
			CS$<>8__locals1.maxRadius = maxRadius;
			if (DebugViewSettings.drawDestSearch)
			{
				CS$<>8__locals1.map.debugDrawer.FlashCell(CS$<>8__locals1.center, 1f, "center", 50);
			}
			CS$<>8__locals1.centerRoom = CS$<>8__locals1.center.GetRoom(CS$<>8__locals1.map);
			if ((CS$<>8__locals1.allowIndoors & CS$<>8__locals1.canRoofPunch) && CS$<>8__locals1.centerRoom != null && !CS$<>8__locals1.centerRoom.PsychologicallyOutdoors)
			{
				if (CS$<>8__locals1.<TryFindDropSpotNear>g__TryFindCell|1(new Predicate<IntVec3>(CS$<>8__locals1.<TryFindDropSpotNear>g__SameRoomValidator|2), out result))
				{
					return true;
				}
				if (CS$<>8__locals1.<TryFindDropSpotNear>g__TryFindCell|1(new Predicate<IntVec3>(CS$<>8__locals1.<TryFindDropSpotNear>g__AnyRoomValidator|3), out result))
				{
					return true;
				}
			}
			return CS$<>8__locals1.<TryFindDropSpotNear>g__TryFindCell|1(new Predicate<IntVec3>(CS$<>8__locals1.<TryFindDropSpotNear>g__Validator|0), out result);
		}

		// Token: 0x0600C900 RID: 51456 RVA: 0x0049C814 File Offset: 0x0049AA14
		public static bool IsGoodDropSpot(IntVec3 c, Map map, bool allowFogged, bool canRoofPunch, bool allowIndoors = true)
		{
			if (map == null || !c.InBounds(map) || !c.Standable(map))
			{
				return false;
			}
			if (!DropCellFinder.CanPhysicallyDropInto(c, map, canRoofPunch, allowIndoors))
			{
				if (DebugViewSettings.drawDestSearch)
				{
					map.debugDrawer.FlashCell(c, 0f, "phys", 50);
				}
				return false;
			}
			if (Current.ProgramState == ProgramState.Playing && !allowFogged && c.Fogged(map))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing is IActiveTransporter || thing is Skyfaller)
				{
					return false;
				}
				Building building = thing as Building;
				if (building == null || !building.IsClearableFreeBuilding)
				{
					if (thing.def.IsEdifice())
					{
						return false;
					}
					if (thing.def.preventSkyfallersLandingOn)
					{
						return false;
					}
					if (thing.def.category != ThingCategory.Plant && GenSpawn.SpawningWipes(ThingDefOf.ActiveDropPod, thing.def, false))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600C901 RID: 51457 RVA: 0x0049C904 File Offset: 0x0049AB04
		private static bool AnyAdjacentGoodDropSpot(IntVec3 c, Map map, bool allowFogged, bool canRoofPunch)
		{
			return DropCellFinder.IsGoodDropSpot(c + IntVec3.North, map, allowFogged, canRoofPunch, true) || DropCellFinder.IsGoodDropSpot(c + IntVec3.East, map, allowFogged, canRoofPunch, true) || DropCellFinder.IsGoodDropSpot(c + IntVec3.South, map, allowFogged, canRoofPunch, true) || DropCellFinder.IsGoodDropSpot(c + IntVec3.West, map, allowFogged, canRoofPunch, true);
		}

		// Token: 0x0600C902 RID: 51458 RVA: 0x0049C96C File Offset: 0x0049AB6C
		public static IntVec3 FindRaidDropCenterDistant(Map map, bool allowRoofed = false, bool allowWater = true)
		{
			Faction hostFaction = map.ParentFaction ?? Faction.OfPlayer;
			IEnumerable<Thing> enumerable = map.mapPawns.FreeHumanlikesSpawnedOfFaction(hostFaction);
			if (hostFaction == Faction.OfPlayer)
			{
				enumerable = enumerable.Concat(map.listerBuildings.allBuildingsColonist);
			}
			else
			{
				enumerable = enumerable.Concat(from x in map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)
				where x.Faction == hostFaction
				select x);
			}
			int num = 0;
			float num2 = 65f;
			foreach (IntVec3 intVec in (from cell in map.AllCells
			where DropCellFinder.CanPhysicallyDropInto(cell, map, true, false) && !cell.Fogged(map) && cell.DistanceToEdge(map) >= 5 && (allowRoofed || !cell.Roofed(map)) && (allowWater || !cell.GetTerrain(map).IsWater)
			select cell).InRandomOrder(null))
			{
				num++;
				if (num > 300)
				{
					return intVec;
				}
				num2 -= 0.2f;
				bool flag = false;
				foreach (Thing thing in enumerable)
				{
					if ((float)(intVec - thing.Position).LengthHorizontalSquared < num2 * num2)
					{
						flag = true;
						break;
					}
				}
				if (!flag && map.reachability.CanReachFactionBase(intVec, hostFaction))
				{
					return intVec;
				}
			}
			return map.AllCells.RandomElement<IntVec3>();
		}

		// Token: 0x0600C903 RID: 51459 RVA: 0x0049CB30 File Offset: 0x0049AD30
		public static bool TryFindRaidDropCenterClose(out IntVec3 spot, Map map, bool canRoofPunch = true, bool allowIndoors = true, bool closeWalk = true, int maxRadius = -1)
		{
			Faction parentFaction = map.ParentFaction;
			if (parentFaction == null)
			{
				return RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => DropCellFinder.CanPhysicallyDropInto(x, map, canRoofPunch, allowIndoors) && !x.Fogged(map) && x.Standable(map), map, out spot);
			}
			int num = 0;
			Predicate<IntVec3> <>9__1;
			for (;;)
			{
				IntVec3 root = IntVec3.Invalid;
				if (map.mapPawns.FreeHumanlikesSpawnedOfFaction(parentFaction).Any<Pawn>())
				{
					root = map.mapPawns.FreeHumanlikesSpawnedOfFaction(parentFaction).RandomElement<Pawn>().Position;
				}
				else
				{
					if (parentFaction == Faction.OfPlayer)
					{
						List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
						for (int i = 0; i < allBuildingsColonist.Count; i++)
						{
							if (DropCellFinder.TryFindDropSpotNear(allBuildingsColonist[i].Position, map, out root, true, canRoofPunch, allowIndoors, null, true))
							{
								break;
							}
						}
					}
					else
					{
						List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
						int num2 = 0;
						while (num2 < list.Count && (list[num2].Faction != parentFaction || !DropCellFinder.TryFindDropSpotNear(list[num2].Position, map, out root, true, canRoofPunch, allowIndoors, null, true)))
						{
							num2++;
						}
					}
					if (!root.IsValid)
					{
						Predicate<IntVec3> validator;
						if ((validator = <>9__1) == null)
						{
							validator = (<>9__1 = ((IntVec3 x) => DropCellFinder.CanPhysicallyDropInto(x, map, canRoofPunch, allowIndoors) && !x.Fogged(map) && x.Standable(map)));
						}
						RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(validator, map, out root);
					}
				}
				int num3 = (maxRadius >= 0) ? maxRadius : 10;
				if (!closeWalk)
				{
					CellFinder.TryFindRandomCellNear(root, map, num3 * num3, null, out spot, 50);
				}
				else
				{
					spot = CellFinder.RandomClosewalkCellNear(root, map, num3, null);
				}
				if (DropCellFinder.CanPhysicallyDropInto(spot, map, canRoofPunch, allowIndoors) && !spot.Fogged(map))
				{
					break;
				}
				num++;
				if (num > 300)
				{
					goto Block_13;
				}
			}
			return true;
			Block_13:
			Predicate<IntVec3> <>9__2;
			Predicate<IntVec3> validator2;
			if ((validator2 = <>9__2) == null)
			{
				validator2 = (<>9__2 = ((IntVec3 c) => DropCellFinder.CanPhysicallyDropInto(c, map, canRoofPunch, allowIndoors)));
			}
			spot = CellFinderLoose.RandomCellWith(validator2, map, 1000);
			return false;
		}

		// Token: 0x0600C904 RID: 51460 RVA: 0x0049CD8C File Offset: 0x0049AF8C
		public static bool FindSafeLandingSpot(out IntVec3 spot, Faction faction, Map map, int distToHostiles = 35, int distToFires = 15, int distToEdge = 25, IntVec2? size = null, IntVec3? interactionCell = null)
		{
			spot = IntVec3.Invalid;
			int num = 200;
			while (num-- > 0)
			{
				IntVec3 intVec = DropCellFinder.RandomDropSpot(map, true);
				if (DropCellFinder.IsSafeDropSpot(intVec, map, faction, size, distToEdge, distToHostiles, distToFires, interactionCell))
				{
					spot = intVec;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C905 RID: 51461 RVA: 0x0049CDDC File Offset: 0x0049AFDC
		public static bool FindSafeLandingSpotNearAvoidingHostiles(Thing thing, Map map, out IntVec3 spot, int distToHostiles = 35, int distToFires = 15, int distToEdge = 25, IntVec2? size = null, IntVec3? interactionCell = null)
		{
			return RCellFinder.TryFindRandomSpotNearAvoidingHostilePawns(thing, map, (IntVec3 cell) => DropCellFinder.IsSafeDropSpot(cell, map, thing.Faction, size, distToEdge, distToHostiles, distToFires, interactionCell), out spot, 100f, 10f, 50f, true);
		}

		// Token: 0x0600C906 RID: 51462 RVA: 0x0049CE54 File Offset: 0x0049B054
		public static bool CanPhysicallyDropInto(IntVec3 c, Map map, bool canRoofPunch, bool allowedIndoors = true)
		{
			if (!c.Walkable(map))
			{
				return false;
			}
			if (c.GetTerrain(map).exposesToVacuum)
			{
				return false;
			}
			if (c.GetTerrain(map).IsWater)
			{
				return false;
			}
			RoofDef roof = c.GetRoof(map);
			if (roof != null)
			{
				if (!canRoofPunch)
				{
					return false;
				}
				if (roof.isThickRoof)
				{
					return false;
				}
			}
			if (!allowedIndoors)
			{
				Room room = c.GetRoom(map);
				if (room != null && !room.PsychologicallyOutdoors)
				{
					return false;
				}
			}
			if (!canRoofPunch)
			{
				foreach (Building building in map.listerBuildings.allBuildingsAnimalPenMarkers)
				{
					if (building.Position.GetDistrict(map, RegionType.Set_Passable) == c.GetDistrict(map, RegionType.Set_Passable))
					{
						CompAnimalPenMarker compAnimalPenMarker = building.TryGetComp<CompAnimalPenMarker>();
						if (compAnimalPenMarker != null && map.animalPenManager.GetPenMarkerState(compAnimalPenMarker).Enclosed)
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600C907 RID: 51463 RVA: 0x0049CF48 File Offset: 0x0049B148
		private static bool IsSafeDropSpot(IntVec3 cell, Map map, Faction faction, IntVec2? size = null, int distToEdge = 25, int distToHostiles = 35, int distToFires = 15, IntVec3? interactionCell = null)
		{
			DropCellFinder.<>c__DisplayClass18_0 CS$<>8__locals1 = new DropCellFinder.<>c__DisplayClass18_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.cell = cell;
			Faction factionBaseFaction = CS$<>8__locals1.map.ParentFaction ?? Faction.OfPlayer;
			if (!CS$<>8__locals1.cell.InBounds(CS$<>8__locals1.map))
			{
				return false;
			}
			if (size != null)
			{
				CellRect cellRect = GenAdj.OccupiedRect(CS$<>8__locals1.cell, Rot4.North, size.Value);
				if (!cellRect.InBounds(CS$<>8__locals1.map))
				{
					return false;
				}
				using (CellRect.Enumerator enumerator = cellRect.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 c = enumerator.Current;
						if (!c.SupportsStructureType(CS$<>8__locals1.map, TerrainAffordanceDefOf.Light))
						{
							return false;
						}
						if (!DropCellFinder.IsGoodDropSpot(c, CS$<>8__locals1.map, false, false, false))
						{
							return false;
						}
					}
					goto IL_ED;
				}
			}
			if (!DropCellFinder.IsGoodDropSpot(CS$<>8__locals1.cell, CS$<>8__locals1.map, false, false, false))
			{
				return false;
			}
			IL_ED:
			if (interactionCell != null && !DropCellFinder.IsGoodDropSpot(CS$<>8__locals1.cell + interactionCell.Value, CS$<>8__locals1.map, false, false, false))
			{
				return false;
			}
			if (distToEdge > 0 && CS$<>8__locals1.cell.CloseToEdge(CS$<>8__locals1.map, distToEdge))
			{
				return false;
			}
			if (faction != null)
			{
				foreach (IAttackTarget attackTarget in CS$<>8__locals1.map.attackTargetsCache.TargetsHostileToFaction(faction))
				{
					if (!attackTarget.ThreatDisabled(null) && attackTarget.Thing.Position.InHorDistOf(CS$<>8__locals1.cell, (float)distToHostiles))
					{
						return false;
					}
				}
			}
			if (!CS$<>8__locals1.map.reachability.CanReachFactionBase(CS$<>8__locals1.cell, factionBaseFaction))
			{
				return false;
			}
			if (size != null)
			{
				using (IEnumerator<IntVec3> enumerator3 = CellRect.CenteredOn(CS$<>8__locals1.cell, size.Value.x, size.Value.z).Cells.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						IntVec3 c2 = enumerator3.Current;
						if (CS$<>8__locals1.<IsSafeDropSpot>g__CellHasCrops|0(c2))
						{
							return false;
						}
					}
					goto IL_242;
				}
			}
			if (CS$<>8__locals1.<IsSafeDropSpot>g__CellHasCrops|0(CS$<>8__locals1.cell))
			{
				return false;
			}
			IL_242:
			CS$<>8__locals1.minDistToFiresSq = (float)(distToFires * distToFires);
			CS$<>8__locals1.closestDistSq = float.MaxValue;
			CS$<>8__locals1.firesCount = 0;
			RegionTraverser.BreadthFirstTraverse(CS$<>8__locals1.cell, CS$<>8__locals1.map, (Region from, Region to) => true, delegate(Region x)
			{
				List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Fire);
				for (int i = 0; i < list.Count; i++)
				{
					float num = (float)CS$<>8__locals1.cell.DistanceToSquared(list[i].Position);
					if (num <= CS$<>8__locals1.minDistToFiresSq)
					{
						if (num < CS$<>8__locals1.closestDistSq)
						{
							CS$<>8__locals1.closestDistSq = num;
						}
						int firesCount = CS$<>8__locals1.firesCount;
						CS$<>8__locals1.firesCount = firesCount + 1;
					}
				}
				return CS$<>8__locals1.closestDistSq <= CS$<>8__locals1.minDistToFiresSq && CS$<>8__locals1.firesCount >= 5;
			}, 15, RegionType.Set_Passable);
			return CS$<>8__locals1.closestDistSq > CS$<>8__locals1.minDistToFiresSq || CS$<>8__locals1.firesCount < 5;
		}

		// Token: 0x04007275 RID: 29301
		private static List<Building> tmpColonyBuildings = new List<Building>();

		// Token: 0x04007276 RID: 29302
		public static List<ShipLandingArea> tmpShipLandingAreas = new List<ShipLandingArea>();
	}
}
