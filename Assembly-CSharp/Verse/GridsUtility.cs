using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000284 RID: 644
	public static class GridsUtility
	{
		// Token: 0x060012F1 RID: 4849 RVA: 0x0006BF90 File Offset: 0x0006A190
		public static float GetTemperature(this IntVec3 loc, Map map)
		{
			return GenTemperature.GetTemperatureForCell(loc, map);
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x0006BF99 File Offset: 0x0006A199
		public static Region GetRegion(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RegionAt(loc, map, allowedRegionTypes);
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0006BFA3 File Offset: 0x0006A1A3
		public static District GetDistrict(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.DistrictAt(loc, map, allowedRegionTypes);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0006BFB0 File Offset: 0x0006A1B0
		public static bool ConnectedCellCountExceeds(this IntVec3 cell, Map map, int count, RegionType allowed = RegionType.Set_Passable)
		{
			Region region = cell.GetRegion(map, allowed);
			if (region == null)
			{
				return false;
			}
			int num = region.CellCount;
			GridsUtility.connected.Add(region);
			GridsUtility.queue.Enqueue(region);
			Region region2;
			while (GridsUtility.queue.TryDequeue(out region2) && num < count)
			{
				num += region2.CellCount;
				using (List<RegionLink>.Enumerator enumerator = region2.links.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RegionLink regionLink = enumerator.Current;
						Region otherRegion = regionLink.GetOtherRegion(region2);
						if (GridsUtility.connected.Add(otherRegion))
						{
							GridsUtility.queue.Enqueue(otherRegion);
						}
					}
					continue;
				}
				break;
			}
			GridsUtility.connected.Clear();
			GridsUtility.queue.Clear();
			return num >= count;
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0006C080 File Offset: 0x0006A280
		public static Room GetRoom(this IntVec3 loc, Map map)
		{
			return RegionAndRoomQuery.RoomAt(loc, map, RegionType.Set_All);
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0006C08B File Offset: 0x0006A28B
		public static Room GetRoomOrAdjacent(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RoomAtOrAdjacent(loc, map, allowedRegionTypes);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0006C095 File Offset: 0x0006A295
		public static List<Thing> GetThingList(this IntVec3 c, Map map)
		{
			return map.thingGrid.ThingsListAt(c);
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x0006C0A3 File Offset: 0x0006A2A3
		public static float GetSnowDepth(this IntVec3 c, Map map)
		{
			return map.snowGrid.GetDepth(c);
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x0006C0B1 File Offset: 0x0006A2B1
		public static float GetSandDepth(this IntVec3 c, Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return 0f;
			}
			return map.sandGrid.GetDepth(c);
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x0006C0CC File Offset: 0x0006A2CC
		public static bool Fogged(this Thing t)
		{
			return t.MapHeld.fogGrid.IsFogged(t.PositionHeld);
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x0006C0E4 File Offset: 0x0006A2E4
		public static bool Fogged(this IntVec3 c, Map map)
		{
			return map.fogGrid.IsFogged(c);
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x0006C0F2 File Offset: 0x0006A2F2
		public static RoofDef GetRoof(this IntVec3 c, Map map)
		{
			return map.roofGrid.RoofAt(c);
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0006C100 File Offset: 0x0006A300
		public static bool Roofed(this IntVec3 c, Map map)
		{
			return map.roofGrid.Roofed(c);
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x0006C110 File Offset: 0x0006A310
		public static bool Filled(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.Fillage == FillCategory.Full;
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x0006C138 File Offset: 0x0006A338
		public static TerrainDef GetTerrain(this IntVec3 c, Map map)
		{
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(c);
			if (terrainDef == null)
			{
				return TerrainDefOf.Soil;
			}
			return terrainDef;
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x0006C15C File Offset: 0x0006A35C
		public static List<TerrainAffordanceDef> GetAffordances(this IntVec3 c, Map map)
		{
			if (map.terrainGrid.FoundationAt(c) != null)
			{
				return map.terrainGrid.FoundationAt(c).affordances;
			}
			return map.terrainGrid.TerrainAt(c).affordances;
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x0006C190 File Offset: 0x0006A390
		public static WaterBodyType GetWaterBodyType(this IntVec3 c, Map map)
		{
			if (map.terrainGrid.FoundationAt(c) != null)
			{
				return map.terrainGrid.BaseTerrainAt(c).waterBodyType;
			}
			if (map.terrainGrid.TerrainAt(c).temporary)
			{
				return map.terrainGrid.TerrainAtIgnoreTemp(map.cellIndices.CellToIndex(c)).waterBodyType;
			}
			return map.terrainGrid.TerrainAt(c).waterBodyType;
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x0006C1FE File Offset: 0x0006A3FE
		public static Plan GetPlan(this IntVec3 c, Map map)
		{
			return map.planManager.PlanAt(c);
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0006C20C File Offset: 0x0006A40C
		public static Zone GetZone(this IntVec3 c, Map map)
		{
			return map.zoneManager.ZoneAt(c);
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0006C21C File Offset: 0x0006A41C
		public static Plant GetPlant(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Plant)
				{
					return (Plant)list[i];
				}
			}
			return null;
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0006C26C File Offset: 0x0006A46C
		public static Thing GetRoofHolderOrImpassable(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.holdsRoof || thingList[i].def.passability == Traversability.Impassable)
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0006C2C4 File Offset: 0x0006A4C4
		public static Thing GetFirstThing(this IntVec3 c, Map map, ThingDef def)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == def)
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0006C304 File Offset: 0x0006A504
		public static ThingWithComps GetFirstThingWithComp<TComp>(this IntVec3 c, Map map) where TComp : ThingComp
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].TryGetComp<TComp>() != null)
				{
					return (ThingWithComps)thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0006C34C File Offset: 0x0006A54C
		public static T GetFirstThing<T>(this IntVec3 c, Map map) where T : Thing
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				T t = thingList[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0006C397 File Offset: 0x0006A597
		public static bool TryGetFirstThing<T>(this IntVec3 c, Map map, out T thing) where T : Thing
		{
			thing = c.GetFirstThing(map);
			return thing != null;
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x0006C3B4 File Offset: 0x0006A5B4
		public static Thing GetFirstHaulable(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.designateHaulable)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x0006C3FC File Offset: 0x0006A5FC
		public static Thing GetFirstItem(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Item)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x0006C444 File Offset: 0x0006A644
		public static IEnumerable<Thing> GetItems(this IntVec3 c, Map map)
		{
			GridsUtility.<GetItems>d__29 <GetItems>d__ = new GridsUtility.<GetItems>d__29(-2);
			<GetItems>d__.<>3__c = c;
			<GetItems>d__.<>3__map = map;
			return <GetItems>d__;
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0006C45C File Offset: 0x0006A65C
		public static int GetMaxItemsAllowedInCell(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			if (edifice != null)
			{
				return edifice.MaxItemsInCell;
			}
			return 1;
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x0006C47C File Offset: 0x0006A67C
		public static int GetItemCount(this IntVec3 c, Map map)
		{
			int num = 0;
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Item)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0006C4C4 File Offset: 0x0006A6C4
		public static int GetAllItemsStackCount(this IntVec3 c, Map map, ThingDef itemDef)
		{
			int num = 0;
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == itemDef)
				{
					num += list[i].stackCount;
				}
			}
			return num;
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0006C510 File Offset: 0x0006A710
		public static int GetItemStackSpaceLeftFor(this IntVec3 c, Map map, ThingDef itemDef)
		{
			int num = 0;
			int num2 = 0;
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Item)
				{
					num2++;
				}
				if (list[i].def == itemDef)
				{
					num += Mathf.Max(list[i].def.stackLimit - list[i].stackCount, 0);
				}
			}
			return num + Mathf.Max(c.GetMaxItemsAllowedInCell(map) - num2, 0) * itemDef.stackLimit;
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0006C5A8 File Offset: 0x0006A7A8
		public static bool IsBuildingInteractionCell(this IntVec3 cell, Map map)
		{
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = cell + GenAdj.CardinalDirections[i];
				if (c.InBounds(map))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null && edifice.def.hasInteractionCell && edifice.InteractionCell == cell)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0006C608 File Offset: 0x0006A808
		public static Building GetFirstBuilding(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Building building = list[i] as Building;
				if (building != null)
				{
					return building;
				}
			}
			return null;
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x0006C648 File Offset: 0x0006A848
		public static Pawn GetFirstPawn(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Pawn pawn = thingList[i] as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x0006C684 File Offset: 0x0006A884
		public static Mineable GetFirstMineable(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Mineable mineable = thingList[i] as Mineable;
				if (mineable != null)
				{
					return mineable;
				}
			}
			return null;
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0006C6C0 File Offset: 0x0006A8C0
		public static Blight GetFirstBlight(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Blight blight = thingList[i] as Blight;
				if (blight != null)
				{
					return blight;
				}
			}
			return null;
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0006C6FC File Offset: 0x0006A8FC
		public static Skyfaller GetFirstSkyfaller(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Skyfaller skyfaller = thingList[i] as Skyfaller;
				if (skyfaller != null)
				{
					return skyfaller;
				}
			}
			return null;
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0006C738 File Offset: 0x0006A938
		public static IPlantToGrowSettable GetPlantToGrowSettable(this IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetEdifice(map) as IPlantToGrowSettable;
			if (plantToGrowSettable == null)
			{
				plantToGrowSettable = (c.GetZone(map) as IPlantToGrowSettable);
			}
			return plantToGrowSettable;
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0006C764 File Offset: 0x0006A964
		public static Building GetTransmitter(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.EverTransmitsPower)
				{
					return (Building)list[i];
				}
			}
			return null;
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0006C7B0 File Offset: 0x0006A9B0
		public static Building_Door GetDoor(this IntVec3 c, Map map)
		{
			Building_Door building_Door = c.GetEdifice(map) as Building_Door;
			if (building_Door != null)
			{
				return building_Door;
			}
			return null;
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0006C7D0 File Offset: 0x0006A9D0
		public static Building GetFence(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			if (edifice != null && edifice.def.IsFence)
			{
				return edifice;
			}
			return null;
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0006C7F8 File Offset: 0x0006A9F8
		public static Building GetEdifice(this IntVec3 c, Map map)
		{
			return map.edificeGrid[c];
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0006C806 File Offset: 0x0006AA06
		public static Building GetEdificeSafe(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return null;
			}
			return map.edificeGrid[c];
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x0006C81F File Offset: 0x0006AA1F
		public static Thing GetCover(this IntVec3 c, Map map)
		{
			return map.coverGrid[c];
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0006C830 File Offset: 0x0006AA30
		public static Gas GetGas(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.category == ThingCategory.Gas)
				{
					return (Gas)thingList[i];
				}
			}
			return null;
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0006C878 File Offset: 0x0006AA78
		public static bool IsInPrisonCell(this IntVec3 c, Map map)
		{
			Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_All);
			if (roomOrAdjacent != null)
			{
				return roomOrAdjacent.IsPrisonCell;
			}
			string str = "Checking prison cell status of ";
			IntVec3 intVec = c;
			Log.Error(str + intVec.ToString() + " which is not in or adjacent to a room.");
			return false;
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0006C8C0 File Offset: 0x0006AAC0
		public static bool UsesOutdoorTemperature(this IntVec3 c, Map map)
		{
			Room room = c.GetRoom(map);
			if (room != null)
			{
				return room.UsesOutdoorTemperature;
			}
			Building edifice = c.GetEdifice(map);
			if (edifice != null)
			{
				IntVec3[] array = GenAdj.CellsAdjacent8Way(edifice).ToArray<IntVec3>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].InBounds(map))
					{
						room = array[i].GetRoom(map);
						if (room != null && room.UsesOutdoorTemperature)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0006C931 File Offset: 0x0006AB31
		public static bool IsPolluted(this IntVec3 c, Map map)
		{
			return map.pollutionGrid.IsPolluted(c);
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x0006C93F File Offset: 0x0006AB3F
		public static bool CanPollute(this IntVec3 c, Map map)
		{
			return map.pollutionGrid.CanPollute(c);
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0006C94D File Offset: 0x0006AB4D
		public static bool CanUnpollute(this IntVec3 c, Map map)
		{
			return map.pollutionGrid.CanUnpollute(c);
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0006C95B File Offset: 0x0006AB5B
		public static void Pollute(this IntVec3 c, Map map, bool silent = false)
		{
			map.pollutionGrid.SetPolluted(c, true, silent);
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0006C96C File Offset: 0x0006AB6C
		public static void Unpollute(this IntVec3 c, Map map)
		{
			map.pollutionGrid.SetPolluted(c, false, false);
			if (map.snowGrid.GetDepth(c) > 1E-45f)
			{
				map.snowGrid.MakeMeshDirty(c);
			}
			if (ModsConfig.OdysseyActive && map.sandGrid.GetDepth(c) > 1E-45f)
			{
				map.sandGrid.MakeMeshDirty(c);
			}
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0006C9CC File Offset: 0x0006ABCC
		public static float GetFertility(this IntVec3 c, Map map)
		{
			return map.fertilityGrid.FertilityAt(c);
		}

		// Token: 0x040010E0 RID: 4320
		private static readonly HashSet<Region> connected = new HashSet<Region>();

		// Token: 0x040010E1 RID: 4321
		private static readonly Queue<Region> queue = new Queue<Region>();
	}
}
