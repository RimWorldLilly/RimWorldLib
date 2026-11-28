using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200026F RID: 623
	public static class FloodFillerFog
	{
		// Token: 0x06001216 RID: 4630 RVA: 0x00067990 File Offset: 0x00065B90
		public static FloodUnfogResult FloodUnfog(IntVec3 root, Map map)
		{
			FloodFillerFog.<>c__DisplayClass5_0 CS$<>8__locals1 = new FloodFillerFog.<>c__DisplayClass5_0();
			CS$<>8__locals1.map = map;
			FloodFillerFog.cellsToUnfog.Clear();
			FloodFillerFog.lordsToWakeOnPawnUnfogged.Clear();
			CS$<>8__locals1.result = default(FloodUnfogResult);
			CS$<>8__locals1.fogGridDirect = CS$<>8__locals1.map.fogGrid.FogGrid_Unsafe;
			CS$<>8__locals1.fogGrid = CS$<>8__locals1.map.fogGrid;
			CS$<>8__locals1.newlyUnfoggedCells = new List<IntVec3>();
			CS$<>8__locals1.numUnfogged = 0;
			CS$<>8__locals1.expanding = false;
			CS$<>8__locals1.viewRect = CellRect.ViewRect(CS$<>8__locals1.map);
			CS$<>8__locals1.result.allOnScreen = true;
			FloodFillerFog.deferPawnUnfoggedWake = true;
			try
			{
				CS$<>8__locals1.map.floodFiller.FloodFill(root, new Predicate<IntVec3>(CS$<>8__locals1.<FloodUnfog>g__PassCheck|0), new Action<IntVec3>(CS$<>8__locals1.<FloodUnfog>g__Processor|1), int.MaxValue, false, null);
				CS$<>8__locals1.expanding = true;
				for (int i = 0; i < CS$<>8__locals1.newlyUnfoggedCells.Count; i++)
				{
					IntVec3 a = CS$<>8__locals1.newlyUnfoggedCells[i];
					for (int j = 0; j < 8; j++)
					{
						IntVec3 intVec = a + GenAdj.AdjacentCells[j];
						if (intVec.InBounds(CS$<>8__locals1.map) && CS$<>8__locals1.fogGrid.IsFogged(intVec) && !CS$<>8__locals1.<FloodUnfog>g__PassCheck|0(intVec))
						{
							FloodFillerFog.cellsToUnfog.Add(intVec);
						}
					}
				}
				for (int k = 0; k < FloodFillerFog.cellsToUnfog.Count; k++)
				{
					CS$<>8__locals1.fogGrid.Unfog(FloodFillerFog.cellsToUnfog[k]);
					if (FloodFillerFog.testMode)
					{
						CS$<>8__locals1.map.debugDrawer.FlashCell(FloodFillerFog.cellsToUnfog[k], 0.3f, "x", 50);
					}
				}
			}
			finally
			{
				FloodFillerFog.deferPawnUnfoggedWake = false;
				FloodFillerFog.cellsToUnfog.Clear();
			}
			FloodFillerFog.WakeDeferredUnfoggedPawns();
			return CS$<>8__locals1.result;
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00067B74 File Offset: 0x00065D74
		public static bool TryDeferPawnUnfoggedWake(Pawn pawn)
		{
			if (!FloodFillerFog.deferPawnUnfoggedWake)
			{
				return false;
			}
			Lord lord = pawn.GetLord();
			LordJob_SleepThenAssaultColony lordJob_SleepThenAssaultColony = ((lord != null) ? lord.LordJob : null) as LordJob_SleepThenAssaultColony;
			if (lordJob_SleepThenAssaultColony == null)
			{
				return false;
			}
			if (!FloodFillerFog.lordsToWakeOnPawnUnfogged.Contains(lordJob_SleepThenAssaultColony))
			{
				FloodFillerFog.lordsToWakeOnPawnUnfogged.Add(lordJob_SleepThenAssaultColony);
			}
			return true;
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x00067BC0 File Offset: 0x00065DC0
		private static void WakeDeferredUnfoggedPawns()
		{
			for (int i = 0; i < FloodFillerFog.lordsToWakeOnPawnUnfogged.Count; i++)
			{
				FloodFillerFog.lordsToWakeOnPawnUnfogged[i].Notify_PawnUnfogged();
			}
			FloodFillerFog.lordsToWakeOnPawnUnfogged.Clear();
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x00067BFC File Offset: 0x00065DFC
		public static void DebugFloodUnfog(IntVec3 root, Map map)
		{
			map.fogGrid.SetAllFogged();
			foreach (IntVec3 loc in map.AllCells)
			{
				map.mapDrawer.MapMeshDirty(loc, MapMeshFlagDefOf.FogOfWar);
			}
			FloodFillerFog.testMode = true;
			FloodFillerFog.FloodUnfog(root, map);
			FloodFillerFog.testMode = false;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x00067C78 File Offset: 0x00065E78
		public static void DebugRefogMap(Map map)
		{
			map.fogGrid.SetAllFogged();
			foreach (IntVec3 loc in map.AllCells)
			{
				map.mapDrawer.MapMeshDirty(loc, MapMeshFlagDefOf.FogOfWar);
			}
			FloodFillerFog.FloodUnfog(map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>().Position, map);
		}

		// Token: 0x0400106E RID: 4206
		private static bool testMode = false;

		// Token: 0x0400106F RID: 4207
		private static List<IntVec3> cellsToUnfog = new List<IntVec3>(1024);

		// Token: 0x04001070 RID: 4208
		private static bool deferPawnUnfoggedWake;

		// Token: 0x04001071 RID: 4209
		private static List<LordJob_SleepThenAssaultColony> lordsToWakeOnPawnUnfogged = new List<LordJob_SleepThenAssaultColony>();

		// Token: 0x04001072 RID: 4210
		private const int MaxNumTestUnfog = 500;
	}
}
