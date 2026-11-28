using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200031F RID: 799
	public class SkyManager
	{
		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001884 RID: 6276 RVA: 0x00093468 File Offset: 0x00091668
		public float CurSkyGlow
		{
			get
			{
				return this.curSkyGlowInt;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001885 RID: 6277 RVA: 0x00093470 File Offset: 0x00091670
		public SkyTarget CurSky
		{
			get
			{
				return this.curSky;
			}
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x00093478 File Offset: 0x00091678
		public SkyManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x000934A0 File Offset: 0x000916A0
		public void SkyManagerUpdate()
		{
			this.curSky = this.CurrentSkyTarget();
			this.curSkyGlowInt = this.curSky.glow;
			if (this.map == Find.CurrentMap)
			{
				if (this.map.Biome != null && this.map.Biome.disableSkyLighting)
				{
					MatBases.LightOverlay.color = new Color(1f, 1f, 1f, 0f);
					MatBases.FogOfWar.color = (this.map.FogOfWarColor ?? SkyManager.FogOfWarBaseColor);
				}
				else
				{
					MatBases.LightOverlay.color = this.curSky.colors.sky;
					Find.CameraColor.saturation = this.curSky.colors.saturation;
					Color color = this.curSky.colors.sky;
					color.a = 1f;
					color *= (this.map.FogOfWarColor ?? SkyManager.FogOfWarBaseColor);
					MatBases.FogOfWar.color = color;
				}
				Color color2 = this.curSky.colors.shadow;
				Vector3? overridenShadowVector = this.GetOverridenShadowVector();
				if (overridenShadowVector != null)
				{
					this.SetSunShadowVector(overridenShadowVector.Value);
				}
				else
				{
					this.SetSunShadowVector(GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.Shadow).vector);
					color2 = Color.Lerp(Color.white, color2, GenCelestial.CurShadowStrength(this.map));
				}
				GenCelestial.LightInfo lightSourceInfo = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.LightingSun);
				GenCelestial.LightInfo lightSourceInfo2 = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.LightingMoon);
				Shader.SetGlobalVector(ShaderPropertyIDs.WaterCastVectSun, new Vector4(lightSourceInfo.vector.x, 0f, lightSourceInfo.vector.y, lightSourceInfo.intensity));
				Shader.SetGlobalVector(ShaderPropertyIDs.WaterCastVectMoon, new Vector4(lightSourceInfo2.vector.x, 0f, lightSourceInfo2.vector.y, lightSourceInfo2.intensity));
				Shader.SetGlobalFloat(SkyManager.LightsourceShineSizeReduction, 20f * (1f / this.curSky.lightsourceShineSize));
				Shader.SetGlobalFloat(SkyManager.LightsourceShineIntensity, this.curSky.lightsourceShineIntensity);
				Shader.SetGlobalFloat(SkyManager.DayPercent, GenLocalDate.DayPercent(this.map));
				MatBases.SunShadow.color = color2;
				MatBases.SunShadowFade.color = color2;
				this.UpdateOverlays(this.curSky);
			}
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x00093724 File Offset: 0x00091924
		public void ForceSetCurSkyGlow(float curSkyGlow)
		{
			this.curSkyGlowInt = curSkyGlow;
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x00093730 File Offset: 0x00091930
		private void UpdateOverlays(SkyTarget curSky)
		{
			this.tempOverlays.Clear();
			List<SkyOverlay> overlays = this.map.weatherManager.curWeather.Worker.overlays;
			for (int i = 0; i < overlays.Count; i++)
			{
				this.AddTempOverlay(new Pair<SkyOverlay, float>(overlays[i], this.map.weatherManager.TransitionLerpFactor));
			}
			List<SkyOverlay> overlays2 = this.map.weatherManager.lastWeather.Worker.overlays;
			for (int j = 0; j < overlays2.Count; j++)
			{
				this.AddTempOverlay(new Pair<SkyOverlay, float>(overlays2[j], 1f - this.map.weatherManager.TransitionLerpFactor));
			}
			for (int k = 0; k < this.map.gameConditionManager.ActiveConditions.Count; k++)
			{
				GameCondition gameCondition = this.map.gameConditionManager.ActiveConditions[k];
				List<SkyOverlay> list = gameCondition.SkyOverlays(this.map);
				if (list != null)
				{
					for (int l = 0; l < list.Count; l++)
					{
						this.AddTempOverlay(new Pair<SkyOverlay, float>(list[l], gameCondition.SkyTargetLerpFactor(this.map)));
					}
				}
			}
			for (int m = 0; m < this.tempOverlays.Count; m++)
			{
				Color overlayColor;
				if (this.tempOverlays[m].First.ForcedOverlayColor != null)
				{
					overlayColor = this.tempOverlays[m].First.ForcedOverlayColor.Value;
				}
				else
				{
					overlayColor = curSky.colors.overlay;
				}
				overlayColor.a = this.tempOverlays[m].Second;
				this.tempOverlays[m].First.SetOverlayColor(overlayColor);
			}
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x0009392C File Offset: 0x00091B2C
		private void AddTempOverlay(Pair<SkyOverlay, float> pair)
		{
			for (int i = 0; i < this.tempOverlays.Count; i++)
			{
				if (this.tempOverlays[i].First == pair.First)
				{
					this.tempOverlays[i] = new Pair<SkyOverlay, float>(this.tempOverlays[i].First, Mathf.Clamp01(this.tempOverlays[i].Second + pair.Second));
					return;
				}
			}
			this.tempOverlays.Add(pair);
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x000939BF File Offset: 0x00091BBF
		private void SetSunShadowVector(Vector2 vec)
		{
			Shader.SetGlobalVector(ShaderPropertyIDs.MapSunLightDirection, new Vector4(vec.x, 0f, vec.y, GenCelestial.CurShadowStrength(this.map)));
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x000939EC File Offset: 0x00091BEC
		private SkyTarget CurrentSkyTarget()
		{
			SkyTarget b = this.map.weatherManager.curWeather.Worker.CurSkyTarget(this.map);
			SkyTarget skyTarget = SkyTarget.Lerp(this.map.weatherManager.lastWeather.Worker.CurSkyTarget(this.map), b, this.map.weatherManager.TransitionLerpFactor);
			this.map.gameConditionManager.GetAllGameConditionsAffectingMap(this.map, this.tempAllGameConditionsAffectingMap);
			for (int i = 0; i < this.tempAllGameConditionsAffectingMap.Count; i++)
			{
				SkyTarget? skyTarget2 = this.tempAllGameConditionsAffectingMap[i].SkyTarget(this.map);
				if (skyTarget2 != null)
				{
					skyTarget = SkyTarget.LerpDarken(skyTarget, skyTarget2.Value, this.tempAllGameConditionsAffectingMap[i].SkyTargetLerpFactor(this.map));
				}
			}
			this.tempAllGameConditionsAffectingMap.Clear();
			List<WeatherEvent> liveEventsListForReading = this.map.weatherManager.eventHandler.LiveEventsListForReading;
			for (int j = 0; j < liveEventsListForReading.Count; j++)
			{
				if (liveEventsListForReading[j].CurrentlyAffectsSky)
				{
					skyTarget = SkyTarget.Lerp(skyTarget, liveEventsListForReading[j].SkyTarget, liveEventsListForReading[j].SkyTargetLerpFactor);
				}
			}
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.AffectsSky);
			for (int k = 0; k < list.Count; k++)
			{
				CompAffectsSky compAffectsSky = list[k].TryGetComp<CompAffectsSky>();
				if (compAffectsSky.LerpFactor > 0f)
				{
					if (compAffectsSky.Props.lerpDarken)
					{
						skyTarget = SkyTarget.LerpDarken(skyTarget, compAffectsSky.SkyTarget, compAffectsSky.LerpFactor);
					}
					else
					{
						skyTarget = SkyTarget.Lerp(skyTarget, compAffectsSky.SkyTarget, compAffectsSky.LerpFactor);
					}
				}
			}
			return skyTarget;
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x00093BB8 File Offset: 0x00091DB8
		private Vector3? GetOverridenShadowVector()
		{
			List<WeatherEvent> liveEventsListForReading = this.map.weatherManager.eventHandler.LiveEventsListForReading;
			int i = 0;
			while (i < liveEventsListForReading.Count)
			{
				Vector2? overrideShadowVector = liveEventsListForReading[i].OverrideShadowVector;
				if (overrideShadowVector != null)
				{
					Vector2? vector = overrideShadowVector;
					if (vector == null)
					{
						return null;
					}
					return new Vector3?(vector.GetValueOrDefault());
				}
				else
				{
					i++;
				}
			}
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.AffectsSky);
			int j = 0;
			while (j < list.Count)
			{
				Vector2? overrideShadowVector2 = list[j].TryGetComp<CompAffectsSky>().OverrideShadowVector;
				if (overrideShadowVector2 != null)
				{
					Vector2? vector = overrideShadowVector2;
					if (vector == null)
					{
						return null;
					}
					return new Vector3?(vector.GetValueOrDefault());
				}
				else
				{
					j++;
				}
			}
			return null;
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x00093CA4 File Offset: 0x00091EA4
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("SkyManager: ");
			stringBuilder.AppendLine("CurCelestialSunGlow: " + GenCelestial.CurCelestialSunGlow(Find.CurrentMap).ToString());
			stringBuilder.AppendLine("CurSkyGlow: " + this.CurSkyGlow.ToStringPercent());
			stringBuilder.AppendLine("CurrentSkyTarget: " + this.CurrentSkyTarget().ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x04001469 RID: 5225
		private readonly Map map;

		// Token: 0x0400146A RID: 5226
		private float curSkyGlowInt;

		// Token: 0x0400146B RID: 5227
		private SkyTarget curSky;

		// Token: 0x0400146C RID: 5228
		private readonly List<Pair<SkyOverlay, float>> tempOverlays = new List<Pair<SkyOverlay, float>>();

		// Token: 0x0400146D RID: 5229
		public const float NightMaxCelGlow = 0.1f;

		// Token: 0x0400146E RID: 5230
		public const float DuskMaxCelGlow = 0.6f;

		// Token: 0x0400146F RID: 5231
		private static readonly Color FogOfWarBaseColor = new Color32(77, 69, 66, byte.MaxValue);

		// Token: 0x04001470 RID: 5232
		private readonly List<GameCondition> tempAllGameConditionsAffectingMap = new List<GameCondition>();

		// Token: 0x04001471 RID: 5233
		private static readonly int LightsourceShineIntensity = Shader.PropertyToID("_LightsourceShineIntensity");

		// Token: 0x04001472 RID: 5234
		private static readonly int LightsourceShineSizeReduction = Shader.PropertyToID("_LightsourceShineSizeReduction");

		// Token: 0x04001473 RID: 5235
		private static readonly int DayPercent = Shader.PropertyToID("_DayPercent");
	}
}
