using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001389 RID: 5001
	public class RoomRoleWorker_Workshop : RoomRoleWorker
	{
		// Token: 0x06007BC1 RID: 31681 RVA: 0x002C0AAC File Offset: 0x002BECAC
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				BuildingProperties building = containedAndAdjacentThings[i].def.building;
				if (((building != null) ? building.workTableRoomRole : null) == RoomRoleDefOf.Workshop)
				{
					num++;
				}
			}
			return 27f * (float)num;
		}

		// Token: 0x06007BC2 RID: 31682 RVA: 0x002C0B03 File Offset: 0x002BED03
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			BuildingProperties building = buildingDef.building;
			if (((building != null) ? building.workTableRoomRole : null) == RoomRoleDefOf.Workshop)
			{
				return 27f;
			}
			return 0f;
		}
	}
}
