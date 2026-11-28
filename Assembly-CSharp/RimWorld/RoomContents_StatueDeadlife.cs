using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001259 RID: 4697
	public class RoomContents_StatueDeadlife : RoomContents_DeadBodyLabyrinth
	{
		// Token: 0x170013CF RID: 5071
		// (get) Token: 0x06007461 RID: 29793 RVA: 0x0028B639 File Offset: 0x00289839
		protected override IntRange CorpseRange
		{
			get
			{
				return new IntRange(1, 3);
			}
		}

		// Token: 0x170013D0 RID: 5072
		// (get) Token: 0x06007462 RID: 29794 RVA: 0x0028B642 File Offset: 0x00289842
		protected override IntRange BloodFilthRange
		{
			get
			{
				return new IntRange(2, 4);
			}
		}

		// Token: 0x06007463 RID: 29795 RVA: 0x0028B64C File Offset: 0x0028984C
		protected override void SpawnCorpses(Map map, LayoutRoom room)
		{
			IntVec3 centerCell;
			if (!room.TryGetRandomCellInRoom(map, out centerCell, 4, 0, null, false))
			{
				centerCell = room.rects[0].CenterCell;
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.GrayStatueDeadlifeDust, null), centerCell, map, WipeMode.Vanish);
			int randomInRange = this.CorpseRange.RandomInRange;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 near = centerCell;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => this.ValidCell(c, map)));
				}
				IntVec3 cell;
				if (RCellFinder.TryFindRandomCellNearWith(near, validator, map, out cell, 2, 6))
				{
					base.SpawnCorpse(cell, map).InnerPawn.inventory.DestroyAll(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06007464 RID: 29796 RVA: 0x0028B720 File Offset: 0x00289920
		private bool ValidCell(IntVec3 cell, Map map)
		{
			return cell.GetFirstThing(map) == null;
		}
	}
}
