using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001381 RID: 4993
	public class RoomRoleWorker_Playroom : RoomRoleWorker
	{
		// Token: 0x06007BA9 RID: 31657 RVA: 0x002C0528 File Offset: 0x002BE728
		public override float GetScore(Room room)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				Building_Bed building_Bed = thing as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike)
				{
					return 0f;
				}
				if (thing.GetStatValue(StatDefOf.BabyPlayGainFactor, true, -1) > 1f)
				{
					num++;
				}
			}
			return (float)num * 8f;
		}

		// Token: 0x06007BAA RID: 31658 RVA: 0x002C05A9 File Offset: 0x002BE7A9
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			if (buildingDef.GetStatValueAbstract(StatDefOf.BabyPlayGainFactor, null) > 1f)
			{
				return 8f;
			}
			return 0f;
		}
	}
}
