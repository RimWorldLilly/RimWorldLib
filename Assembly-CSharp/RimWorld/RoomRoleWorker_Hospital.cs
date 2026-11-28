using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137C RID: 4988
	public class RoomRoleWorker_Hospital : RoomRoleWorker
	{
		// Token: 0x06007B9B RID: 31643 RVA: 0x002C00C8 File Offset: 0x002BE2C8
		public override float GetScore(Room room)
		{
			bool flag = false;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike)
				{
					if (building_Bed.ForPrisoners)
					{
						return 0f;
					}
					if (building_Bed.Medical)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return 0f;
			}
			return 100000f;
		}

		// Token: 0x06007B9C RID: 31644 RVA: 0x002C013C File Offset: 0x002BE33C
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (room.Role != null && room.Role.Worker is RoomRoleWorker_Hospital)
			{
				return 0f;
			}
			if (buildingDef.building != null && buildingDef.building.bed_humanlike && buildingDef.building.bed_defaultMedical)
			{
				return 100000f;
			}
			return 0f;
		}
	}
}
