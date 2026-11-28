using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E4C RID: 7756
	public static class LargeBuildingCellFinder
	{
		// Token: 0x0600C918 RID: 51480 RVA: 0x0049DC37 File Offset: 0x0049BE37
		private static float MinRequiredScore(IntVec2 size)
		{
			return 0.5f * (float)size.Area;
		}

		// Token: 0x0600C919 RID: 51481 RVA: 0x0049DC48 File Offset: 0x0049BE48
		public static bool AnyCellFast(Map map, LargeBuildingSpawnParms parms)
		{
			for (int i = 0; i < 1000; i++)
			{
				if (LargeBuildingCellFinder.GetAreaScore(CellFinder.RandomCell(map), map, parms) >= LargeBuildingCellFinder.MinRequiredScore(parms.Size))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C91A RID: 51482 RVA: 0x0049DC84 File Offset: 0x0049BE84
		public static bool TryFindCell(out IntVec3 cell, Map map, LargeBuildingSpawnParms parms, List<CellRect> usedRects = null, Predicate<IntVec3> extraValidator = null, bool forceRecalculate = false)
		{
			if (LargeBuildingCellFinder.lastCalculatedCandidatesTick != Find.TickManager.TicksGame || forceRecalculate)
			{
				using (new ProfilerBlock("CalculateLocationCandidates"))
				{
					LargeBuildingCellFinder.CalculateLocationCandidates(map, parms);
				}
			}
			int num = 0;
			cell = IntVec3.Invalid;
			while (cell == IntVec3.Invalid && num < 1000)
			{
				num++;
				LocationCandidate locationCandidate;
				if (LargeBuildingCellFinder.locationCandidates.TryRandomElementByWeight((LocationCandidate x) => x.score, out locationCandidate) && !LargeBuildingCellFinder.IsRectUsed(GenAdj.OccupiedRect(locationCandidate.cell, Rot4.North, parms.Size), usedRects, parms) && (extraValidator == null || extraValidator(locationCandidate.cell)))
				{
					cell = locationCandidate.cell;
				}
			}
			return num < 1000;
		}

		// Token: 0x0600C91B RID: 51483 RVA: 0x0049DD88 File Offset: 0x0049BF88
		public static bool TryFindCellNear(IntVec3 root, Map map, int squareRadius, LargeBuildingSpawnParms parms, out IntVec3 cell, bool forceRecalculate = false)
		{
			if (LargeBuildingCellFinder.lastCalculatedCandidatesTick != Find.TickManager.TicksGame || forceRecalculate)
			{
				using (new ProfilerBlock("CalculateLocationCandidates"))
				{
					LargeBuildingCellFinder.CalculateLocationCandidates(map, parms);
				}
			}
			int minX = root.x - squareRadius;
			int maxX = root.x + squareRadius;
			int minZ = root.z - squareRadius;
			int maxZ = root.z + squareRadius;
			LocationCandidate locationCandidate;
			if (!LargeBuildingCellFinder.locationCandidates.TryRandomElementByWeight(delegate(LocationCandidate x)
			{
				if (x.cell.x < minX || x.cell.x > maxX || x.cell.z < minZ || x.cell.z > maxZ)
				{
					return 0f;
				}
				return x.score;
			}, out locationCandidate))
			{
				cell = IntVec3.Invalid;
				return false;
			}
			cell = locationCandidate.cell;
			return true;
		}

		// Token: 0x0600C91C RID: 51484 RVA: 0x0049DE58 File Offset: 0x0049C058
		private static bool IsRectUsed(CellRect rect, List<CellRect> usedRects, LargeBuildingSpawnParms parms)
		{
			if (usedRects.NullOrEmpty<CellRect>())
			{
				return false;
			}
			foreach (CellRect cellRect in usedRects)
			{
				if (cellRect.Overlaps(rect))
				{
					return true;
				}
			}
			if (parms.minDistanceFromUsedRects > 0f)
			{
				foreach (CellRect cellRect2 in usedRects)
				{
					if (cellRect2.CenterCell.InHorDistOf(rect.CenterCell, parms.minDistanceFromUsedRects))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600C91D RID: 51485 RVA: 0x0049DF20 File Offset: 0x0049C120
		private static List<LocationCandidate> CalculateLocationCandidates(Map map, LargeBuildingSpawnParms parms)
		{
			LargeBuildingCellFinder.locationCandidates.Clear();
			if (parms.maxDistanceToColonyBuilding > 0f || parms.minDistanceToColonyBuilding > 0f || parms.preferFarFromColony)
			{
				CellFinderUtility.CalculateDistanceToColonyBuildingGrid(map);
			}
			int num = Mathf.Max(parms.Size.x / 2, 1);
			int num2 = Mathf.Max(parms.Size.z / 2, 1);
			for (int i = 0; i < map.Size.z; i += num2)
			{
				for (int j = 0; j < map.Size.x; j += num)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					float areaScore = LargeBuildingCellFinder.GetAreaScore(intVec, map, parms);
					if (areaScore >= LargeBuildingCellFinder.MinRequiredScore(parms.Size))
					{
						LargeBuildingCellFinder.locationCandidates.Add(new LocationCandidate(intVec + (parms.Size / 2).ToIntVec3, areaScore));
					}
				}
			}
			LargeBuildingCellFinder.lastCalculatedCandidatesTick = Find.TickManager.TicksGame;
			return LargeBuildingCellFinder.locationCandidates;
		}

		// Token: 0x0600C91E RID: 51486 RVA: 0x0049E01C File Offset: 0x0049C21C
		private static float GetAreaScore(IntVec3 topLeftCell, Map map, LargeBuildingSpawnParms parms)
		{
			float num = 0f;
			IntVec2 size = parms.Size;
			for (int i = 0; i < size.x; i++)
			{
				for (int j = 0; j < size.z; j++)
				{
					IntVec3 intVec = topLeftCell + new IntVec3(i, 0, j);
					if (!intVec.InBounds(map))
					{
						return 0f;
					}
					if (!parms.canSpawnOnImpassable && !intVec.Walkable(map))
					{
						return 0f;
					}
					float cellScore = LargeBuildingCellFinder.GetCellScore(intVec, map, parms);
					if (cellScore <= 0f)
					{
						return 0f;
					}
					num += cellScore;
				}
			}
			if (parms.preferFarFromColony && (float)CellFinderUtility.DistToColonyBuilding[topLeftCell] < 50f)
			{
				num *= 0.5f;
			}
			return num;
		}

		// Token: 0x0600C91F RID: 51487 RVA: 0x0049E0D4 File Offset: 0x0049C2D4
		private static float GetCellScore(IntVec3 cell, Map map, LargeBuildingSpawnParms parms)
		{
			float num = 1f;
			if (!parms.ignoreTerrainAffordance && !cell.SupportsStructureType(map, parms.thingDef.terrainAffordanceNeeded))
			{
				return 0f;
			}
			if (!parms.ignoreFoundations && map.terrainGrid.FoundationAt(cell) != null)
			{
				return 0f;
			}
			if (parms.maxDistanceToColonyBuilding > 0f && (float)CellFinderUtility.DistToColonyBuilding[cell] > parms.maxDistanceToColonyBuilding)
			{
				return 0f;
			}
			if (parms.minDistanceToColonyBuilding > 0f && (float)CellFinderUtility.DistToColonyBuilding[cell] < parms.minDistanceToColonyBuilding)
			{
				return 0f;
			}
			if (parms.minDistToEdge > 0 && cell.DistanceToEdge(map) < parms.minDistToEdge)
			{
				return 0f;
			}
			if (parms.maxDistanceFromPlayerStartPosition > 0f && !cell.InHorDistOf(MapGenerator.PlayerStartSpot, parms.maxDistanceFromPlayerStartPosition))
			{
				return 0f;
			}
			Building edifice = cell.GetEdifice(map);
			bool flag;
			if (edifice == null)
			{
				flag = false;
			}
			else
			{
				ThingDef def = edifice.def;
				bool? flag2;
				if (def == null)
				{
					flag2 = null;
				}
				else
				{
					BuildingProperties building = def.building;
					flag2 = ((building != null) ? new bool?(building.isNaturalRock) : null);
				}
				bool? flag3 = flag2;
				flag = flag3.GetValueOrDefault();
			}
			if (flag)
			{
				return 0f;
			}
			if (!parms.allowFogged && cell.Fogged(map))
			{
				return 0f;
			}
			if (cell.GetTerrain(map).IsWater)
			{
				return 0f;
			}
			if (!parms.canSpawnOnImpassable && !cell.Walkable(map))
			{
				return 0f;
			}
			foreach (Thing thing in cell.GetThingList(map))
			{
				if (thing.def.preventSkyfallersLandingOn)
				{
					return 0f;
				}
				Building building2 = thing as Building;
				if (building2 != null)
				{
					if (!building2.def.Minifiable && building2.MarketValue > 50f)
					{
						return 0f;
					}
					if (!building2.def.destroyable || !building2.def.building.IsDeconstructible)
					{
						return 0f;
					}
					if (parms.attemptNotUnderBuildings)
					{
						num *= 0.5f;
					}
				}
			}
			SpawnLocationType attemptSpawnLocationType = parms.attemptSpawnLocationType;
			if (attemptSpawnLocationType != SpawnLocationType.Indoors)
			{
				if (attemptSpawnLocationType == SpawnLocationType.Outdoors)
				{
					if (!cell.GetRoom(map).PsychologicallyOutdoors)
					{
						num *= 0.01f;
					}
				}
			}
			else if (cell.GetRoom(map).PsychologicallyOutdoors)
			{
				num *= 0.01f;
			}
			return num;
		}

		// Token: 0x0600C920 RID: 51488 RVA: 0x0049E360 File Offset: 0x0049C560
		public static void DebugDraw()
		{
			if (DebugViewSettings.drawFleshmassHeartChance)
			{
				if (LargeBuildingCellFinder.tmpHeartChanceCellColors == null)
				{
					LargeBuildingCellFinder.tmpHeartChanceCellColors = new List<Pair<IntVec3, float>>();
				}
				if (Time.frameCount % 60 == 0)
				{
					LargeBuildingCellFinder.tmpHeartChanceCellColors.Clear();
					Map currentMap = Find.CurrentMap;
					CellRect cellRect = Find.CameraDriver.CurrentViewRect;
					cellRect.ClipInsideMap(currentMap);
					cellRect = cellRect.ExpandedBy(1);
					CellFinderUtility.CalculateDistanceToColonyBuildingGrid(currentMap);
					List<LocationCandidate> list = LargeBuildingCellFinder.CalculateLocationCandidates(currentMap, IncidentWorker_FleshmassHeart.HeartSpawnParms.ForThing(ThingDefOf.FleshmassHeart));
					if (list.NullOrEmpty<LocationCandidate>())
					{
						return;
					}
					float inTo = list.Max((LocationCandidate c) => c.score);
					foreach (LocationCandidate locationCandidate in list)
					{
						if (cellRect.Contains(locationCandidate.cell) && locationCandidate.score >= LargeBuildingCellFinder.MinRequiredScore(ThingDefOf.FleshmassHeart.size))
						{
							float second = GenMath.LerpDouble(LargeBuildingCellFinder.MinRequiredScore(ThingDefOf.FleshmassHeart.size) - 1f, inTo, 0.25f, 0.5f, locationCandidate.score);
							LargeBuildingCellFinder.tmpHeartChanceCellColors.Add(new Pair<IntVec3, float>(locationCandidate.cell, second));
						}
					}
				}
				for (int i = 0; i < LargeBuildingCellFinder.tmpHeartChanceCellColors.Count; i++)
				{
					IntVec3 first = LargeBuildingCellFinder.tmpHeartChanceCellColors[i].First;
					float second2 = LargeBuildingCellFinder.tmpHeartChanceCellColors[i].Second;
					CellRenderer.RenderCell(first, SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0f, 1f, second2), false));
				}
				return;
			}
			LargeBuildingCellFinder.tmpHeartChanceCellColors = null;
		}

		// Token: 0x04007297 RID: 29335
		private static int lastCalculatedCandidatesTick = -99999;

		// Token: 0x04007298 RID: 29336
		private static List<LocationCandidate> locationCandidates = new List<LocationCandidate>();

		// Token: 0x04007299 RID: 29337
		private const float MinRequiredScorePerCell = 0.5f;

		// Token: 0x0400729A RID: 29338
		private const float SafeBuildingMarketValue = 50f;

		// Token: 0x0400729B RID: 29339
		private static List<Pair<IntVec3, float>> tmpHeartChanceCellColors;
	}
}
