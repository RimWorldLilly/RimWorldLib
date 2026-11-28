using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200138C RID: 5004
	public class RoomStatWorker_Cleanliness : RoomStatWorker
	{
		// Token: 0x06007BCB RID: 31691 RVA: 0x002C0E1C File Offset: 0x002BF01C
		public override float GetScore(Room room)
		{
			float num = 0f;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Filth || thing.def.category == ThingCategory.Plant)
				{
					num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Cleanliness, true, -1);
				}
			}
			foreach (IntVec3 c in room.Cells)
			{
				num += c.GetTerrain(room.Map).GetStatValueAbstract(StatDefOf.Cleanliness, null);
			}
			return num / (float)room.CellCount;
		}
	}
}
