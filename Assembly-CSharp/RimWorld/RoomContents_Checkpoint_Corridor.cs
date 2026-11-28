using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001248 RID: 4680
	public class RoomContents_Checkpoint_Corridor : RoomContents_Corridor
	{
		// Token: 0x170013C1 RID: 5057
		// (get) Token: 0x0600741C RID: 29724 RVA: 0x0028A487 File Offset: 0x00288687
		protected override TerrainDef StripCorridorTerrain
		{
			get
			{
				return TerrainDefOf.AncientTile;
			}
		}

		// Token: 0x170013C2 RID: 5058
		// (get) Token: 0x0600741D RID: 29725 RVA: 0x0028A48E File Offset: 0x0028868E
		protected override ThingDef DoorThing
		{
			get
			{
				return ThingDefOf.AncientBlastDoor;
			}
		}

		// Token: 0x170013C3 RID: 5059
		// (get) Token: 0x0600741E RID: 29726 RVA: 0x0028A495 File Offset: 0x00288695
		protected override IntRange ExteriorDoorCount
		{
			get
			{
				return IntRange.Between(4, 4);
			}
		}

		// Token: 0x0600741F RID: 29727 RVA: 0x0028A49E File Offset: 0x0028869E
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomContents_Checkpoint_Corridor.SpawnCheckpoints(map, room, faction);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x06007420 RID: 29728 RVA: 0x0028A4B4 File Offset: 0x002886B4
		private static void SpawnCheckpoints(Map map, LayoutRoom room, Faction faction)
		{
			foreach (CellRect cellRect in room.rects)
			{
				Rot4 rot = (cellRect.Width > cellRect.Height) ? Rot4.East : Rot4.North;
				Rot4 rot2 = rot.Rotated(RotationDirection.Clockwise);
				CellRect cellRect2 = cellRect.ContractedBy(1);
				int sideLength = cellRect2.GetSideLength(rot2);
				float chance = (float)sideLength / 100f * 8f / (float)sideLength;
				IntVec3 intVec = cellRect2.GetCenterCellOnEdge(rot.Opposite);
				while (cellRect2.Contains(intVec))
				{
					CellRect rect = intVec.RectAbout(new IntVec2(3, 3));
					if (Rand.Chance(chance) && RoomContents_Checkpoint_Corridor.CanSpawnCheckpoint(map, rect, rot2))
					{
						RoomContents_Checkpoint_Corridor.SpawnCheckpoint(map, rect, rot2, faction);
						intVec += rot.FacingCell * 20;
					}
					else
					{
						intVec += rot.FacingCell;
					}
				}
			}
		}

		// Token: 0x06007421 RID: 29729 RVA: 0x0028A5CC File Offset: 0x002887CC
		private static bool CanSpawnCheckpoint(Map map, CellRect rect, Rot4 rot)
		{
			using (CellRect.Enumerator enumerator = rect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			CellRect cellRect = rect.ExpandedBy(1);
			for (int i = 0; i < 2; i++)
			{
				Rot4 dir = rot;
				if (i == 1)
				{
					dir = dir.Opposite;
				}
				using (IEnumerator<IntVec3> enumerator2 = cellRect.GetEdgeCells(dir).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.GetEdifice(map) == null)
						{
							return false;
						}
					}
				}
			}
			using (IEnumerator<IntVec3> enumerator2 = rect.AdjacentCellsCardinal.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.GetDoor(map) != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06007422 RID: 29730 RVA: 0x0028A6D8 File Offset: 0x002888D8
		private static void SpawnCheckpoint(Map map, CellRect rect, Rot4 rot, Faction faction)
		{
			foreach (IntVec3 loc in rect.Corners)
			{
				GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Barricade, ThingDefOf.Steel), loc, map, WipeMode.Vanish);
			}
			for (int i = 0; i < 2; i++)
			{
				Rot4 rot2 = rot;
				if (i == 1)
				{
					rot2 = rot.Opposite;
				}
				IntVec3 centerCellOnEdge = rect.GetCenterCellOnEdge(rot2);
				Thing thing = ThingMaker.MakeThing(Rand.Bool ? ThingDefOf.Turret_AncientArmoredTurret : ThingDefOf.AncientSecurityTurret, null);
				thing.SetFaction(faction ?? Faction.OfAncientsHostile, null);
				GenSpawn.Spawn(thing, centerCellOnEdge, map, WipeMode.Vanish);
			}
		}

		// Token: 0x04004914 RID: 18708
		private const float CheckpointsPerHundredCells = 8f;

		// Token: 0x04004915 RID: 18709
		private const int MinTilesPerCheckpoint = 20;
	}
}
