using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F1A RID: 7962
	public static class FuelingPortUtility
	{
		// Token: 0x0600CAFE RID: 51966 RVA: 0x004A8409 File Offset: 0x004A6609
		public static IntVec3 GetFuelingPortCell(Building podLauncher)
		{
			return FuelingPortUtility.GetFuelingPortCell(podLauncher.Position, podLauncher.Rotation);
		}

		// Token: 0x0600CAFF RID: 51967 RVA: 0x004A841C File Offset: 0x004A661C
		public static IntVec3 GetFuelingPortCell(IntVec3 center, Rot4 rot)
		{
			rot.Rotate(RotationDirection.Clockwise);
			return center + rot.FacingCell;
		}

		// Token: 0x0600CB00 RID: 51968 RVA: 0x004A8433 File Offset: 0x004A6633
		public static bool AnyFuelingPortGiverAt(IntVec3 c, Map map)
		{
			return FuelingPortUtility.FuelingPortGiverAt(c, map) != null;
		}

		// Token: 0x0600CB01 RID: 51969 RVA: 0x004A8440 File Offset: 0x004A6640
		public static Building FuelingPortGiverAt(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Building building = thingList[i] as Building;
				if (building != null && building.def.building.hasFuelingPort)
				{
					return building;
				}
			}
			return null;
		}

		// Token: 0x0600CB02 RID: 51970 RVA: 0x004A848C File Offset: 0x004A668C
		public static Building FuelingPortGiverAtFuelingPortCell(IntVec3 c, Map map)
		{
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c2 = c + GenAdj.CardinalDirections[i];
				if (c2.InBounds(map))
				{
					List<Thing> thingList = c2.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Building building = thingList[j] as Building;
						if (building != null && building.def.building.hasFuelingPort && FuelingPortUtility.GetFuelingPortCell(building) == c)
						{
							return building;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600CB03 RID: 51971 RVA: 0x004A8514 File Offset: 0x004A6714
		public static CompLaunchable LaunchableAt(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				CompLaunchable compLaunchable = thingList[i].TryGetComp<CompLaunchable>();
				if (compLaunchable != null)
				{
					return compLaunchable;
				}
			}
			return null;
		}
	}
}
