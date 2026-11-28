using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001376 RID: 4982
	public class RoomRoleWorker_Bedroom : RoomRoleWorker
	{
		// Token: 0x06007B86 RID: 31622 RVA: 0x002BFA34 File Offset: 0x002BDC34
		public override float GetScore(Room room)
		{
			RoomRoleWorker_Bedroom.tmpBeds.Clear();
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			int num = 0;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike && building_Bed.def.building.bed_countsForBedroomOrBarracks)
				{
					if (building_Bed.Medical || building_Bed.ForPrisoners)
					{
						RoomRoleWorker_Bedroom.tmpBeds.Clear();
						return 0f;
					}
					num++;
					RoomRoleWorker_Bedroom.tmpBeds.Add(building_Bed);
				}
			}
			if (num == 0)
			{
				RoomRoleWorker_Bedroom.tmpBeds.Clear();
				return 0f;
			}
			bool flag = RoomRoleWorker_Bedroom.IsBedroom(RoomRoleWorker_Bedroom.tmpBeds);
			RoomRoleWorker_Bedroom.tmpBeds.Clear();
			if (!flag)
			{
				return 0f;
			}
			return 100000f;
		}

		// Token: 0x06007B87 RID: 31623 RVA: 0x002BFAFC File Offset: 0x002BDCFC
		public static bool IsBedroom(List<Building_Bed> beds)
		{
			RoomRoleWorker_Bedroom.children.Clear();
			RoomRoleWorker_Bedroom.adults.Clear();
			bool result = RoomRoleWorker_Bedroom.IsBedroomHelper(beds);
			RoomRoleWorker_Bedroom.children.Clear();
			RoomRoleWorker_Bedroom.adults.Clear();
			return result;
		}

		// Token: 0x06007B88 RID: 31624 RVA: 0x002BFB2C File Offset: 0x002BDD2C
		private static bool IsBedroomHelper(List<Building_Bed> beds)
		{
			List<Pawn> list = null;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (Building_Bed building_Bed in beds)
			{
				List<Pawn> ownersForReading = building_Bed.OwnersForReading;
				if (ownersForReading.NullOrEmpty<Pawn>() && building_Bed.def.building.bed_emptyCountsForBarracks)
				{
					num++;
				}
				else
				{
					if (ownersForReading.Count > 0)
					{
						num2++;
					}
					bool flag = false;
					foreach (Pawn pawn in ownersForReading)
					{
						if (pawn.DevelopmentalStage.Juvenile())
						{
							RoomRoleWorker_Bedroom.children.Add(pawn);
						}
						else
						{
							RoomRoleWorker_Bedroom.adults.Add(pawn);
							if (list == null)
							{
								list = pawn.GetLoveCluster();
							}
							if (!list.Contains(pawn))
							{
								flag = true;
							}
						}
					}
					if (flag)
					{
						num3++;
					}
				}
			}
			if (num == 1 && num2 == 0)
			{
				return true;
			}
			if (num == 0 && num2 == 1)
			{
				return true;
			}
			if (num > 0)
			{
				return false;
			}
			if (RoomRoleWorker_Bedroom.adults.NullOrEmpty<Pawn>())
			{
				return true;
			}
			if (num3 > 0)
			{
				return false;
			}
			foreach (Pawn pawn2 in RoomRoleWorker_Bedroom.children)
			{
				Pawn mother = pawn2.GetMother();
				Pawn father = pawn2.GetFather();
				if (!RoomRoleWorker_Bedroom.adults.Any((Pawn adult) => adult == mother || adult == father))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007B89 RID: 31625 RVA: 0x002BFCF0 File Offset: 0x002BDEF0
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef.building == null || !buildingDef.thingClass.IsAssignableFrom(typeof(Building_Bed)) || !buildingDef.building.bed_humanlike || !buildingDef.building.bed_countsForBedroomOrBarracks)
			{
				return 0f;
			}
			bool flag = room.Role != null;
			if (flag)
			{
				RoomRoleWorker worker = room.Role.Worker;
				bool flag2 = worker is RoomRoleWorker_Barracks || worker is RoomRoleWorker_Hospital || worker is RoomRoleWorker_PrisonCell || worker is RoomRoleWorker_PrisonBarracks;
				flag = flag2;
			}
			if (flag)
			{
				return 0f;
			}
			if (room.Role != null && room.Role.Worker is RoomRoleWorker_Bedroom)
			{
				return -100000f;
			}
			return 100000f;
		}

		// Token: 0x04004DF3 RID: 19955
		private static List<Building_Bed> tmpBeds = new List<Building_Bed>();

		// Token: 0x04004DF4 RID: 19956
		private static List<Pawn> children = new List<Pawn>();

		// Token: 0x04004DF5 RID: 19957
		private static List<Pawn> adults = new List<Pawn>();
	}
}
