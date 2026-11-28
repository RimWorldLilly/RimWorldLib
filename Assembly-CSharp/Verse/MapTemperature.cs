using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000316 RID: 790
	public class MapTemperature : ICellBoolGiver
	{
		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001851 RID: 6225 RVA: 0x00091FB4 File Offset: 0x000901B4
		public float OutdoorTemp
		{
			get
			{
				if (this.map.Biome.constantOutdoorTemperature != null)
				{
					return this.map.Biome.constantOutdoorTemperature.Value;
				}
				if (this.map.IsPocketMap)
				{
					return this.map.generatorDef.pocketMapProperties.temperature;
				}
				return Find.World.tileTemperatures.GetOutdoorTemp(this.map.Tile);
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001852 RID: 6226 RVA: 0x0009202C File Offset: 0x0009022C
		public float SeasonalTemp
		{
			get
			{
				if (this.map.Biome.constantOutdoorTemperature != null)
				{
					return this.map.Biome.constantOutdoorTemperature.Value;
				}
				if (this.map.IsPocketMap)
				{
					return this.map.generatorDef.pocketMapProperties.temperature;
				}
				return Find.World.tileTemperatures.GetSeasonalTemp(this.map.Tile);
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001853 RID: 6227 RVA: 0x000920A4 File Offset: 0x000902A4
		public CellBoolDrawer Drawer
		{
			get
			{
				CellBoolDrawer result;
				if ((result = this.drawerInt) == null)
				{
					result = (this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3600, 0.33f));
				}
				return result;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001854 RID: 6228 RVA: 0x000920F4 File Offset: 0x000902F4
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x000920FB File Offset: 0x000902FB
		public MapTemperature(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x0009210C File Offset: 0x0009030C
		public void MapTemperatureTick()
		{
			if (Find.TickManager.TicksGame % 120 == 7 || DebugSettings.fastEcology)
			{
				IReadOnlyList<Room> allRooms = this.map.regionGrid.AllRooms;
				for (int i = 0; i < allRooms.Count; i++)
				{
					allRooms[i].TempTracker.EqualizeTemperature();
				}
			}
			if (Find.TickManager.TicksGame % 60 == 0)
			{
				this.Drawer.SetDirty();
			}
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x0009217D File Offset: 0x0009037D
		public void TemperatureUpdate()
		{
			if (Find.PlaySettings.showTemperatureOverlay && !Find.ScreenshotModeHandler.Active)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x000921B0 File Offset: 0x000903B0
		public bool SeasonAcceptableFor(ThingDef animalRace, float buffer = 0f)
		{
			float seasonalTemp = this.SeasonalTemp;
			return seasonalTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - buffer && seasonalTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null) + buffer;
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x000921E8 File Offset: 0x000903E8
		public bool OutdoorTemperatureAcceptableFor(ThingDef animalRace, float buffer = 0f)
		{
			float outdoorTemp = this.OutdoorTemp;
			return outdoorTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - buffer && outdoorTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null) + buffer;
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x00092220 File Offset: 0x00090420
		public bool SeasonAndOutdoorTemperatureAcceptableFor(ThingDef animalRace, float buffer = 0f)
		{
			return this.SeasonAcceptableFor(animalRace, buffer) && this.OutdoorTemperatureAcceptableFor(animalRace, buffer);
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x00092238 File Offset: 0x00090438
		public bool LocalSeasonsAreMeaningful()
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < 12; i++)
			{
				float num = Find.World.tileTemperatures.AverageTemperatureForTwelfth(this.map.Tile, (Twelfth)i);
				if (num > 0f)
				{
					flag2 = true;
				}
				if (num < 0f)
				{
					flag = true;
				}
			}
			return flag2 && flag;
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x00092288 File Offset: 0x00090488
		public bool GetCellBool(int index)
		{
			IntVec3 intVec = this.map.cellIndices.IndexToCell(index);
			return !intVec.Fogged(this.map) && (!ModsConfig.OdysseyActive || intVec.GetTerrain(this.map) != TerrainDefOf.Space) && intVec.GetRoom(this.map) != null;
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x000922E4 File Offset: 0x000904E4
		public Color GetCellExtraColor(int index)
		{
			float temperature = this.map.cellIndices.IndexToCell(index).GetTemperature(this.map);
			return this.GetColorForTemperature(temperature);
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x00092318 File Offset: 0x00090518
		private Color GetColorForTemperature(float temperature)
		{
			List<ValueTuple<float, Color>> temperatureColorMap = MapTemperature.TemperatureColorMap;
			if (temperature <= temperatureColorMap[0].Item1)
			{
				return temperatureColorMap[0].Item2;
			}
			if (temperature >= temperatureColorMap[temperatureColorMap.Count - 1].Item1)
			{
				return temperatureColorMap[temperatureColorMap.Count - 1].Item2;
			}
			int i = 1;
			while (i < temperatureColorMap.Count)
			{
				if (temperatureColorMap[i].Item1 > temperature)
				{
					if (i >= temperatureColorMap.Count - 1)
					{
						return temperatureColorMap[i].Item2;
					}
					int index = i - 1;
					int index2 = i;
					float item = temperatureColorMap[index].Item1;
					float item2 = temperatureColorMap[index2].Item1;
					float t = (temperature - item) / (item2 - item);
					return Color.Lerp(temperatureColorMap[index].Item2, temperatureColorMap[index2].Item2, t);
				}
				else
				{
					i++;
				}
			}
			Log.Error("Error when trying to determine correct color for temperature grid.");
			return Color.white;
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x00092409 File Offset: 0x00090609
		public void Notify_ThingSpawned(Thing thing)
		{
			if (thing.def.AffectsRegions)
			{
				this.Drawer.SetDirty();
			}
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x00092424 File Offset: 0x00090624
		public void DebugLogTemps()
		{
			StringBuilder stringBuilder = new StringBuilder();
			float latitude = (Find.CurrentMap != null) ? Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile).y : 0f;
			stringBuilder.AppendLine("Latitude " + latitude.ToString());
			stringBuilder.AppendLine("-----Temperature for each hour this day------");
			stringBuilder.AppendLine("Hour    Temp    SunEffect");
			int num = Find.TickManager.TicksAbs - Find.TickManager.TicksAbs % 60000;
			for (int i = 0; i < 24; i++)
			{
				int absTick = num + i * 2500;
				stringBuilder.Append(i.ToString().PadRight(5));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick).ToString("F2").PadRight(8));
				stringBuilder.Append(GenTemperature.OffsetFromSunCycle(absTick, this.map.Tile).ToString("F2"));
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-----Temperature for each twelfth this year------");
			for (int j = 0; j < 12; j++)
			{
				Twelfth twelfth = (Twelfth)j;
				float num2 = Find.World.tileTemperatures.AverageTemperatureForTwelfth(this.map.Tile, twelfth);
				stringBuilder.AppendLine(string.Concat(new string[]
				{
					twelfth.GetQuadrum().ToString(),
					"/",
					SeasonUtility.GetReportedSeason(twelfth.GetMiddleYearPct(), latitude).ToString(),
					" - ",
					twelfth.ToString(),
					" ",
					num2.ToString("F2")
				}));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-----Temperature for each day this year------");
			stringBuilder.AppendLine("Tile avg: " + this.map.TileInfo.temperature.ToString() + "°C");
			stringBuilder.AppendLine("Seasonal shift: " + GenTemperature.SeasonalShiftAmplitudeAt(this.map.Tile).ToString());
			stringBuilder.AppendLine("Equatorial distance: " + Find.WorldGrid.DistanceFromEquatorNormalized(this.map.Tile).ToString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Day  Lo   Hi   OffsetFromSeason RandomDailyVariation");
			for (int k = 0; k < 60; k++)
			{
				int absTick2 = (int)((float)(k * 60000) + 15000f);
				int absTick3 = (int)((float)(k * 60000) + 45000f);
				stringBuilder.Append(k.ToString().PadRight(8));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick2).ToString("F2").PadRight(11));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick3).ToString("F2").PadRight(11));
				stringBuilder.Append(GenTemperature.OffsetFromSeasonCycle(absTick3, this.map.Tile).ToString("F2").PadRight(11));
				stringBuilder.Append(Find.World.tileTemperatures.OffsetFromDailyRandomVariation(this.map.Tile, absTick3).ToString("F2"));
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x000927EC File Offset: 0x000909EC
		public void DebugLogTemperatureOverlayColors()
		{
			int num = -50;
			int num2 = 150;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("-----Temperature overlay colors by temperature------");
			for (int i = num; i <= num2; i++)
			{
				stringBuilder.AppendLine(i.ToString() + ": " + this.GetColorForTemperature((float)i).ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04001441 RID: 5185
		private static readonly List<ValueTuple<float, Color>> TemperatureColorMap = new List<ValueTuple<float, Color>>
		{
			new ValueTuple<float, Color>(-25f, ColorLibrary.DarkBlue),
			new ValueTuple<float, Color>(0f, ColorLibrary.Blue),
			new ValueTuple<float, Color>(25f, ColorLibrary.Green),
			new ValueTuple<float, Color>(50f, ColorLibrary.Yellow),
			new ValueTuple<float, Color>(100f, ColorLibrary.Red)
		};

		// Token: 0x04001442 RID: 5186
		private const int TemperatureOverlayUpdateInterval = 60;

		// Token: 0x04001443 RID: 5187
		private Map map;

		// Token: 0x04001444 RID: 5188
		private CellBoolDrawer drawerInt;
	}
}
