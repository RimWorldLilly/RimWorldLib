using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200132F RID: 4911
	public class TileMutatorWorker_InsectMegahive : TileMutatorWorker
	{
		// Token: 0x060079B4 RID: 31156 RVA: 0x002B514E File Offset: 0x002B334E
		public TileMutatorWorker_InsectMegahive(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079B5 RID: 31157 RVA: 0x002B5158 File Offset: 0x002B3358
		public override void GeneratePostFog(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			IntVec3 entranceCell;
			if (!TileMutatorWorker_InsectMegahive.TryGetEntranceCell(map, out entranceCell))
			{
				return;
			}
			TileMutatorWorker_InsectMegahive.GenerateMegahiveEntrance(map, entranceCell, false);
		}

		// Token: 0x060079B6 RID: 31158 RVA: 0x002B5180 File Offset: 0x002B3380
		public static void GenerateMegahiveEntrance(Map map, IntVec3 entranceCell, bool spawnGravcore = false)
		{
			foreach (IntVec3 intVec in GridShapeMaker.IrregularLump(entranceCell, map, 150, (IntVec3 _) => true))
			{
				Building edifice = intVec.GetEdifice(map);
				if (edifice != null)
				{
					edifice.Destroy(DestroyMode.Vanish);
					map.fogGrid.FloodUnfogAdjacent(edifice, false);
				}
				map.terrainGrid.SetTerrain(intVec, GenStep_RocksFromGrid.RockDefAt(intVec).building.naturalTerrain);
				map.terrainGrid.SetTerrain(intVec, TerrainDefOf.InsectSludge);
				if (Rand.Chance(0.2f))
				{
					GenSpawn.Spawn(ThingDefOf.Filth_Slime, intVec, map, WipeMode.Vanish);
				}
			}
			(GenSpawn.Spawn(ThingDefOf.InsectLairEntrance, entranceCell, map, WipeMode.Vanish) as InsectLairEntrance).spawnGravcore = spawnGravcore;
			MapGenerator.UsedRects.Add(GenAdj.OccupiedRect(entranceCell, Rot4.North, 8, 8));
			int randomInRange = TileMutatorWorker_InsectMegahive.NumHivesRange.RandomInRange;
			Predicate<IntVec3> <>9__1;
			for (int i = 0; i < randomInRange; i++)
			{
				Map map2 = map;
				int squareRadius = 6;
				Predicate<IntVec3> validator;
				if ((validator = <>9__1) == null)
				{
					validator = (<>9__1 = ((IntVec3 c) => GenSpawn.CanSpawnAt(ThingDefOf.Hive, c, map, null, true)));
				}
				IntVec3 spawnCell;
				if (CellFinder.TryFindRandomCellNear(entranceCell, map2, squareRadius, validator, out spawnCell, -1))
				{
					HiveUtility.SpawnHive(spawnCell, map, WipeMode.VanishOrMoveAside, true, false, true, false, true, false, true);
				}
			}
			FloodFillerFog.FloodUnfog(entranceCell, map);
		}

		// Token: 0x060079B7 RID: 31159 RVA: 0x002B5338 File Offset: 0x002B3538
		public static bool TryGetEntranceCell(Map map, out IntVec3 entranceCell)
		{
			return CellFinder.TryFindRandomCell(map, (IntVec3 cell) => GenSpawn.CanSpawnAt(ThingDefOf.InsectLairEntrance, cell, map, null, true) && cell.Roofed(map) && !cell.Fogged(map), out entranceCell) || CellFinder.TryFindRandomCell(map, (IntVec3 cell) => GenSpawn.CanSpawnAt(ThingDefOf.InsectLairEntrance, cell, map, null, true) && !cell.Fogged(map), out entranceCell);
		}

		// Token: 0x04004CC3 RID: 19651
		private static readonly IntRange NumHivesRange = new IntRange(1, 2);
	}
}
