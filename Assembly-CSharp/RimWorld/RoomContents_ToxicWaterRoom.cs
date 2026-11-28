using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001240 RID: 4672
	public class RoomContents_ToxicWaterRoom : RoomContentsWorker
	{
		// Token: 0x060073E8 RID: 29672 RVA: 0x00289641 File Offset: 0x00287841
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.SpawnPool(map, room);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x060073E9 RID: 29673 RVA: 0x00289658 File Offset: 0x00287858
		private void SpawnPool(Map map, LayoutRoom room)
		{
			RoomContents_ToxicWaterRoom.<>c__DisplayClass1_0 CS$<>8__locals1 = new RoomContents_ToxicWaterRoom.<>c__DisplayClass1_0();
			CS$<>8__locals1.room = room;
			CS$<>8__locals1.map = map;
			CellRect cellRect = CS$<>8__locals1.room.rects.MaxBy((CellRect r) => r.Area);
			int numCells = Mathf.RoundToInt((float)(Mathf.Min(cellRect.Width, cellRect.Height) * 2));
			List<IntVec3> list = GridShapeMaker.IrregularLump(cellRect.CenterCell, CS$<>8__locals1.map, numCells, new Predicate<IntVec3>(CS$<>8__locals1.<SpawnPool>g__Validator|0));
			for (int i = 0; i < list.Count; i++)
			{
				CS$<>8__locals1.map.terrainGrid.SetTerrain(list[i], TerrainDefOf.ToxicWaterShallow);
			}
			foreach (IntVec3 intVec in CS$<>8__locals1.room.Cells)
			{
				if (intVec.GetEdifice(CS$<>8__locals1.map) == null && CS$<>8__locals1.room.Contains(intVec, 1) && !intVec.GetTerrain(CS$<>8__locals1.map).IsWater && RoomGenUtility.IsAdjacentTo(intVec, new Predicate<IntVec3>(CS$<>8__locals1.<SpawnPool>g__IsWaterCell|2), false))
				{
					CS$<>8__locals1.map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Gravel);
				}
			}
		}
	}
}
