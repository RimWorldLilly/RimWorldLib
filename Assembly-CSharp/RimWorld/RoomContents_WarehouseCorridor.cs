using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200124D RID: 4685
	public class RoomContents_WarehouseCorridor : RoomContents_Corridor
	{
		// Token: 0x170013CB RID: 5067
		// (get) Token: 0x0600743C RID: 29756 RVA: 0x0028AD6F File Offset: 0x00288F6F
		protected override IntRange ExteriorDoorCount
		{
			get
			{
				return new IntRange(2, 3);
			}
		}

		// Token: 0x0600743D RID: 29757 RVA: 0x0028AD78 File Offset: 0x00288F78
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			RoomContents_WarehouseCorridor.SpawnShelves(map, room);
		}

		// Token: 0x0600743E RID: 29758 RVA: 0x0028AD8C File Offset: 0x00288F8C
		private static void SpawnShelves(Map map, LayoutRoom room)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_WarehouseCorridor.ShelvesPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.Shelf, count, RoomContents_WarehouseCorridor.ShelfGroupSizeRange, room, map, null, null, 1, 0, ThingDefOf.Steel, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x0400491A RID: 18714
		private static readonly FloatRange ShelvesPer10EdgeCells = new FloatRange(0.7f, 1.4f);

		// Token: 0x0400491B RID: 18715
		private static readonly IntRange ShelfGroupSizeRange = new IntRange(1, 2);
	}
}
