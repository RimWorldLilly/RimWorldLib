using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E5F RID: 7775
	public static class TwelfthUtility
	{
		// Token: 0x0600CA0E RID: 51726 RVA: 0x004A5638 File Offset: 0x004A3838
		public static Quadrum GetQuadrum(this Twelfth twelfth)
		{
			switch (twelfth)
			{
			case Twelfth.First:
				return Quadrum.Aprimay;
			case Twelfth.Second:
				return Quadrum.Aprimay;
			case Twelfth.Third:
				return Quadrum.Aprimay;
			case Twelfth.Fourth:
				return Quadrum.Jugust;
			case Twelfth.Fifth:
				return Quadrum.Jugust;
			case Twelfth.Sixth:
				return Quadrum.Jugust;
			case Twelfth.Seventh:
				return Quadrum.Septober;
			case Twelfth.Eighth:
				return Quadrum.Septober;
			case Twelfth.Ninth:
				return Quadrum.Septober;
			case Twelfth.Tenth:
				return Quadrum.Decembary;
			case Twelfth.Eleventh:
				return Quadrum.Decembary;
			case Twelfth.Twelfth:
				return Quadrum.Decembary;
			default:
				return Quadrum.Undefined;
			}
		}

		// Token: 0x0600CA0F RID: 51727 RVA: 0x004A5698 File Offset: 0x004A3898
		public static Twelfth PreviousTwelfth(this Twelfth twelfth)
		{
			if (twelfth == Twelfth.Undefined)
			{
				return Twelfth.Undefined;
			}
			int num = (int)(twelfth - Twelfth.Second);
			if (num == -1)
			{
				num = 11;
			}
			return (Twelfth)num;
		}

		// Token: 0x0600CA10 RID: 51728 RVA: 0x004A56BA File Offset: 0x004A38BA
		public static Twelfth NextTwelfth(this Twelfth twelfth)
		{
			if (twelfth == Twelfth.Undefined)
			{
				return Twelfth.Undefined;
			}
			return (twelfth + 1) % Twelfth.Undefined;
		}

		// Token: 0x0600CA11 RID: 51729 RVA: 0x004A56CB File Offset: 0x004A38CB
		public static float GetMiddleYearPct(this Twelfth twelfth)
		{
			return ((float)twelfth + 0.5f) / 12f;
		}

		// Token: 0x0600CA12 RID: 51730 RVA: 0x004A56DB File Offset: 0x004A38DB
		public static float GetBeginningYearPct(this Twelfth twelfth)
		{
			return (float)twelfth / 12f;
		}

		// Token: 0x0600CA13 RID: 51731 RVA: 0x004A56E8 File Offset: 0x004A38E8
		public static Twelfth FindStartingWarmTwelfth(PlanetTile tile)
		{
			Twelfth twelfth = GenTemperature.EarliestTwelfthInAverageTemperatureRange(tile, 12f, 9999f);
			if (twelfth == Twelfth.Undefined)
			{
				twelfth = Season.Summer.GetFirstTwelfth(Find.WorldGrid.LongLatOf(tile).y);
			}
			return twelfth;
		}

		// Token: 0x0600CA14 RID: 51732 RVA: 0x004A5724 File Offset: 0x004A3924
		public static Twelfth GetLeftMostTwelfth(List<Twelfth> twelfths, Twelfth rootTwelfth)
		{
			if (twelfths.Count >= 12)
			{
				return Twelfth.Undefined;
			}
			Twelfth result;
			do
			{
				result = rootTwelfth;
				rootTwelfth = TwelfthUtility.TwelfthBefore(rootTwelfth);
			}
			while (twelfths.Contains(rootTwelfth));
			return result;
		}

		// Token: 0x0600CA15 RID: 51733 RVA: 0x004A5754 File Offset: 0x004A3954
		public static Twelfth GetRightMostTwelfth(List<Twelfth> twelfths, Twelfth rootTwelfth)
		{
			if (twelfths.Count >= 12)
			{
				return Twelfth.Undefined;
			}
			Twelfth m;
			do
			{
				m = rootTwelfth;
				rootTwelfth = TwelfthUtility.TwelfthAfter(rootTwelfth);
			}
			while (twelfths.Contains(rootTwelfth));
			return TwelfthUtility.TwelfthAfter(m);
		}

		// Token: 0x0600CA16 RID: 51734 RVA: 0x004A5787 File Offset: 0x004A3987
		public static Twelfth TwelfthBefore(Twelfth m)
		{
			if (m == Twelfth.First)
			{
				return Twelfth.Twelfth;
			}
			return (Twelfth)(m - Twelfth.Second);
		}

		// Token: 0x0600CA17 RID: 51735 RVA: 0x004A5793 File Offset: 0x004A3993
		public static Twelfth TwelfthAfter(Twelfth m)
		{
			if (m == Twelfth.Twelfth)
			{
				return Twelfth.First;
			}
			return m + 1;
		}
	}
}
