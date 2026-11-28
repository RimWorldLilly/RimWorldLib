using System;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E55 RID: 7765
	public static class GenCelestial
	{
		// Token: 0x17002513 RID: 9491
		// (get) Token: 0x0600C980 RID: 51584 RVA: 0x004A32F4 File Offset: 0x004A14F4
		private static int TicksAbsForSunPosInWorldSpace
		{
			get
			{
				if (Current.ProgramState != ProgramState.Entry)
				{
					return GenTicks.TicksAbs;
				}
				PlanetTile startingTile = Find.GameInitData.startingTile;
				float longitude = startingTile.Valid ? Find.WorldGrid.LongLatOf(startingTile).x : 0f;
				return Mathf.RoundToInt(2500f * (12f - GenDate.TimeZoneFloatAt(longitude)));
			}
		}

		// Token: 0x0600C981 RID: 51585 RVA: 0x004A3351 File Offset: 0x004A1551
		public static float CurCelestialSunGlow(Map map)
		{
			return GenCelestial.CelestialSunGlow(map, Find.TickManager.TicksAbs);
		}

		// Token: 0x0600C982 RID: 51586 RVA: 0x004A3363 File Offset: 0x004A1563
		public static float CelestialSunGlow(Map map, int ticksAbs)
		{
			return GenCelestial.CelestialSunGlow(map.Tile, ticksAbs);
		}

		// Token: 0x0600C983 RID: 51587 RVA: 0x004A3374 File Offset: 0x004A1574
		public static float CelestialSunGlow(PlanetTile tile, int ticksAbs)
		{
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			return GenCelestial.CelestialSunGlowPercent(vector.y, GenDate.DayOfYear((long)ticksAbs, vector.x), GenDate.DayPercent((long)ticksAbs, vector.x));
		}

		// Token: 0x0600C984 RID: 51588 RVA: 0x004A33B2 File Offset: 0x004A15B2
		public static float CurShadowStrength(Map map)
		{
			return Mathf.Clamp01(Mathf.Abs(GenCelestial.CurCelestialSunGlow(map) - 0.6f) / 0.15f);
		}

		// Token: 0x0600C985 RID: 51589 RVA: 0x004A33D0 File Offset: 0x004A15D0
		public static GenCelestial.LightInfo GetLightSourceInfo(Map map, GenCelestial.LightType type)
		{
			float num = GenLocalDate.DayPercent(map);
			bool flag;
			float intensity;
			if (type == GenCelestial.LightType.Shadow)
			{
				flag = GenCelestial.IsDaytime(GenCelestial.CurCelestialSunGlow(map));
				intensity = GenCelestial.CurShadowStrength(map);
			}
			else if (type == GenCelestial.LightType.LightingSun)
			{
				flag = true;
				intensity = Mathf.Clamp01((GenCelestial.CurCelestialSunGlow(map) - 0.6f + 0.2f) / 0.15f);
			}
			else if (type == GenCelestial.LightType.LightingMoon)
			{
				flag = false;
				intensity = Mathf.Clamp01(-(GenCelestial.CurCelestialSunGlow(map) - 0.6f - 0.2f) / 0.15f);
			}
			else
			{
				Log.ErrorOnce("Invalid light type requested", 64275614);
				flag = true;
				intensity = 0f;
			}
			float t;
			float num2;
			float num3;
			if (flag)
			{
				t = num;
				num2 = -1.5f;
				num3 = 15f;
			}
			else
			{
				if (num > 0.5f)
				{
					t = Mathf.InverseLerp(0.5f, 1f, num) * 0.5f;
				}
				else
				{
					t = 0.5f + Mathf.InverseLerp(0f, 0.5f, num) * 0.5f;
				}
				num2 = -0.9f;
				num3 = 15f;
			}
			float num4 = Mathf.LerpUnclamped(-num3, num3, t);
			float y = num2 - 2.5f * (num4 * num4 / 100f);
			return new GenCelestial.LightInfo
			{
				vector = new Vector2(num4, y),
				intensity = intensity
			};
		}

		// Token: 0x0600C986 RID: 51590 RVA: 0x004A3508 File Offset: 0x004A1708
		public static Vector3 CurSunPositionInWorldSpace()
		{
			int ticksAbsForSunPosInWorldSpace = GenCelestial.TicksAbsForSunPosInWorldSpace;
			return GenCelestial.SunPositionUnmodified((float)GenDate.DayOfYear((long)ticksAbsForSunPosInWorldSpace, 0f), GenDate.DayPercent((long)ticksAbsForSunPosInWorldSpace, 0f), new Vector3(0f, 0f, -1f), 0f);
		}

		// Token: 0x0600C987 RID: 51591 RVA: 0x004A3552 File Offset: 0x004A1752
		public static bool IsDaytime(float glow)
		{
			return glow > 0.6f;
		}

		// Token: 0x0600C988 RID: 51592 RVA: 0x004A355C File Offset: 0x004A175C
		private static Vector3 SunPosition(float latitude, int dayOfYear, float dayPercent)
		{
			Vector3 target = GenCelestial.SurfaceNormal(latitude);
			Vector3 current = GenCelestial.SunPositionUnmodified((float)dayOfYear, dayPercent, new Vector3(1f, 0f, 0f), latitude);
			float num = GenCelestial.SunPeekAroundDegreesFactorCurve.Evaluate(latitude);
			current = Vector3.RotateTowards(current, target, 0.33161256f * num, 9999999f);
			float num2 = Mathf.InverseLerp(60f, 0f, Mathf.Abs(latitude));
			if (num2 > 0f)
			{
				current = Vector3.RotateTowards(current, target, 6.2831855f * (17f * num2 / 360f), 9999999f);
			}
			return current.normalized;
		}

		// Token: 0x0600C989 RID: 51593 RVA: 0x004A35F4 File Offset: 0x004A17F4
		private static Vector3 SunPositionUnmodified(float dayOfYear, float dayPercent, Vector3 initialSunPos, float latitude = 0f)
		{
			Vector3 point = initialSunPos * 100f;
			float num = -Mathf.Cos(dayOfYear / 60f * 3.1415927f * 2f);
			point.y += num * 100f * GenCelestial.SunOffsetFractionFromLatitudeCurve.Evaluate(latitude);
			point = Quaternion.AngleAxis((dayPercent - 0.5f) * 360f, Vector3.up) * point;
			return point.normalized;
		}

		// Token: 0x0600C98A RID: 51594 RVA: 0x004A366C File Offset: 0x004A186C
		private static float CelestialSunGlowPercent(float latitude, int dayOfYear, float dayPercent)
		{
			Vector3 vector = GenCelestial.SurfaceNormal(latitude);
			Vector3 rhs = GenCelestial.SunPosition(latitude, dayOfYear, dayPercent);
			float value = Vector3.Dot(vector.normalized, rhs);
			return Mathf.Clamp01(Mathf.InverseLerp(0f, 0.7f, value));
		}

		// Token: 0x0600C98B RID: 51595 RVA: 0x004A36AC File Offset: 0x004A18AC
		public static float AverageGlow(float latitude, int dayOfYear)
		{
			float num = 0f;
			for (int i = 0; i < 24; i++)
			{
				num += GenCelestial.CelestialSunGlowPercent(latitude, dayOfYear, (float)i / 24f);
			}
			return num / 24f;
		}

		// Token: 0x0600C98C RID: 51596 RVA: 0x004A36E8 File Offset: 0x004A18E8
		private static Vector3 SurfaceNormal(float latitude)
		{
			Vector3 vector = new Vector3(1f, 0f, 0f);
			vector = Quaternion.AngleAxis(latitude, new Vector3(0f, 0f, 1f)) * vector;
			return vector;
		}

		// Token: 0x0600C98D RID: 51597 RVA: 0x004A3730 File Offset: 0x004A1930
		public static void LogSunGlowForYear()
		{
			for (int i = -90; i <= 90; i += 10)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Sun visibility percents for latitude " + i.ToString() + ", for each hour of each day of the year");
				stringBuilder.AppendLine("---------------------------------------");
				stringBuilder.Append("Day/hr".PadRight(6));
				for (int j = 0; j < 24; j += 2)
				{
					stringBuilder.Append((j.ToString() + "h").PadRight(6));
				}
				stringBuilder.AppendLine();
				for (int k = 0; k < 60; k += 5)
				{
					stringBuilder.Append(k.ToString().PadRight(6));
					for (int l = 0; l < 24; l += 2)
					{
						stringBuilder.Append(GenCelestial.CelestialSunGlowPercent((float)i, k, (float)l / 24f).ToString("F2").PadRight(6));
					}
					stringBuilder.AppendLine();
				}
				Log.Message(stringBuilder.ToString());
			}
		}

		// Token: 0x0600C98E RID: 51598 RVA: 0x004A3838 File Offset: 0x004A1A38
		public static void LogSunAngleForYear()
		{
			for (int i = -90; i <= 90; i += 10)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Sun angles for latitude " + i.ToString() + ", for each hour of each day of the year");
				stringBuilder.AppendLine("---------------------------------------");
				stringBuilder.Append("Day/hr".PadRight(6));
				for (int j = 0; j < 24; j += 2)
				{
					stringBuilder.Append((j.ToString() + "h").PadRight(6));
				}
				stringBuilder.AppendLine();
				for (int k = 0; k < 60; k += 5)
				{
					stringBuilder.Append(k.ToString().PadRight(6));
					for (int l = 0; l < 24; l += 2)
					{
						float num = Vector3.Angle(GenCelestial.SurfaceNormal((float)i), GenCelestial.SunPositionUnmodified((float)k, (float)l / 24f, new Vector3(1f, 0f, 0f), 0f));
						stringBuilder.Append((90f - num).ToString("F0").PadRight(6));
					}
					stringBuilder.AppendLine();
				}
				Log.Message(stringBuilder.ToString());
			}
		}

		// Token: 0x040072B5 RID: 29365
		public const float ShadowMaxLengthDay = 15f;

		// Token: 0x040072B6 RID: 29366
		public const float ShadowMaxLengthNight = 15f;

		// Token: 0x040072B7 RID: 29367
		private const float ShadowGlowLerpSpan = 0.15f;

		// Token: 0x040072B8 RID: 29368
		private const float ShadowDayNightThreshold = 0.6f;

		// Token: 0x040072B9 RID: 29369
		private static SimpleCurve SunPeekAroundDegreesFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(70f, 1f),
				true
			},
			{
				new CurvePoint(75f, 0.05f),
				true
			}
		};

		// Token: 0x040072BA RID: 29370
		private static SimpleCurve SunOffsetFractionFromLatitudeCurve = new SimpleCurve
		{
			{
				new CurvePoint(70f, 0.2f),
				true
			},
			{
				new CurvePoint(75f, 1.5f),
				true
			}
		};

		// Token: 0x02003BA5 RID: 15269
		public struct LightInfo
		{
			// Token: 0x0400EC5E RID: 60510
			public Vector2 vector;

			// Token: 0x0400EC5F RID: 60511
			public float intensity;
		}

		// Token: 0x02003BA6 RID: 15270
		public enum LightType
		{
			// Token: 0x0400EC61 RID: 60513
			Shadow,
			// Token: 0x0400EC62 RID: 60514
			LightingSun,
			// Token: 0x0400EC63 RID: 60515
			LightingMoon
		}
	}
}
