using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001276 RID: 4726
	public class RoomPart_InsectHive : RoomPartWorker
	{
		// Token: 0x170013D9 RID: 5081
		// (get) Token: 0x06007503 RID: 29955 RVA: 0x00291454 File Offset: 0x0028F654
		protected virtual IntRange HivesCountRange
		{
			get
			{
				return new IntRange(1, 3);
			}
		}

		// Token: 0x06007504 RID: 29956 RVA: 0x0029145D File Offset: 0x0028F65D
		public RoomPart_InsectHive(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x06007505 RID: 29957 RVA: 0x00291468 File Offset: 0x0028F668
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			IntVec3 centerCell;
			if (!room.TryGetRandomCellInRoom(map, out centerCell, 3, 0, null, false))
			{
				centerCell = room.rects[0].CenterCell;
			}
			int randomInRange = this.HivesCountRange.RandomInRange;
			HiveUtility.SpawnHives(centerCell, map, randomInRange, 5f, WipeMode.VanishOrMoveAside, false, false, true, true, true, true);
		}
	}
}
