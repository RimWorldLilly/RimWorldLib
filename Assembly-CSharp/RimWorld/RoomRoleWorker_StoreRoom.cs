using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001386 RID: 4998
	public class RoomRoleWorker_StoreRoom : RoomRoleWorker
	{
		// Token: 0x06007BB7 RID: 31671 RVA: 0x002C0924 File Offset: 0x002BEB24
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_Storage)
				{
					num++;
				}
			}
			return 1f * (float)num;
		}

		// Token: 0x06007BB8 RID: 31672 RVA: 0x002C0965 File Offset: 0x002BEB65
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef.thingClass.IsAssignableFrom(typeof(Building_Storage)))
			{
				return 1f;
			}
			return 0f;
		}
	}
}
