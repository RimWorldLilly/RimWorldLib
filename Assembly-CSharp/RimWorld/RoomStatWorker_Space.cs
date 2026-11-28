using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001390 RID: 5008
	public class RoomStatWorker_Space : RoomStatWorker
	{
		// Token: 0x06007BD5 RID: 31701 RVA: 0x002C117C File Offset: 0x002BF37C
		public override float GetScore(Room room)
		{
			if (room.PsychologicallyOutdoors)
			{
				return 350f;
			}
			float num = 0f;
			foreach (IntVec3 c in room.Cells)
			{
				if (c.Standable(room.Map))
				{
					num += 1.4f;
				}
				else if (c.WalkableByNormal(room.Map))
				{
					num += 0.5f;
				}
			}
			return Mathf.Min(num, 350f);
		}
	}
}
