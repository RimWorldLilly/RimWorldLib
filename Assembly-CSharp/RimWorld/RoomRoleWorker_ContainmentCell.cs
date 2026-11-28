using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001379 RID: 4985
	public class RoomRoleWorker_ContainmentCell : RoomRoleWorker
	{
		// Token: 0x06007B92 RID: 31634 RVA: 0x002BFEF4 File Offset: 0x002BE0F4
		public override float GetScore(Room room)
		{
			float num = 0f;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (ThingRequestGroup.EntityHolder.Includes(thing.def))
				{
					num += 100f;
				}
				else if (thing.def == ThingDefOf.Electroharvester || thing.def == ThingDefOf.ElectricInhibitor || thing.def == ThingDefOf.BioferriteHarvester)
				{
					num += 50f;
				}
			}
			return num;
		}

		// Token: 0x06007B93 RID: 31635 RVA: 0x002BFF70 File Offset: 0x002BE170
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (ThingRequestGroup.EntityHolder.Includes(buildingDef))
			{
				return 100f;
			}
			if (buildingDef == ThingDefOf.Electroharvester || buildingDef == ThingDefOf.ElectricInhibitor || buildingDef == ThingDefOf.BioferriteHarvester)
			{
				return 50f;
			}
			return 0f;
		}
	}
}
