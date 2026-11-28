using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200123E RID: 4670
	public class RoomContents_SecurityRoom_Stockpile : RoomContentsWorker
	{
		// Token: 0x060073E0 RID: 29664 RVA: 0x002894C6 File Offset: 0x002876C6
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			RoomContents_SecurityRoom_Stockpile.SpawnAncientMachine(map, room);
			RoomContents_SecurityRoom_Stockpile.SpawnDestroyedConsoles(map, room);
		}

		// Token: 0x060073E1 RID: 29665 RVA: 0x002894E4 File Offset: 0x002876E4
		private static void SpawnAncientMachine(Map map, LayoutRoom room)
		{
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientMachine, 1, room, map, null, null, null, 1, null, false, false, null, null);
		}

		// Token: 0x060073E2 RID: 29666 RVA: 0x00289510 File Offset: 0x00287710
		private static void SpawnDestroyedConsoles(Map map, LayoutRoom room)
		{
			RoomContents_SecurityRoom_Stockpile.<>c__DisplayClass4_0 CS$<>8__locals1 = new RoomContents_SecurityRoom_Stockpile.<>c__DisplayClass4_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.room = room;
			float num = (float)CS$<>8__locals1.room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_SecurityRoom_Stockpile.DestroyedConsolesPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientDestroyedConsole, count, RoomContents_SecurityRoom_Stockpile.DestroyedConsolesGroupSize, CS$<>8__locals1.room, CS$<>8__locals1.map, new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals1.<SpawnDestroyedConsoles>g__ConsoleValidator|1), null, 1, 0, null, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x04004907 RID: 18695
		private static readonly FloatRange DestroyedConsolesPer10EdgeCells = new FloatRange(1f, 3f);

		// Token: 0x04004908 RID: 18696
		private static readonly IntRange DestroyedConsolesGroupSize = new IntRange(2, 3);
	}
}
