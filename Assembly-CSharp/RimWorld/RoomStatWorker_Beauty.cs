using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200138B RID: 5003
	public class RoomStatWorker_Beauty : RoomStatWorker
	{
		// Token: 0x06007BC8 RID: 31688 RVA: 0x002C0C94 File Offset: 0x002BEE94
		public override float GetScore(Room room)
		{
			float num = 0f;
			int num2 = 0;
			RoomStatWorker_Beauty.countedThings.Clear();
			foreach (IntVec3 c in room.Cells)
			{
				num += BeautyUtility.CellBeauty(c, room.Map, RoomStatWorker_Beauty.countedThings);
				num2++;
			}
			RoomStatWorker_Beauty.countedAdjCells.Clear();
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.GetRoom(RegionType.Set_All) != room && !RoomStatWorker_Beauty.countedAdjCells.Contains(thing.Position))
				{
					num += BeautyUtility.CellBeauty(thing.Position, room.Map, RoomStatWorker_Beauty.countedThings);
					RoomStatWorker_Beauty.countedAdjCells.Add(thing.Position);
				}
			}
			RoomStatWorker_Beauty.countedThings.Clear();
			if (num2 == 0)
			{
				return 0f;
			}
			return num / RoomStatWorker_Beauty.CellCountCurve.Evaluate((float)num2);
		}

		// Token: 0x04004DF9 RID: 19961
		private static readonly SimpleCurve CellCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 20f),
				true
			},
			{
				new CurvePoint(40f, 40f),
				true
			},
			{
				new CurvePoint(100000f, 100000f),
				true
			}
		};

		// Token: 0x04004DFA RID: 19962
		private static HashSet<Thing> countedThings = new HashSet<Thing>();

		// Token: 0x04004DFB RID: 19963
		private static HashSet<IntVec3> countedAdjCells = new HashSet<IntVec3>();
	}
}
