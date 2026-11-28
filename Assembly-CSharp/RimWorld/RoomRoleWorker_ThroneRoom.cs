using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001387 RID: 4999
	public class RoomRoleWorker_ThroneRoom : RoomRoleWorker
	{
		// Token: 0x06007BBA RID: 31674 RVA: 0x002C0991 File Offset: 0x002BEB91
		public static string Validate(Room room)
		{
			if (room == null || room.OutdoorsForWork)
			{
				return "ThroneMustBePlacedInside".Translate();
			}
			return null;
		}

		// Token: 0x06007BBB RID: 31675 RVA: 0x002C09B0 File Offset: 0x002BEBB0
		public override float GetScore(Room room)
		{
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			bool flag = false;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_Throne)
				{
					flag = true;
					break;
				}
			}
			return (float)((flag && RoomRoleWorker_ThroneRoom.Validate(room) == null) ? 10000 : 0);
		}

		// Token: 0x06007BBC RID: 31676 RVA: 0x002C09FD File Offset: 0x002BEBFD
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (room.Role == RoomRoleDefOf.ThroneRoom)
			{
				return 0f;
			}
			if (buildingDef != ThingDefOf.Throne)
			{
				return 0f;
			}
			return (float)((RoomRoleWorker_ThroneRoom.Validate(room) == null) ? 10000 : 0);
		}
	}
}
