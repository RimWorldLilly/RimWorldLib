using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A5 RID: 5029
	public class WeatherDecider : IExposable
	{
		// Token: 0x1700153E RID: 5438
		// (get) Token: 0x06007CB7 RID: 31927 RVA: 0x002C92A0 File Offset: 0x002C74A0
		public WeatherDef ForcedWeather
		{
			get
			{
				WeatherDecider.allConditionsTmp.Clear();
				this.map.gameConditionManager.GetAllGameConditionsAffectingMap(this.map, WeatherDecider.allConditionsTmp);
				WeatherDef result = null;
				foreach (GameCondition gameCondition in WeatherDecider.allConditionsTmp)
				{
					WeatherDef weatherDef = gameCondition.ForcedWeather();
					if (weatherDef != null)
					{
						result = weatherDef;
					}
				}
				return result;
			}
		}

		// Token: 0x1700153F RID: 5439
		// (get) Token: 0x06007CB8 RID: 31928 RVA: 0x002C9320 File Offset: 0x002C7520
		public IEnumerable<WeatherCommonalityRecord> WeatherCommonalities
		{
			get
			{
				return this.map.Biome.baseWeatherCommonalities;
			}
		}

		// Token: 0x17001540 RID: 5440
		// (get) Token: 0x06007CB9 RID: 31929 RVA: 0x002C9332 File Offset: 0x002C7532
		public bool ClearWeatherAllowed
		{
			get
			{
				if (!this.WeatherCommonalities.EnumerableNullOrEmpty<WeatherCommonalityRecord>())
				{
					return this.WeatherCommonalities.Any((WeatherCommonalityRecord w) => w.weather == WeatherDefOf.Clear);
				}
				return false;
			}
		}

		// Token: 0x06007CBA RID: 31930 RVA: 0x002C936D File Offset: 0x002C756D
		public WeatherDecider(Map map)
		{
			this.map = map;
		}

		// Token: 0x06007CBB RID: 31931 RVA: 0x002C9387 File Offset: 0x002C7587
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.curWeatherDuration, "curWeatherDuration", 0, true);
			Scribe_Values.Look<int>(ref this.ticksWhenRainAllowedAgain, "ticksWhenRainAllowedAgain", 0, false);
		}

		// Token: 0x06007CBC RID: 31932 RVA: 0x002C93B0 File Offset: 0x002C75B0
		public void WeatherDeciderTick()
		{
			WeatherDef forcedWeather = this.ForcedWeather;
			int num = this.curWeatherDuration;
			if (this.map.fireWatcher.LargeFireDangerPresent || !this.map.weatherManager.curWeather.temperatureRange.Includes(this.map.mapTemperature.OutdoorTemp) || (ModsConfig.AnomalyActive && this.map.gameConditionManager.BrightnessChanging))
			{
				num = (int)((float)num * 0.25f);
			}
			if (forcedWeather != null && this.map.weatherManager.curWeather != forcedWeather)
			{
				if (ModsConfig.AnomalyActive && forcedWeather.transitionTicksOverride != 2147483647)
				{
					num = forcedWeather.transitionTicksOverride;
				}
				else
				{
					num = 4000;
				}
			}
			if (this.map.weatherManager.curWeatherAge > num)
			{
				this.StartNextWeather();
			}
		}

		// Token: 0x06007CBD RID: 31933 RVA: 0x002C9484 File Offset: 0x002C7684
		public void StartNextWeather()
		{
			WeatherDef weatherDef = this.ChooseNextWeather();
			this.map.weatherManager.TransitionTo(weatherDef);
			this.curWeatherDuration = weatherDef.durationRange.RandomInRange;
		}

		// Token: 0x06007CBE RID: 31934 RVA: 0x002C94BC File Offset: 0x002C76BC
		public void StartInitialWeather()
		{
			if (Find.GameInitData != null)
			{
				if (this.ClearWeatherAllowed)
				{
					this.map.weatherManager.curWeather = WeatherDefOf.Clear;
				}
				else
				{
					this.map.weatherManager.curWeather = this.WeatherCommonalities.RandomElement<WeatherCommonalityRecord>().weather;
				}
				this.curWeatherDuration = 10000;
				this.map.weatherManager.lastWeather = this.map.weatherManager.curWeather;
				this.map.weatherManager.curWeatherAge = 0;
			}
			else
			{
				this.map.weatherManager.curWeather = null;
				WeatherDef weatherDef = this.ChooseNextWeather();
				WeatherDef lastWeather = this.ChooseNextWeather();
				this.map.weatherManager.curWeather = weatherDef;
				this.map.weatherManager.lastWeather = lastWeather;
				this.curWeatherDuration = weatherDef.durationRange.RandomInRange;
				this.map.weatherManager.curWeatherAge = Rand.Range(0, this.curWeatherDuration);
			}
			this.map.weatherManager.ResetSkyTargetLerpCache();
		}

		// Token: 0x06007CBF RID: 31935 RVA: 0x002C95CC File Offset: 0x002C77CC
		private WeatherDef ChooseNextWeather()
		{
			if (TutorSystem.TutorialMode && this.ClearWeatherAllowed)
			{
				return WeatherDefOf.Clear;
			}
			WeatherDef forcedWeather = this.ForcedWeather;
			if (forcedWeather != null)
			{
				return forcedWeather;
			}
			WeatherDef result;
			if (DefDatabase<WeatherDef>.AllDefs.TryRandomElementByWeight(new Func<WeatherDef, float>(this.CurrentWeatherCommonality), out result))
			{
				return result;
			}
			Log.Warning("All weather commonalities were zero. Defaulting to " + WeatherDefOf.Clear.defName + ".");
			if (!this.WeatherCommonalities.EnumerableNullOrEmpty<WeatherCommonalityRecord>())
			{
				return this.WeatherCommonalities.RandomElement<WeatherCommonalityRecord>().weather;
			}
			return WeatherDefOf.Clear;
		}

		// Token: 0x06007CC0 RID: 31936 RVA: 0x002C9657 File Offset: 0x002C7857
		public void DisableRainFor(int ticks)
		{
			this.ticksWhenRainAllowedAgain = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x06007CC1 RID: 31937 RVA: 0x002C966C File Offset: 0x002C786C
		private float CurrentWeatherCommonality(WeatherDef weather)
		{
			if (this.map.weatherManager.curWeather != null && !this.map.weatherManager.curWeather.repeatable && weather == this.map.weatherManager.curWeather)
			{
				return 0f;
			}
			if (!weather.temperatureRange.Includes(this.map.mapTemperature.OutdoorTemp))
			{
				return 0f;
			}
			if (weather.favorability < Favorability.Neutral && GenDate.DaysPassedSinceSettle < 8)
			{
				return 0f;
			}
			if (weather.rainRate > 0.1f && Find.TickManager.TicksGame < this.ticksWhenRainAllowedAgain)
			{
				return 0f;
			}
			if (weather.rainRate > 0.1f)
			{
				if (this.map.gameConditionManager.ActiveConditions.Any((GameCondition x) => x.def.preventRain))
				{
					return 0f;
				}
			}
			if (ModsConfig.AnomalyActive && weather.minMonolithLevel > Find.Anomaly.HighestLevelReached && (!weather.canOccurInAmbientHorror || !Find.Anomaly.AmbientHorrorMode))
			{
				return 0f;
			}
			BiomeDef biome = this.map.Biome;
			float result = 0f;
			for (int i = 0; i < biome.baseWeatherCommonalities.Count; i++)
			{
				WeatherCommonalityRecord weatherCommonalityRecord = biome.baseWeatherCommonalities[i];
				if (weatherCommonalityRecord.weather == weather)
				{
					float num = weatherCommonalityRecord.commonality;
					if (this.map.fireWatcher.LargeFireDangerPresent && weather.rainRate > 0.1f)
					{
						num *= 15f;
					}
					if (weatherCommonalityRecord.weather.commonalityRainfallFactor != null)
					{
						num *= weatherCommonalityRecord.weather.commonalityRainfallFactor.Evaluate(this.map.TileInfo.rainfall);
					}
					foreach (GameCondition gameCondition in this.map.gameConditionManager.ActiveConditions)
					{
						num *= gameCondition.WeatherCommonalityFactor(weather, this.map);
					}
					result = num;
					break;
				}
			}
			PlanetTile tile = this.map.Tile;
			for (int j = 0; j < this.map.TileInfo.Mutators.Count; j++)
			{
				TileMutatorWorker worker = this.map.TileInfo.Mutators[j].Worker;
				if (worker != null)
				{
					worker.MutateWeatherCommonalityFor(weather, tile, ref result);
				}
			}
			return result;
		}

		// Token: 0x06007CC2 RID: 31938 RVA: 0x002C9904 File Offset: 0x002C7B04
		public void LogWeatherChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (WeatherDef weatherDef in from w in DefDatabase<WeatherDef>.AllDefs
			orderby this.CurrentWeatherCommonality(w) descending
			select w)
			{
				stringBuilder.AppendLine(weatherDef.label + " - " + this.CurrentWeatherCommonality(weatherDef).ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04004E88 RID: 20104
		private Map map;

		// Token: 0x04004E89 RID: 20105
		private int curWeatherDuration = 10000;

		// Token: 0x04004E8A RID: 20106
		private int ticksWhenRainAllowedAgain;

		// Token: 0x04004E8B RID: 20107
		private const int FirstWeatherDuration = 10000;

		// Token: 0x04004E8C RID: 20108
		private const float ChanceFactorRainOnFire = 15f;

		// Token: 0x04004E8D RID: 20109
		private static List<GameCondition> allConditionsTmp = new List<GameCondition>();
	}
}
