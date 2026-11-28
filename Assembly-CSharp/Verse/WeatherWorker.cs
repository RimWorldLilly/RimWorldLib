using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000329 RID: 809
	public class WeatherWorker
	{
		// Token: 0x060018C9 RID: 6345 RVA: 0x00094934 File Offset: 0x00092B34
		public WeatherWorker(WeatherDef def)
		{
			this.def = def;
			foreach (Type genericParam in def.overlayClasses)
			{
				SkyOverlay item = (SkyOverlay)GenGeneric.InvokeStaticGenericMethod(typeof(WeatherPartPool), genericParam, "GetInstanceOf");
				this.overlays.Add(item);
			}
			this.skyTargets[0] = new WeatherWorker.SkyThreshold(def.skyColorsNightMid, 0f);
			this.skyTargets[1] = new WeatherWorker.SkyThreshold(def.skyColorsNightEdge, 0.1f);
			this.skyTargets[2] = new WeatherWorker.SkyThreshold(def.skyColorsDusk, 0.6f);
			this.skyTargets[3] = new WeatherWorker.SkyThreshold(def.skyColorsDay, 1f);
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x00094A3C File Offset: 0x00092C3C
		public virtual void OnWeatherStart(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].Reset();
			}
			if (this.def.letterDef == null)
			{
				return;
			}
			ChoiceLetter let = LetterMaker.MakeLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef, null, null);
			Find.LetterStack.ReceiveLetter(let, null, 0, true);
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x00094AC0 File Offset: 0x00092CC0
		public virtual void OnWeatherEnd(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].Reset();
			}
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x00094AF4 File Offset: 0x00092CF4
		public void DrawWeather(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x00094B2C File Offset: 0x00092D2C
		public void WeatherTick(Map map, float lerpFactor)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].TickOverlay(map, lerpFactor);
			}
			for (int j = 0; j < this.def.eventMakers.Count; j++)
			{
				this.def.eventMakers[j].WeatherEventMakerTick(map, lerpFactor);
			}
			if (this.def.doToxicBuildup && Find.TickManager.TicksGame % 3451 == 0 && Mathf.Approximately(lerpFactor, 1f))
			{
				IReadOnlyList<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
				for (int k = 0; k < allPawnsSpawned.Count; k++)
				{
					if (!allPawnsSpawned[k].kindDef.immuneToGameConditionEffects)
					{
						ToxicUtility.DoAirbornePawnToxicDamage(allPawnsSpawned[k], 1f);
					}
				}
			}
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x00094C04 File Offset: 0x00092E04
		public SkyTarget CurSkyTarget(Map map)
		{
			float num = GenCelestial.CurCelestialSunGlow(map);
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < this.skyTargets.Length; i++)
			{
				num3 = i;
				if (num + 0.001f < this.skyTargets[i].celGlowThreshold)
				{
					break;
				}
				num2 = i;
			}
			WeatherWorker.SkyThreshold skyThreshold = this.skyTargets[num2];
			WeatherWorker.SkyThreshold skyThreshold2 = this.skyTargets[num3];
			float num4 = skyThreshold2.celGlowThreshold - skyThreshold.celGlowThreshold;
			float num5;
			if (num4 == 0f)
			{
				num5 = 1f;
			}
			else
			{
				num5 = (num - skyThreshold.celGlowThreshold) / num4;
			}
			if (Find.WorldGrid.LongLatOf(map.Tile).y >= 75f)
			{
				num5 = WeatherWorker.ClampLerpDelta(map, num5);
			}
			SkyTarget result = default(SkyTarget);
			result.glow = Math.Min(num, this.def.maxGlow);
			result.colors = SkyColorSet.Lerp(skyThreshold.colors, skyThreshold2.colors, num5);
			if (GenCelestial.IsDaytime(num))
			{
				result.lightsourceShineIntensity = 1f;
				result.lightsourceShineSize = 1f;
			}
			else
			{
				result.lightsourceShineIntensity = 0.7f;
				result.lightsourceShineSize = 0.5f;
			}
			return result;
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x00094D3C File Offset: 0x00092F3C
		private static float ClampLerpDelta(Map map, float lerpVal)
		{
			WeatherManager weatherManager = map.weatherManager;
			if (Mathf.Approximately(weatherManager.prevSkyTargetLerp, weatherManager.currSkyTargetLerp) && Mathf.Approximately(weatherManager.currSkyTargetLerp, -1f))
			{
				weatherManager.prevSkyTargetLerp = (weatherManager.currSkyTargetLerp = lerpVal);
			}
			else
			{
				weatherManager.prevSkyTargetLerp = weatherManager.currSkyTargetLerp;
				weatherManager.currSkyTargetLerp = lerpVal;
				float num = Mathf.Abs(weatherManager.prevSkyTargetLerp - weatherManager.currSkyTargetLerp);
				if (num > 0.1f && num < 0.6f && !Mathf.Approximately(weatherManager.prevSkyTargetLerp, -1f))
				{
					lerpVal = ((weatherManager.prevSkyTargetLerp > weatherManager.currSkyTargetLerp) ? (weatherManager.prevSkyTargetLerp - 0.002f) : (weatherManager.prevSkyTargetLerp + 0.002f));
					weatherManager.currSkyTargetLerp = lerpVal;
				}
			}
			return lerpVal;
		}

		// Token: 0x04001495 RID: 5269
		private WeatherDef def;

		// Token: 0x04001496 RID: 5270
		public List<SkyOverlay> overlays = new List<SkyOverlay>();

		// Token: 0x04001497 RID: 5271
		private WeatherWorker.SkyThreshold[] skyTargets = new WeatherWorker.SkyThreshold[4];

		// Token: 0x04001498 RID: 5272
		private const float LerpDeltaMin = 0.1f;

		// Token: 0x04001499 RID: 5273
		private const float LerpDeltaMax = 0.6f;

		// Token: 0x0400149A RID: 5274
		private const float LerpStep = 0.002f;

		// Token: 0x02002657 RID: 9815
		private struct SkyThreshold
		{
			// Token: 0x0600F327 RID: 62247 RVA: 0x0058FC33 File Offset: 0x0058DE33
			public SkyThreshold(SkyColorSet colors, float celGlowThreshold)
			{
				this.colors = colors;
				this.celGlowThreshold = celGlowThreshold;
			}

			// Token: 0x04009D15 RID: 40213
			public SkyColorSet colors;

			// Token: 0x04009D16 RID: 40214
			public float celGlowThreshold;
		}
	}
}
