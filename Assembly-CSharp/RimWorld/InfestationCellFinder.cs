using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E49 RID: 7753
	public static class InfestationCellFinder
	{
		// Token: 0x0600C909 RID: 51465 RVA: 0x0049D254 File Offset: 0x0049B454
		public static bool TryFindCell(out IntVec3 cell, Map map)
		{
			InfestationCellFinder.CalculateLocationCandidates(map);
			LocationCandidate locationCandidate;
			if (!InfestationCellFinder.locationCandidates.TryRandomElementByWeight((LocationCandidate x) => x.score, out locationCandidate))
			{
				cell = IntVec3.Invalid;
				return false;
			}
			cell = CellFinder.FindNoWipeSpawnLocNear(locationCandidate.cell, map, ThingDefOf.Hive, Rot4.North, 2, (IntVec3 x) => InfestationCellFinder.GetScoreAt(x, map) > 0f && x.GetFirstThing(map, ThingDefOf.Hive) == null && x.GetFirstThing(map, ThingDefOf.TunnelHiveSpawner) == null);
			return true;
		}

		// Token: 0x0600C90A RID: 51466 RVA: 0x0049D2E4 File Offset: 0x0049B4E4
		private static float GetScoreAt(IntVec3 cell, Map map)
		{
			if ((float)CellFinderUtility.DistToColonyBuilding[cell] > 30f)
			{
				return 0f;
			}
			if (!cell.Walkable(map))
			{
				return 0f;
			}
			if (cell.Fogged(map))
			{
				return 0f;
			}
			if (InfestationCellFinder.CellHasBlockingThings(cell, map))
			{
				return 0f;
			}
			if (!cell.Roofed(map) || !cell.GetRoof(map).isThickRoof)
			{
				return 0f;
			}
			Region region = cell.GetRegion(map, RegionType.Set_Passable);
			if (region == null)
			{
				return 0f;
			}
			if (InfestationCellFinder.closedAreaSize[cell] < 2)
			{
				return 0f;
			}
			float temperature = cell.GetTemperature(map);
			if (temperature < -17f)
			{
				return 0f;
			}
			float mountainousnessScoreAt = InfestationCellFinder.GetMountainousnessScoreAt(cell, map);
			if (mountainousnessScoreAt < 0.17f)
			{
				return 0f;
			}
			int num = InfestationCellFinder.StraightLineDistToUnroofed(cell, map);
			float num2;
			if (!InfestationCellFinder.regionsDistanceToUnroofed.TryGetValue(region, out num2))
			{
				num2 = (float)num * 1.15f;
			}
			else
			{
				num2 = Mathf.Min(num2, (float)num * 4f);
			}
			num2 = Mathf.Pow(num2, 1.55f);
			float num3 = Mathf.InverseLerp(0f, 12f, (float)num);
			float num4 = Mathf.Lerp(1f, 0.18f, map.glowGrid.GroundGlowAt(cell, false, false));
			float num5 = 1f - Mathf.Clamp(InfestationCellFinder.DistToBlocker(cell, map) / 11f, 0f, 0.6f);
			float num6 = Mathf.InverseLerp(-17f, -7f, temperature);
			float num7 = num2 * num3 * num5 * mountainousnessScoreAt * num4 * num6;
			num7 = Mathf.Pow(num7, 1.2f);
			if (num7 < 7.5f)
			{
				return 0f;
			}
			return num7;
		}

		// Token: 0x0600C90B RID: 51467 RVA: 0x0049D484 File Offset: 0x0049B684
		public static void DebugDraw()
		{
			if (DebugViewSettings.drawInfestationChance)
			{
				if (InfestationCellFinder.tmpCachedInfestationChanceCellColors == null)
				{
					InfestationCellFinder.tmpCachedInfestationChanceCellColors = new List<Pair<IntVec3, float>>();
				}
				if (Time.frameCount % 8 == 0)
				{
					InfestationCellFinder.tmpCachedInfestationChanceCellColors.Clear();
					Map currentMap = Find.CurrentMap;
					CellRect cellRect = Find.CameraDriver.CurrentViewRect;
					cellRect.ClipInsideMap(currentMap);
					cellRect = cellRect.ExpandedBy(1);
					InfestationCellFinder.CalculateTraversalDistancesToUnroofed(currentMap);
					InfestationCellFinder.CalculateClosedAreaSizeGrid(currentMap);
					CellFinderUtility.CalculateDistanceToColonyBuildingGrid(currentMap);
					float num = 0.001f;
					for (int i = 0; i < currentMap.Size.z; i++)
					{
						for (int j = 0; j < currentMap.Size.x; j++)
						{
							float scoreAt = InfestationCellFinder.GetScoreAt(new IntVec3(j, 0, i), currentMap);
							if (scoreAt > num)
							{
								num = scoreAt;
							}
						}
					}
					for (int k = 0; k < currentMap.Size.z; k++)
					{
						for (int l = 0; l < currentMap.Size.x; l++)
						{
							IntVec3 intVec = new IntVec3(l, 0, k);
							if (cellRect.Contains(intVec))
							{
								float scoreAt2 = InfestationCellFinder.GetScoreAt(intVec, currentMap);
								if (scoreAt2 > 7.5f)
								{
									float second = GenMath.LerpDouble(7.5f, num, 0f, 1f, scoreAt2);
									InfestationCellFinder.tmpCachedInfestationChanceCellColors.Add(new Pair<IntVec3, float>(intVec, second));
								}
							}
						}
					}
				}
				for (int m = 0; m < InfestationCellFinder.tmpCachedInfestationChanceCellColors.Count; m++)
				{
					IntVec3 first = InfestationCellFinder.tmpCachedInfestationChanceCellColors[m].First;
					float second2 = InfestationCellFinder.tmpCachedInfestationChanceCellColors[m].Second;
					CellRenderer.RenderCell(first, SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0f, 1f, second2), false));
				}
				return;
			}
			InfestationCellFinder.tmpCachedInfestationChanceCellColors = null;
		}

		// Token: 0x0600C90C RID: 51468 RVA: 0x0049D640 File Offset: 0x0049B840
		private static void CalculateLocationCandidates(Map map)
		{
			InfestationCellFinder.locationCandidates.Clear();
			InfestationCellFinder.CalculateTraversalDistancesToUnroofed(map);
			InfestationCellFinder.CalculateClosedAreaSizeGrid(map);
			CellFinderUtility.CalculateDistanceToColonyBuildingGrid(map);
			for (int i = 0; i < map.Size.z; i++)
			{
				for (int j = 0; j < map.Size.x; j++)
				{
					IntVec3 cell = new IntVec3(j, 0, i);
					float scoreAt = InfestationCellFinder.GetScoreAt(cell, map);
					if (scoreAt > 0f)
					{
						InfestationCellFinder.locationCandidates.Add(new LocationCandidate(cell, scoreAt));
					}
				}
			}
		}

		// Token: 0x0600C90D RID: 51469 RVA: 0x0049D6C0 File Offset: 0x0049B8C0
		private static bool CellHasBlockingThings(IntVec3 cell, Map map)
		{
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i] is Pawn || thingList[i] is Hive || thingList[i] is TunnelHiveSpawner)
				{
					return true;
				}
				if (thingList[i].def.category == ThingCategory.Building && thingList[i].def.passability == Traversability.Impassable && GenSpawn.SpawningWipes(ThingDefOf.Hive, thingList[i].def, false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C90E RID: 51470 RVA: 0x0049D760 File Offset: 0x0049B960
		private static int StraightLineDistToUnroofed(IntVec3 cell, Map map)
		{
			int num = int.MaxValue;
			int i = 0;
			while (i < 4)
			{
				IntVec3 facingCell = new Rot4(i).FacingCell;
				int num2 = 0;
				int num3;
				for (;;)
				{
					IntVec3 intVec = cell + facingCell * num2;
					if (!intVec.InBounds(map))
					{
						goto Block_1;
					}
					num3 = num2;
					if (InfestationCellFinder.NoRoofAroundAndWalkable(intVec, map))
					{
						break;
					}
					num2++;
				}
				IL_56:
				if (num3 < num)
				{
					num = num3;
				}
				i++;
				continue;
				Block_1:
				num3 = int.MaxValue;
				goto IL_56;
			}
			if (num == 2147483647)
			{
				return map.Size.x;
			}
			return num;
		}

		// Token: 0x0600C90F RID: 51471 RVA: 0x0049D7E8 File Offset: 0x0049B9E8
		private static float DistToBlocker(IntVec3 cell, Map map)
		{
			int num = int.MinValue;
			int num2 = int.MinValue;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 facingCell = new Rot4(i).FacingCell;
				int num3 = 0;
				int num4;
				for (;;)
				{
					IntVec3 c = cell + facingCell * num3;
					num4 = num3;
					if (!c.InBounds(map) || !c.Walkable(map))
					{
						break;
					}
					num3++;
				}
				if (num4 > num)
				{
					num2 = num;
					num = num4;
				}
				else if (num4 > num2)
				{
					num2 = num4;
				}
			}
			return (float)Mathf.Min(num, num2);
		}

		// Token: 0x0600C910 RID: 51472 RVA: 0x0049D86C File Offset: 0x0049BA6C
		private static bool NoRoofAroundAndWalkable(IntVec3 cell, Map map)
		{
			if (!cell.Walkable(map))
			{
				return false;
			}
			if (cell.Roofed(map))
			{
				return false;
			}
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = new Rot4(i).FacingCell + cell;
				if (c.InBounds(map) && c.Roofed(map))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C911 RID: 51473 RVA: 0x0049D8C8 File Offset: 0x0049BAC8
		private static float GetMountainousnessScoreAt(IntVec3 cell, Map map)
		{
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < 700; i += 10)
			{
				IntVec3 c = cell + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null && edifice.def.category == ThingCategory.Building && edifice.def.building.isNaturalRock)
					{
						num += 1f;
					}
					else if (c.Roofed(map) && c.GetRoof(map).isThickRoof)
					{
						num += 0.5f;
					}
					num2++;
				}
			}
			return num / (float)num2;
		}

		// Token: 0x0600C912 RID: 51474 RVA: 0x0049D96C File Offset: 0x0049BB6C
		private static void CalculateTraversalDistancesToUnroofed(Map map)
		{
			InfestationCellFinder.tempUnroofedRegions.Clear();
			for (int i = 0; i < map.Size.z; i++)
			{
				for (int j = 0; j < map.Size.x; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					Region region = intVec.GetRegion(map, RegionType.Set_Passable);
					if (region != null && InfestationCellFinder.NoRoofAroundAndWalkable(intVec, map))
					{
						InfestationCellFinder.tempUnroofedRegions.Add(region);
					}
				}
			}
			Dijkstra<Region>.Run(InfestationCellFinder.tempUnroofedRegions, (Region x) => x.Neighbors, (Region a, Region b) => Mathf.Sqrt((float)a.extentsClose.CenterCell.DistanceToSquared(b.extentsClose.CenterCell)), InfestationCellFinder.regionsDistanceToUnroofed, null);
			InfestationCellFinder.tempUnroofedRegions.Clear();
		}

		// Token: 0x0600C913 RID: 51475 RVA: 0x0049DA34 File Offset: 0x0049BC34
		private static void CalculateClosedAreaSizeGrid(Map map)
		{
			if (InfestationCellFinder.closedAreaSize == null)
			{
				InfestationCellFinder.closedAreaSize = new ByteGrid(map);
			}
			else
			{
				InfestationCellFinder.closedAreaSize.ClearAndResizeTo(map);
			}
			Predicate<IntVec3> <>9__0;
			Predicate<IntVec3> <>9__2;
			for (int i = 0; i < map.Size.z; i++)
			{
				for (int j = 0; j < map.Size.x; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					if (InfestationCellFinder.closedAreaSize[j, i] == 0 && !intVec.Impassable(map))
					{
						int area = 0;
						FloodFiller floodFiller = map.floodFiller;
						IntVec3 root = intVec;
						Predicate<IntVec3> passCheck;
						if ((passCheck = <>9__0) == null)
						{
							passCheck = (<>9__0 = ((IntVec3 c) => !c.Impassable(map)));
						}
						floodFiller.FloodFill(root, passCheck, delegate(IntVec3 c)
						{
							int area = area;
							area++;
						}, int.MaxValue, false, null);
						area = Mathf.Min(area, 255);
						FloodFiller floodFiller2 = map.floodFiller;
						IntVec3 root2 = intVec;
						Predicate<IntVec3> passCheck2;
						if ((passCheck2 = <>9__2) == null)
						{
							passCheck2 = (<>9__2 = ((IntVec3 c) => !c.Impassable(map)));
						}
						floodFiller2.FloodFill(root2, passCheck2, delegate(IntVec3 c)
						{
							InfestationCellFinder.closedAreaSize[c] = (byte)area;
						}, int.MaxValue, false, null);
					}
				}
			}
		}

		// Token: 0x04007277 RID: 29303
		private static List<LocationCandidate> locationCandidates = new List<LocationCandidate>();

		// Token: 0x04007278 RID: 29304
		private static Dictionary<Region, float> regionsDistanceToUnroofed = new Dictionary<Region, float>();

		// Token: 0x04007279 RID: 29305
		private static ByteGrid closedAreaSize;

		// Token: 0x0400727A RID: 29306
		private const float MinRequiredScore = 7.5f;

		// Token: 0x0400727B RID: 29307
		private const float MinMountainousnessScore = 0.17f;

		// Token: 0x0400727C RID: 29308
		private const int MountainousnessScoreRadialPatternIdx = 700;

		// Token: 0x0400727D RID: 29309
		private const int MountainousnessScoreRadialPatternSkip = 10;

		// Token: 0x0400727E RID: 29310
		private const float MountainousnessScorePerRock = 1f;

		// Token: 0x0400727F RID: 29311
		private const float MountainousnessScorePerThickRoof = 0.5f;

		// Token: 0x04007280 RID: 29312
		private const float MinCellTempToSpawnHive = -17f;

		// Token: 0x04007281 RID: 29313
		private const float MaxDistanceToColonyBuilding = 30f;

		// Token: 0x04007282 RID: 29314
		private static List<Pair<IntVec3, float>> tmpCachedInfestationChanceCellColors;

		// Token: 0x04007283 RID: 29315
		private static HashSet<Region> tempUnroofedRegions = new HashSet<Region>();
	}
}
