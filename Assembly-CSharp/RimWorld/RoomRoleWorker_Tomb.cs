using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001388 RID: 5000
	public class RoomRoleWorker_Tomb : RoomRoleWorker
	{
		// Token: 0x06007BBE RID: 31678 RVA: 0x002C0A3C File Offset: 0x002BEC3C
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_Sarcophagus)
				{
					num++;
				}
			}
			return 50f * (float)num;
		}

		// Token: 0x06007BBF RID: 31679 RVA: 0x002C0A7D File Offset: 0x002BEC7D
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (!buildingDef.thingClass.IsAssignableFrom(typeof(Building_Sarcophagus)))
			{
				return 0f;
			}
			return 50f;
		}
	}
}
