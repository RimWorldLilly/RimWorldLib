using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001384 RID: 4996
	public class RoomRoleWorker_RecRoom : RoomRoleWorker
	{
		// Token: 0x06007BB2 RID: 31666 RVA: 0x002C07F4 File Offset: 0x002BE9F4
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.category == ThingCategory.Building)
				{
					List<JoyGiverDef> allDefsListForReading = DefDatabase<JoyGiverDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].countsForRecRoom && allDefsListForReading[j].thingDefs != null && allDefsListForReading[j].thingDefs.Contains(thing.def))
						{
							num++;
							break;
						}
					}
				}
			}
			return (float)num * 7f;
		}

		// Token: 0x06007BB3 RID: 31667 RVA: 0x002C089C File Offset: 0x002BEA9C
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef.category != ThingCategory.Building)
			{
				return 0f;
			}
			List<JoyGiverDef> allDefsListForReading = DefDatabase<JoyGiverDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].countsForRecRoom && allDefsListForReading[i].thingDefs != null && allDefsListForReading[i].thingDefs.Contains(buildingDef))
				{
					return 7f;
				}
			}
			return 0f;
		}
	}
}
