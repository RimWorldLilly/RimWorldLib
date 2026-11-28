using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001383 RID: 4995
	public class RoomRoleWorker_PrisonCell : RoomRoleWorker
	{
		// Token: 0x06007BAF RID: 31663 RVA: 0x002C06FC File Offset: 0x002BE8FC
		public override float GetScore(Room room)
		{
			int num = 0;
			int num2 = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike)
				{
					if (!building_Bed.ForPrisoners)
					{
						return 0f;
					}
					if (building_Bed.Medical)
					{
						num2++;
					}
					else
					{
						num++;
					}
				}
			}
			if (num == 1)
			{
				return 170000f;
			}
			if (num2 == 1)
			{
				return 100000f;
			}
			return 0f;
		}

		// Token: 0x06007BB0 RID: 31664 RVA: 0x002C0788 File Offset: 0x002BE988
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (room.Role == null || !buildingDef.thingClass.IsAssignableFrom(typeof(Building_Bed)) || !(room.Role.Worker is RoomRoleWorker_PrisonCell))
			{
				return 0f;
			}
			if (buildingDef.building == null || !buildingDef.building.bed_humanlike)
			{
				return 0f;
			}
			return -170000f;
		}
	}
}
