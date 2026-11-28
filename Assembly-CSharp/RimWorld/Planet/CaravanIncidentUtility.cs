using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002389 RID: 9097
	public static class CaravanIncidentUtility
	{
		// Token: 0x0600E23D RID: 57917 RVA: 0x0053F9F7 File Offset: 0x0053DBF7
		public static int CalculateIncidentMapSize(List<Pawn> caravanPawns, List<Pawn> enemies)
		{
			return Mathf.Clamp(Mathf.RoundToInt(Mathf.Sqrt((float)Mathf.RoundToInt((float)((caravanPawns.Count + enemies.Count) * 900)))), 75, 110);
		}

		// Token: 0x0600E23E RID: 57918 RVA: 0x0053FA28 File Offset: 0x0053DC28
		public static bool CanFireIncidentWhichWantsToGenerateMapAt(PlanetTile tile)
		{
			if (Current.Game.FindMap(tile) != null)
			{
				return false;
			}
			if (!Find.WorldGrid[tile].PrimaryBiome.implemented)
			{
				return false;
			}
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (allWorldObjects[i].Tile == tile && !allWorldObjects[i].def.allowCaravanIncidentsWhichGenerateMap)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600E23F RID: 57919 RVA: 0x0053FAA4 File Offset: 0x0053DCA4
		public static Map SetupCaravanAttackMap(Caravan caravan, List<Pawn> enemies, bool sendLetterIfRelatedPawns)
		{
			int num = CaravanIncidentUtility.CalculateIncidentMapSize(caravan.PawnsListForReading, enemies);
			Map map = CaravanIncidentUtility.GetOrGenerateMapForIncident(caravan, new IntVec3(num, 1, num), WorldObjectDefOf.Ambush);
			IntVec3 playerStartingSpot;
			IntVec3 root;
			MultipleCaravansCellFinder.FindStartingCellsFor2Groups(map, out playerStartingSpot, out root);
			CaravanEnterMapUtility.Enter(caravan, map, (Pawn x) => CellFinder.RandomSpawnCellForPawnNear(playerStartingSpot, map, 4), CaravanDropInventoryMode.DoNotDrop, true);
			for (int i = 0; i < enemies.Count; i++)
			{
				IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(root, map, 4);
				GenSpawn.Spawn(enemies[i], loc, map, Rot4.Random, WipeMode.Vanish, false, false);
			}
			if (sendLetterIfRelatedPawns)
			{
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(enemies, "LetterRelatedPawnsGroupGeneric".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, true, true);
			}
			return map;
		}

		// Token: 0x0600E240 RID: 57920 RVA: 0x0053FB80 File Offset: 0x0053DD80
		public static Map GetOrGenerateMapForIncident(Caravan caravan, IntVec3 size, WorldObjectDef suggestedMapParentDef)
		{
			PlanetTile tile = caravan.Tile;
			bool flag = Current.Game.FindMap(tile) == null;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, size, suggestedMapParentDef, null, false);
			if (flag && orGenerateMap != null)
			{
				orGenerateMap.retainedCaravanData.Notify_GeneratedTempIncidentMapFor(caravan);
			}
			return orGenerateMap;
		}

		// Token: 0x0400907B RID: 36987
		private const int MapCellsPerPawn = 900;

		// Token: 0x0400907C RID: 36988
		private const int MinMapSize = 75;

		// Token: 0x0400907D RID: 36989
		private const int MaxMapSize = 110;
	}
}
