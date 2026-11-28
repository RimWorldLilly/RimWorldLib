using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E57 RID: 7767
	public static class GenLocalDate
	{
		// Token: 0x1700251E RID: 9502
		// (get) Token: 0x0600C9C5 RID: 51653 RVA: 0x004A469F File Offset: 0x004A289F
		private static int TicksAbs
		{
			get
			{
				return GenTicks.TicksAbs;
			}
		}

		// Token: 0x0600C9C6 RID: 51654 RVA: 0x004A46A6 File Offset: 0x004A28A6
		public static int DayOfYear(Map map)
		{
			return GenLocalDate.DayOfYear(map.Tile);
		}

		// Token: 0x0600C9C7 RID: 51655 RVA: 0x004A46B3 File Offset: 0x004A28B3
		public static int HourOfDay(Map map)
		{
			return GenLocalDate.HourOfDay(map.Tile);
		}

		// Token: 0x0600C9C8 RID: 51656 RVA: 0x004A46C0 File Offset: 0x004A28C0
		public static int DayOfTwelfth(Map map)
		{
			return GenLocalDate.DayOfTwelfth(map.Tile);
		}

		// Token: 0x0600C9C9 RID: 51657 RVA: 0x004A46CD File Offset: 0x004A28CD
		public static Twelfth Twelfth(Map map)
		{
			return GenLocalDate.Twelfth(map.Tile);
		}

		// Token: 0x0600C9CA RID: 51658 RVA: 0x004A46DA File Offset: 0x004A28DA
		public static Season Season(Map map)
		{
			return GenLocalDate.Season(map.Tile);
		}

		// Token: 0x0600C9CB RID: 51659 RVA: 0x004A46E7 File Offset: 0x004A28E7
		public static int Year(Map map)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenLocalDate.Year(map.Tile);
		}

		// Token: 0x0600C9CC RID: 51660 RVA: 0x004A4702 File Offset: 0x004A2902
		public static int DayOfSeason(Map map)
		{
			return GenLocalDate.DayOfSeason(map.Tile);
		}

		// Token: 0x0600C9CD RID: 51661 RVA: 0x004A470F File Offset: 0x004A290F
		public static int DayOfQuadrum(Map map)
		{
			return GenLocalDate.DayOfQuadrum(map.Tile);
		}

		// Token: 0x0600C9CE RID: 51662 RVA: 0x004A471C File Offset: 0x004A291C
		public static int DayTick(Map map)
		{
			return GenLocalDate.DayTick(map.Tile);
		}

		// Token: 0x0600C9CF RID: 51663 RVA: 0x004A4729 File Offset: 0x004A2929
		public static float DayPercent(Map map)
		{
			return GenLocalDate.DayPercent(map.Tile);
		}

		// Token: 0x0600C9D0 RID: 51664 RVA: 0x004A4736 File Offset: 0x004A2936
		public static float YearPercent(Map map)
		{
			return GenLocalDate.YearPercent(map.Tile);
		}

		// Token: 0x0600C9D1 RID: 51665 RVA: 0x004A4743 File Offset: 0x004A2943
		public static int HourInteger(Map map)
		{
			return GenLocalDate.HourInteger(map.Tile);
		}

		// Token: 0x0600C9D2 RID: 51666 RVA: 0x004A4750 File Offset: 0x004A2950
		public static float HourFloat(Map map)
		{
			return GenLocalDate.HourFloat(map.Tile);
		}

		// Token: 0x0600C9D3 RID: 51667 RVA: 0x004A475D File Offset: 0x004A295D
		public static int DayOfYear(Thing thing)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				return GenDate.DayOfYear((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
			}
			return 0;
		}

		// Token: 0x0600C9D4 RID: 51668 RVA: 0x004A477A File Offset: 0x004A297A
		public static int HourOfDay(Thing thing)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0600C9D5 RID: 51669 RVA: 0x004A478D File Offset: 0x004A298D
		public static int DayOfTwelfth(Thing thing)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0600C9D6 RID: 51670 RVA: 0x004A47A0 File Offset: 0x004A29A0
		public static Twelfth Twelfth(Thing thing)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0600C9D7 RID: 51671 RVA: 0x004A47B3 File Offset: 0x004A29B3
		public static Season Season(Thing thing)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, GenLocalDate.LocationForDate(thing));
		}

		// Token: 0x0600C9D8 RID: 51672 RVA: 0x004A47C6 File Offset: 0x004A29C6
		public static int Year(Thing thing)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenDate.Year((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0600C9D9 RID: 51673 RVA: 0x004A47E7 File Offset: 0x004A29E7
		public static int DayOfSeason(Thing thing)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0600C9DA RID: 51674 RVA: 0x004A47FA File Offset: 0x004A29FA
		public static int DayOfQuadrum(Thing thing)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0600C9DB RID: 51675 RVA: 0x004A480D File Offset: 0x004A2A0D
		public static int DayTick(Thing thing)
		{
			return GenDate.DayTick((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0600C9DC RID: 51676 RVA: 0x004A4820 File Offset: 0x004A2A20
		public static float DayPercent(Thing thing)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0600C9DD RID: 51677 RVA: 0x004A4833 File Offset: 0x004A2A33
		public static float YearPercent(Thing thing)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0600C9DE RID: 51678 RVA: 0x004A4846 File Offset: 0x004A2A46
		public static int HourInteger(Thing thing)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0600C9DF RID: 51679 RVA: 0x004A4859 File Offset: 0x004A2A59
		public static float HourFloat(Thing thing)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0600C9E0 RID: 51680 RVA: 0x004A486C File Offset: 0x004A2A6C
		public static int DayOfYear(PlanetTile tile)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				return GenDate.DayOfYear((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
			}
			return 0;
		}

		// Token: 0x0600C9E1 RID: 51681 RVA: 0x004A4893 File Offset: 0x004A2A93
		public static int HourOfDay(PlanetTile tile)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0600C9E2 RID: 51682 RVA: 0x004A48B0 File Offset: 0x004A2AB0
		public static int DayOfTwelfth(PlanetTile tile)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0600C9E3 RID: 51683 RVA: 0x004A48CD File Offset: 0x004A2ACD
		public static Twelfth Twelfth(PlanetTile tile)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0600C9E4 RID: 51684 RVA: 0x004A48EA File Offset: 0x004A2AEA
		public static Season Season(PlanetTile tile)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile));
		}

		// Token: 0x0600C9E5 RID: 51685 RVA: 0x004A4902 File Offset: 0x004A2B02
		public static int Year(PlanetTile tile)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenDate.Year((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0600C9E6 RID: 51686 RVA: 0x004A492D File Offset: 0x004A2B2D
		public static int DayOfSeason(PlanetTile tile)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0600C9E7 RID: 51687 RVA: 0x004A494A File Offset: 0x004A2B4A
		public static int DayOfQuadrum(PlanetTile tile)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0600C9E8 RID: 51688 RVA: 0x004A4967 File Offset: 0x004A2B67
		public static int DayTick(PlanetTile tile)
		{
			return GenDate.DayTick((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0600C9E9 RID: 51689 RVA: 0x004A4984 File Offset: 0x004A2B84
		public static float DayPercent(PlanetTile tile)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0600C9EA RID: 51690 RVA: 0x004A49A1 File Offset: 0x004A2BA1
		public static float YearPercent(PlanetTile tile)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0600C9EB RID: 51691 RVA: 0x004A49BE File Offset: 0x004A2BBE
		public static int HourInteger(PlanetTile tile)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0600C9EC RID: 51692 RVA: 0x004A49DB File Offset: 0x004A2BDB
		public static float HourFloat(PlanetTile tile)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0600C9ED RID: 51693 RVA: 0x004A49F8 File Offset: 0x004A2BF8
		private static float LongitudeForDate(Thing thing)
		{
			return GenLocalDate.LocationForDate(thing).x;
		}

		// Token: 0x0600C9EE RID: 51694 RVA: 0x004A4A08 File Offset: 0x004A2C08
		private static Vector2 LocationForDate(Thing thing)
		{
			PlanetTile tile = thing.Tile;
			if (tile.Valid)
			{
				return Find.WorldGrid.LongLatOf(tile);
			}
			return Vector2.zero;
		}
	}
}
