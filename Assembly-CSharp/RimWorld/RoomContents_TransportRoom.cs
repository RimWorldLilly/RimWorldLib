using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001241 RID: 4673
	public class RoomContents_TransportRoom : RoomContentsWorker
	{
		// Token: 0x060073EB RID: 29675 RVA: 0x002897C4 File Offset: 0x002879C4
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.subroom = RoomContents_TransportRoom.GetSubroomRect(map, room);
			RoomContents_TransportRoom.SetDefaultRoof(map, room);
			RoomContents_TransportRoom.FillSubroom(map, room, this.subroom);
			this.SpawnShelves(map, room);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x060073EC RID: 29676 RVA: 0x002897FC File Offset: 0x002879FC
		private static void SetDefaultRoof(Map map, LayoutRoom room)
		{
			foreach (CellRect cellRect in room.rects)
			{
				foreach (IntVec3 c in cellRect.Cells)
				{
					map.roofGrid.SetRoof(c, RoofDefOf.RoofConstructed);
				}
			}
		}

		// Token: 0x060073ED RID: 29677 RVA: 0x00289890 File Offset: 0x00287A90
		protected override bool IsValidCellBase(ThingDef thing, ThingDef stuff, IntVec3 cell, LayoutRoom room, Map map)
		{
			return !this.subroom.Contains(cell) && base.IsValidCellBase(thing, stuff, cell, room, map);
		}

		// Token: 0x060073EE RID: 29678 RVA: 0x002898B0 File Offset: 0x00287AB0
		private static void FillSubroom(Map map, LayoutRoom room, CellRect rect)
		{
			CellRect cellRect = rect.ContractedBy(1);
			foreach (IntVec3 intVec in rect.EdgeCells)
			{
				if (intVec.GetEdifice(map) == null)
				{
					GenSpawn.Spawn(ThingDefOf.AncientFortifiedWall, intVec, map, WipeMode.Vanish);
				}
			}
			for (int i = 0; i < 4; i++)
			{
				Rot4 rot = new Rot4(i);
				IntVec3 centerCellOnEdge = rect.GetCenterCellOnEdge(rot);
				if (room.rects[0].ContractedBy(1).Contains(centerCellOnEdge) && RoomGenUtility.IsGoodForDoor(centerCellOnEdge, map))
				{
					GenSpawn.Spawn(ThingDefOf.AncientBlastDoor, centerCellOnEdge, map, WipeMode.Vanish);
					break;
				}
			}
			foreach (IntVec3 c in cellRect.Cells)
			{
				map.terrainGrid.SetTerrain(c, TerrainDefOf.MetalTile);
				map.roofGrid.SetRoof(c, null);
			}
			foreach (IntVec3 loc in cellRect.Corners)
			{
				GenSpawn.Spawn(ThingDefOf.AncientShipBeacon, loc, map, WipeMode.Vanish);
			}
			if (Rand.Bool)
			{
				GenSpawn.Spawn(ThingDefOf.AncientTransportPod, rect.CenterCell, map, WipeMode.Vanish);
				return;
			}
			GenSpawn.Spawn(ThingDefOf.MalfunctioningTransportPod, rect.CenterCell, map, WipeMode.Vanish);
		}

		// Token: 0x060073EF RID: 29679 RVA: 0x00289A44 File Offset: 0x00287C44
		private static CellRect GetSubroomRect(Map map, LayoutRoom room)
		{
			CellRect result = CellRect.Empty;
			int @int = Rand.Int;
			for (int i = 0; i < 4; i++)
			{
				Rot4 rot = new Rot4(i + @int);
				IntVec3 corner = room.rects[0].GetCorner(rot, true);
				Rot4 rot2 = (rot == Rot4.North || rot == Rot4.West) ? Rot4.South : Rot4.North;
				Rot4 rot3 = (rot == Rot4.North || rot == Rot4.East) ? Rot4.West : Rot4.East;
				int num = Mathf.Min(RoomContents_TransportRoom.RoomSizeRange.TrueMax, room.rects[0].Width - 4);
				int num2 = Mathf.Min(RoomContents_TransportRoom.RoomSizeRange.TrueMax, room.rects[0].Height - 4);
				if ((float)num > (float)num2 * 1.25f)
				{
					num = (int)((float)num2 * 1.25f);
				}
				else if ((float)num2 > (float)num * 1.25f)
				{
					num2 = (int)((float)num * 1.25f);
				}
				for (int j = RoomContents_TransportRoom.RoomSizeRange.TrueMin; j < num; j++)
				{
					bool flag = true;
					for (int k = RoomContents_TransportRoom.RoomSizeRange.TrueMin; k < num2; k++)
					{
						IntVec3 b = rot3.FacingCell * j;
						IntVec3 b2 = rot2.FacingCell * k;
						CellRect cellRect = CellRect.FromLimits(corner + b + b2, corner);
						using (IEnumerator<IntVec3> enumerator = cellRect.EdgeCells.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.GetDoor(map) != null)
								{
									flag = false;
									break;
								}
							}
						}
						if (!flag)
						{
							break;
						}
						if (cellRect.Area > result.Area && cellRect.Width >= RoomContents_TransportRoom.RoomSizeRange.TrueMin && cellRect.Height >= RoomContents_TransportRoom.RoomSizeRange.TrueMin)
						{
							result = cellRect;
						}
					}
					if (!flag)
					{
						break;
					}
				}
			}
			if (result.Area == 0)
			{
				int maxExclusive = Mathf.Min(RoomContents_TransportRoom.RoomSizeRange.TrueMax, room.rects[0].Width / 2 - 3);
				int maxExclusive2 = Mathf.Min(RoomContents_TransportRoom.RoomSizeRange.TrueMax, room.rects[0].Height / 2 - 3);
				result = room.rects[0].CenterCell.RectAbout(new IntVec2(Rand.Range(RoomContents_TransportRoom.RoomSizeRange.TrueMin, maxExclusive), Rand.Range(RoomContents_TransportRoom.RoomSizeRange.TrueMin, maxExclusive2)));
			}
			return result;
		}

		// Token: 0x060073F0 RID: 29680 RVA: 0x00289D3C File Offset: 0x00287F3C
		private void SpawnShelves(Map map, LayoutRoom room)
		{
			RoomContents_TransportRoom.<>c__DisplayClass10_0 CS$<>8__locals1 = new RoomContents_TransportRoom.<>c__DisplayClass10_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.room = room;
			CS$<>8__locals1.possibleSpots = new List<IntVec3>(6);
			float num = (float)CS$<>8__locals1.room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_TransportRoom.ShelvesPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.Shelf, count, RoomContents_TransportRoom.ShelfGroupSizeRange, CS$<>8__locals1.room, CS$<>8__locals1.map, new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals1.<SpawnShelves>g__Validator|2), null, 1, 0, null, true, RotationDirection.Opposite, new Func<IntVec3, Rot4, Map, Thing>(CS$<>8__locals1.<SpawnShelves>g__SpawnAction|1), null);
			if (Rand.Bool && CS$<>8__locals1.possibleSpots.Any<IntVec3>())
			{
				IntVec3 intVec = CS$<>8__locals1.possibleSpots.RandomElement<IntVec3>();
				CS$<>8__locals1.possibleSpots.Remove(intVec);
				RoomContents_TransportRoom.SpawnApparel(ThingDefOf.Apparel_VacsuitHelmet, intVec, CS$<>8__locals1.map);
			}
			if (Rand.Bool && CS$<>8__locals1.possibleSpots.Any<IntVec3>())
			{
				IntVec3 intVec2 = CS$<>8__locals1.possibleSpots.RandomElement<IntVec3>();
				CS$<>8__locals1.possibleSpots.Remove(intVec2);
				RoomContents_TransportRoom.SpawnApparel(ThingDefOf.Apparel_Vacsuit, intVec2, CS$<>8__locals1.map);
			}
		}

		// Token: 0x060073F1 RID: 29681 RVA: 0x00289E80 File Offset: 0x00288080
		private static void SpawnApparel(ThingDef apparel, IntVec3 cell, Map map)
		{
			Thing thing = ThingMaker.MakeThing(apparel, null);
			GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Near, null, null, null, 1);
			thing.TrySetForbidden(true);
		}

		// Token: 0x04004909 RID: 18697
		private static readonly FloatRange ShelvesPer10EdgeCells = new FloatRange(1f, 3f);

		// Token: 0x0400490A RID: 18698
		private static readonly IntRange ShelfGroupSizeRange = new IntRange(2, 3);

		// Token: 0x0400490B RID: 18699
		private static readonly IntRange RoomSizeRange = new IntRange(5, 12);

		// Token: 0x0400490C RID: 18700
		private const float SubroomRatioMaximum = 1.25f;

		// Token: 0x0400490D RID: 18701
		private CellRect subroom;
	}
}
