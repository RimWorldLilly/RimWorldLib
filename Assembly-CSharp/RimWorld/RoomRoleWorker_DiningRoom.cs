using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137B RID: 4987
	public class RoomRoleWorker_DiningRoom : RoomRoleWorker
	{
		// Token: 0x06007B98 RID: 31640 RVA: 0x002C0048 File Offset: 0x002BE248
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.category == ThingCategory.Building && thing.def.surfaceType == SurfaceType.Eat)
				{
					num++;
				}
			}
			return (float)num * 12f;
		}

		// Token: 0x06007B99 RID: 31641 RVA: 0x002C009F File Offset: 0x002BE29F
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef.category == ThingCategory.Building && buildingDef.surfaceType == SurfaceType.Eat)
			{
				return 12f;
			}
			return 0f;
		}
	}
}
