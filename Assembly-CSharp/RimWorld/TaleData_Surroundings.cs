using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017D2 RID: 6098
	public class TaleData_Surroundings : TaleData
	{
		// Token: 0x170019D8 RID: 6616
		// (get) Token: 0x06009827 RID: 38951 RVA: 0x00372AF9 File Offset: 0x00370CF9
		public bool Outdoors
		{
			get
			{
				return this.weather != null;
			}
		}

		// Token: 0x06009828 RID: 38952 RVA: 0x00372B04 File Offset: 0x00370D04
		public override void ExposeData()
		{
			Scribe_Values.Look<PlanetTile>(ref this.tile, "tile", default(PlanetTile), false);
			Scribe_Values.Look<float>(ref this.temperature, "temperature", 0f, false);
			Scribe_Values.Look<float>(ref this.snowDepth, "snowDepth", 0f, false);
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
			Scribe_Defs.Look<RoomRoleDef>(ref this.roomRole, "roomRole");
			Scribe_Defs.Look<BiomeDef>(ref this.biome, "biome");
			Scribe_Values.Look<float>(ref this.roomImpressiveness, "roomImpressiveness", 0f, false);
			Scribe_Values.Look<float>(ref this.roomBeauty, "roomBeauty", 0f, false);
			Scribe_Values.Look<float>(ref this.roomCleanliness, "roomCleanliness", 0f, false);
		}

		// Token: 0x06009829 RID: 38953 RVA: 0x00372BC9 File Offset: 0x00370DC9
		public override IEnumerable<Rule> GetRules(Dictionary<string, string> constants = null)
		{
			TaleData_Surroundings.<GetRules>d__12 <GetRules>d__ = new TaleData_Surroundings.<GetRules>d__12(-2);
			<GetRules>d__.<>4__this = this;
			return <GetRules>d__;
		}

		// Token: 0x0600982A RID: 38954 RVA: 0x00372BDC File Offset: 0x00370DDC
		public static TaleData_Surroundings GenerateFrom(IntVec3 c, Map map)
		{
			TaleData_Surroundings taleData_Surroundings = new TaleData_Surroundings
			{
				tile = map.Tile,
				biome = map.Biome
			};
			Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_All);
			if (roomOrAdjacent != null)
			{
				if (roomOrAdjacent.PsychologicallyOutdoors)
				{
					taleData_Surroundings.weather = map.weatherManager.CurWeatherPerceived;
				}
				taleData_Surroundings.roomRole = roomOrAdjacent.Role;
				taleData_Surroundings.roomImpressiveness = roomOrAdjacent.GetStat(RoomStatDefOf.Impressiveness);
				taleData_Surroundings.roomBeauty = roomOrAdjacent.GetStat(RoomStatDefOf.Beauty);
				taleData_Surroundings.roomCleanliness = roomOrAdjacent.GetStat(RoomStatDefOf.Cleanliness);
			}
			if (!GenTemperature.TryGetTemperatureForCell(c, map, out taleData_Surroundings.temperature))
			{
				taleData_Surroundings.temperature = 21f;
			}
			taleData_Surroundings.snowDepth = map.snowGrid.GetDepth(c);
			return taleData_Surroundings;
		}

		// Token: 0x0600982B RID: 38955 RVA: 0x00372C99 File Offset: 0x00370E99
		public static TaleData_Surroundings GenerateRandom(Map map)
		{
			return TaleData_Surroundings.GenerateFrom(CellFinder.RandomCell(map), map);
		}

		// Token: 0x04005B0D RID: 23309
		public PlanetTile tile;

		// Token: 0x04005B0E RID: 23310
		public BiomeDef biome;

		// Token: 0x04005B0F RID: 23311
		public float temperature;

		// Token: 0x04005B10 RID: 23312
		public float snowDepth;

		// Token: 0x04005B11 RID: 23313
		public WeatherDef weather;

		// Token: 0x04005B12 RID: 23314
		public RoomRoleDef roomRole;

		// Token: 0x04005B13 RID: 23315
		public float roomImpressiveness;

		// Token: 0x04005B14 RID: 23316
		public float roomBeauty;

		// Token: 0x04005B15 RID: 23317
		public float roomCleanliness;
	}
}
