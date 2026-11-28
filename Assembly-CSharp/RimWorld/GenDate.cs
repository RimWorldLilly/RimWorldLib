using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E56 RID: 7766
	public static class GenDate
	{
		// Token: 0x17002514 RID: 9492
		// (get) Token: 0x0600C990 RID: 51600 RVA: 0x004A39ED File Offset: 0x004A1BED
		private static int TicksGame
		{
			get
			{
				return Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17002515 RID: 9493
		// (get) Token: 0x0600C991 RID: 51601 RVA: 0x004A39F9 File Offset: 0x004A1BF9
		private static int TicksSinceSettle
		{
			get
			{
				return Find.TickManager.TicksSinceSettle;
			}
		}

		// Token: 0x17002516 RID: 9494
		// (get) Token: 0x0600C992 RID: 51602 RVA: 0x004A3A05 File Offset: 0x004A1C05
		public static int DaysPassed
		{
			get
			{
				return GenDate.DaysPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x17002517 RID: 9495
		// (get) Token: 0x0600C993 RID: 51603 RVA: 0x004A3A11 File Offset: 0x004A1C11
		public static int DaysPassedSinceSettle
		{
			get
			{
				return GenDate.DaysPassedAt(GenDate.TicksSinceSettle);
			}
		}

		// Token: 0x17002518 RID: 9496
		// (get) Token: 0x0600C994 RID: 51604 RVA: 0x004A3A1D File Offset: 0x004A1C1D
		public static float DaysPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 60000f;
			}
		}

		// Token: 0x17002519 RID: 9497
		// (get) Token: 0x0600C995 RID: 51605 RVA: 0x004A3A2B File Offset: 0x004A1C2B
		public static float DaysPassedSinceSettleFloat
		{
			get
			{
				return (float)GenDate.TicksSinceSettle / 60000f;
			}
		}

		// Token: 0x1700251A RID: 9498
		// (get) Token: 0x0600C996 RID: 51606 RVA: 0x004A3A39 File Offset: 0x004A1C39
		public static int TwelfthsPassed
		{
			get
			{
				return GenDate.TwelfthsPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x1700251B RID: 9499
		// (get) Token: 0x0600C997 RID: 51607 RVA: 0x004A3A45 File Offset: 0x004A1C45
		public static float TwelfthsPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 300000f;
			}
		}

		// Token: 0x1700251C RID: 9500
		// (get) Token: 0x0600C998 RID: 51608 RVA: 0x004A3A53 File Offset: 0x004A1C53
		public static int YearsPassed
		{
			get
			{
				return GenDate.YearsPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x1700251D RID: 9501
		// (get) Token: 0x0600C999 RID: 51609 RVA: 0x004A3A5F File Offset: 0x004A1C5F
		public static float YearsPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 3600000f;
			}
		}

		// Token: 0x0600C99A RID: 51610 RVA: 0x004A3A6D File Offset: 0x004A1C6D
		public static int TickAbsToGame(int absTick)
		{
			return absTick - Find.TickManager.gameStartAbsTick;
		}

		// Token: 0x0600C99B RID: 51611 RVA: 0x004A3A7B File Offset: 0x004A1C7B
		public static int TickGameToAbs(int gameTick)
		{
			return gameTick + Find.TickManager.gameStartAbsTick;
		}

		// Token: 0x0600C99C RID: 51612 RVA: 0x004A3A89 File Offset: 0x004A1C89
		public static int TickGameToSettled(int gameTick)
		{
			return gameTick - Find.TickManager.SettleTick;
		}

		// Token: 0x0600C99D RID: 51613 RVA: 0x004A3A97 File Offset: 0x004A1C97
		public static int DaysPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 60000f);
		}

		// Token: 0x0600C99E RID: 51614 RVA: 0x004A3AA6 File Offset: 0x004A1CA6
		public static int TwelfthsPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 300000f);
		}

		// Token: 0x0600C99F RID: 51615 RVA: 0x004A3AB5 File Offset: 0x004A1CB5
		public static int YearsPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 3600000f);
		}

		// Token: 0x0600C9A0 RID: 51616 RVA: 0x004A3AC4 File Offset: 0x004A1CC4
		public static long LocalTicksOffsetFromLongitude(float longitude)
		{
			return (long)GenDate.TimeZoneAt(longitude) * 2500L;
		}

		// Token: 0x0600C9A1 RID: 51617 RVA: 0x004A3AD4 File Offset: 0x004A1CD4
		public static int HourOfDay(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 2500, 24);
		}

		// Token: 0x0600C9A2 RID: 51618 RVA: 0x004A3AEA File Offset: 0x004A1CEA
		public static int DayOfTwelfth(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000, 5);
		}

		// Token: 0x0600C9A3 RID: 51619 RVA: 0x004A3AFF File Offset: 0x004A1CFF
		public static int DayOfYear(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000, 60);
		}

		// Token: 0x0600C9A4 RID: 51620 RVA: 0x004A3B15 File Offset: 0x004A1D15
		public static Twelfth Twelfth(long absTicks, float longitude)
		{
			return (Twelfth)GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 300000, 12);
		}

		// Token: 0x0600C9A5 RID: 51621 RVA: 0x004A3B2C File Offset: 0x004A1D2C
		public static Season Season(long absTicks, Vector2 longLat)
		{
			return GenDate.Season(absTicks, longLat.y, longLat.x);
		}

		// Token: 0x0600C9A6 RID: 51622 RVA: 0x004A3B40 File Offset: 0x004A1D40
		public static Season Season(long absTicks, float latitude, float longitude)
		{
			return SeasonUtility.GetReportedSeason(GenDate.YearPercent(absTicks, longitude), latitude);
		}

		// Token: 0x0600C9A7 RID: 51623 RVA: 0x004A3B4F File Offset: 0x004A1D4F
		public static Quadrum Quadrum(long absTicks, float longitude)
		{
			return GenDate.Twelfth(absTicks, longitude).GetQuadrum();
		}

		// Token: 0x0600C9A8 RID: 51624 RVA: 0x004A3B60 File Offset: 0x004A1D60
		public static int Year(long absTicks, float longitude)
		{
			long num = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			return 5500 + Mathf.FloorToInt((float)num / 3600000f);
		}

		// Token: 0x0600C9A9 RID: 51625 RVA: 0x004A3B89 File Offset: 0x004A1D89
		public static int DayOfSeason(long absTicks, float longitude)
		{
			return (GenDate.DayOfYear(absTicks, longitude) - (int)(SeasonUtility.FirstSeason.GetFirstTwelfth(0f) * RimWorld.Twelfth.Sixth)) % 15;
		}

		// Token: 0x0600C9AA RID: 51626 RVA: 0x004A3BA7 File Offset: 0x004A1DA7
		public static int DayOfQuadrum(long absTicks, float longitude)
		{
			return (GenDate.DayOfYear(absTicks, longitude) - (int)(QuadrumUtility.FirstQuadrum.GetFirstTwelfth() * RimWorld.Twelfth.Sixth)) % 15;
		}

		// Token: 0x0600C9AB RID: 51627 RVA: 0x004A3BC0 File Offset: 0x004A1DC0
		public static int DayTick(long absTicks, float longitude)
		{
			return (int)GenMath.PositiveMod(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000L);
		}

		// Token: 0x0600C9AC RID: 51628 RVA: 0x004A3BD8 File Offset: 0x004A1DD8
		public static float DayPercent(long absTicks, float longitude)
		{
			int num = GenDate.DayTick(absTicks, longitude);
			if (num == 0)
			{
				num = 1;
			}
			return (float)num / 60000f;
		}

		// Token: 0x0600C9AD RID: 51629 RVA: 0x004A3BFA File Offset: 0x004A1DFA
		public static float YearPercent(long absTicks, float longitude)
		{
			return (float)((int)GenMath.PositiveMod(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 3600000L)) / 3600000f;
		}

		// Token: 0x0600C9AE RID: 51630 RVA: 0x004A3C17 File Offset: 0x004A1E17
		public static int HourInteger(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 2500, 24);
		}

		// Token: 0x0600C9AF RID: 51631 RVA: 0x004A3C2D File Offset: 0x004A1E2D
		public static float HourFloat(long absTicks, float longitude)
		{
			return GenDate.DayPercent(absTicks, longitude) * 24f;
		}

		// Token: 0x0600C9B0 RID: 51632 RVA: 0x004A3C3C File Offset: 0x004A1E3C
		public static string DateFullStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "FullDate".Translate(value, GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x), num);
		}

		// Token: 0x0600C9B1 RID: 51633 RVA: 0x004A3CA7 File Offset: 0x004A1EA7
		public static string DateMonthYearStringAt(long absTicks, Vector2 location)
		{
			return "MonthYearDate".Translate(GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x));
		}

		// Token: 0x0600C9B2 RID: 51634 RVA: 0x004A3CE0 File Offset: 0x004A1EE0
		public static string DateFullStringWithHourAt(long absTicks, Vector2 location)
		{
			return GenDate.DateFullStringAt(absTicks, location) + ", " + GenDate.HourInteger(absTicks, location.x).ToString() + "LetterHour".Translate();
		}

		// Token: 0x0600C9B3 RID: 51635 RVA: 0x004A3D28 File Offset: 0x004A1F28
		public static string DateReadoutStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "DateReadout".Translate(value, GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x), num);
		}

		// Token: 0x0600C9B4 RID: 51636 RVA: 0x004A3D94 File Offset: 0x004A1F94
		public static string DateShortStringAt(long absTicks, Vector2 location)
		{
			int value = GenDate.DayOfSeason(absTicks, location.x) + 1;
			return "ShortDate".Translate(value, GenDate.Quadrum(absTicks, location.x).LabelShort(), GenDate.Year(absTicks, location.x), value);
		}

		// Token: 0x0600C9B5 RID: 51637 RVA: 0x004A3DF4 File Offset: 0x004A1FF4
		public static string SeasonDateStringAt(long absTicks, Vector2 longLat)
		{
			int num = GenDate.DayOfSeason(absTicks, longLat.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "SeasonFullDate".Translate(value, GenDate.Season(absTicks, longLat).Label(), num);
		}

		// Token: 0x0600C9B6 RID: 51638 RVA: 0x004A3E49 File Offset: 0x004A2049
		public static string SeasonDateStringAt(Twelfth twelfth, Vector2 longLat)
		{
			return GenDate.SeasonDateStringAt((long)((int)twelfth * 300000 + 1), longLat);
		}

		// Token: 0x0600C9B7 RID: 51639 RVA: 0x004A3E5C File Offset: 0x004A205C
		public static string QuadrumDateStringAt(long absTicks, float longitude)
		{
			int num = GenDate.DayOfQuadrum(absTicks, longitude) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "SeasonFullDate".Translate(value, GenDate.Quadrum(absTicks, longitude).Label(), num);
		}

		// Token: 0x0600C9B8 RID: 51640 RVA: 0x004A3EAC File Offset: 0x004A20AC
		public static string QuadrumDateStringAt(Quadrum quadrum)
		{
			return GenDate.QuadrumDateStringAt((long)((int)quadrum * 900000 + 1), 0f);
		}

		// Token: 0x0600C9B9 RID: 51641 RVA: 0x004A3EC2 File Offset: 0x004A20C2
		public static string QuadrumDateStringAt(Twelfth twelfth)
		{
			return GenDate.QuadrumDateStringAt((long)((int)twelfth * 300000 + 1), 0f);
		}

		// Token: 0x0600C9BA RID: 51642 RVA: 0x004A3ED8 File Offset: 0x004A20D8
		public static float TicksToDays(this int numTicks)
		{
			return (float)numTicks / 60000f;
		}

		// Token: 0x0600C9BB RID: 51643 RVA: 0x004A3EE2 File Offset: 0x004A20E2
		public static int DaysToTicks(float days)
		{
			return Mathf.RoundToInt(days * 60000f);
		}

		// Token: 0x0600C9BC RID: 51644 RVA: 0x004A3EF0 File Offset: 0x004A20F0
		public static string ToStringTicksToDays(this int numTicks, string format = "F1")
		{
			string text = numTicks.TicksToDays().ToString(format);
			if (text == "1")
			{
				return "Period1Day".Translate();
			}
			return "PeriodDays".Translate(text);
		}

		// Token: 0x0600C9BD RID: 51645 RVA: 0x004A3F40 File Offset: 0x004A2140
		public static string ToStringTicksToPeriod(this int numTicks, bool allowSeconds = true, bool shortForm = false, bool canUseDecimals = true, bool allowYears = true, bool canUseDecimalsShortForm = false)
		{
			if (allowSeconds && numTicks < 2500 && (numTicks < 600 || Math.Round((double)((float)numTicks / 2500f), 1) == 0.0))
			{
				int num = Mathf.RoundToInt((float)numTicks / 60f);
				if (shortForm)
				{
					return num.ToString() + "LetterSecond".Translate();
				}
				if (num == 1)
				{
					return "Period1Second".Translate();
				}
				return "PeriodSeconds".Translate(num);
			}
			else if (numTicks < 60000)
			{
				if (shortForm)
				{
					if (canUseDecimals && canUseDecimalsShortForm)
					{
						float num2 = (float)numTicks / 2500f;
						return num2.ToString("F1") + "LetterHour".Translate();
					}
					return Mathf.RoundToInt((float)numTicks / 2500f).ToString() + "LetterHour".Translate();
				}
				else if (numTicks < 2500)
				{
					float num2 = (float)numTicks / 2500f;
					string text = num2.ToString(canUseDecimals ? "0.#" : "0");
					if (text == "1")
					{
						return "Period1Hour".Translate();
					}
					return "PeriodHours".Translate(text);
				}
				else
				{
					int num3 = Mathf.RoundToInt((float)numTicks / 2500f);
					if (num3 == 1)
					{
						return "Period1Hour".Translate();
					}
					return "PeriodHours".Translate(num3);
				}
			}
			else if (numTicks < 3600000 || !allowYears)
			{
				if (shortForm)
				{
					if (canUseDecimals && canUseDecimalsShortForm)
					{
						float num2 = (float)numTicks / 60000f;
						return num2.ToString("F1") + "LetterDay".Translate();
					}
					return Mathf.RoundToInt((float)numTicks / 60000f).ToString() + "LetterDay".Translate();
				}
				else
				{
					string text2;
					if (canUseDecimals)
					{
						text2 = ((float)numTicks / 60000f).ToStringDecimalIfSmall();
					}
					else
					{
						text2 = Mathf.RoundToInt((float)numTicks / 60000f).ToString();
					}
					if (text2 == "1")
					{
						return "Period1Day".Translate();
					}
					return "PeriodDays".Translate(text2);
				}
			}
			else if (shortForm)
			{
				if (canUseDecimals && canUseDecimalsShortForm)
				{
					float num2 = (float)numTicks / 3600000f;
					return num2.ToString("F1") + "LetterYear".Translate();
				}
				return Mathf.RoundToInt((float)numTicks / 3600000f).ToString() + "LetterYear".Translate();
			}
			else
			{
				string text3;
				if (canUseDecimals)
				{
					text3 = ((float)numTicks / 3600000f).ToStringDecimalIfSmall();
				}
				else
				{
					text3 = Mathf.RoundToInt((float)numTicks / 3600000f).ToString();
				}
				if (text3 == "1")
				{
					return "Period1Year".Translate();
				}
				return "PeriodYears".Translate(text3);
			}
		}

		// Token: 0x0600C9BE RID: 51646 RVA: 0x004A425C File Offset: 0x004A245C
		public static string ToStringTicksToPeriodVerbose(this int numTicks, bool allowHours = true, bool allowQuadrums = true)
		{
			if (numTicks < 0)
			{
				return "0";
			}
			int num;
			int num2;
			int num3;
			float num4;
			numTicks.TicksToPeriod(out num, out num2, out num3, out num4);
			if (!allowQuadrums)
			{
				num3 += 15 * num2;
				num2 = 0;
			}
			if (num > 0)
			{
				string text;
				if (num == 1)
				{
					text = "Period1Year".Translate();
				}
				else
				{
					text = "PeriodYears".Translate(num);
				}
				if (num2 > 0)
				{
					text += ", ";
					if (num2 == 1)
					{
						text += "Period1Quadrum".Translate();
					}
					else
					{
						text += "PeriodQuadrums".Translate(num2);
					}
				}
				return text;
			}
			if (num2 > 0)
			{
				string text2;
				if (num2 == 1)
				{
					text2 = "Period1Quadrum".Translate();
				}
				else
				{
					text2 = "PeriodQuadrums".Translate(num2);
				}
				if (num3 > 0)
				{
					text2 += ", ";
					if (num3 == 1)
					{
						text2 += "Period1Day".Translate();
					}
					else
					{
						text2 += "PeriodDays".Translate(num3);
					}
				}
				return text2;
			}
			if (num3 > 0)
			{
				string text3;
				if (num3 == 1)
				{
					text3 = "Period1Day".Translate();
				}
				else
				{
					text3 = "PeriodDays".Translate(num3);
				}
				int num5 = (int)num4;
				if (allowHours && num5 > 0)
				{
					text3 += ", ";
					if (num5 == 1)
					{
						text3 += "Period1Hour".Translate();
					}
					else
					{
						text3 += "PeriodHours".Translate(num5);
					}
				}
				return text3;
			}
			if (!allowHours)
			{
				return "PeriodDays".Translate(0);
			}
			if (num4 > 1f)
			{
				int num6 = Mathf.RoundToInt(num4);
				if (num6 == 1)
				{
					return "Period1Hour".Translate();
				}
				return "PeriodHours".Translate(num6);
			}
			else
			{
				if (Math.Round((double)num4, 1) == 1.0)
				{
					return "Period1Hour".Translate();
				}
				return "PeriodHours".Translate(num4.ToString("0.#"));
			}
		}

		// Token: 0x0600C9BF RID: 51647 RVA: 0x004A44C0 File Offset: 0x004A26C0
		public static string ToStringTicksToPeriodVague(this int numTicks, bool vagueMin = true, bool vagueMax = true)
		{
			if (vagueMax && numTicks > 36000000)
			{
				return "OverADecade".Translate();
			}
			if (vagueMin && numTicks < 60000)
			{
				return "LessThanADay".Translate();
			}
			return numTicks.ToStringTicksToPeriod(true, false, true, true, false);
		}

		// Token: 0x0600C9C0 RID: 51648 RVA: 0x004A450E File Offset: 0x004A270E
		public static void TicksToPeriod(this int numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			((long)numTicks).TicksToPeriod(out years, out quadrums, out days, out hoursFloat);
		}

		// Token: 0x0600C9C1 RID: 51649 RVA: 0x004A451C File Offset: 0x004A271C
		public static void TicksToPeriod(this long numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			if (numTicks < 0L)
			{
				Log.ErrorOnce("Tried to calculate period for negative ticks", 12841103);
			}
			years = (int)(numTicks / 3600000L);
			long num = numTicks - (long)years * 3600000L;
			quadrums = (int)(num / 900000L);
			num -= (long)quadrums * 900000L;
			days = (int)(num / 60000L);
			num -= (long)days * 60000L;
			hoursFloat = (float)num / 2500f;
		}

		// Token: 0x0600C9C2 RID: 51650 RVA: 0x004A4594 File Offset: 0x004A2794
		public static string ToStringApproxAge(this float yearsFloat)
		{
			if (yearsFloat >= 1f)
			{
				return ((int)yearsFloat).ToStringCached();
			}
			int num;
			int num2;
			int num3;
			float num4;
			Mathf.Min((int)(yearsFloat * 3600000f), 3599999).TicksToPeriod(out num, out num2, out num3, out num4);
			if (num > 0)
			{
				if (num == 1)
				{
					return "Period1Year".Translate();
				}
				return "PeriodYears".Translate(num);
			}
			else if (num2 > 0)
			{
				if (num2 == 1)
				{
					return "Period1Quadrum".Translate();
				}
				return "PeriodQuadrums".Translate(num2);
			}
			else if (num3 > 0)
			{
				if (num3 == 1)
				{
					return "Period1Day".Translate();
				}
				return "PeriodDays".Translate(num3);
			}
			else
			{
				int num5 = (int)num4;
				if (num5 == 1)
				{
					return "Period1Hour".Translate();
				}
				return "PeriodHours".Translate(num5);
			}
		}

		// Token: 0x0600C9C3 RID: 51651 RVA: 0x004A4689 File Offset: 0x004A2889
		public static int TimeZoneAt(float longitude)
		{
			return Mathf.RoundToInt(GenDate.TimeZoneFloatAt(longitude));
		}

		// Token: 0x0600C9C4 RID: 51652 RVA: 0x004A4696 File Offset: 0x004A2896
		public static float TimeZoneFloatAt(float longitude)
		{
			return longitude / 15f;
		}

		// Token: 0x040072BB RID: 29371
		public const int TicksPerDay = 60000;

		// Token: 0x040072BC RID: 29372
		public const int HoursPerDay = 24;

		// Token: 0x040072BD RID: 29373
		public const int DaysPerTwelfth = 5;

		// Token: 0x040072BE RID: 29374
		public const int TwelfthsPerYear = 12;

		// Token: 0x040072BF RID: 29375
		public const int GameStartHourOfDay = 6;

		// Token: 0x040072C0 RID: 29376
		public const int TicksPerTwelfth = 300000;

		// Token: 0x040072C1 RID: 29377
		public const int TicksPerSeason = 900000;

		// Token: 0x040072C2 RID: 29378
		public const int TicksPerQuadrum = 900000;

		// Token: 0x040072C3 RID: 29379
		public const int TicksPerYear = 3600000;

		// Token: 0x040072C4 RID: 29380
		public const int DaysPerYear = 60;

		// Token: 0x040072C5 RID: 29381
		public const int DaysPerSeason = 15;

		// Token: 0x040072C6 RID: 29382
		public const int DaysPerQuadrum = 15;

		// Token: 0x040072C7 RID: 29383
		public const int TicksPerHour = 2500;

		// Token: 0x040072C8 RID: 29384
		public const float TimeZoneWidth = 15f;

		// Token: 0x040072C9 RID: 29385
		public const int DefaultStartingYear = 5500;
	}
}
