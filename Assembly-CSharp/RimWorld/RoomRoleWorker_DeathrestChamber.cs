using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137A RID: 4986
	public class RoomRoleWorker_DeathrestChamber : RoomRoleWorker
	{
		// Token: 0x06007B95 RID: 31637 RVA: 0x002BFFB0 File Offset: 0x002BE1B0
		public override float GetScore(Room room)
		{
			float num = 0f;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def == ThingDefOf.DeathrestCasket)
				{
					num += 1f;
				}
				else if (thing.TryGetComp<CompDeathrestBindable>() != null)
				{
					num += 0.5f;
				}
			}
			return num * 100f;
		}

		// Token: 0x06007B96 RID: 31638 RVA: 0x002C0011 File Offset: 0x002BE211
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef == ThingDefOf.DeathrestCasket)
			{
				return 100f;
			}
			if (buildingDef.HasComp(typeof(CompDeathrestBindable)))
			{
				return 50f;
			}
			return 0f;
		}
	}
}
