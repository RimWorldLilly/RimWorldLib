using System;
using RimWorld;
using RimWorld.Planet;
using Unity.Burst;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006F2 RID: 1778
	public static class GenTicks
	{
		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06003893 RID: 14483 RVA: 0x0016AC5C File Offset: 0x00168E5C
		public static int TicksAbs
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing && Find.GameInitData != null && Find.GameInitData.gameToLoad.NullOrEmpty())
				{
					return GenTicks.ConfiguredTicksAbsAtGameStart;
				}
				if (Current.Game != null && Find.TickManager != null)
				{
					return Find.TickManager.TicksAbs;
				}
				return 0;
			}
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06003894 RID: 14484 RVA: 0x0016ACA9 File Offset: 0x00168EA9
		public static int TicksGame
		{
			get
			{
				if (Current.Game != null && Find.TickManager != null)
				{
					return Find.TickManager.TicksGame;
				}
				return 0;
			}
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06003895 RID: 14485 RVA: 0x0016ACC8 File Offset: 0x00168EC8
		public static int ConfiguredTicksAbsAtGameStart
		{
			get
			{
				GameInitData gameInitData = Find.GameInitData;
				ConfiguredTicksAbsAtGameStartCache ticksAbsCache = Find.World.ticksAbsCache;
				int result;
				if (ticksAbsCache.TryGetCachedValue(gameInitData, out result))
				{
					return result;
				}
				Vector2 vector;
				if (gameInitData.startingTile.Valid)
				{
					vector = Find.WorldGrid.LongLatOf(gameInitData.startingTile);
				}
				else
				{
					vector = Vector2.zero;
				}
				Twelfth twelfth;
				if (gameInitData.startingSeason != Season.Undefined)
				{
					twelfth = gameInitData.startingSeason.GetFirstTwelfth(vector.y);
				}
				else if (gameInitData.startingTile.Valid)
				{
					twelfth = TwelfthUtility.FindStartingWarmTwelfth(gameInitData.startingTile);
				}
				else
				{
					twelfth = Season.Summer.GetFirstTwelfth(0f);
				}
				int num = (24 - GenDate.TimeZoneAt(vector.x)) % 24;
				int num2 = 300000 * (int)twelfth + 2500 * (6 + num);
				ticksAbsCache.Cache(num2, gameInitData);
				return num2;
			}
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x0016AD92 File Offset: 0x00168F92
		public static float TicksToSeconds(this int numTicks)
		{
			return (float)numTicks / 60f;
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x0016AD9C File Offset: 0x00168F9C
		public static int SecondsToTicks(this float numSeconds)
		{
			return Mathf.RoundToInt(60f * numSeconds);
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x0016ADAC File Offset: 0x00168FAC
		public static string ToStringSecondsFromTicks(this int numTicks)
		{
			return numTicks.TicksToSeconds().ToString("F1") + " " + "SecondsLower".Translate();
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x0016ADEC File Offset: 0x00168FEC
		public static string ToStringSecondsFromTicks(this int numTicks, string format)
		{
			return numTicks.TicksToSeconds().ToString(format) + " " + "SecondsLower".Translate();
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x0016AE28 File Offset: 0x00169028
		public static string ToStringTicksFromSeconds(this float numSeconds)
		{
			return numSeconds.SecondsToTicks().ToString();
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x0016AE43 File Offset: 0x00169043
		[BurstCompile]
		public static int GetTickIntervalOffset(int index, int count, int period)
		{
			return Mathf.CeilToInt((float)period / (float)count * (float)index % (float)period);
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x0016AE55 File Offset: 0x00169055
		[BurstCompile]
		public static bool IsTickIntervalDelta(int offset, int period, int delta)
		{
			return Mathf.Abs(GenTicks.TicksGame + offset) % period < delta;
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x0016AE68 File Offset: 0x00169068
		[BurstCompile]
		public static bool IsTickIntervalDelta(int period, int delta)
		{
			return GenTicks.TicksGame % period < delta;
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x0016AE74 File Offset: 0x00169074
		[BurstCompile]
		public static bool IsTickInterval(int offset, int period)
		{
			return (GenTicks.TicksGame + offset) % period == 0;
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x0016AE82 File Offset: 0x00169082
		[BurstCompile]
		public static bool IsTickInterval(int period)
		{
			return GenTicks.TicksGame % period == 0;
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x0016AE90 File Offset: 0x00169090
		public static int GetCameraUpdateRate(Thing thing)
		{
			if (!WorldRendererUtility.DrawingMap || Find.CurrentMap == null || thing.MapHeld != Find.CurrentMap)
			{
				return 15;
			}
			CameraDriver cameraDriver = Find.CameraDriver;
			if (!cameraDriver.InViewOf(thing))
			{
				return 15;
			}
			return (int)(cameraDriver.CurrentZoom + 1);
		}

		// Token: 0x040024DB RID: 9435
		public const int TicksPerRealSecond = 60;

		// Token: 0x040024DC RID: 9436
		public const int TickRareInterval = 250;

		// Token: 0x040024DD RID: 9437
		public const int TickLongInterval = 2000;

		// Token: 0x040024DE RID: 9438
		public const int MaxTickInterval = 15;

		// Token: 0x040024DF RID: 9439
		public const float SecondsPerTick = 0.016666668f;
	}
}
