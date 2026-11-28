using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020012D5 RID: 4821
	public class GenStep_Labyrinth : GenStep
	{
		// Token: 0x17001453 RID: 5203
		// (get) Token: 0x0600779D RID: 30621 RVA: 0x002A3D5C File Offset: 0x002A1F5C
		public override int SeedPart
		{
			get
			{
				return 8767466;
			}
		}

		// Token: 0x0600779E RID: 30622 RVA: 0x002A3D64 File Offset: 0x002A1F64
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckAnomaly("Labyrinth"))
			{
				return;
			}
			TerrainGrid terrainGrid = map.terrainGrid;
			foreach (IntVec3 c in map.AllCells)
			{
				terrainGrid.SetTerrain(c, TerrainDefOf.GraySurface);
			}
			CellRect rect = map.BoundsRect(0);
			GenStep_Labyrinth.FillEdges(rect, map);
			StructureGenParams parms2 = new StructureGenParams
			{
				size = rect.ContractedBy(2).Size
			};
			LayoutWorker worker = LayoutDefOf.Labyrinth.Worker;
			int num = 10;
			do
			{
				this.structureSketch = worker.GenerateStructureSketch(parms2);
			}
			while (!this.structureSketch.structureLayout.HasRoomWithDef(LayoutRoomDefOf.LabyrinthObelisk) && num-- > 0);
			if (num == 0)
			{
				Log.ErrorOnce("Failed to generate labyrinth, guard exceeded. Check layout worker for errors placing minimum rooms", 9868797);
				return;
			}
			worker.Spawn(this.structureSketch, map, new IntVec3(2, 0, 2), null, null, false, false, null);
			map.layoutStructureSketches.Add(this.structureSketch);
			LabyrinthMapComponent component = map.GetComponent<LabyrinthMapComponent>();
			LayoutRoom firstRoomOfDef = this.structureSketch.structureLayout.GetFirstRoomOfDef(LayoutRoomDefOf.LabyrinthObelisk);
			List<LayoutRoom> spawnableRooms = this.GetSpawnableRooms(firstRoomOfDef);
			component.SetSpawnRooms(spawnableRooms);
			MapGenerator.PlayerStartSpot = IntVec3.Zero;
			map.fogGrid.SetAllFogged();
		}

		// Token: 0x0600779F RID: 30623 RVA: 0x002A3EC8 File Offset: 0x002A20C8
		private List<LayoutRoom> GetSpawnableRooms(LayoutRoom obelisk)
		{
			List<LayoutRoom> list = new List<LayoutRoom>();
			list.AddRange(this.structureSketch.structureLayout.Rooms);
			list.Remove(obelisk);
			foreach (ValueTuple<LayoutRoom, CellRect, CellRect> valueTuple in this.structureSketch.structureLayout.GetLogicalRoomConnections(obelisk))
			{
				LayoutRoom item = valueTuple.Item1;
				if (list.Contains(item))
				{
					list.Remove(item);
					foreach (ValueTuple<LayoutRoom, CellRect, CellRect> valueTuple2 in this.structureSketch.structureLayout.GetLogicalRoomConnections(item))
					{
						LayoutRoom item2 = valueTuple2.Item1;
						if (list.Contains(item2))
						{
							list.Remove(item2);
						}
					}
				}
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				using (List<LayoutRoomDef>.Enumerator enumerator3 = list[i].defs.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (!enumerator3.Current.isValidPlayerSpawnRoom)
						{
							list.RemoveAt(i);
							break;
						}
					}
				}
			}
			if (list.Empty<LayoutRoom>())
			{
				list.Clear();
				list.AddRange(this.structureSketch.structureLayout.Rooms);
				list.Remove(obelisk);
			}
			return list;
		}

		// Token: 0x060077A0 RID: 30624 RVA: 0x002A4044 File Offset: 0x002A2244
		private static void FillEdges(CellRect rect, Map map)
		{
			for (int i = 0; i < rect.Width; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					GenStep_Labyrinth.SpawnWall(new IntVec3(i, 0, j), map);
					GenStep_Labyrinth.SpawnWall(new IntVec3(i, 0, rect.Height - j - 1), map);
				}
			}
			for (int k = 2; k < rect.Height - 2; k++)
			{
				for (int l = 0; l < 2; l++)
				{
					GenStep_Labyrinth.SpawnWall(new IntVec3(l, 0, k), map);
					GenStep_Labyrinth.SpawnWall(new IntVec3(rect.Width - l - 1, 0, k), map);
				}
			}
		}

		// Token: 0x060077A1 RID: 30625 RVA: 0x002A40DB File Offset: 0x002A22DB
		private static void SpawnWall(IntVec3 pos, Map map)
		{
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.GrayWall, ThingDefOf.LabyrinthMatter), pos, map, WipeMode.Vanish);
		}

		// Token: 0x04004B11 RID: 19217
		private LayoutStructureSketch structureSketch;

		// Token: 0x04004B12 RID: 19218
		private const int Border = 2;
	}
}
