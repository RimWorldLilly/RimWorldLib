using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001256 RID: 4694
	public class RoomContents_Obelisk : RoomContentsWorker
	{
		// Token: 0x0600745B RID: 29787 RVA: 0x0028B398 File Offset: 0x00289598
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			CellRect cellRect = room.rects[0];
			foreach (IntVec3 intVec in cellRect.Cells)
			{
				if (intVec.GetFirstBuilding(map) == null)
				{
					if (!(intVec - cellRect.CenterCell).IsCardinal && cellRect.CenterCell.DistanceTo(intVec) >= 8.9f)
					{
						GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.GrayWall, ThingDefOf.LabyrinthMatter), intVec, map, WipeMode.Vanish);
					}
					if (cellRect.CenterCell.DistanceTo(intVec) < 3.9f)
					{
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Voidmetal);
					}
				}
			}
			Building building = (Building)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.WarpedObelisk_Labyrinth, null), cellRect.CenterCell, map, WipeMode.Vanish);
			map.GetComponent<LabyrinthMapComponent>().labyrinthObelisk = building;
			string signalTag = "ThingDiscovered" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			SignalAction_Letter signalAction_Letter = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
			signalAction_Letter.signalTag = signalTag;
			signalAction_Letter.letterDef = LetterDefOf.PositiveEvent;
			signalAction_Letter.letterLabelKey = "LetterLabelObeliskDiscovered";
			signalAction_Letter.letterMessageKey = "LetterObeliskDiscovered";
			GenSpawn.Spawn(signalAction_Letter, building.Position, map, WipeMode.Vanish);
			room.SpawnRectTriggersForAction(signalAction_Letter, map);
			RoomContents_GrayBox.SpawnBoxInRoom(cellRect.CenterCell + Rot4.East.FacingCell * 3, map, null, true);
			RoomContents_GrayBox.SpawnBoxInRoom(cellRect.CenterCell + Rot4.West.FacingCell * 3, map, null, true);
		}

		// Token: 0x04004926 RID: 18726
		private const float WallRadius = 8.9f;

		// Token: 0x04004927 RID: 18727
		private const float MetalRadius = 3.9f;
	}
}
