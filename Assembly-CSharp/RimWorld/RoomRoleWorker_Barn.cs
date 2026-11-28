using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001374 RID: 4980
	public class RoomRoleWorker_Barn : RoomRoleWorker
	{
		// Token: 0x06007B7F RID: 31615 RVA: 0x002BF864 File Offset: 0x002BDA64
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
				if (building_Bed != null && !building_Bed.def.building.bed_humanlike)
				{
					num++;
				}
			}
			return (float)num * 7.6f;
		}

		// Token: 0x06007B80 RID: 31616 RVA: 0x002BF8B9 File Offset: 0x002BDAB9
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef.building == null || !buildingDef.building.bed_humanlike)
			{
				return 0f;
			}
			return 7.6f;
		}
	}
}
