using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001238 RID: 4664
	public class RoomContents_InnerCourtyardRooms : RoomContentsWorker
	{
		// Token: 0x060073B4 RID: 29620 RVA: 0x00288450 File Offset: 0x00286650
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.placedCrates = 0;
			CellRect rect = room.rects[0];
			foreach (IntVec3 cell in rect.ContractedBy(2).EdgeCells)
			{
				if (!RoomContents_InnerCourtyardRooms.ContainedWithinOtherRect(rect, cell, room, 1))
				{
					RoomContents_InnerCourtyardRooms.SpawnWall(cell, map);
				}
			}
			this.SpawnInteriorRooms(map, rect, room);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x060073B5 RID: 29621 RVA: 0x002884DC File Offset: 0x002866DC
		private void SpawnInteriorRooms(Map map, CellRect rect, LayoutRoom room)
		{
			CellRect cellRect = rect.ContractedBy(3);
			Queue<CellRect> queue = RoomGenUtility.SubdividedIntoRooms_NewTemp(map, cellRect, 4, 1, 2, ThingDefOf.AncientFortifiedWall, null);
			CellRect rect2;
			while (queue.TryDequeue(out rect2))
			{
				this.DoSubroomRoomInterior(map, rect2, cellRect, room);
			}
		}

		// Token: 0x060073B6 RID: 29622 RVA: 0x0028851C File Offset: 0x0028671C
		private void DoSubroomRoomInterior(Map map, CellRect rect, CellRect container, LayoutRoom room)
		{
			foreach (IntVec3 c in rect.Cells)
			{
				map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientTile);
			}
			float num = (float)rect.Area / 10f;
			int num2 = 0;
			int num3 = Mathf.Max(Mathf.RoundToInt(1f * num), 1);
			foreach (IntVec3 intVec in rect.ContractedBy(1).Cells.InRandomOrder(null))
			{
				if (RoomContents_InnerCourtyardRooms.IsValidCrateCell(intVec, map))
				{
					this.SpawnBox(intVec, map);
					if (++num2 >= num3)
					{
						break;
					}
				}
			}
			CellRect item = rect.ExpandedBy(1);
			ThingDef def = ModsConfig.OdysseyActive ? ThingDefOf.AncientBlastDoor : ThingDefOf.Door;
			ThingDef stuff = ModsConfig.OdysseyActive ? null : ThingDefOf.Steel;
			foreach (IntVec3 intVec2 in item.EdgeCells.InRandomOrder(null))
			{
				if (RoomGenUtility.IsGoodForDoor(intVec2, map) && !container.ContractedBy(1).Contains(intVec2))
				{
					GenSpawn.Spawn(ThingMaker.MakeThing(def, stuff), intVec2, map, WipeMode.Vanish);
					break;
				}
			}
			RoomGenUtility.SpawnWallAttatchments(room.sketch.layoutSketch.WallLampThing, map, new List<CellRect>(1)
			{
				item
			}, IntRange.Between(1, 3), null, null, null);
		}

		// Token: 0x060073B7 RID: 29623 RVA: 0x002886DC File Offset: 0x002868DC
		private void SpawnBox(IntVec3 cell, Map map)
		{
			ThingSetMakerDef mapGen_HighValueCrate = ThingSetMakerDefOf.MapGen_HighValueCrate;
			int num = this.placedCrates;
			this.placedCrates = num + 1;
			RoomGenUtility.SpawnHermeticCrate(cell, map, mapGen_HighValueCrate, num < 1, null);
		}

		// Token: 0x060073B8 RID: 29624 RVA: 0x0028870C File Offset: 0x0028690C
		private static bool IsValidCrateCell(IntVec3 pos, Map map)
		{
			foreach (IntVec3 intVec in pos.RectAbout(ThingDefOf.AncientHermeticCrate.Size, Rot4.South).Cells)
			{
				if (intVec.GetEdifice(map) != null)
				{
					return false;
				}
				using (List<IntVec3>.Enumerator enumerator2 = GenAdjFast.AdjacentCells8Way(intVec).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.GetEdifice(map) != null)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060073B9 RID: 29625 RVA: 0x002887C0 File Offset: 0x002869C0
		private static void SpawnWall(IntVec3 cell, Map map)
		{
			GenSpawn.Spawn(ThingDefOf.AncientFortifiedWall, cell, map, WipeMode.Vanish);
		}

		// Token: 0x060073BA RID: 29626 RVA: 0x002887D0 File Offset: 0x002869D0
		private static bool ContainedWithinOtherRect(CellRect rect, IntVec3 cell, LayoutRoom room, int contractedBy)
		{
			if (!rect.EdgeCells.Contains(cell))
			{
				return false;
			}
			foreach (CellRect rhs in room.rects)
			{
				if (!(rect == rhs) && rhs.ContractedBy(contractedBy).Contains(cell))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040048F9 RID: 18681
		private const int NormalCrates = 1;

		// Token: 0x040048FA RID: 18682
		private const int CratesPer10Cells = 1;

		// Token: 0x040048FB RID: 18683
		private const int InnerWallContract = 3;

		// Token: 0x040048FC RID: 18684
		private int placedCrates;
	}
}
