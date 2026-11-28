using System;

namespace Verse
{
	// Token: 0x020002F6 RID: 758
	public static class RegionAndRoomQuery
	{
		// Token: 0x06001719 RID: 5913 RVA: 0x0008AF38 File Offset: 0x00089138
		public static Region RegionAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			if (!c.InBounds(map))
			{
				return null;
			}
			Region validRegionAt = map.regionGrid.GetValidRegionAt(c);
			if (validRegionAt != null && (validRegionAt.type & allowedRegionTypes) != RegionType.None)
			{
				return validRegionAt;
			}
			return null;
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x0008AF6D File Offset: 0x0008916D
		public static Region GetRegion(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			return RegionAndRoomQuery.RegionAt(thing.Position, thing.Map, allowedRegionTypes);
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x0008AF8B File Offset: 0x0008918B
		public static Region GetRegionHeld(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RegionAt(thing.PositionHeld, thing.MapHeld, allowedRegionTypes);
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x0008AFA0 File Offset: 0x000891A0
		public static District DistrictAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region region = RegionAndRoomQuery.RegionAt(c, map, allowedRegionTypes);
			if (region == null)
			{
				return null;
			}
			return region.District;
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x0008AFC4 File Offset: 0x000891C4
		public static Room RoomAt(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_All)
		{
			District district = RegionAndRoomQuery.DistrictAt(c, map, allowedRegionTypes);
			if (district == null)
			{
				return null;
			}
			return district.Room;
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x0008AFE8 File Offset: 0x000891E8
		public static District GetDistrict(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Thing spawnedParentOrMe = thing.SpawnedParentOrMe;
			if (spawnedParentOrMe != null)
			{
				return RegionAndRoomQuery.DistrictAt(spawnedParentOrMe.Position, spawnedParentOrMe.Map, allowedRegionTypes);
			}
			return null;
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x0008B014 File Offset: 0x00089214
		public static Room GetRoom(this Thing thing, RegionType allowedRegionTypes = RegionType.Set_All)
		{
			District district = thing.GetDistrict(allowedRegionTypes);
			if (district == null)
			{
				return null;
			}
			return district.Room;
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x0008B034 File Offset: 0x00089234
		public static bool IsOutside(this Thing thing)
		{
			Room room = thing.GetRoom(RegionType.Set_All);
			return room == null || room.PsychologicallyOutdoors;
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x0008B058 File Offset: 0x00089258
		public static District DistirctAtFast(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Region validRegionAt = map.regionGrid.GetValidRegionAt(c);
			if (validRegionAt != null && (validRegionAt.type & allowedRegionTypes) != RegionType.None)
			{
				return validRegionAt.District;
			}
			return null;
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x0008B088 File Offset: 0x00089288
		public static Room RoomAtOrAdjacent(IntVec3 c, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			Room room = RegionAndRoomQuery.RoomAt(c, map, allowedRegionTypes);
			if (room != null)
			{
				return room;
			}
			for (int i = 0; i < 8; i++)
			{
				room = RegionAndRoomQuery.RoomAt(c + GenAdj.AdjacentCells[i], map, allowedRegionTypes);
				if (room != null)
				{
					return room;
				}
			}
			return room;
		}
	}
}
