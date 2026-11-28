using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B8F RID: 2959
	public class LearningGiver_Floordrawing : LearningGiver
	{
		// Token: 0x060055D2 RID: 21970 RVA: 0x001FF278 File Offset: 0x001FD478
		public static bool TryFindFloordrawingSpots(Pawn pawn, out IntVec3 drawFrom, out IntVec3 drawOn)
		{
			LearningGiver_Floordrawing.<>c__DisplayClass2_0 CS$<>8__locals1 = new LearningGiver_Floordrawing.<>c__DisplayClass2_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.traverseParams = TraverseParms.For(CS$<>8__locals1.pawn, Danger.None, TraverseMode.ByPawn, false, false, false, true);
			CS$<>8__locals1.innerDrawFrom = IntVec3.Invalid;
			CS$<>8__locals1.innerDrawOn = IntVec3.Invalid;
			CS$<>8__locals1.<TryFindFloordrawingSpots>g__FindDrawCell|0(false);
			if (CS$<>8__locals1.innerDrawFrom == IntVec3.Invalid)
			{
				CS$<>8__locals1.<TryFindFloordrawingSpots>g__FindDrawCell|0(true);
			}
			drawFrom = CS$<>8__locals1.innerDrawFrom;
			drawOn = CS$<>8__locals1.innerDrawOn;
			return drawFrom != IntVec3.Invalid && drawOn != IntVec3.Invalid;
		}

		// Token: 0x060055D3 RID: 21971 RVA: 0x001FF320 File Offset: 0x001FD520
		public static bool CanFloorDrawFrom(IntVec3 spot, Pawn drawer)
		{
			Map map = drawer.Map;
			return spot.InBounds(map) && spot.Standable(map) && !spot.IsForbidden(drawer) && map.areaManager.Home[spot] && spot.GetDoor(map) == null && drawer.CanReserve(spot, 1, -1, null, false);
		}

		// Token: 0x060055D4 RID: 21972 RVA: 0x001FF37D File Offset: 0x001FD57D
		private static bool CanDrawOn(IntVec3 cell, Map map, bool desperate)
		{
			return FilthMaker.CanMakeFilth(cell, map, ThingDefOf.Filth_Floordrawing, FilthSourceFlags.Pawn) && (desperate || (cell.GetFirstItem(map) == null && cell.GetFirstBuilding(map) == null && cell.GetFirstThing(map) == null));
		}

		// Token: 0x060055D5 RID: 21973 RVA: 0x001FF3B4 File Offset: 0x001FD5B4
		public static bool TryFloorDrawCellPair(IntVec3 companionCell, bool canDrawOnRoot, Pawn drawer, bool desperate, out bool shouldDrawOnRoot)
		{
			shouldDrawOnRoot = false;
			if (!LearningGiver_Floordrawing.CanFloorDrawFrom(companionCell, drawer))
			{
				return false;
			}
			bool flag = LearningGiver_Floordrawing.CanDrawOn(companionCell, drawer.Map, desperate);
			if (!canDrawOnRoot && !flag)
			{
				return false;
			}
			if (!canDrawOnRoot)
			{
				return true;
			}
			if (!flag)
			{
				shouldDrawOnRoot = true;
				return true;
			}
			shouldDrawOnRoot = Rand.Bool;
			return true;
		}

		// Token: 0x060055D6 RID: 21974 RVA: 0x001FF400 File Offset: 0x001FD600
		public override bool CanDo(Pawn pawn)
		{
			IntVec3 intVec;
			IntVec3 intVec2;
			return base.CanDo(pawn) && LearningGiver_Floordrawing.TryFindFloordrawingSpots(pawn, out intVec, out intVec2);
		}

		// Token: 0x060055D7 RID: 21975 RVA: 0x001FF424 File Offset: 0x001FD624
		public override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			IntVec3 c2;
			if (!LearningGiver_Floordrawing.TryFindFloordrawingSpots(pawn, out c, out c2))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c, c2);
		}

		// Token: 0x040032F8 RID: 13048
		private static readonly IntVec3[] offsets = new IntVec3[]
		{
			IntVec3.East,
			IntVec3.South
		};
	}
}
