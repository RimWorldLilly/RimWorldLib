using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001380 RID: 4992
	public class RoomRoleWorker_Nursery : RoomRoleWorker
	{
		// Token: 0x06007BA6 RID: 31654 RVA: 0x002C03A4 File Offset: 0x002BE5A4
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
				Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike && !building_Bed.Medical)
				{
					if (building_Bed.ForPrisoners || building_Bed.def.building.bed_maxBodySize >= LifeStageDefOf.HumanlikeChild.bodySizeFactor)
					{
						return 0f;
					}
					num++;
				}
			}
			if (num < 2)
			{
				return 0f;
			}
			return (float)num * 100200f;
		}

		// Token: 0x06007BA7 RID: 31655 RVA: 0x002C0444 File Offset: 0x002BE644
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			bool flag = room.Role != null;
			if (flag)
			{
				RoomRoleWorker worker = room.Role.Worker;
				bool flag2 = worker is RoomRoleWorker_Bedroom || worker is RoomRoleWorker_Barracks || worker is RoomRoleWorker_Hospital || worker is RoomRoleWorker_PrisonCell || worker is RoomRoleWorker_PrisonBarracks;
				flag = flag2;
			}
			if (flag)
			{
				return 0f;
			}
			if (buildingDef.building == null || !buildingDef.building.bed_humanlike)
			{
				return 0f;
			}
			bool flag3 = room.Role != null && room.Role.Worker is RoomRoleWorker_Nursery;
			if (buildingDef.building.bed_maxBodySize >= LifeStageDefOf.HumanlikeChild.bodySizeFactor)
			{
				if (!flag3)
				{
					return 0f;
				}
				return -100200f;
			}
			else
			{
				if (!flag3)
				{
					return 100200f;
				}
				return 0f;
			}
		}

		// Token: 0x04004DF7 RID: 19959
		private const int MinBabyBeds = 2;
	}
}
