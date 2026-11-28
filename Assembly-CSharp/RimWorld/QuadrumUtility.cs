using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E5B RID: 7771
	public static class QuadrumUtility
	{
		// Token: 0x1700251F RID: 9503
		// (get) Token: 0x0600C9F3 RID: 51699 RVA: 0x004A4CED File Offset: 0x004A2EED
		public static Quadrum FirstQuadrum
		{
			get
			{
				return Quadrum.Aprimay;
			}
		}

		// Token: 0x17002520 RID: 9504
		// (get) Token: 0x0600C9F4 RID: 51700 RVA: 0x004A4CF0 File Offset: 0x004A2EF0
		public static List<Quadrum> Quadrums
		{
			get
			{
				return QuadrumUtility.quadrumList;
			}
		}

		// Token: 0x17002521 RID: 9505
		// (get) Token: 0x0600C9F5 RID: 51701 RVA: 0x004A4CF7 File Offset: 0x004A2EF7
		public static List<Quadrum> QuadrumsInChronologicalOrder
		{
			get
			{
				return QuadrumUtility.quadrumListInChronologicanOrder;
			}
		}

		// Token: 0x0600C9F6 RID: 51702 RVA: 0x004A4CFE File Offset: 0x004A2EFE
		public static Twelfth GetFirstTwelfth(this Quadrum quadrum)
		{
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				return Twelfth.First;
			case Quadrum.Jugust:
				return Twelfth.Fourth;
			case Quadrum.Septober:
				return Twelfth.Seventh;
			case Quadrum.Decembary:
				return Twelfth.Tenth;
			default:
				return Twelfth.Undefined;
			}
		}

		// Token: 0x0600C9F7 RID: 51703 RVA: 0x004A4D23 File Offset: 0x004A2F23
		public static Twelfth GetMiddleTwelfth(this Quadrum quadrum)
		{
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				return Twelfth.Second;
			case Quadrum.Jugust:
				return Twelfth.Fifth;
			case Quadrum.Septober:
				return Twelfth.Eighth;
			case Quadrum.Decembary:
				return Twelfth.Eleventh;
			default:
				return Twelfth.Undefined;
			}
		}

		// Token: 0x0600C9F8 RID: 51704 RVA: 0x004A4D48 File Offset: 0x004A2F48
		public static float GetMiddleYearPct(this Quadrum quadrum)
		{
			return quadrum.GetMiddleTwelfth().GetMiddleYearPct();
		}

		// Token: 0x0600C9F9 RID: 51705 RVA: 0x004A4D58 File Offset: 0x004A2F58
		public static string Label(this Quadrum quadrum)
		{
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				return "QuadrumAprimay".Translate();
			case Quadrum.Jugust:
				return "QuadrumJugust".Translate();
			case Quadrum.Septober:
				return "QuadrumSeptober".Translate();
			case Quadrum.Decembary:
				return "QuadrumDecembary".Translate();
			default:
				return "Unknown quadrum";
			}
		}

		// Token: 0x0600C9FA RID: 51706 RVA: 0x004A4DC4 File Offset: 0x004A2FC4
		public static string LabelShort(this Quadrum quadrum)
		{
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				return "QuadrumAprimay_Short".Translate();
			case Quadrum.Jugust:
				return "QuadrumJugust_Short".Translate();
			case Quadrum.Septober:
				return "QuadrumSeptober_Short".Translate();
			case Quadrum.Decembary:
				return "QuadrumDecembary_Short".Translate();
			default:
				return "Unknown quadrum";
			}
		}

		// Token: 0x0600C9FB RID: 51707 RVA: 0x004A4E2E File Offset: 0x004A302E
		public static Season GetSeason(this Quadrum q, Vector2 location)
		{
			return q.GetSeason(location.y);
		}

		// Token: 0x0600C9FC RID: 51708 RVA: 0x004A4E3C File Offset: 0x004A303C
		public static Season GetSeason(this Quadrum q, float latitude)
		{
			return SeasonUtility.GetReportedSeason(q.GetMiddleYearPct(), latitude);
		}

		// Token: 0x0600C9FD RID: 51709 RVA: 0x004A4E4C File Offset: 0x004A304C
		public static string QuadrumsRangeLabel(List<Twelfth> twelfths)
		{
			if (twelfths.Count == 0)
			{
				return "";
			}
			if (twelfths.Count == 12)
			{
				return "WholeYear".Translate();
			}
			string text = "";
			for (int i = 0; i < 12; i++)
			{
				Twelfth twelfth = (Twelfth)i;
				if (twelfths.Contains(twelfth))
				{
					if (!text.NullOrEmpty())
					{
						text += ", ";
					}
					text += QuadrumUtility.QuadrumsContinuousRangeLabel(twelfths, twelfth);
				}
			}
			return text;
		}

		// Token: 0x0600C9FE RID: 51710 RVA: 0x004A4EC4 File Offset: 0x004A30C4
		private static string QuadrumsContinuousRangeLabel(List<Twelfth> twelfths, Twelfth rootTwelfth)
		{
			Twelfth leftMostTwelfth = TwelfthUtility.GetLeftMostTwelfth(twelfths, rootTwelfth);
			Twelfth rightMostTwelfth = TwelfthUtility.GetRightMostTwelfth(twelfths, rootTwelfth);
			for (Twelfth twelfth = leftMostTwelfth; twelfth != rightMostTwelfth; twelfth = TwelfthUtility.TwelfthAfter(twelfth))
			{
				if (!twelfths.Contains(twelfth))
				{
					Log.Error(string.Concat(new string[]
					{
						"Twelfths doesn't contain ",
						twelfth.ToString(),
						" (",
						leftMostTwelfth.ToString(),
						"..",
						rightMostTwelfth.ToString(),
						")"
					}));
					break;
				}
				twelfths.Remove(twelfth);
			}
			twelfths.Remove(rightMostTwelfth);
			return GenDate.QuadrumDateStringAt(leftMostTwelfth) + " - " + GenDate.QuadrumDateStringAt(rightMostTwelfth);
		}

		// Token: 0x040072D6 RID: 29398
		private static readonly List<Quadrum> quadrumList = new List<Quadrum>
		{
			Quadrum.Aprimay,
			Quadrum.Decembary,
			Quadrum.Jugust,
			Quadrum.Septober
		};

		// Token: 0x040072D7 RID: 29399
		private static readonly List<Quadrum> quadrumListInChronologicanOrder = new List<Quadrum>
		{
			Quadrum.Aprimay,
			Quadrum.Jugust,
			Quadrum.Septober,
			Quadrum.Decembary
		};
	}
}
