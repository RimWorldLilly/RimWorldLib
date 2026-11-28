using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013A9 RID: 5033
	public sealed class WeatherManager : IExposable
	{
		// Token: 0x17001549 RID: 5449
		// (get) Token: 0x06007CDD RID: 31965 RVA: 0x002C9E1C File Offset: 0x002C801C
		public float TransitionLerpFactor
		{
			get
			{
				float num = (float)this.curWeatherAge / 4000f;
				if (num > 1f)
				{
					num = 1f;
				}
				return num;
			}
		}

		// Token: 0x1700154A RID: 5450
		// (get) Token: 0x06007CDE RID: 31966 RVA: 0x002C9E46 File Offset: 0x002C8046
		public float RainRate
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.rainRate, this.curWeather.rainRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x1700154B RID: 5451
		// (get) Token: 0x06007CDF RID: 31967 RVA: 0x002C9E69 File Offset: 0x002C8069
		public float SnowRate
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.snowRate, this.curWeather.snowRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x1700154C RID: 5452
		// (get) Token: 0x06007CE0 RID: 31968 RVA: 0x002C9E8C File Offset: 0x002C808C
		public float SandRate
		{
			get
			{
				if (!ModsConfig.OdysseyActive)
				{
					return 0f;
				}
				return Mathf.Lerp(this.lastWeather.sandRate, this.curWeather.sandRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x1700154D RID: 5453
		// (get) Token: 0x06007CE1 RID: 31969 RVA: 0x002C9EBC File Offset: 0x002C80BC
		public float CurWindSpeedFactor
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.windSpeedFactor, this.curWeather.windSpeedFactor, this.TransitionLerpFactor);
			}
		}

		// Token: 0x1700154E RID: 5454
		// (get) Token: 0x06007CE2 RID: 31970 RVA: 0x002C9EDF File Offset: 0x002C80DF
		public float CurWindSpeedOffset
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.windSpeedOffset, this.curWeather.windSpeedOffset, this.TransitionLerpFactor);
			}
		}

		// Token: 0x1700154F RID: 5455
		// (get) Token: 0x06007CE3 RID: 31971 RVA: 0x002C9F02 File Offset: 0x002C8102
		public float CurMoveSpeedMultiplier
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.moveSpeedMultiplier, this.curWeather.moveSpeedMultiplier, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17001550 RID: 5456
		// (get) Token: 0x06007CE4 RID: 31972 RVA: 0x002C9F25 File Offset: 0x002C8125
		public float CurWeatherAccuracyMultiplier
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.accuracyMultiplier, this.curWeather.accuracyMultiplier, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17001551 RID: 5457
		// (get) Token: 0x06007CE5 RID: 31973 RVA: 0x002C9F48 File Offset: 0x002C8148
		public float CurWeatherMaxRangeCap
		{
			get
			{
				return this.curWeather.maxRangeCap;
			}
		}

		// Token: 0x17001552 RID: 5458
		// (get) Token: 0x06007CE6 RID: 31974 RVA: 0x002C9F58 File Offset: 0x002C8158
		public WeatherDef CurWeatherPerceived
		{
			get
			{
				if (this.curWeather == null)
				{
					return this.lastWeather;
				}
				if (this.lastWeather == null)
				{
					return this.curWeather;
				}
				float num;
				if (this.curWeather.perceivePriority > this.lastWeather.perceivePriority)
				{
					num = 0.18f;
				}
				else if (this.lastWeather.perceivePriority > this.curWeather.perceivePriority)
				{
					num = 0.82f;
				}
				else
				{
					num = 0.5f;
				}
				if (this.TransitionLerpFactor >= num)
				{
					return this.curWeather;
				}
				return this.lastWeather;
			}
		}

		// Token: 0x17001553 RID: 5459
		// (get) Token: 0x06007CE7 RID: 31975 RVA: 0x002C9FDF File Offset: 0x002C81DF
		public WeatherDef CurWeatherLerped
		{
			get
			{
				if (this.curWeather == null)
				{
					return this.lastWeather;
				}
				if (this.lastWeather == null)
				{
					return this.curWeather;
				}
				if (this.TransitionLerpFactor >= 0.5f)
				{
					return this.curWeather;
				}
				return this.lastWeather;
			}
		}

		// Token: 0x06007CE8 RID: 31976 RVA: 0x002CA01C File Offset: 0x002C821C
		public WeatherManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06007CE9 RID: 31977 RVA: 0x002CA078 File Offset: 0x002C8278
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.curWeatherAge, "curWeatherAge", 0, true);
			Scribe_Values.Look<float>(ref this.prevSkyTargetLerp, "prevSkyTargetLerp", -1f, false);
			Scribe_Values.Look<float>(ref this.currSkyTargetLerp, "currSkyTargetLerp", -1f, false);
			Scribe_Defs.Look<WeatherDef>(ref this.curWeather, "curWeather");
			Scribe_Defs.Look<WeatherDef>(ref this.lastWeather, "lastWeather");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.ambienceSustainers.Clear();
			}
		}

		// Token: 0x06007CEA RID: 31978 RVA: 0x002CA0F8 File Offset: 0x002C82F8
		public void TransitionTo(WeatherDef newWeather)
		{
			this.lastWeather = this.curWeather;
			this.curWeather = newWeather;
			this.lastWeather.Worker.OnWeatherEnd(this.map);
			this.ResetSkyTargetLerpCache();
			this.curWeatherAge = 0;
			if (this.lastWeather != newWeather)
			{
				newWeather.Worker.OnWeatherStart(this.map);
			}
		}

		// Token: 0x06007CEB RID: 31979 RVA: 0x002CA158 File Offset: 0x002C8358
		public void DoWeatherGUI(Rect rect)
		{
			WeatherDef curWeatherPerceived = this.CurWeatherPerceived;
			Text.Anchor = TextAnchor.MiddleRight;
			Rect rect2 = new Rect(rect);
			rect2.width -= 15f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, curWeatherPerceived.LabelCap);
			if (!curWeatherPerceived.description.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, curWeatherPerceived.description);
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06007CEC RID: 31980 RVA: 0x002CA1C4 File Offset: 0x002C83C4
		public void WeatherManagerTick()
		{
			this.eventHandler.WeatherEventHandlerTick();
			this.curWeatherAge++;
			this.curWeather.Worker.WeatherTick(this.map, this.TransitionLerpFactor);
			if (this.TransitionLerpFactor < 1f || !this.tickedLastWeather)
			{
				this.lastWeather.Worker.WeatherTick(this.map, 1f - this.TransitionLerpFactor);
				this.tickedLastWeather = true;
			}
			this.AmbientSoundsTick();
		}

		// Token: 0x06007CED RID: 31981 RVA: 0x002CA24C File Offset: 0x002C844C
		private void AmbientSoundsTick()
		{
			foreach (SoundDef soundDef in this.curWeather.ambientSounds)
			{
				if (!this.IsAmbienceSoundPlaying(soundDef) && this.VolumeOfAmbientSound(soundDef) > 0.0001f)
				{
					SoundInfo info = SoundInfo.OnCamera(MaintenanceType.None);
					Sustainer sustainer = soundDef.TrySpawnSustainer(info);
					if (sustainer != null)
					{
						this.ambienceSustainers[sustainer.def] = sustainer;
					}
				}
			}
		}

		// Token: 0x06007CEE RID: 31982 RVA: 0x002CA2D8 File Offset: 0x002C84D8
		private bool IsAmbienceSoundPlaying(SoundDef sound)
		{
			return this.ambienceSustainers.ContainsKey(sound);
		}

		// Token: 0x06007CEF RID: 31983 RVA: 0x002CA2E6 File Offset: 0x002C84E6
		public void WeatherManagerUpdate()
		{
			this.SetAmbienceSustainersVolume();
		}

		// Token: 0x06007CF0 RID: 31984 RVA: 0x002CA2F0 File Offset: 0x002C84F0
		public void EndAllSustainers()
		{
			foreach (KeyValuePair<SoundDef, Sustainer> keyValuePair in this.ambienceSustainers)
			{
				SoundDef soundDef;
				Sustainer sustainer;
				keyValuePair.Deconstruct(out soundDef, out sustainer);
				sustainer.End();
			}
			this.ambienceSustainers.Clear();
		}

		// Token: 0x06007CF1 RID: 31985 RVA: 0x002CA358 File Offset: 0x002C8558
		public void ResetSkyTargetLerpCache()
		{
			this.prevSkyTargetLerp = -1f;
			this.currSkyTargetLerp = -1f;
		}

		// Token: 0x06007CF2 RID: 31986 RVA: 0x002CA370 File Offset: 0x002C8570
		private void SetAmbienceSustainersVolume()
		{
			WeatherManager.TmpToRemove.Clear();
			foreach (KeyValuePair<SoundDef, Sustainer> keyValuePair in this.ambienceSustainers)
			{
				SoundDef soundDef;
				Sustainer sustainer;
				keyValuePair.Deconstruct(out soundDef, out sustainer);
				SoundDef soundDef2 = soundDef;
				Sustainer sustainer2 = sustainer;
				float num = this.VolumeOfAmbientSound(soundDef2);
				if (num > 0.0001f)
				{
					sustainer2.externalParams["LerpFactor"] = num;
				}
				else
				{
					sustainer2.End();
					WeatherManager.TmpToRemove.Add(soundDef2);
				}
			}
			foreach (SoundDef key in WeatherManager.TmpToRemove)
			{
				this.ambienceSustainers.Remove(key);
			}
			WeatherManager.TmpToRemove.Clear();
		}

		// Token: 0x06007CF3 RID: 31987 RVA: 0x002CA464 File Offset: 0x002C8664
		private float VolumeOfAmbientSound(SoundDef soundDef)
		{
			if (this.map != Find.CurrentMap)
			{
				return 0f;
			}
			for (int i = 0; i < Find.WindowStack.Count; i++)
			{
				if (Find.WindowStack[i].silenceAmbientSound)
				{
					return 0f;
				}
			}
			float num = 0f;
			using (List<SoundDef>.Enumerator enumerator = this.lastWeather.ambientSounds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == soundDef)
					{
						num += 1f - this.TransitionLerpFactor;
					}
				}
			}
			using (List<SoundDef>.Enumerator enumerator = this.curWeather.ambientSounds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == soundDef)
					{
						num += this.TransitionLerpFactor;
					}
				}
			}
			return num;
		}

		// Token: 0x06007CF4 RID: 31988 RVA: 0x002CA558 File Offset: 0x002C8758
		public void DrawAllWeather()
		{
			this.eventHandler.WeatherEventsDraw();
			this.lastWeather.Worker.DrawWeather(this.map);
			this.curWeather.Worker.DrawWeather(this.map);
		}

		// Token: 0x04004E9D RID: 20125
		public Map map;

		// Token: 0x04004E9E RID: 20126
		public readonly WeatherEventHandler eventHandler = new WeatherEventHandler();

		// Token: 0x04004E9F RID: 20127
		public WeatherDef curWeather = WeatherDefOf.Clear;

		// Token: 0x04004EA0 RID: 20128
		public WeatherDef lastWeather = WeatherDefOf.Clear;

		// Token: 0x04004EA1 RID: 20129
		public float prevSkyTargetLerp = -1f;

		// Token: 0x04004EA2 RID: 20130
		public float currSkyTargetLerp = -1f;

		// Token: 0x04004EA3 RID: 20131
		public int curWeatherAge;

		// Token: 0x04004EA4 RID: 20132
		private readonly Dictionary<SoundDef, Sustainer> ambienceSustainers = new Dictionary<SoundDef, Sustainer>();

		// Token: 0x04004EA5 RID: 20133
		public const float TransitionTicks = 4000f;

		// Token: 0x04004EA6 RID: 20134
		private bool tickedLastWeather;

		// Token: 0x04004EA7 RID: 20135
		private static readonly HashSet<SoundDef> TmpToRemove = new HashSet<SoundDef>();
	}
}
