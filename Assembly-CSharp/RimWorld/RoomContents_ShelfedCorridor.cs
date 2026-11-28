using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200124C RID: 4684
	public class RoomContents_ShelfedCorridor : RoomContents_Corridor
	{
		// Token: 0x170013C9 RID: 5065
		// (get) Token: 0x06007436 RID: 29750 RVA: 0x0028ACA8 File Offset: 0x00288EA8
		protected override ThingDef DoorThing
		{
			get
			{
				return ThingDefOf.AncientBlastDoor;
			}
		}

		// Token: 0x170013CA RID: 5066
		// (get) Token: 0x06007437 RID: 29751 RVA: 0x0028ACAF File Offset: 0x00288EAF
		protected override IntRange ExteriorDoorCount
		{
			get
			{
				return new IntRange(2, 3);
			}
		}

		// Token: 0x06007438 RID: 29752 RVA: 0x0028ACB8 File Offset: 0x00288EB8
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			RoomContents_ShelfedCorridor.SpawnShelves(map, room);
		}

		// Token: 0x06007439 RID: 29753 RVA: 0x0028ACCC File Offset: 0x00288ECC
		private static void SpawnShelves(Map map, LayoutRoom room)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_ShelfedCorridor.ShelvesPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.Shelf, count, RoomContents_ShelfedCorridor.ShelfGroupSizeRange, room, map, null, null, 1, 0, ThingDefOf.Steel, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x04004918 RID: 18712
		private static readonly FloatRange ShelvesPer10EdgeCells = new FloatRange(0.35f, 0.35f);

		// Token: 0x04004919 RID: 18713
		private static readonly IntRange ShelfGroupSizeRange = new IntRange(1, 2);
	}
}
