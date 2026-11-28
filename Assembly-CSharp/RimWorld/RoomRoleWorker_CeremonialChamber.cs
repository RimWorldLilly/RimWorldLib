using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001377 RID: 4983
	public class RoomRoleWorker_CeremonialChamber : RoomRoleWorker
	{
		// Token: 0x06007B8C RID: 31628 RVA: 0x002BFDD4 File Offset: 0x002BDFD4
		public override float GetScore(Room room)
		{
			float num = 0f;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i].def == ThingDefOf.PsychicRitualSpot)
				{
					num += 200f;
				}
			}
			return num;
		}

		// Token: 0x06007B8D RID: 31629 RVA: 0x002BFE1B File Offset: 0x002BE01B
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef == ThingDefOf.PsychicRitualSpot)
			{
				return 200f;
			}
			return 0f;
		}
	}
}
