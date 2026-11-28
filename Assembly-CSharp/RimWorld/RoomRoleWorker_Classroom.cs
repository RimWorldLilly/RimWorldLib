using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001378 RID: 4984
	public class RoomRoleWorker_Classroom : RoomRoleWorker
	{
		// Token: 0x06007B8F RID: 31631 RVA: 0x002BFE38 File Offset: 0x002BE038
		public override float GetScore(Room room)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			int num = 0;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.IsBed && thing.def.building.bed_humanlike)
				{
					return 0f;
				}
				if (thing.def == ThingDefOf.SchoolDesk || thing.def == ThingDefOf.Blackboard)
				{
					num++;
				}
			}
			return (float)num * 8f;
		}

		// Token: 0x06007B90 RID: 31632 RVA: 0x002BFEBF File Offset: 0x002BE0BF
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			if (buildingDef == ThingDefOf.SchoolDesk || buildingDef == ThingDefOf.Blackboard)
			{
				return 8f;
			}
			return 0f;
		}

		// Token: 0x04004DF6 RID: 19958
		private const int MinBabyBeds = 2;
	}
}
