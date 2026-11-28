using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001249 RID: 4681
	public class RoomContents_ComplexCorridor : RoomContents_Corridor
	{
		// Token: 0x170013C4 RID: 5060
		// (get) Token: 0x06007424 RID: 29732 RVA: 0x0028A798 File Offset: 0x00288998
		protected override IntRange ExteriorDoorCount
		{
			get
			{
				return new IntRange(2, 3);
			}
		}

		// Token: 0x06007425 RID: 29733 RVA: 0x0028A7A1 File Offset: 0x002889A1
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			RoomContents_ComplexCorridor.SpawnShelves(map, room);
		}

		// Token: 0x06007426 RID: 29734 RVA: 0x0028A7B8 File Offset: 0x002889B8
		private static void SpawnShelves(Map map, LayoutRoom room)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_ComplexCorridor.ShelvesPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.Shelf, count, RoomContents_ComplexCorridor.ShelfGroupSizeRange, room, map, null, null, 1, 0, ThingDefOf.Steel, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x04004916 RID: 18710
		private static readonly FloatRange ShelvesPer10EdgeCells = new FloatRange(0.35f, 0.35f);

		// Token: 0x04004917 RID: 18711
		private static readonly IntRange ShelfGroupSizeRange = new IntRange(1, 2);
	}
}
