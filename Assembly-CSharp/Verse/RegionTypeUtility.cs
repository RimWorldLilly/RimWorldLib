using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000305 RID: 773
	public static class RegionTypeUtility
	{
		// Token: 0x06001787 RID: 6023 RVA: 0x0008D508 File Offset: 0x0008B708
		public static bool IsOneCellRegion(this RegionType regionType)
		{
			return regionType == RegionType.Portal;
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x0008D50E File Offset: 0x0008B70E
		public static bool AllowsMultipleRegionsPerDistrict(this RegionType regionType)
		{
			return regionType != RegionType.Portal;
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x0008D518 File Offset: 0x0008B718
		public static RegionType GetExpectedRegionType(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return RegionType.None;
			}
			if (c.GetDoor(map) != null)
			{
				return RegionType.Portal;
			}
			if (c.GetFence(map) != null)
			{
				return RegionType.Fence;
			}
			if (c.WalkableByNormal(map))
			{
				return RegionType.Normal;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.Fillage == FillCategory.Full)
				{
					return RegionType.None;
				}
			}
			return RegionType.ImpassableFreeAirExchange;
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x0008D581 File Offset: 0x0008B781
		public static bool Passable(this RegionType regionType)
		{
			return (regionType & RegionType.Set_Passable) > RegionType.None;
		}
	}
}
