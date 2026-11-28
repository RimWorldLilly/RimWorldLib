using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137E RID: 4990
	public class RoomRoleWorker_Laboratory : RoomRoleWorker
	{
		// Token: 0x06007BA1 RID: 31649 RVA: 0x002C0308 File Offset: 0x002BE508
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				BuildingProperties building = containedAndAdjacentThings[i].def.building;
				if (((building != null) ? building.workTableRoomRole : null) == RoomRoleDefOf.Laboratory)
				{
					num++;
				}
			}
			return 60f * (float)num;
		}

		// Token: 0x06007BA2 RID: 31650 RVA: 0x002C0367 File Offset: 0x002BE567
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			BuildingProperties building = buildingDef.building;
			if (((building != null) ? building.workTableRoomRole : null) == RoomRoleDefOf.Laboratory)
			{
				return 60f;
			}
			return 0f;
		}
	}
}
