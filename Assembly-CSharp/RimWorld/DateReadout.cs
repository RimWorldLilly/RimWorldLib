using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D49 RID: 7497
	public static class DateReadout
	{
		// Token: 0x170023C9 RID: 9161
		// (get) Token: 0x0600C1FE RID: 49662 RVA: 0x0046A649 File Offset: 0x00468849
		public static float Height
		{
			get
			{
				return (float)(48 + (DateReadout.SeasonLabelVisible ? 26 : 0));
			}
		}

		// Token: 0x170023CA RID: 9162
		// (get) Token: 0x0600C1FF RID: 49663 RVA: 0x0046A65B File Offset: 0x0046885B
		private static bool SeasonLabelVisible
		{
			get
			{
				return !WorldRendererUtility.WorldSelected && Find.CurrentMap != null;
			}
		}

		// Token: 0x0600C200 RID: 49664 RVA: 0x0046A66E File Offset: 0x0046886E
		static DateReadout()
		{
			DateReadout.Reset();
		}

		// Token: 0x0600C201 RID: 49665 RVA: 0x0046A6AC File Offset: 0x004688AC
		public static void Reset()
		{
			DateReadout.dateString = null;
			DateReadout.dateStringDay = -1;
			DateReadout.dateStringSeason = Season.Undefined;
			DateReadout.dateStringQuadrum = Quadrum.Undefined;
			DateReadout.dateStringYear = -1;
			DateReadout.fastHourStrings24h.Clear();
			for (int i = 0; i < 24; i++)
			{
				DateReadout.fastHourStrings24h.Add(i.ToString() + "LetterHour".Translate());
			}
			DateReadout.fastHourStrings12h.Clear();
			for (int j = 0; j < 24; j++)
			{
				TaggedString value = (j >= 12) ? "PM".Translate() : "AM".Translate();
				string text;
				if (j != 0)
				{
					if (j <= 12)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
						defaultInterpolatedStringHandler.AppendFormatted<int>(j);
						defaultInterpolatedStringHandler.AppendLiteral(" ");
						defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(value);
						text = defaultInterpolatedStringHandler.ToStringAndClear();
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(1, 2);
						defaultInterpolatedStringHandler2.AppendFormatted<int>(j - 12);
						defaultInterpolatedStringHandler2.AppendLiteral(" ");
						defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>(value);
						text = defaultInterpolatedStringHandler2.ToStringAndClear();
					}
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(3, 1);
					defaultInterpolatedStringHandler3.AppendLiteral("12 ");
					defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>(value);
					text = defaultInterpolatedStringHandler3.ToStringAndClear();
				}
				string item = text;
				DateReadout.fastHourStrings12h.Add(item);
			}
			DateReadout.seasonsCached.Clear();
			int length = Enum.GetValues(typeof(Season)).Length;
			for (int k = 0; k < length; k++)
			{
				Season season = (Season)k;
				DateReadout.seasonsCached.Add((season == Season.Undefined) ? "" : season.LabelCap());
			}
		}

		// Token: 0x0600C202 RID: 49666 RVA: 0x0046A838 File Offset: 0x00468A38
		public static void DateOnGUI(Rect dateRect)
		{
			Vector2 vector;
			if (WorldRendererUtility.WorldSelected && Find.WorldSelector.SelectedTile.Valid)
			{
				vector = Find.WorldGrid.LongLatOf(Find.WorldSelector.SelectedTile);
			}
			else if (WorldRendererUtility.WorldSelected && Find.WorldSelector.NumSelectedObjects > 0)
			{
				vector = Find.WorldGrid.LongLatOf(Find.WorldSelector.FirstSelectedObject.Tile);
			}
			else
			{
				if (Find.CurrentMap == null)
				{
					return;
				}
				vector = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
			}
			int index = GenDate.HourInteger((long)Find.TickManager.TicksAbs, vector.x);
			int num = GenDate.DayOfTwelfth((long)Find.TickManager.TicksAbs, vector.x);
			Season season = GenDate.Season((long)Find.TickManager.TicksAbs, vector);
			Quadrum quadrum = GenDate.Quadrum((long)Find.TickManager.TicksAbs, vector.x);
			int num2 = GenDate.Year((long)Find.TickManager.TicksAbs, vector.x);
			string text = DateReadout.SeasonLabelVisible ? DateReadout.seasonsCached[(int)season] : "";
			if (num != DateReadout.dateStringDay || season != DateReadout.dateStringSeason || quadrum != DateReadout.dateStringQuadrum || num2 != DateReadout.dateStringYear)
			{
				DateReadout.dateString = GenDate.DateReadoutStringAt((long)Find.TickManager.TicksAbs, vector);
				DateReadout.dateStringDay = num;
				DateReadout.dateStringSeason = season;
				DateReadout.dateStringQuadrum = quadrum;
				DateReadout.dateStringYear = num2;
			}
			Text.Font = GameFont.Small;
			float num3 = Mathf.Max(Mathf.Max(Text.CalcSize(Prefs.TwelveHourClockMode ? DateReadout.fastHourStrings12h[index] : DateReadout.fastHourStrings24h[index]).x, Text.CalcSize(DateReadout.dateString).x), Text.CalcSize(text).x) + 7f;
			dateRect.xMin = dateRect.xMax - num3;
			if (Mouse.IsOver(dateRect))
			{
				Widgets.DrawHighlight(dateRect);
			}
			Widgets.BeginGroup(dateRect);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect = dateRect.AtZero();
			rect.xMax -= 7f;
			if (Prefs.TwelveHourClockMode)
			{
				Widgets.Label(rect, DateReadout.fastHourStrings12h[index]);
			}
			else
			{
				Widgets.Label(rect, DateReadout.fastHourStrings24h[index]);
			}
			rect.yMin += 26f;
			Widgets.Label(rect, DateReadout.dateString);
			rect.yMin += 26f;
			if (!text.NullOrEmpty())
			{
				Widgets.Label(rect, text);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.EndGroup();
			if (Mouse.IsOver(dateRect))
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 4; i++)
				{
					Quadrum quadrum2 = (Quadrum)i;
					stringBuilder.AppendLine(quadrum2.Label() + " - " + quadrum2.GetSeason(vector.y).LabelCap());
				}
				TaggedString taggedString = "DateReadoutTip".Translate(GenDate.DaysPassed, 15, season.LabelCap(), 15, GenDate.Quadrum((long)GenTicks.TicksAbs, vector.x).Label(), stringBuilder.ToString());
				TooltipHandler.TipRegion(dateRect, new TipSignal(taggedString, 86423));
			}
		}

		// Token: 0x04006EDF RID: 28383
		private static string dateString;

		// Token: 0x04006EE0 RID: 28384
		private static int dateStringDay = -1;

		// Token: 0x04006EE1 RID: 28385
		private static Season dateStringSeason = Season.Undefined;

		// Token: 0x04006EE2 RID: 28386
		private static Quadrum dateStringQuadrum = Quadrum.Undefined;

		// Token: 0x04006EE3 RID: 28387
		private static int dateStringYear = -1;

		// Token: 0x04006EE4 RID: 28388
		private static readonly List<string> fastHourStrings24h = new List<string>();

		// Token: 0x04006EE5 RID: 28389
		private static readonly List<string> fastHourStrings12h = new List<string>();

		// Token: 0x04006EE6 RID: 28390
		private static readonly List<string> seasonsCached = new List<string>();

		// Token: 0x04006EE7 RID: 28391
		private const float DateRightPadding = 7f;
	}
}
