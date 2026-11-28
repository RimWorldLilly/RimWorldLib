using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000315 RID: 789
	public static class GenTemperature
	{
		// Token: 0x06001830 RID: 6192 RVA: 0x00091148 File Offset: 0x0008F348
		public static float AverageTemperatureAtTileForTwelfth(PlanetTile tile, Twelfth twelfth)
		{
			int num = 30000;
			int num2 = 300000 * (int)twelfth;
			float num3 = 0f;
			for (int i = 0; i < 120; i++)
			{
				int absTick = num2 + num + Mathf.RoundToInt((float)i / 120f * 300000f);
				num3 += GenTemperature.GetTemperatureFromSeasonAtTile(absTick, tile);
			}
			return num3 / 120f;
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x000911A4 File Offset: 0x0008F3A4
		public static float MinTemperatureAtTile(PlanetTile tile)
		{
			float num = float.MaxValue;
			for (int i = 0; i < 3600000; i += 27000)
			{
				num = Mathf.Min(num, GenTemperature.GetTemperatureFromSeasonAtTile(i, tile));
			}
			return num;
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x000911DC File Offset: 0x0008F3DC
		public static float MaxTemperatureAtTile(PlanetTile tile)
		{
			float num = float.MinValue;
			for (int i = 0; i < 3600000; i += 27000)
			{
				num = Mathf.Max(num, GenTemperature.GetTemperatureFromSeasonAtTile(i, tile));
			}
			return num;
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x00091212 File Offset: 0x0008F412
		public static FloatRange ComfortableTemperatureRange(this Pawn p)
		{
			return new FloatRange(p.GetStatValue(StatDefOf.ComfyTemperatureMin, true, 1), p.GetStatValue(StatDefOf.ComfyTemperatureMax, true, 1));
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x00091234 File Offset: 0x0008F434
		public static FloatRange ComfortableTemperatureRange(this Pawn p, List<ThingStuffPair> apparel)
		{
			float num = p.GetStatValue(StatDefOf.ComfyTemperatureMin, true, 1);
			float num2 = p.GetStatValue(StatDefOf.ComfyTemperatureMax, true, 1);
			if (apparel != null)
			{
				num -= apparel.Sum((ThingStuffPair x) => x.InsulationCold);
				num2 += apparel.Sum((ThingStuffPair x) => x.InsulationHeat);
			}
			return new FloatRange(num, num2);
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x000912B8 File Offset: 0x0008F4B8
		public static FloatRange ComfortableTemperatureRange(ThingDef raceDef, List<ThingStuffPair> apparel = null)
		{
			FloatRange result = new FloatRange(raceDef.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null), raceDef.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null));
			if (apparel != null)
			{
				result.min -= apparel.Sum((ThingStuffPair x) => x.InsulationCold);
				result.max += apparel.Sum((ThingStuffPair x) => x.InsulationHeat);
			}
			return result;
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x00091348 File Offset: 0x0008F548
		public static FloatRange SafeTemperatureRange(this Pawn p)
		{
			FloatRange result = p.ComfortableTemperatureRange();
			result.min -= 10f;
			result.max += 10f;
			return result;
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x00091380 File Offset: 0x0008F580
		public static FloatRange SafeTemperatureRange(this Pawn p, List<ThingStuffPair> apparel)
		{
			FloatRange result = p.ComfortableTemperatureRange(apparel);
			result.min -= 10f;
			result.max += 10f;
			return result;
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x000913B8 File Offset: 0x0008F5B8
		public static bool SafeTemperatureAtCell(this Pawn p, IntVec3 cell, Map map)
		{
			return p.SafeTemperatureRange().Includes(GenTemperature.GetTemperatureForCell(cell, map));
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x000913DC File Offset: 0x0008F5DC
		public static bool ComfortableTemperatureAtCell(this Pawn p, IntVec3 cell, Map map)
		{
			return p.ComfortableTemperatureRange().Includes(GenTemperature.GetTemperatureForCell(cell, map));
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x00091400 File Offset: 0x0008F600
		public static FloatRange SafeTemperatureRange(ThingDef raceDef, List<ThingStuffPair> apparel = null)
		{
			FloatRange result = GenTemperature.ComfortableTemperatureRange(raceDef, apparel);
			result.min -= 10f;
			result.max += 10f;
			return result;
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x00091438 File Offset: 0x0008F638
		public static float GetTemperatureForCell(IntVec3 c, Map map)
		{
			float result;
			GenTemperature.TryGetTemperatureForCell(c, map, out result);
			return result;
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x00091450 File Offset: 0x0008F650
		public static bool TryGetTemperatureForCell(IntVec3 c, Map map, out float tempResult)
		{
			if (map == null)
			{
				Log.Error("Got temperature for null map.");
				tempResult = 21f;
				return true;
			}
			if (!c.InBounds(map))
			{
				tempResult = 21f;
				return false;
			}
			if (GenTemperature.TryGetDirectAirTemperatureForCell(c, map, out tempResult))
			{
				return true;
			}
			List<Thing> list = map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability == Traversability.Impassable)
				{
					return GenTemperature.TryGetAirTemperatureAroundThing(list[i], out tempResult);
				}
			}
			return false;
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x000914D4 File Offset: 0x0008F6D4
		public static bool TryGetDirectAirTemperatureForCell(IntVec3 c, Map map, out float temperature)
		{
			if (!c.InBounds(map))
			{
				temperature = 21f;
				return false;
			}
			Room room = c.GetRoom(map);
			if (room == null)
			{
				temperature = 21f;
				return false;
			}
			temperature = room.Temperature;
			return true;
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x00091510 File Offset: 0x0008F710
		public static bool TryGetAirTemperatureAroundThing(Thing t, out float temperature)
		{
			float num = 0f;
			int num2 = 0;
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(t);
			for (int i = 0; i < list.Count; i++)
			{
				float num3;
				if (list[i].InBounds(t.Map) && GenTemperature.TryGetDirectAirTemperatureForCell(list[i], t.Map, out num3))
				{
					num += num3;
					num2++;
				}
			}
			if (num2 > 0)
			{
				temperature = num / (float)num2;
				return true;
			}
			temperature = 21f;
			return false;
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x0009158C File Offset: 0x0008F78C
		public static float OffsetFromSunCycle(int absTick, PlanetTile tile)
		{
			if (!tile.Valid)
			{
				return 0f;
			}
			float num = GenDate.DayPercent((long)absTick, Find.WorldGrid.LongLatOf(tile).x);
			return Mathf.Cos(6.2831855f * (num + 0.32f)) * 7f;
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x000915D8 File Offset: 0x0008F7D8
		public static float OffsetFromSeasonCycle(int absTick, PlanetTile tile)
		{
			float num = (float)absTick / 60000f % 60f / 60f;
			return Mathf.Cos(6.2831855f * (num - Season.Winter.GetMiddleTwelfth(0f).GetBeginningYearPct())) * -GenTemperature.SeasonalShiftAmplitudeAt(tile);
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x00091620 File Offset: 0x0008F820
		public static float GetTemperatureFromSeasonAtTile(int absTick, PlanetTile tile)
		{
			if (absTick == 0)
			{
				absTick = 1;
			}
			Tile tile2 = Find.WorldGrid[tile];
			if (tile2 == null)
			{
				return 21f;
			}
			return tile2.temperature + GenTemperature.OffsetFromSeasonCycle(absTick, tile);
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x00091658 File Offset: 0x0008F858
		public static float GetTemperatureAtTile(PlanetTile tile)
		{
			Map map = Current.Game.FindMap(tile);
			if (map != null)
			{
				return map.mapTemperature.OutdoorTemp;
			}
			return GenTemperature.GetTemperatureFromSeasonAtTile(GenTicks.TicksAbs, tile);
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x0009168C File Offset: 0x0008F88C
		public static float SeasonalShiftAmplitudeAt(PlanetTile tile)
		{
			if (!tile.Valid)
			{
				return 0f;
			}
			if (Find.WorldGrid.LongLatOf(tile).y >= 0f)
			{
				return TemperatureTuning.SeasonalTempVariationCurve.Evaluate(Find.WorldGrid.DistanceFromEquatorNormalized(tile));
			}
			return -TemperatureTuning.SeasonalTempVariationCurve.Evaluate(Find.WorldGrid.DistanceFromEquatorNormalized(tile));
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x000916EC File Offset: 0x0008F8EC
		public static List<Twelfth> TwelfthsInAverageTemperatureRange(PlanetTile tile, float minTemp, float maxTemp)
		{
			List<Twelfth> twelfths = new List<Twelfth>();
			for (int i = 0; i < 12; i++)
			{
				float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				if (num >= minTemp && num <= maxTemp)
				{
					twelfths.Add((Twelfth)i);
				}
			}
			if (twelfths.Count <= 1 || twelfths.Count == 12)
			{
				return twelfths;
			}
			if (twelfths.Contains(Twelfth.Twelfth) && twelfths.Contains(Twelfth.First))
			{
				int num2 = (int)twelfths.First((Twelfth m) => !twelfths.Contains((Twelfth)(m - Twelfth.Second)));
				List<Twelfth> list = new List<Twelfth>();
				int num3 = num2;
				while (num3 < 12 && twelfths.Contains((Twelfth)num3))
				{
					list.Add((Twelfth)num3);
					num3++;
				}
				int num4 = 0;
				while (num4 < 12 && twelfths.Contains((Twelfth)num4))
				{
					list.Add((Twelfth)num4);
					num4++;
				}
			}
			return twelfths;
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x000917EC File Offset: 0x0008F9EC
		public static Twelfth EarliestTwelfthInAverageTemperatureRange(PlanetTile tile, float minTemp, float maxTemp)
		{
			int i = 0;
			while (i < 12)
			{
				float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				if (num >= minTemp && num <= maxTemp)
				{
					if (i != 0)
					{
						return (Twelfth)i;
					}
					Twelfth twelfth = (Twelfth)i;
					for (int j = 0; j < 12; j++)
					{
						float num2 = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth.PreviousTwelfth());
						if (num2 < minTemp || num2 > maxTemp)
						{
							return twelfth;
						}
						twelfth = twelfth.PreviousTwelfth();
					}
					return (Twelfth)i;
				}
				else
				{
					i++;
				}
			}
			return Twelfth.Undefined;
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x00091854 File Offset: 0x0008FA54
		public static bool PushHeat(IntVec3 c, Map map, float energy)
		{
			if (map == null)
			{
				Log.Error("Added heat to null map.");
				return false;
			}
			Room room = c.GetRoom(map);
			if (room != null)
			{
				return room.PushHeat(energy);
			}
			GenTemperature.neighRooms.Clear();
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = c + GenAdj.AdjacentCells[i];
				if (intVec.InBounds(map))
				{
					room = intVec.GetRoom(map);
					if (room != null)
					{
						GenTemperature.neighRooms.Add(room);
					}
				}
			}
			float energy2 = energy / (float)GenTemperature.neighRooms.Count;
			for (int j = 0; j < GenTemperature.neighRooms.Count; j++)
			{
				GenTemperature.neighRooms[j].PushHeat(energy2);
			}
			bool result = GenTemperature.neighRooms.Count > 0;
			GenTemperature.neighRooms.Clear();
			return result;
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x0009191C File Offset: 0x0008FB1C
		public static void PushHeat(Thing t, float energy)
		{
			if (t.GetRoom(RegionType.Set_All) != null)
			{
				GenTemperature.PushHeat(t.Position, t.Map, energy);
				return;
			}
			IntVec3 c;
			if (GenAdj.TryFindRandomAdjacentCell8WayWithRoom(t, out c))
			{
				GenTemperature.PushHeat(c, t.Map, energy);
			}
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x00091960 File Offset: 0x0008FB60
		public static float ControlTemperatureTempChange(IntVec3 cell, Map map, float energyLimit, float targetTemperature)
		{
			Room room = cell.GetRoom(map);
			if (room == null || room.UsesOutdoorTemperature)
			{
				return 0f;
			}
			float b = energyLimit / (float)room.CellCount;
			float a = targetTemperature - room.Temperature;
			float num;
			if (energyLimit > 0f)
			{
				num = Mathf.Min(a, b);
				num = Mathf.Max(num, 0f);
			}
			else
			{
				num = Mathf.Max(a, b);
				num = Mathf.Min(num, 0f);
			}
			return num;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x000919D4 File Offset: 0x0008FBD4
		public static void EqualizeTemperaturesThroughBuilding(Building b, float rate, bool twoWay)
		{
			GenTemperature.beqRooms.Clear();
			float num = 0f;
			foreach (IntVec3 a in b.OccupiedRect())
			{
				if (twoWay)
				{
					for (int i = 0; i < 2; i++)
					{
						IntVec3 intVec = (i == 0) ? (a + b.Rotation.FacingCell) : (a - b.Rotation.FacingCell);
						if (intVec.InBounds(b.Map))
						{
							Room room = intVec.GetRoom(b.Map);
							if (room != null && GenTemperature.beqRooms.Add(room))
							{
								num += room.Temperature;
							}
						}
					}
				}
				else
				{
					for (int j = 0; j < 4; j++)
					{
						IntVec3 intVec2 = a + GenAdj.CardinalDirections[j];
						if (intVec2.InBounds(b.Map))
						{
							Room room2 = intVec2.GetRoom(b.Map);
							if (room2 != null && GenTemperature.beqRooms.Add(room2))
							{
								num += room2.Temperature;
							}
						}
					}
				}
			}
			int count = GenTemperature.beqRooms.Count;
			if (count == 0)
			{
				return;
			}
			float num2 = num / (float)count;
			foreach (IntVec3 loc in b.OccupiedRect())
			{
				Room room3 = loc.GetRoom(b.Map);
				if (room3 != null)
				{
					room3.Temperature = num2;
				}
			}
			if (GenTemperature.beqRooms.Count == 1)
			{
				return;
			}
			float num3 = 1f;
			foreach (Room room4 in GenTemperature.beqRooms)
			{
				if (!room4.UsesOutdoorTemperature)
				{
					float temperature = room4.Temperature;
					float num4 = (num2 - temperature) * rate;
					float num5 = num4 / (float)room4.CellCount;
					float num6 = room4.Temperature + num5;
					if (num4 > 0f && num6 > num2)
					{
						num6 = num2;
					}
					else if (num4 < 0f && num6 < num2)
					{
						num6 = num2;
					}
					float num7 = Mathf.Abs((num6 - temperature) * (float)room4.CellCount / num4);
					if (num7 < num3)
					{
						num3 = num7;
					}
				}
			}
			foreach (Room room5 in GenTemperature.beqRooms)
			{
				if (!room5.UsesOutdoorTemperature)
				{
					float temperature2 = room5.Temperature;
					float num8 = num2 - temperature2;
					float num9 = (b.Map.Biome.inVacuum && num8 < 0f) ? 0.1f : 1f;
					float num10 = num8 * rate * num3 * num9 / (float)room5.CellCount;
					room5.Temperature += num10;
				}
			}
			GenTemperature.beqRooms.Clear();
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x00091D38 File Offset: 0x0008FF38
		public static float RotRateAtTemperature(float temperature)
		{
			if (temperature < 0f)
			{
				return 0f;
			}
			if (temperature >= 10f)
			{
				return 1f;
			}
			return (temperature - 0f) / 10f;
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x00091D64 File Offset: 0x0008FF64
		public static bool FactionOwnsPassableRoomInTemperatureRange(Faction faction, FloatRange tempRange, Map map)
		{
			if (faction == Faction.OfPlayer)
			{
				IReadOnlyList<Room> allRooms = map.regionGrid.AllRooms;
				for (int i = 0; i < allRooms.Count; i++)
				{
					Room room = allRooms[i];
					if (room.AnyPassable && !room.Fogged && tempRange.Includes(room.Temperature))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x00091DC4 File Offset: 0x0008FFC4
		public static string GetAverageTemperatureLabel(PlanetTile tile)
		{
			string str = ((!tile.Valid) ? 21f : Find.WorldGrid[tile].temperature).ToStringTemperature("F1");
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 3);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted(GenTemperature.MinTemperatureAtTile(tile).ToStringTemperature("F0"));
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("RangeTo".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(GenTemperature.MaxTemperatureAtTile(tile).ToStringTemperature("F0"));
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return str + defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x00091E7F File Offset: 0x0009007F
		public static float CelsiusTo(float temp, TemperatureDisplayMode oldMode)
		{
			switch (oldMode)
			{
			case TemperatureDisplayMode.Celsius:
				return temp;
			case TemperatureDisplayMode.Fahrenheit:
				return temp * 1.8f + 32f;
			case TemperatureDisplayMode.Kelvin:
				return temp + 273.15f;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x00091EB2 File Offset: 0x000900B2
		public static float CelsiusToOffset(float temp, TemperatureDisplayMode oldMode)
		{
			switch (oldMode)
			{
			case TemperatureDisplayMode.Celsius:
				return temp;
			case TemperatureDisplayMode.Fahrenheit:
				return temp * 1.8f;
			case TemperatureDisplayMode.Kelvin:
				return temp;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x00091ED9 File Offset: 0x000900D9
		public static float ConvertTemperatureOffset(float temp, TemperatureDisplayMode oldMode, TemperatureDisplayMode newMode)
		{
			switch (oldMode)
			{
			case TemperatureDisplayMode.Fahrenheit:
				temp /= 1.8f;
				break;
			}
			switch (newMode)
			{
			case TemperatureDisplayMode.Fahrenheit:
				temp *= 1.8f;
				break;
			}
			return temp;
		}

		// Token: 0x0400143A RID: 5178
		public static readonly Color ColorSpotHot = new Color(1f, 0f, 0f, 0.6f);

		// Token: 0x0400143B RID: 5179
		public static readonly Color ColorSpotCold = new Color(0f, 0f, 1f, 0.6f);

		// Token: 0x0400143C RID: 5180
		public static readonly Color ColorRoomHot = new Color(1f, 0f, 0f, 0.3f);

		// Token: 0x0400143D RID: 5181
		public static readonly Color ColorRoomCold = new Color(0f, 0f, 1f, 0.3f);

		// Token: 0x0400143E RID: 5182
		private static readonly List<Room> neighRooms = new List<Room>();

		// Token: 0x0400143F RID: 5183
		private static readonly HashSet<Room> beqRooms = new HashSet<Room>();

		// Token: 0x04001440 RID: 5184
		private const float VacuumTemperatureEqualizationFactor = 0.1f;
	}
}
