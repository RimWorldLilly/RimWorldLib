using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020D9 RID: 8409
	public static class BaseGenUtility
	{
		// Token: 0x0600D39C RID: 54172 RVA: 0x004E109C File Offset: 0x004DF29C
		public static ThingDef CheapStuffFor(ThingDef thingDef, Faction faction)
		{
			ThingDef thingDef2 = (from stuff in GenStuff.AllowedStuffsFor(thingDef, (faction != null) ? faction.def.techLevel : TechLevel.Undefined, true)
			where stuff.BaseMarketValue / stuff.VolumePerUnit < 5f && stuff.stuffProps.categories.Contains(StuffCategoryDefOf.Stony)
			select stuff).RandomElementWithFallback(null);
			if (thingDef2 != null)
			{
				return thingDef2;
			}
			if (ThingDefOf.WoodLog.stuffProps.CanMake(thingDef))
			{
				return ThingDefOf.WoodLog;
			}
			return GenStuff.RandomStuffInexpensiveFor(thingDef, faction, null);
		}

		// Token: 0x0600D39D RID: 54173 RVA: 0x004E1111 File Offset: 0x004DF311
		public static ThingDef RandomCheapWallStuff(Faction faction, bool notVeryFlammable = false)
		{
			return BaseGenUtility.RandomCheapWallStuff((faction == null) ? TechLevel.Spacer : faction.def.techLevel, notVeryFlammable);
		}

		// Token: 0x0600D39E RID: 54174 RVA: 0x004E112C File Offset: 0x004DF32C
		public static ThingDef RandomCheapWallStuff(TechLevel techLevel, bool notVeryFlammable = false)
		{
			if (techLevel.IsNeolithicOrWorse())
			{
				return ThingDefOf.WoodLog;
			}
			ThingDef thingDef = (from d in GenStuff.AllowedStuffsFor(ThingDefOf.Wall, techLevel, true)
			where d.BaseMarketValue / d.VolumePerUnit < 5f && d.stuffProps.categories.Contains(StuffCategoryDefOf.Stony)
			select d).RandomElementWithFallback(null);
			if (thingDef != null)
			{
				return thingDef;
			}
			return (from d in GenStuff.AllowedStuffsFor(ThingDefOf.Wall, techLevel, true)
			where d.BaseMarketValue / d.VolumePerUnit < 5f && (!notVeryFlammable || d.BaseFlammability < 0.5f)
			select d).RandomElement<ThingDef>();
		}

		// Token: 0x0600D39F RID: 54175 RVA: 0x004E11B2 File Offset: 0x004DF3B2
		public static ThingDef RandomHightechWallStuff()
		{
			if (Rand.Value < 0.15f)
			{
				return ThingDefOf.Plasteel;
			}
			return ThingDefOf.Steel;
		}

		// Token: 0x0600D3A0 RID: 54176 RVA: 0x004E11CC File Offset: 0x004DF3CC
		private static bool BuildCostsAllowedInStuffGen(BuildableDef buildableDef)
		{
			if (buildableDef.costList.NullOrEmpty<ThingDefCountClass>())
			{
				return true;
			}
			for (int i = 0; i < buildableDef.costList.Count; i++)
			{
				ThingDef thingDef = buildableDef.costList[i].thingDef;
				if (thingDef != null && thingDef.IsStuff && !thingDef.stuffProps.allowedInStuffGeneration)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600D3A1 RID: 54177 RVA: 0x004E122B File Offset: 0x004DF42B
		public static TerrainDef RandomHightechFloorDef()
		{
			return Rand.Element<TerrainDef>(TerrainDefOf.AncientConcrete, TerrainDefOf.PavedTile, TerrainDefOf.AncientTile);
		}

		// Token: 0x0600D3A2 RID: 54178 RVA: 0x004E1241 File Offset: 0x004DF441
		private static IEnumerable<TerrainDef> IdeoFloorTypes(Faction faction, bool allowCarpet = true)
		{
			BaseGenUtility.<IdeoFloorTypes>d__7 <IdeoFloorTypes>d__ = new BaseGenUtility.<IdeoFloorTypes>d__7(-2);
			<IdeoFloorTypes>d__.<>3__faction = faction;
			<IdeoFloorTypes>d__.<>3__allowCarpet = allowCarpet;
			return <IdeoFloorTypes>d__;
		}

		// Token: 0x0600D3A3 RID: 54179 RVA: 0x004E1258 File Offset: 0x004DF458
		public static TerrainDef RandomBasicFloorDef(Faction faction, bool allowCarpet = false)
		{
			bool flag = allowCarpet && (faction == null || !faction.def.techLevel.IsNeolithicOrWorse()) && Rand.Chance(0.1f);
			TerrainDef result;
			if (faction != null && faction.ideos != null && BaseGenUtility.IdeoFloorTypes(faction, flag).TryRandomElement(out result))
			{
				return result;
			}
			if (flag)
			{
				return (from x in DefDatabase<TerrainDef>.AllDefsListForReading
				where x.IsCarpet
				select x).RandomElement<TerrainDef>();
			}
			return Rand.Element<TerrainDef>(TerrainDefOf.AncientTile, TerrainDefOf.PavedTile, TerrainDefOf.WoodPlankFloor, TerrainDefOf.TileSandstone);
		}

		// Token: 0x0600D3A4 RID: 54180 RVA: 0x004E12F4 File Offset: 0x004DF4F4
		public static bool TryRandomInexpensiveFloor(out TerrainDef floor, Predicate<TerrainDef> validator = null)
		{
			Func<TerrainDef, float> costCalculator = delegate(TerrainDef x)
			{
				List<ThingDefCountClass> list = x.CostListAdjusted(null, true);
				float num2 = 0f;
				for (int i = 0; i < list.Count; i++)
				{
					num2 += (float)list[i].count * list[i].thingDef.BaseMarketValue;
				}
				return num2;
			};
			IEnumerable<TerrainDef> enumerable = from x in DefDatabase<TerrainDef>.AllDefs
			where x.BuildableByPlayer && x.terrainAffordanceNeeded != TerrainAffordanceDefOf.Bridgeable && !x.IsCarpet && BaseGenUtility.BuildCostsAllowedInStuffGen(x) && (validator == null || validator(x)) && costCalculator(x) > 0f
			select x;
			float cheapest = -1f;
			foreach (TerrainDef arg in enumerable)
			{
				float num = costCalculator(arg);
				if (cheapest == -1f || num < cheapest)
				{
					cheapest = num;
				}
			}
			return (from x in enumerable
			where costCalculator(x) <= cheapest * 4f
			select x).TryRandomElement(out floor);
		}

		// Token: 0x0600D3A5 RID: 54181 RVA: 0x004E13D0 File Offset: 0x004DF5D0
		public static TerrainDef CorrespondingTerrainDef(ThingDef stuffDef, bool beautiful, Faction faction = null)
		{
			BaseGenUtility.tmpFactionFloors.Clear();
			if (faction != null && faction.ideos != null)
			{
				foreach (TerrainDef terrainDef in BaseGenUtility.IdeoFloorTypes(faction, true))
				{
					if (terrainDef.CostList != null)
					{
						for (int i = 0; i < terrainDef.CostList.Count; i++)
						{
							if (terrainDef.CostList[i].thingDef == stuffDef)
							{
								BaseGenUtility.tmpFactionFloors.Add(terrainDef);
								break;
							}
						}
					}
				}
				TerrainDef result;
				if (BaseGenUtility.tmpFactionFloors.Any<TerrainDef>() && BaseGenUtility.tmpFactionFloors.TryRandomElementByWeight(delegate(TerrainDef x)
				{
					float statOffsetFromList = x.statBases.GetStatOffsetFromList(StatDefOf.Beauty);
					if (statOffsetFromList == 0f)
					{
						return 0f;
					}
					if (!beautiful)
					{
						return 1f / statOffsetFromList;
					}
					return statOffsetFromList;
				}, out result))
				{
					return result;
				}
			}
			TerrainDef terrainDef2 = null;
			List<TerrainDef> allDefsListForReading = DefDatabase<TerrainDef>.AllDefsListForReading;
			for (int j = 0; j < allDefsListForReading.Count; j++)
			{
				if (allDefsListForReading[j].CostList != null)
				{
					for (int k = 0; k < allDefsListForReading[j].CostList.Count; k++)
					{
						if (allDefsListForReading[j].CostList[k].thingDef == stuffDef && (terrainDef2 == null || (beautiful ? (terrainDef2.statBases.GetStatOffsetFromList(StatDefOf.Beauty) < allDefsListForReading[j].statBases.GetStatOffsetFromList(StatDefOf.Beauty)) : (terrainDef2.statBases.GetStatOffsetFromList(StatDefOf.Beauty) > allDefsListForReading[j].statBases.GetStatOffsetFromList(StatDefOf.Beauty)))))
						{
							terrainDef2 = allDefsListForReading[j];
						}
					}
				}
			}
			if (terrainDef2 == null)
			{
				terrainDef2 = TerrainDefOf.Concrete;
			}
			return terrainDef2;
		}

		// Token: 0x0600D3A6 RID: 54182 RVA: 0x004E15A4 File Offset: 0x004DF7A4
		public static TerrainDef RegionalRockTerrainDef(PlanetTile tile, bool beautiful)
		{
			ThingDef thingDef = Find.World.NaturalRockTypesIn(tile).RandomElementWithFallback(null);
			ThingDef thingDef2 = (thingDef != null) ? thingDef.building.mineableThing : null;
			return BaseGenUtility.CorrespondingTerrainDef((thingDef2 != null && thingDef2.butcherProducts != null && thingDef2.butcherProducts.Count > 0) ? thingDef2.butcherProducts[0].thingDef : null, beautiful, null);
		}

		// Token: 0x0600D3A7 RID: 54183 RVA: 0x004E1608 File Offset: 0x004DF808
		public static bool AnyDoorAdjacentCardinalTo(IntVec3 cell, Map map)
		{
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = cell + GenAdj.CardinalDirections[i];
				if (c.InBounds(map) && c.GetDoor(map) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D3A8 RID: 54184 RVA: 0x004E1648 File Offset: 0x004DF848
		public static bool AnyDoorAdjacentCardinalTo(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect.AdjacentCellsCardinal)
			{
				if (c.InBounds(map) && c.GetDoor(map) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D3A9 RID: 54185 RVA: 0x004E16A8 File Offset: 0x004DF8A8
		public static ThingDef WallStuffAt(IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			if (edifice != null && edifice.def == ThingDefOf.Wall)
			{
				return edifice.Stuff;
			}
			return null;
		}

		// Token: 0x0600D3AA RID: 54186 RVA: 0x004E16D8 File Offset: 0x004DF8D8
		public static void ScatterSentryDronesInMap(SimpleCurve sentryCountFromPointsCurve, Map map, Faction faction, GenStepParams parms)
		{
			CellRect rect = MapGenerator.GetVar<CellRect>("SpawnRect");
			if (rect.Width <= 1 || rect.Height <= 1)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(76, 1);
				defaultInterpolatedStringHandler.AppendLiteral("BaseGenUtility.ScatterSentryDronesInMap called but spawn rect wasn't valid: ");
				defaultInterpolatedStringHandler.AppendFormatted<CellRect>(rect);
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			SitePart sitePart = parms.sitePart;
			float? num;
			if (sitePart == null)
			{
				num = null;
			}
			else
			{
				SitePartParams parms2 = sitePart.parms;
				num = ((parms2 != null) ? new float?(parms2.points) : null);
			}
			int num2 = Mathf.RoundToInt(sentryCountFromPointsCurve.Evaluate(num ?? StorytellerUtility.DefaultThreatPointsNow(map)));
			List<Room> collection = (from r in map.regionGrid.AllRooms
			where r.ExtentsClose.FullyContainedWithin(rect) && !r.ExposedToSpace && !r.OutdoorsForWork && r.CellCount > 10
			select r).ToList<Room>();
			HashSet<Room> hashSet = new HashSet<Room>(collection);
			if (hashSet.Count == 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(89, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("BaseGenUtility.ScatterSentryDronesInMap called but no valid rooms found in rect ");
				defaultInterpolatedStringHandler2.AppendFormatted<CellRect>(rect);
				defaultInterpolatedStringHandler2.AppendLiteral(" on map ");
				defaultInterpolatedStringHandler2.AppendFormatted<Map>(map);
				defaultInterpolatedStringHandler2.AppendLiteral(".");
				Log.Warning(defaultInterpolatedStringHandler2.ToStringAndClear());
				return;
			}
			Func<IntVec3, bool> <>9__1;
			for (int i = 0; i < num2; i++)
			{
				if (hashSet.Count == 0)
				{
					hashSet = new HashSet<Room>(collection);
				}
				Room room = hashSet.RandomElement<Room>();
				hashSet.Remove(room);
				IEnumerable<IntVec3> cells = room.Cells;
				Func<IntVec3, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((IntVec3 c) => c.Standable(map)));
				}
				IntVec3 intVec = cells.Where(predicate).RandomElement<IntVec3>();
				if (intVec == IntVec3.Invalid)
				{
					i--;
				}
				else
				{
					GenSpawn.Spawn(PawnGenerator.GeneratePawn(PawnKindDefOf.Drone_Sentry, faction, null), intVec, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x0600D3AB RID: 54187 RVA: 0x004E18E0 File Offset: 0x004DFAE0
		public static void CheckSpawnBridgeUnder(ThingDef thingDef, IntVec3 c, Rot4 rot)
		{
			if (thingDef.category != ThingCategory.Building)
			{
				return;
			}
			Map map = BaseGen.globalSettings.map;
			CellRect cellRect = GenAdj.OccupiedRect(c, rot, thingDef.size);
			BaseGenUtility.bridgeCells.Clear();
			foreach (IntVec3 intVec in cellRect)
			{
				if (!intVec.SupportsStructureType(map, thingDef.terrainAffordanceNeeded) && GenConstruct.CanBuildOnTerrain(TerrainDefOf.Bridge, intVec, map, Rot4.North, null, null))
				{
					BaseGenUtility.bridgeCells.Add(intVec);
				}
			}
			if (!BaseGenUtility.bridgeCells.Any<IntVec3>())
			{
				return;
			}
			if (thingDef.size.x != 1 || thingDef.size.z != 1)
			{
				for (int i = BaseGenUtility.bridgeCells.Count - 1; i >= 0; i--)
				{
					for (int j = 0; j < 8; j++)
					{
						IntVec3 intVec2 = BaseGenUtility.bridgeCells[i] + GenAdj.AdjacentCells[j];
						if (!BaseGenUtility.bridgeCells.Contains(intVec2) && intVec2.InBounds(map) && !intVec2.SupportsStructureType(map, thingDef.terrainAffordanceNeeded) && GenConstruct.CanBuildOnTerrain(TerrainDefOf.Bridge, intVec2, map, Rot4.North, null, null))
						{
							BaseGenUtility.bridgeCells.Add(intVec2);
						}
					}
				}
			}
			for (int k = 0; k < BaseGenUtility.bridgeCells.Count; k++)
			{
				map.terrainGrid.SetTerrain(BaseGenUtility.bridgeCells[k], TerrainDefOf.Bridge);
			}
		}

		// Token: 0x0600D3AC RID: 54188 RVA: 0x004E1A7C File Offset: 0x004DFC7C
		[DebugOutput]
		private static void WallStuffs()
		{
			IEnumerable<ThingDef> dataSources = GenStuff.AllowedStuffsFor(ThingDefOf.Wall, TechLevel.Undefined, false);
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("cheap", (ThingDef d) => (d.BaseMarketValue / d.VolumePerUnit < 5f).ToStringCheckBlank());
			array[2] = new TableDataGetter<ThingDef>("floor", (ThingDef d) => BaseGenUtility.CorrespondingTerrainDef(d, false, null).defName);
			array[3] = new TableDataGetter<ThingDef>("floor (beautiful)", (ThingDef d) => BaseGenUtility.CorrespondingTerrainDef(d, true, null).defName);
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600D3AD RID: 54189 RVA: 0x004E1B50 File Offset: 0x004DFD50
		public static void DoPathwayBetween(IntVec3 a, IntVec3 b, TerrainDef terrainDef, int size = 3)
		{
			foreach (IntVec3 center in GenSight.PointsOnLineOfSight(a, b))
			{
				foreach (IntVec3 c in CellRect.CenteredOn(center, size, size))
				{
					if (c.InBounds(BaseGen.globalSettings.map))
					{
						BaseGen.globalSettings.map.terrainGrid.SetTerrain(c, terrainDef);
					}
				}
			}
		}

		// Token: 0x04008604 RID: 34308
		public const int MaxCheapWallStuffMarketValuePerUnit = 5;

		// Token: 0x04008605 RID: 34309
		private static List<TerrainDef> tmpFactionFloors = new List<TerrainDef>();

		// Token: 0x04008606 RID: 34310
		private static List<IntVec3> bridgeCells = new List<IntVec3>();
	}
}
