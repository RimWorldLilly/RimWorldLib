using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001239 RID: 4665
	public class RoomContents_Kitchen : RoomContentsWorker
	{
		// Token: 0x170013A6 RID: 5030
		// (get) Token: 0x060073BC RID: 29628 RVA: 0x00288858 File Offset: 0x00286A58
		protected virtual FloatRange GroupsPer100Cells
		{
			get
			{
				return new FloatRange(1f, 2f);
			}
		}

		// Token: 0x170013A7 RID: 5031
		// (get) Token: 0x060073BD RID: 29629 RVA: 0x00288869 File Offset: 0x00286A69
		protected virtual IntRange ItemsPerGroup
		{
			get
			{
				return new IntRange(1, 3);
			}
		}

		// Token: 0x170013A8 RID: 5032
		// (get) Token: 0x060073BE RID: 29630 RVA: 0x00288872 File Offset: 0x00286A72
		protected virtual IntRange SinksCount
		{
			get
			{
				return new IntRange(3, 6);
			}
		}

		// Token: 0x170013A9 RID: 5033
		// (get) Token: 0x060073BF RID: 29631 RVA: 0x0028887B File Offset: 0x00286A7B
		protected virtual IntRange SinkMaxRange
		{
			get
			{
				return new IntRange(2, 4);
			}
		}

		// Token: 0x060073C0 RID: 29632 RVA: 0x00288884 File Offset: 0x00286A84
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.SpawnEdgeThings(ThingDefOf.AncientMicrowave, map, room);
			this.SpawnEdgeThings(ThingDefOf.AncientRefrigerator, map, room);
			this.SpawnEdgeThings(ThingDefOf.AncientStove, map, room);
			this.SpawnSinks(map, room);
			this.ScatterTables(map, room);
			foreach (IntVec3 c in room.Cells)
			{
				if (Rand.Chance(0.75f))
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientTile);
				}
			}
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x060073C1 RID: 29633 RVA: 0x0028892C File Offset: 0x00286B2C
		private void ScatterTables(Map map, LayoutRoom room)
		{
			Rot4 value = Rot4.North;
			CellRect boundary = room.Boundary;
			if (boundary.Height > boundary.Width)
			{
				value = Rot4.East;
			}
			RoomGenUtility.FillWithPadding(ThingDefOf.Table1x2c, this.GetCount(room), room, map, new Rot4?(value), null, null, 1, ThingDefOf.Steel, false, false, null, null);
		}

		// Token: 0x060073C2 RID: 29634 RVA: 0x00288984 File Offset: 0x00286B84
		private void SpawnSinks(Map map, LayoutRoom room)
		{
			IntVec3 centerCell;
			if (!room.TryGetRandomCellInRoom(map, out centerCell, 4, 0, null, false))
			{
				centerCell = room.rects[0].CenterCell;
			}
			GenSpawn.SpawnIrregularLump(ThingDefOf.AncientKitchenSink, centerCell, map, this.SinksCount, this.SinkMaxRange, WipeMode.Vanish, (IntVec3 c) => this.IsValid(c, map, room), null, null, null, null);
		}

		// Token: 0x060073C3 RID: 29635 RVA: 0x00288A10 File Offset: 0x00286C10
		private void SpawnEdgeThings(ThingDef def, Map map, LayoutRoom room)
		{
			RoomGenUtility.FillAroundEdges(def, this.GetCount(room), this.ItemsPerGroup, room, map, null, null, 1, 0, null, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x060073C4 RID: 29636 RVA: 0x00288A3C File Offset: 0x00286C3C
		private int GetCount(LayoutRoom room)
		{
			float num = (float)room.Area / 100f;
			return Mathf.Max(Mathf.RoundToInt(this.GroupsPer100Cells.RandomInRange * num), 1);
		}

		// Token: 0x060073C5 RID: 29637 RVA: 0x00288A72 File Offset: 0x00286C72
		private bool IsValid(IntVec3 cell, Map map, LayoutRoom room)
		{
			return room.Contains(cell, 1) && cell.GetEdifice(map) == null;
		}

		// Token: 0x040048FD RID: 18685
		private const float FloorSteelPercentage = 0.75f;
	}
}
