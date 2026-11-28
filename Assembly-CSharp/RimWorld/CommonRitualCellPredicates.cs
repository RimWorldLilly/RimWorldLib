using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020016F0 RID: 5872
	public static class CommonRitualCellPredicates
	{
		// Token: 0x060092D4 RID: 37588 RVA: 0x0035A9F8 File Offset: 0x00358BF8
		public static bool RemoveLeastDesirableRitualCells(List<IntVec3> cells, IntVec3 spot, Map map, Pawn pawn, CellRect rect)
		{
			cells.RemoveAll(rect, CommonRitualCellPredicates.InsideRect, false);
			cells.RemoveAll(map, CommonRitualCellPredicates.Standable, true);
			cells.RemoveAll(new ValueTuple<Map, IntVec3>(map, spot), CommonRitualCellPredicates.InSameRoomAsSpot, true);
			cells.RemoveAll(pawn, CommonRitualCellPredicates.Reachable, true);
			return cells.RemoveAll_IfNotAll(map, CommonRitualCellPredicates.NotOnBed) && cells.RemoveAll_IfNotAll(map, CommonRitualCellPredicates.NotInDoor);
		}

		// Token: 0x060092D5 RID: 37589 RVA: 0x0035AA60 File Offset: 0x00358C60
		public static Func<IntVec3, bool> DefaultValidator(IntVec3 spot, Map map, Pawn pawn, CellRect rect)
		{
			return (IntVec3 cell) => !CommonRitualCellPredicates.InsideRect(rect, cell) && CommonRitualCellPredicates.Standable(map, cell) && CommonRitualCellPredicates.InSameRoomAsSpot(new ValueTuple<Map, IntVec3>(map, spot), cell) && CommonRitualCellPredicates.Reachable(pawn, cell);
		}

		// Token: 0x060092D6 RID: 37590 RVA: 0x0035AA90 File Offset: 0x00358C90
		private static bool _InSameRoomAsSpot(ValueTuple<Map, IntVec3> context, IntVec3 c)
		{
			Map item = context.Item1;
			IntVec3 item2 = context.Item2;
			return WanderUtility.InSameRoom(c, item2, item);
		}

		// Token: 0x040058F6 RID: 22774
		public static readonly Predicate<CellRect, IntVec3> InsideRect = (CellRect rect, IntVec3 cell) => rect.Contains(cell);

		// Token: 0x040058F7 RID: 22775
		public static readonly Predicate<Pawn, IntVec3> Reachable = (Pawn pawn, IntVec3 cell) => pawn.CanReserveAndReach(cell, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false);

		// Token: 0x040058F8 RID: 22776
		public static readonly Predicate<Map, IntVec3> Standable = (Map map, IntVec3 cell) => cell.Standable(map);

		// Token: 0x040058F9 RID: 22777
		public static readonly Predicate<Map, IntVec3> OnBed = delegate(Map map, IntVec3 cell)
		{
			Building building = map.edificeGrid[cell];
			if (building == null)
			{
				return false;
			}
			ThingDef def = building.def;
			return ((def != null) ? new bool?(def.IsBed) : null).GetValueOrDefault();
		};

		// Token: 0x040058FA RID: 22778
		public static readonly Predicate<Map, IntVec3> NotOnBed = delegate(Map map, IntVec3 cell)
		{
			Building building = map.edificeGrid[cell];
			if (building == null)
			{
				return true;
			}
			ThingDef def = building.def;
			return !((def != null) ? new bool?(def.IsBed) : null).GetValueOrDefault();
		};

		// Token: 0x040058FB RID: 22779
		public static readonly Predicate<Map, IntVec3> InDoor = (Map map, IntVec3 cell) => cell.GetDoor(map) != null;

		// Token: 0x040058FC RID: 22780
		public static readonly Predicate<Map, IntVec3> NotInDoor = (Map map, IntVec3 cell) => cell.GetDoor(map) == null;

		// Token: 0x040058FD RID: 22781
		public static readonly Predicate<ValueTuple<Map, IntVec3>, IntVec3> InSameRoomAsSpot = new Predicate<ValueTuple<Map, IntVec3>, IntVec3>(CommonRitualCellPredicates._InSameRoomAsSpot);
	}
}
