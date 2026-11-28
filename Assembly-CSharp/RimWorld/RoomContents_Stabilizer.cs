using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200125E RID: 4702
	public class RoomContents_Stabilizer : RoomContentsWorker
	{
		// Token: 0x06007474 RID: 29812 RVA: 0x0028BC70 File Offset: 0x00289E70
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			CellRect cellRect = CellRect.CenteredOn(room.Boundary.CenterCell, 5, 5);
			foreach (IntVec3 c in cellRect)
			{
				map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientTile);
			}
			foreach (IntVec3 loc in cellRect.Corners)
			{
				GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.MechPylon, null), loc, map, WipeMode.Vanish);
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.CerebrexStabilizer, null), cellRect.CenterCell, map, WipeMode.Vanish);
		}
	}
}
