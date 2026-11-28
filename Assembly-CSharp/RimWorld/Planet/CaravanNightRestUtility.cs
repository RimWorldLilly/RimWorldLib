using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200238D RID: 9101
	public static class CaravanNightRestUtility
	{
		// Token: 0x0600E25E RID: 57950 RVA: 0x00540A40 File Offset: 0x0053EC40
		public static bool RestingNowAt(PlanetTile tile)
		{
			return CaravanNightRestUtility.WouldBeRestingAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x0600E25F RID: 57951 RVA: 0x00540A50 File Offset: 0x0053EC50
		public static bool WouldBeRestingAt(PlanetTile tile, long ticksAbs)
		{
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			return num < 6f || num > 22f;
		}

		// Token: 0x0600E260 RID: 57952 RVA: 0x00540A86 File Offset: 0x0053EC86
		public static int LeftRestTicksAt(PlanetTile tile)
		{
			return CaravanNightRestUtility.LeftRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x0600E261 RID: 57953 RVA: 0x00540A94 File Offset: 0x0053EC94
		public static int LeftRestTicksAt(PlanetTile tile, long ticksAbs)
		{
			if (!CaravanNightRestUtility.WouldBeRestingAt(tile, ticksAbs))
			{
				return 0;
			}
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			if (num < 6f)
			{
				return Mathf.CeilToInt((6f - num) * 2500f);
			}
			return Mathf.CeilToInt((24f - num + 6f) * 2500f);
		}

		// Token: 0x0600E262 RID: 57954 RVA: 0x00540AF6 File Offset: 0x0053ECF6
		public static int LeftNonRestTicksAt(PlanetTile tile)
		{
			return CaravanNightRestUtility.LeftNonRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x0600E263 RID: 57955 RVA: 0x00540B04 File Offset: 0x0053ED04
		public static int LeftNonRestTicksAt(PlanetTile tile, long ticksAbs)
		{
			if (CaravanNightRestUtility.WouldBeRestingAt(tile, ticksAbs))
			{
				return 0;
			}
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			return Mathf.CeilToInt((22f - num) * 2500f);
		}

		// Token: 0x04009087 RID: 36999
		public const float WakeUpHour = 6f;

		// Token: 0x04009088 RID: 37000
		public const float RestStartHour = 22f;
	}
}
