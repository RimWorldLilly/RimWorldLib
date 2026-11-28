using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200126B RID: 4715
	public class RoomPart_CenterSpace : RoomPartWorker
	{
		// Token: 0x060074DC RID: 29916 RVA: 0x00290719 File Offset: 0x0028E919
		public RoomPart_CenterSpace(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x060074DD RID: 29917 RVA: 0x00290724 File Offset: 0x0028E924
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			ThingDef wall = room.sketch.layoutSketch.wall;
			ThingDef wallStuff = room.sketch.layoutSketch.wallStuff;
			TerrainDef floor = room.sketch.layoutSketch.floor;
			CellRect cellRect = room.rects[0];
			IntVec3 center = cellRect.CenterCell - new IntVec3(1, 0, 1);
			if (cellRect.Size.x < 8 || cellRect.Size.z < 8)
			{
				return;
			}
			CellRect cellRect2 = center.RectAbout(new IntVec2(4, 4));
			foreach (IntVec3 intVec in cellRect2.EdgeCells)
			{
				map.terrainGrid.SetTerrain(intVec, floor);
				GenSpawn.Spawn(ThingMaker.MakeThing(wall, wallStuff), intVec, map, WipeMode.Vanish);
			}
			foreach (IntVec3 c in cellRect2.ContractedBy(1).Cells)
			{
				TerrainDef newTerr = map.Biome.inVacuum ? TerrainDefOf.Space : TerrainDefOf.PackedDirt;
				map.terrainGrid.SetTerrain(c, newTerr);
				map.roofGrid.SetRoof(c, null);
			}
		}

		// Token: 0x04004958 RID: 18776
		private const int Size = 4;
	}
}
